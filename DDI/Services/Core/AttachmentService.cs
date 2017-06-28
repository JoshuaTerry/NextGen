using DDI.Shared.Models.Client.Core;
using DDI.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace DDI.Services.GL
{
    public class AttachmentService : ServiceBase<Attachment>, IAttachmentService
    {
        public AttachmentService(IUnitOfWork uow) : base(uow)
        {
          
        }
        public IDataResponse<List<Object>> GetAttachmentsForNoteId(Guid noteId)
        {
            var attachments = UnitOfWork.GetEntities<Attachment>(p => p.File)
                 .Where(a => a.NoteId == noteId)
                 .Select(f => new
                 {
                     Id = f.Id,
                     Title = f.Title,
                     CreatedBy = f.CreatedBy,
                     CreatedOn = f.CreatedOn,
                     FileId = f.FileId,
                     NoteId = f.NoteId,
                     File = new
                     {
                         Id = f.File.Id,
                         Name = f.File.Name,
                         Extension = f.File.Extension,
                         Size = f.File.Size
                     }
                 }).ToList<Object>();


            return new DataResponse<List<Object>>(attachments);
        }

        public IDataResponse<List<Object>> GetAttachmentsForEntityId(Guid entityId)
        {
            var attachments = UnitOfWork.GetEntities<Attachment>(p => p.File)
                .Where(a => a.ParentEntityId == entityId)
                .Select(f => new             
                {
                    Id = f.Id,
                    Title = f.Title,
                    CreatedBy = f.CreatedBy,
                    CreatedOn = f.CreatedOn,
                    FileId = f.FileId,
                    NoteId = f.NoteId,
                    File = new
                    {
                        Id = f.File.Id,
                        Name = f.File.Name,
                        Extension = f.File.Extension,
                        Size = f.File.Size
                    }
                }).ToList<Object>();
                      

            return new DataResponse<List<Object>>(attachments);
        }

        public  IDataResponse Delete(Guid attachmentId)
        {
            var attachment = UnitOfWork.GetById<Attachment>(attachmentId, a=> a.File);
                      
            return base.Delete(attachment);
        }
    }
}
