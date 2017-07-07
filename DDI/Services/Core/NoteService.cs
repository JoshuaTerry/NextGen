using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Helpers;
using DDI.Search;
using DDI.Search.Models;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Extensions;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;

namespace DDI.Services
{
    public class NoteService : ServiceBase<Note>, INoteService
    {
        #region Private Fields

        private IRepository<Note> _repository;

        #endregion Private Fields

        #region Public Constructors

        public NoteService(IUnitOfWork uow, IRepository<Note> repository) : base(uow)
        {
            _repository = repository;
        }

        #endregion Public Constructors

        #region Public Methods
        public override IDataResponse<Note> Add(Note note)
        {
           
            var ntList = new List<NoteTopic>();
            foreach(NoteTopic topic in note.NoteTopics)
            {
                var newTopic = UnitOfWork.GetById<NoteTopic>(topic.Id);
                ntList.Add(newTopic);
            }

            note.NoteTopics.Clear();
            ntList.ForEach(n => note.NoteTopics.Add(n));

            SetEntityType(note);

            return base.Add(note);
        }

        public IDataResponse<List<Note>> GetAll(string parentEntityType)
        {
            var result = UnitOfWork.GetRepository<Note>().GetEntities().Where(n => n.EntityType == parentEntityType).ToList();
            return new DataResponse<List<Note>>(result);
        }

        public override IDataResponse<List<ICanTransmogrify>> GetAll(string fields, IPageable search)
        {
            var criteria = (NoteSearch)search;

            IDocumentSearchResult<NoteDocument> results = PerformElasticSearch(criteria);

            // If the field list can be satisfied via the search documents, return them.
            if (VerifyFieldList<NoteDocument>(fields))
            {
                return new DataResponse<List<ICanTransmogrify>>(results.Documents.ToList<ICanTransmogrify>()) { TotalResults = results.TotalCount };
            }

            // Otherwise, convert the search documents to notes via a join.
            List<Guid> ids = results.Documents.Select(p => p.Id).ToList(); // This is the list of Ids to retrieve.  See the Where method below that filters notes via this list.
            List<ICanTransmogrify> notesFound = ids.Join(UnitOfWork.GetEntities(IncludesForList).Where(p => ids.Contains(p.Id)),
                                                                outer => outer,
                                                                inner => inner.Id,
                                                                (id, note) => note)
                                                          .ToList<ICanTransmogrify>();

            return new DataResponse<List<ICanTransmogrify>>(notesFound) { TotalResults = results.TotalCount };
        }

        private IDocumentSearchResult<NoteDocument> PerformElasticSearch(NoteSearch search)
        {

            var repo = new ElasticRepository<NoteDocument>();
            var query = repo.CreateQuery();

            // Build the ElasticSearch query based on note search criteria.

            if (search.CategoryId.HasValue)
            {
                query.Must.Equal(search.CategoryId.Value.ToString("d"), p => p.CategoryId);
            }

            if (search.ContactMethodId.HasValue)
            {
                query.Must.Equal(search.ContactMethodId.Value.ToString("d"), p => p.ContactMethodId);
            }

            if (search.NoteCodeId.HasValue)
            {
                query.Must.Equal(search.NoteCodeId.Value.ToString("d"), p => p.NoteCodeId);
            }

            if (search.ParentEntityId.HasValue)
            {
                query.Must.Equal(search.ParentEntityId.Value.ToString("d"), p => p.ParentEntityId);
            }

            if (search.PrimaryContactId.HasValue)
            {
                query.Must.Equal(search.PrimaryContactId.Value.ToString("d"), p => p.PrimaryContactId);
            }

            if (search.UserResponsibleId.HasValue)
            {
                query.Must.Equal(search.UserResponsibleId.Value.ToString("d"), p => p.UserResponsibleId);
            }

            if (search.AlertDate.HasValue)
            {
                query.Must.Range(DateTime.MinValue, search.AlertDate.Value, p => p.AlertStartDate);
                query.Must.Range(search.AlertDate.Value, DateTime.MaxValue, p => p.AlertEndDate);
            }

            if (search.ContactDateFrom.HasValue || search.ContactDateTo.HasValue)
            {
                query.Must.Range(search.ContactDateFrom ?? DateTime.MinValue, search.ContactDateTo ?? DateTime.MaxValue, p => p.ContactDate);
            }

            if (!string.IsNullOrWhiteSpace(search.EntityType))
            {
                query.Must.Equal(search.EntityType, p => p.EntityType);
            }

            if (!string.IsNullOrWhiteSpace(search.Title))
            {
                query.Must.Match(search.Title, p => p.Title);
            }

            if (!string.IsNullOrWhiteSpace(search.Text))
            {
                query.Must.Match(search.Text, p => p.Text);
            }

            return repo.DocumentSearch(query, search.Limit ?? 0, search.Offset ?? 0);
        }

        public IDataResponse<Note> AddTopicsToNote(Guid noteId, JObject topicIds)
        {
            var noteToUpdate = UnitOfWork.GetById<Note>(noteId, nt => nt.NoteTopics);
            IDataResponse<Note> response = null;
            List<NoteTopic> passtedNoteTopics = new List<NoteTopic>();
            List<NoteTopic> noteTopics = new List<NoteTopic>();

            foreach (var pair in topicIds)
            {
                if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    passtedNoteTopics.AddRange(from jToken in (JArray)pair.Value select Guid.Parse(jToken.ToString()) into id select UnitOfWork.GetById<NoteTopic>(id));
                }
            }

            noteTopics = noteToUpdate.NoteTopics.ToList();
            var removes = noteTopics.Except(passtedNoteTopics);
            var adds = passtedNoteTopics.Except(noteTopics);

            if (noteToUpdate != null)
            {
                removes.ForEach(nt => noteToUpdate.NoteTopics.Remove(nt));
                adds.ForEach(na => noteToUpdate.NoteTopics.Add(na));
            }
            
            UnitOfWork.SaveChanges();

            response = new DataResponse<Note>()
            {
                Data = noteToUpdate,
                IsSuccessful = true
            };
            return response;
        }

        public IDataResponse RemoveTopicFromNote(Guid noteId, Guid topicId)
        {
            var noteToUpdate = UnitOfWork.GetById<Note>(noteId, nt => nt.NoteTopics);

            IDataResponse response = null;
            var topicToRemove = noteToUpdate.NoteTopics.Where(nt => nt.Id == topicId).FirstOrDefault();

            if (topicToRemove != null)
            {
                noteToUpdate.NoteTopics.Remove(topicToRemove);
            }

            UnitOfWork.SaveChanges();

            response = new DataResponse<Note>
            {
                Data = noteToUpdate,
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse<List<Note>> GetNotesInAlertDateRange(Guid parentid)
        {
            IDataResponse<List<Note>> response = null;
            DateTime currentDay = DateTime.Today.Date;
            var entityNotesInRange = UnitOfWork.GetRepository<Note>().GetEntities().Where(n => n.ParentEntityId == parentid && 
                (n.AlertStartDate != null || n.AlertEndDate != null) && 
                (n.AlertStartDate == null || n.AlertStartDate <= currentDay) && 
                (n.AlertEndDate == null || n.AlertEndDate >= currentDay)).ToList();
            
            response = new DataResponse<List<Note>>
            {
                Data = entityNotesInRange,
                IsSuccessful = true
            };

            return response;

        }
        #endregion Public Methods

        #region Private Methods

        private void SetEntityType(Note note)
        {
            note.EntityType = LinkedEntityHelper.GetParentEntity(note, UnitOfWork).ToString();
        }

        #endregion Private Methods
    }
}
