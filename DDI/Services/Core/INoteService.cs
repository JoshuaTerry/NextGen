using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using Newtonsoft.Json.Linq;

namespace DDI.Services.ServiceInterfaces
{
    public interface INoteService : IService<Note>
    {
        IDataResponse<Note> AddTopicsToNote(Guid id, JObject topicIds);
        IDataResponse RemoveTopicFromNote(Guid id, Guid topicId);
        IDataResponse<List<Note>> GetNotesInAlertDateRange(Guid parentid);

    }
}
