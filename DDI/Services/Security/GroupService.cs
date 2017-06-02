﻿using DDI.Services.ServiceInterfaces;
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
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse<Group> RemoveRolesFromGroup(Guid groupId, Guid roleId)
        {
            var group = UnitOfWork.GetById<Group>(groupId, g => g.Roles); // group
            var roleToRemove = group.Roles.Where(r => r.Id == roleId).FirstOrDefault(); // role 
            var groupUserIds = group.Users.Select(u => u.Id).ToList(); // user Ids in that group
            var roleUserIds = roleToRemove.Users.Select(u => u.UserId); // user Ids in role
            var usersToRemove = groupUserIds.Intersect(roleUserIds).ToList(); // intersection of users in group and role
            usersToRemove.ForEach(u => roleToRemove.Users.Remove(roleToRemove.Users.FirstOrDefault(uu => uu.UserId == u))); // for each role, remove user where the id is equal to the user we want to remove..
            // Will need to find users associated with this role
            // Collection<User> users = getwhereexpression(user.Roles.Contains(roleID)
            // foreach(User in users) {
            // user.roles.remove(role)
            // and remove it from them too

            UnitOfWork.SaveChanges();

            return new DataResponse<Group>()
            {
                Data = UnitOfWork.GetById<Group>(group.Id),
                IsSuccessful = true
            };
        }

    }
}
