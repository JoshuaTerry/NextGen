using System;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;

namespace DDI.Services.ServiceInterfaces
{
    public interface IRelationshipService : IService<Relationship>
    {
         Constituent TargetConstituent { get; set; }
         Guid? TargetConstituentId { get; set; }
    }
}