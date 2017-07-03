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
using DDI.Logger;

namespace DDI.Services.General
{
    public class GroupService : ServiceBase<Group>, IGroupService
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(GroupService));
        public GroupService(IUnitOfWork uow) : base(uow) { }

        public override IDataResponse Delete(Group entity)
        {
            UnitOfWork.GetReference(entity, g => g.Users); 
            Parallel.ForEach(entity.Users, u => RemoveUser(u, entity));

            return base.Delete(entity);
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

        public IDataResponse<ICollection<User>> GetUsersInGroup(Guid groupId)
        {
            var users = UnitOfWork.GetById<Group>(groupId).Users;
            var response = new DataResponse<ICollection<User>>()
            {
                Data = users,
                IsSuccessful = true
            };
            return response;
        }

        public IDataResponse<Group> UpdateGroupRoles(Guid groupId, JObject roleIds)
        {
            var group = UnitOfWork.GetById<Group>(groupId, g => g.Roles, g => g.Users);
            try
            {
                var rolelist = GetRoleListFromJObject(roleIds);

                var adds = rolelist.Except(group.Roles).ToList();
                var removes = group.Roles.Except(rolelist).ToList(); 

                adds.ForEach(r => group.Roles.Add(r)); 
                removes.ForEach(r => group.Roles.Remove(r));

                UnitOfWork.Update(group);
                Parallel.ForEach(group.Users, u => { UpdateUserRoles(u); });
                
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                var response = new DataResponse<Group>(group);
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return new DataResponse<Group>(group); 
        }
        
        public void UpdateUserRoles(User user)
        {
            UnitOfWork.GetReference(user, u => u.Groups);
            UnitOfWork.GetReference(user, u => u.Roles);
            user.Groups.ForEach(g => UnitOfWork.GetReference(g, r => r.Roles));
            
            // Get all the Roles tied to the User
            var userRoles = user.Roles.Select(r => r.RoleId).ToList();
            // Get all the Roles tied to all the Groups tied to the User
            var groupRoles = user.Groups.SelectMany(g => g.Roles).Select(r => r.Id).Distinct().ToList();

            // Get all the Roles that now exist in the Users's Groups that aren't assigned to the User
            var adds = groupRoles.Except(userRoles);
            // Get all the Roles that no longer exist in the User's Groups but are still assigned to the User
            var removes = userRoles.Except(groupRoles);

            // Remove the Roles the User should no longer have 
            foreach (Guid id in removes)
            { 
                UnitOfWork.Delete(user.Roles.First(r => r.RoleId == id));
            }
            // Add the Roles the User should now have
            adds.ForEach(r => user.Roles.Add(new UserRole() { Id = Guid.NewGuid(), UserId = user.Id, RoleId = r }));

            UnitOfWork.Update(user);
        }

        public void RemoveUser(User user, Group group)
        {
            UnitOfWork.GetReference(group, g => g.Roles);
            UnitOfWork.GetReference(user, u => u.Roles);
            user.Groups.Remove(group);
            var newRoles = user.Groups.SelectMany(g => g.Roles).Select(r => r.Id).Distinct().ToList();
            var removes = user.Roles.Select(r => r.RoleId).Except(newRoles).ToList();

            foreach (Guid id in removes)
            {  
                UnitOfWork.Delete(user.Roles.First(r => r.RoleId == id));
            }
        }
        internal List<Role> GetRoleListFromJObject(JObject roleIds)
        {
            var list = new List<Role>();

            foreach (var pair in roleIds)
            {
                if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    list.AddRange(from jToken in (JArray)pair.Value
                                             select Guid.Parse(jToken.ToString())
                                        into id
                                             select UnitOfWork.GetById<Role>(id));
                }
            }
            return list;
        } 
    }
}
