using DDI.Shared.Helpers;
using System;
using System.Collections.Generic;
using DDI.Shared.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DDI.Shared.Models
{
    /// <summary>
    /// Base class for all entity model classes.
    /// </summary>
    public abstract class EntityBase : IEntity
    {
        #region Properties 

        public abstract Guid Id { get; set; }

        public virtual string DisplayName => string.Empty;
        [Timestamp]
        public virtual Byte[] RowVersion { get; set; }
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
            {
                this.Id = GuidHelper.NewGuid();
            }
        }

        #endregion
    }


}
