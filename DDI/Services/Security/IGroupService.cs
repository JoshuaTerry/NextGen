using DDI.Shared;
using DDI.Shared.Models.Client.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.ServiceInterfaces
{
    public interface IGroupService : IService<Group>
    {
        IDataResponse AddUserToGroup(Guid userId, Guid groupId);
        IDataResponse RemoveUserFromGroup(Guid userId, Guid groupId);
        IDataResponse<Group> UpdateGroupRoles(Guid groupId, JObject roleIds);

    }
}