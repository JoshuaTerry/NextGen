
using DDI.Shared;
using DDI.Shared.Models.Client.Core;

namespace DDI.Business.Core
{
    public class FileStorageLogic : EntityLogicBase<FileStorage>
    {
        public FileStorageLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #region Public Methods

        public override void Validate(FileStorage item)
        {
            item.AssignPrimaryKey();
            item.FileType = GenerateFileType(item.Extension);
            ScheduleUpdateSearchDocument(item);
        }

        #endregion

        private string GenerateFileType(string fileExtension)
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
                    return string.Empty;
            }
        }
    }
}