using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DDI.Business.GL;
using DDI.Data;
using DDI.Data.Statics;
using DDI.Search;
using DDI.Search.Models;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums;
using DDI.Shared.Enums.Core;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Core
{
    public class NoteLogic : EntityLogicBase<Note>
    {

        public NoteLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #region Public Methods

        public override void Validate(Note note)
        {
            note.AssignPrimaryKey();
            ScheduleUpdateSearchDocument(note);
        }

        public override void UpdateSearchDocument(Note note)
        {
            var elasticRepository = new ElasticRepository<NoteDocument>();
            elasticRepository.Update((NoteDocument)BuildSearchDocument(note));
        }

        public override ISearchDocument BuildSearchDocument(Note entity)
        {
            var document = new NoteDocument()
            {
                Id = entity.Id,
                EntityType = entity.EntityType,
                ParentEntityId = entity.ParentEntityId,
                AlertStartDate = entity.AlertStartDate,
                AlertEndDate = entity.AlertEndDate,
                CategoryId = entity.CategoryId,
                ContactDate = entity.ContactDate,
                ContactMethodId = entity.ContactMethodId,
                NoteCodeId = entity.NoteCodeId,
                PrimaryContactId = entity.PrimaryContactId,
                UserResponsibleId = entity.UserResponsibleId,
                Text = entity.Text,
                Title = entity.Title
            };

            return document;
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
