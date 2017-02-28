
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
using DDI.Search.Models;
using DDI.Shared.Models;
using DDI.Search;

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
                
        public Constituent ConvertAgeRange (Constituent constituent)
        {
            if (constituent.BirthYearFrom.HasValue)
            {
                constituent.BirthYearFrom = DateTime.Now.Year - constituent.BirthYearFrom;
            }

            if (constituent.BirthYearTo.HasValue)
            {
                constituent.BirthYearTo = DateTime.Now.Year - constituent.BirthYearTo;
            }

            return constituent;
        }

        public int ConvertAgeRange(int value)
        {
            value = DateTime.Now.Year - value;
            return value;
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

            ScheduleUpdateSearchDocument(constituent);
        }

        public override void UpdateSearchDocument(Constituent constituent)
        {
            var elasticRepository = new ElasticRepository<ConstituentDocument>();
            elasticRepository.Update((ConstituentDocument)BuildSearchDocument(constituent));
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

        /// <summary>
        /// Build a ConstituentDocument for Elasticsearch indexing.
        /// </summary>
        public override ISearchDocument BuildSearchDocument(Constituent entity)
        {
            var document = new ConstituentDocument();

            var constituentAddressLogic = UnitOfWork.GetBusinessLogic<ConstituentAddressLogic>();
            var addressLogic = UnitOfWork.GetBusinessLogic<AddressLogic>();

            document.Id = entity.Id;
            document.ConstituentNumber = entity.ConstituentNumber.ToString();
            document.SortableName = entity.Name;
            document.Name = entity.FormattedName ?? string.Empty;
            document.ConstituentStatusId = entity.ConstituentStatusId;
            document.ConstituentTypeId = entity.ConstituentTypeId;
            document.Source = entity.Source ?? string.Empty;
            document.Name2 = entity.Name2 ?? string.Empty;
            document.Business = entity.Business ?? string.Empty;
            document.Nickname = entity.Nickname ?? string.Empty;
            document.CreationDate = entity.CreatedOn;
            document.LanguageId = entity.LanguageId;
            document.GenderId = entity.GenderId;

            if (entity.BirthYear > 0)
            {
                document.BirthYearFrom = document.BirthYearTo = entity.BirthYear.Value;
            }
            else
            {
                document.BirthYearFrom = entity.BirthYearFrom ?? 0;
                document.BirthYearTo = entity.BirthYearTo ?? 0;
            }

            // Primary address            
            ConstituentAddress constituentAddress = UnitOfWork.GetReference(entity, p => p.ConstituentAddresses).FirstOrDefault(p => p.IsPrimary);
            if (constituentAddress != null)
            {
                document.PrimaryAddress = addressLogic.FormatAddress(UnitOfWork.GetReference(constituentAddress, p => p.Address)).Replace("\n", ", ");
            }

            // Load alternate IDs
            document.AlternateIds = UnitOfWork.GetReference(entity, p => p.AlternateIds).Select(p => p.Name).ToList();

            // Load tags, denominations, ethnicities
            document.Tags = UnitOfWork.GetReference(entity, p => p.Tags).Select(p => p.Id).ToList();
            document.Denominations = UnitOfWork.GetReference(entity, p => p.Denominations).Select(p => p.Id).ToList();
            document.Ethnicities = UnitOfWork.GetReference(entity, p => p.Ethnicities).Select(p => p.Id).ToList();
            
            // Load contact info
            document.ContactInfo = new List<ContactInfoDocument>();
            foreach (var item in UnitOfWork.GetReference(entity, p => p.ContactInfo))
            {
                Guid? categoryId = UnitOfWork.GetReference(item, p => p.ContactType)?.ContactCategoryId;

                document.ContactInfo.Add(new ContactInfoDocument()
                {
                    Id = item.Id,
                    Comment = item.Comment ?? string.Empty,
                    Info = item.Info ?? string.Empty,
                    ContactCategoryId = categoryId
                });
            }

            // Load addresses
            document.Addresses = new List<AddressDocument>();
            foreach (var entry in UnitOfWork.GetReference(entity, p => p.ConstituentAddresses))
            {
                Address address = UnitOfWork.GetReference(entry, p => p.Address);
                if (address != null)
                {
                    document.Addresses.Add(new AddressDocument()
                    {
                        Id = address.Id,
                        StreetAddress = (address.AddressLine1 + " " + address.AddressLine2).Trim(),
                        City = address.City ?? string.Empty,
                        PostalCode = address.PostalCode ?? string.Empty,
                        CountryId = address.CountryId,
                        StateId = address.StateId,
                        Region1Id = address.Region1Id,
                        Region2Id = address.Region2Id,
                        Region3Id = address.Region3Id,
                        Region4Id = address.Region4Id
                    });
                }
            }

            if (UnitOfWork.GetReference(entity, p => p.ConstituentType)?.Category == ConstituentCategory.Organization)
            {
                document.DoingBusinessAs = UnitOfWork.GetReference(entity, p => p.DoingBusinessAs).Select(p => p.Name).ToList();
            }
            else
            {
                document.DoingBusinessAs = null;
            } 

            return document;
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
