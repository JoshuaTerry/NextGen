
using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.CRM
{
    public class ConstituentLogic : BaseEntityLogic<Constituent>
    {
        #region Private Fields

        private IRepository<Constituent> _constituentRepo = null;

        #endregion

        #region Constructors 

        public ConstituentLogic() : this(new UnitOfWorkEF()) { }

        public ConstituentLogic(IUnitOfWork uow) : base(uow)
        {
            _constituentRepo = UnitOfWork.GetRepository<Constituent>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the formatted name for a constituent.
        /// </summary>
        /// <param name="constituent"></param>
        /// <returns></returns>
        public string GetFormattedName(Constituent constituent)
        {
            return string.Join(" ", (new string[]
            {
                constituent.FirstName,
                constituent.MiddleName,
                constituent.LastName
            }).Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        /// <summary>
        /// Get the sort name for a constituent (e.g. Last First Middle)
        /// </summary>
        /// <param name="constituent"></param>
        /// <returns></returns>
        public string GetSortName(Constituent constituent)
        {
            return string.Join(" ", (new string[]
            {
                constituent.LastName,
                constituent.FirstName,
                constituent.MiddleName
            }).Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        public override void Validate(Constituent constituent)
        {
            base.Validate(constituent);           

            // Get list of modified properties
            List<string> modifiedProperties = _constituentRepo.GetModifiedProperties(constituent);

            // Update constituent formatted name and sort name only if name fields were updated.
            if (modifiedProperties.Intersect(new string[]
            {
                nameof(Constituent.FirstName),
                nameof(Constituent.MiddleName),
                nameof(Constituent.LastName),
                nameof(Constituent.Suffix),
                nameof(Constituent.Nickname),
                nameof(Constituent.Prefix),
                nameof(Constituent.NameFormat)
            }).Count() > 0)
            {
                constituent.FormattedName = GetFormattedName(constituent);
                constituent.Name = GetSortName(constituent);
            }
        }

        public int GetNextConstituentNumber()
        {
            if (_constituentRepo.Utilities == null)
            {
                // If this is a mocked repo, get the last constituent # in use and add one.
                return 1 + (_constituentRepo.Entities.OrderByDescending(p => p.ConstituentNumber).FirstOrDefault()?.ConstituentNumber ?? 0);
            }

            int nextNum = 0;
            bool isUnique = false;
            while (!isUnique)
            {
                nextNum = _constituentRepo.Utilities.GetNextSequenceValue(DomainContext.ConstituentNumberSequence);
                isUnique = _constituentRepo.Entities.Count(p => p.ConstituentNumber == nextNum) == 0;
            }

            return nextNum;
        }

        public void SetNextConstituentNumber(int newValue)
        {
            _constituentRepo.Utilities?.SetNextSequenceValue(DomainContext.ConstituentNumberSequence, newValue);
        }

        #endregion

    }
}
