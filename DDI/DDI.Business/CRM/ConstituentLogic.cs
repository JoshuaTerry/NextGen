using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Core;
using DDI.Search;
using DDI.Search.Models;
using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
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
        /// Calculate age range and birth year.
        /// </summary>
        public void CalculateAgeRange(Constituent constituent)
        {
            DateTime? dt1 = null, dt2 = null;
            DateTime baseDate = DateTime.Now;
            int baseYear = baseDate.Year;
            int baseMonth = baseDate.Month;
            int baseDay = baseDate.Day;

            constituent.BirthYear = null;
            constituent.AgeFrom = constituent.AgeTo = null;

            if (constituent.BirthDateType == BirthDateType.FullDate && constituent.BirthMonth > 0 && constituent.BirthDay > 0 && constituent.BirthYearFrom > 0)
            {
                dt1 = dt2 = DateHelper.GetNearestValidDate(constituent.BirthMonth.Value, constituent.BirthDay.Value, constituent.BirthYearFrom.Value, false);
                constituent.BirthYear = dt1.Value.Year;
            }

            else if (constituent.BirthYearFrom > 0 && constituent.BirthYearTo > 0)
            {
                int day = constituent.BirthDay > 0 ? constituent.BirthDay.Value : 1;
                int month = constituent.BirthMonth > 0 ? constituent.BirthMonth.Value : 1;
                dt1 = DateHelper.GetNearestValidDate(month, day, constituent.BirthYearFrom.Value, false);
                dt2 = DateHelper.GetNearestValidDate(month, day, constituent.BirthYearTo.Value, false);
            }

            // If birth year from & birth year to are the same, set the (nonmapped) birth year to this value.
            if (constituent.BirthYear == null && constituent.BirthYearFrom > 0 && constituent.BirthYearFrom == constituent.BirthYearTo)
            {
                constituent.BirthYear = constituent.BirthYearFrom;
            }

            // If either date is null, age range is zero.
            if (dt1 == null || dt2 == null)
            {
                return;
            }

            // Calculate the age range.
            constituent.AgeFrom = baseYear - dt2.Value.Year;
            constituent.AgeTo = baseYear - dt1.Value.Year;
            if (constituent.AgeFrom > constituent.AgeTo)
            {
                int temp = constituent.AgeFrom.Value;
                constituent.AgeFrom = constituent.AgeTo;
                constituent.AgeTo = temp;
            }

            // Adjust age based on month/day
            if (baseDate.Month < dt1.Value.Month ||
                    baseDate.Month == dt1.Value.Month && baseDate.Day < dt1.Value.Day)
            {
                constituent.AgeFrom--;
            }

            if (baseDate.Month < dt2.Value.Month ||
                baseDate.Month == dt2.Value.Month && baseDate.Day < dt2.Value.Day)
            {
                constituent.AgeTo--;
            }

            // Ensure age isn't negative.
            if (constituent.AgeFrom < 0)
            {
                constituent.AgeFrom = 0;
            }
            if (constituent.AgeTo < 0)
            {
                constituent.AgeTo = 0;
            }
        }

        /// <summary>
        /// Valudate and update birth date properties based on (nonmapped) birth year and/or age range properties if non-null.
        /// </summary>
        /// <param name="constituent"></param>
        public void UpdateBirthDate(Constituent constituent)
        {
            DateTime baseDate = DateTime.Now;
            int baseYear = baseDate.Year;
            int baseMonth = baseDate.Month;
            int baseDay = baseDate.Day;

            if (constituent.BirthMonth > 12 || constituent.BirthDay < 0)
            {
                throw new ValidationException(UserMessagesCRM.BirthDateBadMonth);
            }

            if (constituent.BirthDay > 31 || constituent.BirthDay < 0)
            {
                throw new ValidationException(UserMessagesCRM.BirthDateBadDay);
            }

            if (constituent.BirthYear < 0 || constituent.BirthYear > baseYear)
            {
                throw new ValidationException(UserMessagesCRM.BirthDateBadYear);
            }

            if (constituent.BirthYear > 0)
            {
                // Birth year set via API...
                // Reset the birth year range.
                constituent.BirthYearFrom = constituent.BirthYearTo = null;

                if (constituent.BirthDay > 0 && constituent.BirthMonth > 0)
                {
                    // Month, day, year specified.  
                    DateTime date = DateHelper.GetNearestValidDate(constituent.BirthMonth.Value, constituent.BirthDay.Value, constituent.BirthYear.Value, false);
                    constituent.BirthMonth = date.Month;
                    constituent.BirthDay = date.Day;
                    constituent.BirthYear = date.Year;
                    constituent.BirthDateType = BirthDateType.FullDate;
                }
                else
                {
                    constituent.BirthDateType = BirthDateType.AgeRange;
                }

                constituent.BirthYearFrom = constituent.BirthYearTo = constituent.BirthYear;
            }
            else
            {
                // No birth year.
                constituent.BirthYear = null;

                if (constituent.BirthDay > 0 && constituent.BirthMonth > 0)
                {
                    // Use 1980 to adjust birth month & birth day if necessary.  (2/29/1980 is a valid date.)
                    DateTime date = DateHelper.GetNearestValidDate(constituent.BirthMonth.Value, constituent.BirthDay.Value, 1980, false);
                    constituent.BirthMonth = date.Month;
                    constituent.BirthDay = date.Day;
                    constituent.BirthDateType = BirthDateType.MonthDay;
                }
                else if (constituent.AgeFrom > 0 || constituent.AgeTo > 0)
                {
                    constituent.BirthDateType = BirthDateType.AgeRange;
                }
                else
                {
                    constituent.BirthDateType = BirthDateType.None;
                }
            }

            // Age range calculation - if an age range was specified and we don't already have a birth year range.
            if ((constituent.AgeFrom > 0 || constituent.AgeTo > 0) && (constituent.BirthYearFrom == null || constituent.BirthYearTo == null || constituent.BirthYearFrom == 0 || constituent.BirthYearTo == 0))
            {
                int year1 = constituent.AgeTo > 0 ? baseYear - constituent.AgeTo.Value : 0;
                int year2 = constituent.AgeFrom > 0 ? baseYear - constituent.AgeFrom.Value : 0;

                if (year1 > year2)
                {
                    int temp = year1;
                    year1 = year2;
                    year2 = temp;
                }

                if (constituent.BirthMonth > 0)
                {
                    if (constituent.BirthMonth.Value > baseMonth || (constituent.BirthMonth.Value == baseMonth && constituent.BirthDay > 0 && constituent.BirthDay.Value > baseDay))
                    {
                        year1--;
                        year2--;
                    }
                }

                constituent.BirthYearFrom = year1;
                constituent.BirthYearTo = year2;
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
        /// Get the formatted primary address for a constituent.
        /// </summary>
        public string GetFormattedPrimaryAddress(Constituent constituent)
        {
            if (constituent != null)
            {
                ConstituentAddress constituentAddress = UnitOfWork.GetReference(constituent, p => p.ConstituentAddresses).FirstOrDefault(p => p.IsPrimary);
                if (constituentAddress != null)
                {
                    return UnitOfWork.GetBusinessLogic<AddressLogic>().FormatAddress(UnitOfWork.GetReference(constituentAddress, p => p.Address)).Replace("\n", ", ");
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Get the sort name for a constituent (e.g. Last First Middle)
        /// </summary>
        /// <param name="constituent"></param>        
        public string GetSortName(Constituent constituent)
        {
            ConstituentType type = constituent.ConstituentType ?? UnitOfWork.GetReference(constituent, p => p.ConstituentType);

            if (type?.Category == ConstituentCategory.Organization)
            {
                return constituent.Name;
            }

            return NameFormatter.FormatIndividualSortName(constituent);
        }

        private List<string> GetFormattedNameFields()
        {
            return new string[]
            {
                nameof(Constituent.FirstName),
                nameof(Constituent.MiddleName),
                nameof(Constituent.LastName),
                nameof(Constituent.Suffix),
                nameof(Constituent.Nickname),
                nameof(Constituent.Prefix),
                nameof(Constituent.NameFormat)
            }.ToList(); 
        }
        private Constituent CalculateConstituentNameProperties(Constituent constituent)
        {
            var formattedNameFields = GetFormattedNameFields();

            List<string> modifiedProperties = null;
            if (_constituentRepo.GetEntityState(constituent) != EntityState.Added)
            {
                modifiedProperties = _constituentRepo.GetModifiedProperties(constituent);
            }
            else
            {
                modifiedProperties = typeof(Constituent).GetProperties().Where(p => formattedNameFields.Contains(p.Name) && p.GetValue(constituent) != null).Select(p => p.Name).ToList();
            }
            // Update constituent formatted name and sort name only if name fields were updated.
            if (modifiedProperties?.Intersect(GetFormattedNameFields()).Count() > 0)
            {
                constituent.FormattedName = GetFormattedName(constituent);
                constituent.Name = GetSortName(constituent);
            }

            return constituent;
        }
                
        private void ValidateUniqueConstituentNumber(Constituent entity)
        {
            var existing = UnitOfWork.GetRepository<Constituent>().Entities.FirstOrDefault(c => c.ConstituentNumber == entity.ConstituentNumber);
            if (existing != null)
            {
                if (entity.Id == Guid.Empty || entity.Id != existing.Id)
                {
                    throw new ValidationException(string.Format(UserMessagesCRM.ConstituentNumberExists, entity.ConstituentNumber));
                } 
            } 
        }
        public override void Validate(Constituent constituent)
        {
            constituent.AssignPrimaryKey();
            ValidateUniqueConstituentNumber(constituent);
            UpdateBirthDate(constituent);
            constituent = CalculateConstituentNameProperties(constituent);
            ScheduleUpdateSearchDocument(constituent);
        }

        public override void UpdateSearchDocument(Constituent constituent)
        {
            var elasticRepository = new ElasticRepository<ConstituentDocument>();
            elasticRepository.Update((ConstituentDocument)BuildSearchDocument(constituent));
        }

        public int GetNextConstituentNumber()
        {            
            int nextNumber = 0;
            bool isUnique = false;
            int tries = 0;
            var logic = UnitOfWork.GetBusinessLogic<EntityNumberLogic>();

            while (!isUnique)
            {
                tries++;
                nextNumber = logic.GetNextEntityNumber(EntityNumberType.Constituent);
                isUnique = _constituentRepo.Entities.Count(p => p.ConstituentNumber == nextNumber) == 0;

                if (tries >= _maxTries)
                    throw new ValidationException(UserMessagesCRM.NextSequenceValueTooManyTries);
            }

            return nextNumber;
        }

        public void SetNextConstituentNumber(int newValue)
        {
            UnitOfWork.GetBusinessLogic<EntityNumberLogic>().SetNextEntityNumber(EntityNumberType.Constituent, newValue);
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
