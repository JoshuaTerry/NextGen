using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.ServiceInterfaces
{
    public interface INoteService : IService<Note>
    {

        IDataResponse<Note> AddTopicsToNote(Note note, JObject topicIds);
        IDataResponse RemoveTopicFromNote(Note note, Guid topicId);

    }
}
