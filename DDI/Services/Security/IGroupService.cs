using DDI.Shared;
using DDI.Shared.Models.Client.Security;
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
        IDataResponse<ICollection<Role>> GetRolesInGroup(Guid groupId);
        IDataResponse<ICollection<Role>> AddRoleToGroup(Guid groupId, Guid roleId);
    }
}