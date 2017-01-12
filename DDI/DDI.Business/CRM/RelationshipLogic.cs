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
    public class RelationshipLogic : EntityLogicBase<Constituent>
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
        /// Set the left-side constituent and relationship type in a relationship, where target is the right-side consituent.
        ///    "(leftConstituent) is the (relation) of (target)".
        /// </summary>
        public void SetLeftSideConstituent(Relationship thisRelation, Constituent target, Constituent leftConstituent, RelationshipType relation)
        {
            if (target.Id == thisRelation.Constituent2Id)
            {
                thisRelation.RelationshipType = relation;
                thisRelation.Constituent1 = leftConstituent;
                return;
            }

            Gender gender = UnitOfWork.GetReference(leftConstituent, p => p.Gender);

            relation = GetReciprocalRelationshipType(relation, gender);

            thisRelation.RelationshipType = relation;
            thisRelation.Constituent2 = leftConstituent;
        }

        /// <summary>
        /// Set the left-side constituent, where target is the right-side consituent.
        ///    "(leftConstituent) is the (relation) of (target)".
        /// </summary>
        public void SetLeftSideConstituent(Relationship thisRelation, Constituent target, Constituent leftConstituent)
        {
            if (target.Id == thisRelation.Constituent2Id)
                thisRelation.Constituent1 = leftConstituent;
            else
                thisRelation.Constituent2 = leftConstituent;
        }

        public RelationshipType GetReciprocalRelationshipType(RelationshipType relationType, Gender gender)
        {
            RelationshipType recipType;

            if (gender != null && gender.IsMasculine == false)
            {
                recipType = UnitOfWork.GetReference(relationType, p => p.ReciprocalTypeFemale);
            }
            else
            {
                recipType = UnitOfWork.GetReference(relationType, p => p.ReciprocalTypeMale);
            }

            // If no reciprocal, return this relationship.
            if (recipType == null)
            {
                recipType = relationType;
            }

            return recipType;
        }


        #endregion

    }
}
