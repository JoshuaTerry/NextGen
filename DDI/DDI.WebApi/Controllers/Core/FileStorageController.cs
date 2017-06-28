using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class FileStorageController : GenericController<FileStorage>
    {
        public FileStorageController(IService<FileStorage> service) : base(service) { }

        [HttpGet]
        [Route("api/v1/filestorage")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            fields = "Id,Name,Extension,Size";
            return base.GetAll(RouteNames.Note, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/filestorage/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            fields = "Id,Name,Extension,Size,Data,FileType";
            var response = Service.GetById(id);
            response.Data.FileType = ReturnExtension(response.Data.Extension);
            return FinalizeResponse(response, fields, null);
        }
        [HttpGet]
        [Route("api/v1/filestorage/fileextensions/{extension}")]
        public IHttpActionResult GetFileTypeByExtension(string extension)
        {
            string fileType = ReturnExtension(extension);
            return Ok(fileType);
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

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case "html":
                    return "text/HTML";
                case "asc":
                case "csv":
                case "dat":
                case "df":
                case "txt":
                    return "text/plain";
                case "doc":
                case "docm":
                case "docx":
                    return "application/ms-word";
                case "tiff":
                case "tif":
                    return "image/tiff";
                case "avi":
                    return "video/avi";
                case "zip":
                    return "application/zip";
                case "slk":
                case "xls":
                case "xlsb":
                case "xlsm":
                case "xlsx":
                    return "application/x-msexcel";
                case "gif":
                    return "image/gif";
                case "jpe":
                case "jpg":
                case "jpeg":
                    return "image/jpeg";
                case "bmp":
                    return "image/bmp";
                case "mp3":
                    return "audio/mpeg3";
                case "htm":
                    return "application/rtf";
                case "rtf":
                    return "text/rtf";
                case "css":
                    return "text/css";
                case "pdf":
                    return "application/pdf";
                case "msg":
                    return "application/vnd.ms-outlook";
                case "ppam":
                case "pps":
                case "ppsm":
                case "ppsx":
                case "ppt":
                case "pptm":
                case "pptx":
                case "ppz":
                    return "application/mspowerpoint";
                case "dwg":
                    return "image/vnd.dwg";
                case "ps":
                    return "application/postscript";
                case "wp":
                case "wp5":
                case "wp6":
                case "wpd":
                    return "application/workdperfect";
                case "png":
                    return "image/png";
                case "mht":
                    return "message/rfc822";
                case "rtx":
                    return "text/richtext";
                case "xml":
                case "xsd":
                case "xsl":
                    return "text/xml";
                default:
                    return null;
            }
        }
    }
}
