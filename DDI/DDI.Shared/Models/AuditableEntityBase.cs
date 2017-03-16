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

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual string LastModifiedBy { get; set; }

        public virtual DateTime? LastModifiedOn { get; set; }

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion
    }


}
