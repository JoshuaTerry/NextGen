using DDI.Business.CRM;
using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
using System;

namespace DDI.Services
{
    public class RelationshipService : ServiceBase<Relationship>, IRelationshipService
    {

        #region Private Fields

        private RelationshipLogic _logic;
        private IUnitOfWork _uow;

        #endregion

        #region Properties

        protected override Action<Relationship> FormatEntityForGet => FormatRelationshipForTarget;

        /// <summary>
        /// The target constituent used for reformatting Relationship entities.
        /// </summary>
        public Constituent TargetConstituent { get; set; }

        /// <summary>
        /// The Id of the target constituent used for reformatting Relationship entities.
        /// </summary>
        public Guid? TargetConstituentId
        {
            get
            {
                return TargetConstituent?.Id;
            }
            set
            {
                if (value == null)
                {
                    TargetConstituent = null;
                }
                else
                {
                    TargetConstituent = _uow.GetById<Constituent>(value.Value);
                }
            }
        }

        #endregion

        #region Constructors

        public RelationshipService()
            : this(new UnitOfWorkEF())
        {
        }

        public RelationshipService(IUnitOfWork uow)
            : this(uow, uow.GetBusinessLogic<RelationshipLogic>())
        {
            _uow = uow;
        }

        private RelationshipService(IUnitOfWork uow, RelationshipLogic relationshipLogic)
            : base(uow)
        {
            _logic = relationshipLogic;
        }

        #endregion

        #region Protected Methods

        private void FormatRelationshipForTarget(Relationship entity)
        {
            if (entity != null && TargetConstituent != null)
            {
                // Ensure target constituent is the "right side":  [left side] is the [relationship] of [right side].
                RelationshipType relationshipType;
                Constituent leftSide = _logic.GetLeftSideConstituent(entity, TargetConstituent, out relationshipType);
                if (leftSide != entity.Constituent1)
                {
                    // Relationship needs to be swapped.
                    entity.Constituent2 = entity.Constituent1;
                    entity.Constituent1 = leftSide;
                    entity.RelationshipType = relationshipType;
                    entity.RelationshipTypeId = entity.RelationshipType?.Id;
                    entity.Constituent2Id = entity.Constituent2?.Id;
                    entity.Constituent1Id = entity.Constituent1?.Id;
                    entity.IsSwapped = true;
                }
            }
        }

        public override IDataResponse<Relationship> Update(Relationship entity, JObject changes)
        {
            // Overriding Update in order to set the target constituent Id.  This is used during validation to ensure the target (right-side) constituent didn't change.
            entity.TargetConstituentId = TargetConstituentId;

            return base.Update(entity, changes);
        }
        #endregion

    }
}
