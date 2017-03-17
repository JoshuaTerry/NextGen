using DDI.Business.Helpers;
using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGrease.Css.Extensions;
using DDI.Shared.Models;

namespace DDI.Services
{
    public class NoteService : ServiceBase<Note>, INoteService
    {
        #region Private Fields

        private IRepository<Note> _repository;

        private IUnitOfWork _unitOfWork;

        #endregion Private Fields

        #region Public Constructors

        public NoteService()
        {
            Initialize(new UnitOfWorkEF());
        }

        public NoteService(IUnitOfWork uow)
        {
            Initialize(uow);
        }

        #endregion Public Constructors

        #region Public Methods
        public override IDataResponse<Note> Add(Note note)
        {
            SetEntityType(note);

            return base.Add(note);
        }

        public IDataResponse<List<Note>> GetAll(string parentEntityType)
        {
            var result = UnitOfWork.GetRepository<Note>().GetEntities().Where(n => n.EntityType == parentEntityType).ToList();
            return new DataResponse<List<Note>>(result);
        }

        public IDataResponse<Note> AddTopicsToNote(Note note, JObject topicIds)
        {
            var noteToUpdate = UnitOfWork.GetById<Note>(note.Id, nt => nt.NoteTopics);
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
                Data = UnitOfWork.GetById<Note>(note.Id),
                IsSuccessful = true
            };
            return response;
        }

        public IDataResponse RemoveTopicFromNote(Note note, Guid topicId)
        {
            IDataResponse response = null;
            var topicToRemove = note.NoteTopics.Where(nt => nt.Id == topicId).FirstOrDefault();

            if (topicToRemove != null)
            {
                note.NoteTopics.Remove(topicToRemove);
            }

            UnitOfWork.SaveChanges();

            response = new DataResponse<Note>
            {
                Data = UnitOfWork.GetById<Note>(note.Id),
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse GetNotesInAlertDateRange(Guid parentid)
        {
            IDataResponse response = null;
            DateTime currentDay = DateTime.Today.Date;
            var entityNotesInRange = UnitOfWork.GetRepository<Note>().GetEntities().Where(n => n.ParentEntityId == parentid && (n.AlertStartDate <= currentDay && n.AlertEndDate >= currentDay)).ToList();
            
            response = new DataResponse<List<Note>>
            {
                Data = entityNotesInRange,
                IsSuccessful = true
            };

            return response;

        }
        #endregion Public Methods

        #region Private Methods

        private void Initialize(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _repository = _unitOfWork.GetRepository<Note>();
        }

        private void SetEntityType(Note note)
        {
            // As we expand, this will have to encompass more than just constituent
            if (string.Compare(note.EntityType, "constituent", true) == 0)
            {
                note.EntityType = LinkedEntityHelper.GetEntityTypeName<Constituent>();
            }
            else
            {
                throw new TypeAccessException("Invalid Entity Type for Note");
            }
        }

        #endregion Private Methods
    }
}
