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

            List<Role> rolesToAdd = new List<Role>();

            foreach (var child in roleIds["item"].Children())
            {
                string roleId = child.ToString();
                Role role = UnitOfWork.GetById<Role>(new Guid(roleId));
                rolesToAdd.Add(role);

                if (!group.Roles.Contains(role))
                {
                    group.Roles.Add(role);
                }
            }

            foreach (User user in group.Users)
            {
                AddUserRoles(user.Id, rolesToAdd);
            }

            UnitOfWork.SaveChanges();

            var response = new DataResponse<Group>
            {
               Data = group,
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse DeleteGroup(Guid groupId)
        {
            // find the group
            Group group = UnitOfWork.GetById<Group>(groupId, g => g.Roles, g => g.Users); // group

            if (group != null)
            {
                // delete the user roles from the users in the group
                foreach (User user in group.Users)
                {
                    foreach (Role role in group.Roles)
                    {
                        UserRole userRole = UnitOfWork.FirstOrDefault<UserRole>(u => u.RoleId == role.Id && u.UserId == user.Id);
                        if (userRole != null)
                        {
                            UnitOfWork.Delete<UserRole>(userRole);
                        }
                    }
                }

                // delete the group itself
                UnitOfWork.Delete<Group>(group);

                UnitOfWork.SaveChanges();
            }

            var response = new DataResponse<string>
            {
                Data = groupId.ToString(),
                IsSuccessful = true
            };

            return response;
        }



        public IDataResponse<Group> RemoveRolesFromGroup(Guid groupId, Guid roleId)
        {
            Group group = UnitOfWork.GetById<Group>(groupId, g => g.Roles, g => g.Users); // group
            Role roleToRemove = group.Roles.Where(r => r.Id == roleId).FirstOrDefault(); // role 

            if (roleToRemove != null)
            {
                group.Roles.Remove(roleToRemove);
                
                foreach (User user in group.Users)
                {
                    UserRole userRole = UnitOfWork.FirstOrDefault<UserRole>(u => u.UserId == user.Id && u.RoleId == roleId);
                    if (userRole != null)
                    {
                        UnitOfWork.Delete<UserRole>(userRole);
                    }
                }
                
                UnitOfWork.SaveChanges();

            }

            return new DataResponse<Group>()
            {
                Data = group,
                IsSuccessful = true
            };
        }

        //public IDataResponse<Group> UpdateGroup(Guid groupId, JObject roleIds)
        //{


        //    // need to be able to set IsSuccessful to false if something goes wrong
        //    var group = UnitOfWork.GetById<Group>(groupId, r => r.Roles);
        //    var rolesToAdd = new List<Role>();

        //    group.Roles.Clear(); 

        //    foreach (var pair in roleIds)
        //    {
        //        if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
        //        {
        //            rolesToAdd.AddRange(from jToken in (JArray)pair.Value
        //                                select Guid.Parse(jToken.ToString())
        //                                into id
        //                                select UnitOfWork.GetById<Role>(id));
        //        }
        //    }

        //    rolesToAdd.ForEach(r => group.Roles.Add(r));

        //    var groupUserIds = group.Users.Select(u => u.Id).ToList(); // user Ids in that group
        //    foreach(var userId in groupUserIds)
        //    {
        //        AddUserRoles(userId, rolesToAdd);

        //    }

        //    UnitOfWork.SaveChanges();

        //    return new DataResponse<Group>()
        //    {
        //        Data = UnitOfWork.GetById<Group>(group.Id),
        //        IsSuccessful = true
        //    };
        //}

        private void AddUserRoles(Guid userId, List<Role> rolesToAdd)
        {
            var user = UnitOfWork.GetRepository<User>().GetById(userId, u => u.Roles );
            
            foreach (Role role in rolesToAdd)
            {
                UserRole userRole = UnitOfWork.FirstOrDefault<UserRole>(u => u.UserId == userId && u.RoleId == role.Id);
                if (userRole == null)
                {
                    userRole = new UserRole() { UserId = user.Id, RoleId = role.Id };
                    userRole.AssignPrimaryKey();
                    user.Roles.Add(userRole);
                }
            }
        }
    }
}
