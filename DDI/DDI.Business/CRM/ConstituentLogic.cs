using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Data.Enums.CRM;
using DDI.Data.Models.Client.CRM;

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
            while (true)
            {
                nextNum = _constituentRepo.Utilities.GetNextSequenceValue(DomainContext.ConstituentNumberSequence);
                if (_constituentRepo.Entities.Count(p => p.ConstituentNumber == nextNum) == 0)
                    break;
            }

            return nextNum;
        }

        public void SetNextConstituentNumber(int newValue)
        {
            _constituentRepo.Utilities?.SetNextSequenceValue(DomainContext.ConstituentNumberSequence, newValue);
        }

        public Constituent GetSpouse(Constituent name)
        {
            ConstituentType ctype = UnitOfWork.GetReference(name, p => p.ConstituentType);
            if (ctype != null && ctype.BaseType == "Individual")
            {
                Relationship rel = GetRelationships(name).FirstOrDefault(p => p.RelationshipType.IsSpouse);
                if (rel != null)
                {
                    var relationshipLogic = UnitOfWork.GetBusinessLogic<RelationshipLogic>();
                    return relationshipLogic.GetLeftSideConstituent(rel, name);
                }
            }

            return null;
        }

        public bool IsConstituentActive(Constituent name)
        {
            var status = UnitOfWork.GetReference(name, p => p.ConstituentStatus);
            //return (status == null && status.BaseStatus != ConstituentBaseStatus.Inactive);
            return true;
        }

        /// <summary>
        /// Get a collection of relationships for this constituent:  "XXXX is the YYYY of (this)".
        /// </summary>
        public List<Relationship> GetRelationships(Constituent name)
        {
            return GetRelationships(name, null, null);
        }

        /// <summary>
        /// Get a collection of relationships of a specific category for a constituent.
        /// </summary>
        public List<Relationship> GetRelationships(Constituent name, RelationshipCategory category)
        {
            return GetRelationships(name, category, null);
        }

        /// <summary>
        /// Get a collection of relationships for this constituent:  "XXXX is the YYYY of (this)".
        /// <para>memberRelationship is:</para>
        /// <para>null:  Return all relationships</para>
        /// <para>true:  Return only MEMB relationships</para>
        /// <para>false:  Return only non-MEMB relationships</para>
        /// </summary>
        private List<Relationship> GetRelationships(Constituent name, RelationshipCategory category, bool? showInQuickView)
        {
            UnitOfWork.LoadReference(name, p => p.Relationship1s);
            UnitOfWork.LoadReference(name, p => p.Relationship2s);

            // The Relationship2 collection is the starting point:  All these have Constituent2 = (this)            
            List<Relationship> relSet = GetRelationshipQuery(name.Relationship2s, category, showInQuickView).ToList();

            // Add in Relationship1 rows:  All these have Constituent1 = (this)
            foreach (Relationship row in GetRelationshipQuery(name.Relationship1s, category, showInQuickView))
            {
                // Omit any duplicates
                if (relSet.Any(p => p.Constituent1 == row.Constituent2))
                    continue;

                relSet.Add(row);
            }

            return relSet;
        }

        /// <summary>
        /// Get a relationship LINQ query given a relationship XPCollection.
        /// </summary>
        private IQueryable<Relationship> GetRelationshipQuery(ICollection<Relationship> collection, RelationshipCategory category, bool? showInQuickView)
        {
            IQueryable<Relationship> query = collection.AsQueryable().IncludePath(p => p.RelationshipType);

            // Modify the query based on category, showInQuickView parameters.
            if (category != null)
                query = query.Where(p => p.RelationshipType.RelationshipCategory.Id == category.Id);
            if (showInQuickView.HasValue)
                query = query.Where(p => p.RelationshipType.RelationshipCategory.IsShownInQuickView == showInQuickView);
            return query;
        }
        #endregion

    }
}
