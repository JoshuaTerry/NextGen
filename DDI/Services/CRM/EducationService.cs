using DDI.Business.CRM;
using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
using System;

namespace DDI.Services
{
    public class EducationService : ServiceBase<Education>
    {

        #region Private Fields


        #endregion

        #region Properties

        protected override Action<Education> FormatEntityForGet => FormatEducationForGet;

        #endregion

        #region Constructors

        #endregion

        

        #region private Methods

        private void FormatEducationForGet(Education education)
        {
            if (education.School == null && education.SchoolId != null)
            {
                UnitOfWork.LoadReference(education, p => p.School);
            }

            if (education.Degree == null && education.DegreeId != null)
            {
                UnitOfWork.LoadReference(education, p => p.Degree);
            }

            if (education.School != null)
            {
                education.SchoolOther = education.School.Name;
            }

            if (education.Degree != null)
            {
                education.DegreeOther = education.Degree.Name;
            }
        }
        
        #endregion

    }
}
