using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Extensions;

namespace DDI.Services.General
{
    public class GroupService : ServiceBase<Group>, IGroupService
    {

        public GroupService(IUnitOfWork uow) : base(uow) { }

        public IDataResponse AddUserToGroup(Guid userId, Guid groupId)
        {
            User user = UnitOfWork.GetById<User>(userId, u => u.Groups, u => u.Roles);
            Group group = UnitOfWork.GetById<Group>(groupId, u => u.Users, u => u.Roles);

            if (!user.Groups.Contains(group))
            {
                //clear all groups from user, since user can only be in one group at a time
                user.Groups.Clear();

                //add group to user
                user.Groups.Add(group);
                group.Users.Add(user);

                //must iterate through the user.roles and delete all of the userroles for that user from the table.
                List<UserRole> currentUserRoles = new List<UserRole>(user.Roles);
                foreach (UserRole ur in currentUserRoles)
                {
                    UnitOfWork.Delete<UserRole>(ur);
                }

                //add new userrole for each role on the group
                if (group.Roles != null)
                {
                    group.Roles.ToList().ForEach(r => user.Roles.Add(new UserRole() { UserId = user.Id, RoleId = r.Id }));
                    user.Roles.ForEach(r => r.AssignPrimaryKey());
                }

                UnitOfWork.SaveChanges();
            }

            var response = new DataResponse<User>()
            {
                Data = user,
                IsSuccessful = true
            };
            return response;

        }

        public IDataResponse RemoveUserFromGroup(Guid userId, Guid groupId)
        {
            throw new NotImplementedException();
        }

        public IDataResponse<ICollection<Role>> GetRolesInGroup(Guid groupId)
        {
            var roles = UnitOfWork.GetById<Group>(groupId).Roles;
            var response =  new DataResponse<ICollection<Role>>()
            {
                Data = roles,
                IsSuccessful = true
            };
            return response;

        }

        public IDataResponse<Group> AddRolesToGroup(Guid groupId, JObject roleIds)
        {
            var group = UnitOfWork.GetById<Group>(groupId, r => r.Roles, r => r.Users);

            List<Guid> businessUnits = new List<Guid>();
            foreach (var child in roleIds["item"].Children())
            {
                string roleId = child.ToString();
                Role role = UnitOfWork.GetById<Role>(new Guid(roleId));
                
                if (!group.Roles.Contains(role))
                {
                    group.Roles.Add(role);
                }
            }

            UnitOfWork.SaveChanges();

            var response = new DataResponse<Group>
            {
               Data = group,
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse<Group> RemoveRolesFromGroup(Guid groupId, Guid roleId)
        {
            var group = UnitOfWork.GetById<Group>(groupId, g => g.Roles, g => g.Users); // group
            var roleToRemove = group.Roles.Where(r => r.Id == roleId).FirstOrDefault(); // role 

            List<User> users = new List<User>(group.Users);

            if (roleToRemove != null)
            {
                group.Roles.Remove(roleToRemove);
                var groupUserIds = group.Users.Select(u => u.Id).ToList(); // user Ids in that group
                var roleUserIds = roleToRemove.Users.Select(u => u.UserId); // user Ids in role
                var usersToRemove = groupUserIds.Intersect(roleUserIds).ToList(); // intersection of users in group and role

                foreach (Guid userId in usersToRemove)
                {
                    roleToRemove.Users.Remove(roleToRemove.Users.FirstOrDefault(u => u.UserId == userId));
                    var userToRemoveRoles = UnitOfWork.GetRepository<User>().GetById(userId);
                    userToRemoveRoles.Roles.Remove(userToRemoveRoles.Roles.FirstOrDefault(r => r.RoleId == roleToRemove.Id));
                }

                UnitOfWork.SaveChanges();

            }

            return new DataResponse<Group>()
            {
                Data = UnitOfWork.GetById<Group>(group.Id),
                IsSuccessful = true
            };
        }

        public IDataResponse<Group> UpdateGroup(Guid groupId, JObject roleIds)
        {


            // need to be able to set IsSuccessful to false if something goes wrong
            var group = UnitOfWork.GetById<Group>(groupId, r => r.Roles);
            var rolesToAdd = new List<Role>();

            group.Roles.Clear(); 

            foreach (var pair in roleIds)
            {
                if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    rolesToAdd.AddRange(from jToken in (JArray)pair.Value
                                        select Guid.Parse(jToken.ToString())
                                        into id
                                        select UnitOfWork.GetById<Role>(id));
                }
            }

            rolesToAdd.ForEach(r => group.Roles.Add(r));

            var groupUserIds = group.Users.Select(u => u.Id).ToList(); // user Ids in that group
            foreach(var userId in groupUserIds)
            {
                UpdateUserRoles(userId, rolesToAdd);

            }

            UnitOfWork.SaveChanges();

            return new DataResponse<Group>()
            {
                Data = UnitOfWork.GetById<Group>(group.Id),
                IsSuccessful = true
            };
        }

        private void UpdateUserRoles(Guid userId, List<Role> rolesToAdd)
        {
            var user = UnitOfWork.GetRepository<User>().GetById(userId, u => u.Roles );
            user.Roles.Clear();

            rolesToAdd.ForEach(r => user.Roles.Add(new UserRole() { UserId = user.Id, RoleId = r.Id }));
        }
    }
}
