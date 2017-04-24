using DDI.Shared.Attributes.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DDI.Shared.Models
{

    /// <summary>
    /// Base class for all entity model classes that are audited and have changed/modified properties.
    /// </summary>
    [DoLog]        
    public abstract class AuditableEntityBase : EntityBase, IAuditableEntity
    {     
        #region Properties 

        [MaxLength(64)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        [MaxLength(64)]
        public virtual string LastModifiedBy { get; set; }

        public virtual DateTime? LastModifiedOn { get; set; }

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion
    }


}
