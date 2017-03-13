using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGrease.Css.Extensions;

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

        public IDataResponse<Note> AddTopicsToNote(Note note, JObject topicIds)
        {
            var noteToUpdate = UnitOfWork.GetById<Note>(note.Id, nt => nt.NoteTopics);
            IDataResponse<Note> response = null;
            List<NoteTopic> passtedNoteTopics = new List<NoteTopic>();
            List<NoteTopic> noteTopics = new List<NoteTopic>();

            foreach (var pair in topicIds)
            {
                // may need to double check this condition
                if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    // add the id (loop thru jtokens (id's from jarray) and parse to guid, get the notetopic from the unitofwork.
                    // may have to add includes to unit of work. Could also probably do in multiple statements. 
                    passtedNoteTopics.AddRange(from jToken in (JArray)pair.Value select Guid.Parse(jToken.ToString()) into id select UnitOfWork.GetById<NoteTopic>(id));
                }
            }

            noteTopics = noteToUpdate.NoteTopics.ToList();
            var removes = noteTopics.Except(passtedNoteTopics);
            var adds = passtedNoteTopics.Except(noteTopics);

            if (noteToUpdate != null)
            {
                removes.ForEach(nt => noteToUpdate.NoteTopics.Remove(nt));
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

        #endregion Public Methods

        #region Private Methods

        private void Initialize(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _repository = _unitOfWork.GetRepository<Note>();
        }

        #endregion Private Methods
    }
}
