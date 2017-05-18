using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using Newtonsoft.Json.Linq;

namespace DDI.Services.ServiceInterfaces
{
    public interface INoteService : IService<Note>
    {
        IDataResponse<Note> AddTopicsToNote(Note note, JObject topicIds);
        IDataResponse RemoveTopicFromNote(Note note, Guid topicId);
        IDataResponse<List<Note>> GetNotesInAlertDateRange(Guid parentid);

    }
}
