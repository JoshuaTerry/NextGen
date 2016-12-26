using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Data.Enums.CRM;

namespace DDI.Data.Models.Client.CRM
{
    [Table("RelationshipType")]
    public class RelationshipType : BaseEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Guid? ReciprocalTypeMaleId { get; set; }

        [ForeignKey(nameof(ReciprocalTypeMaleId))]
        public RelationshipType ReciprocalTypeMale { get; set; }

        public Guid? ReciprocalTypeFemaleId { get; set; }

        [ForeignKey(nameof(ReciprocalTypeFemaleId))]
        public RelationshipType ReciprocalTypeFemale { get; set; }

        public bool IsSpouse { get; set; }

        public ConstituentCategory ConstituentCategory { get; set; }

        public Guid? RelationshipCategoryId { get; set; }

        public RelationshipCategory RelationshipCategory { get; set; }

        public ICollection<Relationship> Relationships { get; set; }
        
        [InverseProperty(nameof(ReciprocalTypeMale))]
        public ICollection<RelationshipType> MaleTypes { get; set; }

        [InverseProperty(nameof(ReciprocalTypeFemale))]
        public ICollection<RelationshipType> FemaleTypes { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Code + ": " + Name;
            }
        }

        #endregion

    }
}
