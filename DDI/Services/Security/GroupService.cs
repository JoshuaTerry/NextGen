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
            throw new NotImplementedException();
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
            var group = UnitOfWork.GetById<Group>(groupId, r => r.Roles);
            var rolesToAdd = new List<Role>();
            List<Role> roles = new List<Role>();

            foreach(var pair in roleIds)
            {
                if(pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    rolesToAdd.AddRange(from jToken in (JArray)pair.Value
                                        select Guid.Parse(jToken.ToString()) 
                                        into id select UnitOfWork.GetById<Role>(id));
                }
            }

            rolesToAdd.ForEach(r => group.Roles.Remove(r));
            rolesToAdd.ForEach(ra => group.Roles.Add(ra));

            UnitOfWork.SaveChanges();
            var response = new DataResponse<Group>
            {
               Data = UnitOfWork.GetById<Group>(group.Id),
                //Data = group,
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse<Group> RemoveRolesFromGroup(Guid groupId, JObject roleIds)
        {
            // I think I only need to remove one at a time 
            // remove the role(s) from the Group
            // Find the user(s) associated with this group
            // remove the roles from them
            return new DataResponse<Group>();
        }

    }
}
