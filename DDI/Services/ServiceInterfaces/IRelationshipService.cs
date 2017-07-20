using DDI.Shared.Models.Client.CRM;
using System;
using DDI.Shared;

namespace DDI.Services.ServiceInterfaces
{
    public interface IRelationshipService : IService<Relationship>
    {
         Constituent TargetConstituent { get; set; }
         Guid? TargetConstituentId { get; set; }
    }
}