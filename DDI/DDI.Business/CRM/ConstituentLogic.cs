
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using DDI.Shared.Enums.CRM;
using DDI.Business.Core;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.CRM
{
    /// <summary>
    /// Constituent business logic.
    /// </summary>
    public class ConstituentLogic : EntityLogicBase<Constituent>
    {
        #region Private Fields
        private readonly int _maxTries = 5;
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

        private NameFormatter _nameFormatter = null;
        protected NameFormatter NameFormatter
        {
            get
            {
                if (_nameFormatter == null)
                {
                    _nameFormatter = UnitOfWork.GetBusinessLogic<NameFormatter>();
                }
                return _nameFormatter;
            }
        }

        /// <summary>
        /// Get the formatted name for a constituent.
        /// </summary>
        public string GetFormattedName(Constituent constituent)
        {
            LabelFormattingOptions options = new LabelFormattingOptions() { OmitPrefix = true, Recipient = LabelRecipient.Primary };
            string nameLine1, nameLine2;


            NameFormatter.GetNameLines(constituent, null, options, out nameLine1, out nameLine2);
            return nameLine1;
        }


        /// <summary>
        /// Get the sort name for a constituent (e.g. Last First Middle)
        /// </summary>
        /// <param name="constituent"></param>        
        public string GetSortName(Constituent constituent)
        {
            ConstituentType type = constituent.ConstituentType ?? UnitOfWork.GetReference(constituent, p => p.ConstituentType);

            if (type.Category == ConstituentCategory.Organization)
            {
                return constituent.Name;
            }

            return NameFormatter.FormatIndividualSortName(constituent);
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

            int nextNumber = 0;
            bool isUnique = false;
            int tries = 0;
            while (!isUnique)
            {
                tries++;
                nextNumber = _constituentRepo.Utilities.GetNextSequenceValue(DomainContext.ConstituentNumberSequence);
                isUnique = _constituentRepo.Entities.Count(p => p.ConstituentNumber == nextNumber) == 0;

                if (tries >= _maxTries)
                    throw new ValidationException(UserMessagesCRM.NextSequenceValueTooManyTries);
            }

            return nextNumber;
        }

        public void SetNextConstituentNumber(int newValue)
        {
            _constituentRepo.Utilities?.SetNextSequenceValue(DomainContext.ConstituentNumberSequence, newValue);
        }

        public Constituent GetSpouse(Constituent constituent)
        {
            ConstituentType type = constituent.ConstituentType ?? UnitOfWork.GetReference(constituent, p => p.ConstituentType);
            if (type != null && type.Category == ConstituentCategory.Individual)
            {
                Relationship relationship = GetRelationships(constituent).FirstOrDefault(p => p.RelationshipType.IsSpouse);
                if (relationship != null)
                {
                    var relationshipLogic = UnitOfWork.GetBusinessLogic<RelationshipLogic>();
                    return relationshipLogic.GetLeftSideConstituent(relationship, constituent);
                }
            }

            return null;
        }

        public bool IsConstituentActive(Constituent constituent)
        {
            var status = constituent.ConstituentStatus ?? UnitOfWork.GetReference(constituent, p => p.ConstituentStatus);
            return (status == null || status.BaseStatus != ConstituentBaseStatus.Inactive);
        }

        /// <summary>
        /// Get a collection of relationships for this constituent:  "XXXX is the YYYY of (this)".
        /// </summary>
        public List<Relationship> GetRelationships(Constituent constituent)
        {
            return GetRelationships(constituent, null, null);
        }

        /// <summary>
        /// Get a collection of relationships of a specific category for a constituent.
        /// </summary>
        public List<Relationship> GetRelationships(Constituent constituent, RelationshipCategory category)
        {
            return GetRelationships(constituent, category, null);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get a collection of relationships for this constituent:  "XXXX is the YYYY of (this)".
        /// <para>memberRelationship is:</para>
        /// <para>null:  Return all relationships</para>
        /// <para>true:  Return only MEMB relationships</para>
        /// <para>false:  Return only non-MEMB relationships</para>
        /// </summary>
        private List<Relationship> GetRelationships(Constituent constituent, RelationshipCategory category, bool? showInQuickView)
        {
            if (constituent == null)
            {
                return new List<Relationship>();
            }

            UnitOfWork.LoadReference(constituent, p => p.Relationship1s);
            UnitOfWork.LoadReference(constituent, p => p.Relationship2s);

            // The Relationship2 collection is the starting point:  All these have Constituent2 = (this)            
            List<Relationship> list = GetRelationshipQuery(constituent.Id, false, category, showInQuickView).ToList();

            // Add in Relationship1 rows:  All these have Constituent1 = (this)
            foreach (Relationship row in GetRelationshipQuery(constituent.Id, true, category, showInQuickView))
            {
                // Omit any duplicates
                if (list.Any(p => p.Constituent1 == row.Constituent2))
                    continue;

                list.Add(row);
            }

            return list;
        }

        /// <summary>
        /// Get a relationship LINQ query given a relationship collection.
        /// </summary>
        private IQueryable<Relationship> GetRelationshipQuery(Guid constituentId, bool isLeft, RelationshipCategory category, bool? showInQuickView)
        {
            IQueryable<Relationship> query = UnitOfWork.GetEntities<Relationship>(prop => prop.RelationshipType);

            if (isLeft)
            {
                query = query.Where(p => p.Constituent1Id == constituentId);
            }
            else
            {
                query = query.Where(p => p.Constituent2Id == constituentId);
            }
            // Modify the query based on category, showInQuickView parameters.

            if (category != null)
            {
                query = query.Where(p => p.RelationshipType.RelationshipCategory.Id == category.Id);
            }

            if (showInQuickView.HasValue)
            {
                query = query.Where(p => p.RelationshipType.RelationshipCategory.IsShownInQuickView == showInQuickView);
            }

            return query;
        }
        #endregion

    }
}
