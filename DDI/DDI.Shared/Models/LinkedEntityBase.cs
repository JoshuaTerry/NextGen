using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models
{
    /// <summary>
    /// Base class for entities like notes or custom data that can be linked to many other entity types.
    /// </summary>
    public abstract class LinkedEntityBase : AuditableEntityBase
    {

        #region Properties

        /// <summary>
        /// ID of linked parent entity.
        /// </summary>
        [Index]
        public Guid? ParentEntityId { get; set; }

        /// <summary>
        /// Entity type string value.
        /// </summary>
        [Index]
        [MaxLength(128)]
        public string EntityType { get; set; }

 
        #endregion


    }
}