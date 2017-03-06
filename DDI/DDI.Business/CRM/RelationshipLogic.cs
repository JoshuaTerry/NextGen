using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.CRM
{

    // Relationship business logic
    public class RelationshipLogic : EntityLogicBase<Relationship>
    {
        #region Private Fields

        private IRepository<Relationship> _relationRepo = null;

        #endregion

        #region Constructors 

        public RelationshipLogic() : this(new UnitOfWorkEF()) { }

        public RelationshipLogic(IUnitOfWork uow) : base(uow)
        {
            _relationRepo = UnitOfWork.GetRepository<Relationship>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the left-side constituent in a relationship, where target is the right-side consituent.
        /// Also get the recalculated relationship code: <para> 
        ///    "(return) is the (relation) of (target)". </para>
        /// </summary>
        public Constituent GetLeftSideConstituent(Relationship thisRelation, Constituent target, out RelationshipType relationType)
        {
            relationType = UnitOfWork.GetReference(thisRelation, p => p.RelationshipType);

            if (target.Id == thisRelation.Constituent2Id)
            {
                return UnitOfWork.GetReference(thisRelation, p => p.Constituent1);
            }
            Constituent constituent2 = UnitOfWork.GetReference(thisRelation, p => p.Constituent2);
            Gender gender = UnitOfWork.GetReference(constituent2, p => p.Gender);

            relationType = GetReciprocalRelationshipType(relationType, constituent2.Gender);

            return constituent2;
        }

        /// <summary>
        /// Get the left-side constituent in a relationship, where target is the right-side consituent.
        ///    "(return) is the (relation) of (target)". </para>
        /// </summary>
        public Constituent GetLeftSideConstituent(Relationship thisRelation, Constituent target)
        {
            RelationshipType relation;
            return GetLeftSideConstituent(thisRelation, target, out relation);
        }

        /// <summary>
        /// Get a reciprocal relationship type, e.g. Aunt => Niece or Nephew
        /// </summary>
        /// <param name="relationType">Relationship type of person 1</param>
        /// <param name="gender">Gender of person 2</param>
        public RelationshipType GetReciprocalRelationshipType(RelationshipType relationType, Gender gender)
        {
            RelationshipType reciprocalType;

            if (gender != null && gender.IsMasculine == false)
            {
                reciprocalType = UnitOfWork.GetReference(relationType, p => p.ReciprocalTypeFemale);
            }
            else
            {
                reciprocalType = UnitOfWork.GetReference(relationType, p => p.ReciprocalTypeMale);
            }

            // If no reciprocal, return this relationship.
            if (reciprocalType == null)
            {
                reciprocalType = relationType;
            }

            return reciprocalType;
        }

        public override void Validate(Relationship entity)
        {
            Constituent constituent1 = UnitOfWork.GetReference(entity, p => p.Constituent1);
            Constituent constituent2 = UnitOfWork.GetReference(entity, p => p.Constituent2);
            RelationshipType type = UnitOfWork.GetReference(entity, p => p.RelationshipType);

            if (constituent1 == null || constituent2 == null)
            {
                throw new ValidationException("Both consitutents for a relationship are required.");
            }

            if (constituent1.Id == constituent2.Id)
            {
                throw new ValidationException("Constituents for a relationship must be different.");
            }

            if (type == null)
            {
                throw new ValidationException("Relationship type for a relationship is required.");
            }

            // Verify ConstiutentCategory
            if (type.ConstituentCategory != Shared.Enums.CRM.ConstituentCategory.Both)
            {
                var constituentType1 = UnitOfWork.GetReference(constituent1, p => p.ConstituentType);
                var constituentType2 = UnitOfWork.GetReference(constituent2, p => p.ConstituentType);

                if (constituentType1.Category != type.ConstituentCategory ||
                    constituentType2.Category != type.ConstituentCategory)
                {
                    throw new ValidationException($"Both constituents for this relationship must be {type.ConstituentCategory} constituents.");
                }
            }

            // If TargetConstituent is specified, the right side must be the target.
            if (entity.TargetConstituentId != null && 
                constituent2.Id != entity.TargetConstituentId)
            {
                throw new InvalidOperationException($"Constituent 2 of this relationship must match the target constituent Id.");
            }

            // If swapped:
            if (entity.IsSwapped)
            {
                // The target constituent was originally constituent 1.  We must convert it back to this format.
                Gender gender = UnitOfWork.GetReference(constituent2, p => p.Gender);

                type = GetReciprocalRelationshipType(type, gender);

                entity.RelationshipType = type;
                entity.RelationshipTypeId = type.Id;

                // Swap the constituents
                entity.Constituent2 = constituent1;
                entity.Constituent2Id = constituent1.Id;

                entity.Constituent1 = constituent2;
                entity.Constituent1Id = constituent2.Id;

                constituent1 = entity.Constituent1;
                constituent2 = entity.Constituent2;

                // Relationship is no longer swapped.
                entity.IsSwapped = false;
            }

        }

        #endregion

    }
}
