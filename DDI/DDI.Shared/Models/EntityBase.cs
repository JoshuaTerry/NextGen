using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Shared.Models
{
    /// <summary>
    /// Base class for all entity model classes.
    /// </summary>
    public abstract class EntityBase : IEntity
    {
        #region Fields

        private string _entityType = null;
        private static Dictionary<string, Type> _entityTypeDict = null;

        #endregion

        #region Properties 

        public abstract Guid Id { get; set; }

        public virtual string DisplayName => string.Empty;

        public virtual Guid? CreatedBy { get; set; }

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
