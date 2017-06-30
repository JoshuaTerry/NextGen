using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class FileStorageController : GenericController<FileStorage>
    {
        public FileStorageController(IService<FileStorage> service) : base(service) { }

        protected override string FieldsForList => FieldListBuilder
            .Include(p => p.Id)
           .Include(p => p.Name)
           .Include(p => p.Extension)
           .Include(p => p.Size);
        protected override string FieldsForSingle => FieldListBuilder
           .Include(p => p.Id)
           .Include(p => p.Name)
           .Include(p => p.Extension)
           .Include(p => p.Size)
           .Include(p => p.Data)
           .Include(p => p.FileType);

        [HttpGet]
        [Route("api/v1/filestorage")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Note, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/filestorage/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            var response = Service.GetById(id);
            return FinalizeResponse(response, fields, null);
        }

        [HttpPost]
        [Route("api/v1/filestorage")]
        public IHttpActionResult Post([FromBody] FileStorage entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPost]
        [Route("api/v1/filestorage/upload")]
        public async Task<IHttpActionResult> Upload()
        {
            try
            {
                HttpRequestMessage request = this.Request;
                if (!request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                FileStorage newFile = new FileStorage();

                if (provider.Contents.Count > 0)
                {
                    var file = provider.Contents[0];
                    var fileNameParts = file.Headers.ContentDisposition.FileName.Trim('\"').Split('.');
                    var buffer = await file.ReadAsByteArrayAsync();

                    // save the uploaded filestorage
                    newFile = new FileStorage()
                    {
                        Name = fileNameParts[0],
                        Extension = fileNameParts[1],
                        Data = buffer,
                        Size = buffer.LongLength
                    };

                    var response = base.Post(newFile);
                    return response;
                }
                else
                {
                    return InternalServerError(new Exception("No files were uploaded"));
                }
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }

        [HttpPatch]
        [Route("api/v1/filestorage/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/filestorage/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
