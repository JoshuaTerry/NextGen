using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Attributes.Logging;

namespace DDI.Shared.Models
{

    /// <summary>
    /// Base class for all entity model classes that are audited and have changed/modified properties.
    /// </summary>
    [DoLog]        
    public abstract class AuditableEntityBase : EntityBase, IAuditableEntity
    {
        #region Fields

        private string _entityType = null;
        private static Dictionary<string, Type> _entityTypeDict = null;

        #endregion

        #region Properties 

        public virtual Guid? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? CreatedOn { get; set; }

        public virtual Guid? LastModifiedBy { get; set; }

        public virtual DateTime? LastModifiedOn { get; set; }

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        public override string ToString()
        {
            return DisplayName;
        }

        /// <summary>
        /// Ensure the entity's primary key has been assigned.
        /// </summary>
        public void AssignPrimaryKey()
        {
            if (this.Id == default(Guid))
                this.Id = Guid.NewGuid();
        }

        #endregion
    }


}
