using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.CRM
{
    public class EducationLogic : EntityLogicBase<Education>
    {
        public EducationLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #region Public Methods


        public override void Validate(Education entity)
        {
            Constituent constituent = UnitOfWork.GetReference(entity, p => p.Constituent);

            if (constituent == null)
            {
                throw new ValidationException(UserMessagesCRM.EducationNoConstituent);
            }

            // See if degree can be standardized.
            if (!string.IsNullOrWhiteSpace(entity.DegreeOther))
            {
                entity.DegreeOther = entity.DegreeOther.Trim();
                Degree degree = UnitOfWork.FirstOrDefault<Degree>(p => p.Name == entity.DegreeOther);
                if (degree != null)
                {
                    entity.DegreeOther = null;
                    entity.Degree = degree;
                }
            }

            // See if school can be standardized.
            if (!string.IsNullOrWhiteSpace(entity.SchoolOther))
            {
                entity.SchoolOther = entity.SchoolOther.Trim();
                School school = UnitOfWork.FirstOrDefault<School>(p => p.Name == entity.SchoolOther);
                if (school != null)
                {
                    entity.SchoolOther = null;
                    entity.School = school;
                }
            }
            
        }

        #endregion

    }
}
