using System;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Enums;
using System.Collections.Generic;

namespace DDI.Services
{
    public interface IAttachmentService : IService<Attachment>
    {
        IDataResponse<List<Object>> GetAttachmentsForNoteId(Guid noteId);
        IDataResponse<List<Object>> GetAttachmentsForEntityId(Guid entityId);
        IDataResponse Delete(Guid attachmentId);
    }
}