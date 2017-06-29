using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("FileStorage")]
    public class FileStorage : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }        

        [MaxLength(8)]
        public string Extension { get; set; }

        public long Size { get; set; }

        public byte[] Data { get; set; }

        [MaxLength(64)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [NotMapped]
        public override string DisplayName => $"{Name}.{Extension}";

        [NotMapped]
        public string FileType { get
            {
                return ReturnFileType(Extension);
            }
        }

        public string ReturnFileType(string fileExtension)
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
