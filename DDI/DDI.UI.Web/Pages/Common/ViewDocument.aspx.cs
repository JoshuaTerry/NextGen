using System;
using System.IO;
using System.Configuration;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;


namespace DDI.UI.Web.Pages.Common
{
    public partial class ViewDocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Token token = Token.GetToken();

            //api/v1/roles/user/{username}
            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings.Get("ApiUrl") + "filestorage/" + Request.QueryString["Id"]);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = 0;
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + token.Access_Token);

            using (WebResponse response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    string responseData = reader.ReadToEnd();

                    JObject jObject = JObject.Parse(responseData);
                    JToken jFile = jObject["Data"];
                   
                    var document = jFile.ToObject<FileStorage>();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-length", document.Data.Length.ToString());
                    Response.AppendHeader("Content-Disposition", "attachment;filename='" + document.DisplayName + "'");
                    Response.ContentType = ReturnExtension("."+ document.Extension);
                    Response.BinaryWrite(document.Data);
                    Response.End();
                }
            }
        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".html":
                    return "text/HTML";
                case ".asc":
                case ".csv":
                case ".dat":
                case ".df":
                case ".txt":
                    return "text/plain";
                case ".doc":
                case ".docm":
                case ".docx":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".slk":
                case ".xls":
                case ".xlsb":
                case ".xlsm":
                case ".xlsx":
                    return "application/x-msexcel";
                case ".gif":
                    return "image/gif";
                case ".jpe":
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".mp3":
                    return "audio/mpeg3";
                case ".htm":
                    return "application/rtf";
                case ".rtf":
                    return "text/rtf";
                case ".css":
                    return "text/css";
                case ".pdf":
                    return "application/pdf";
                case ".msg":
                    return "application/vnd.ms-outlook";
                case ".ppam":
                case ".pps":
                case ".ppsm":
                case ".ppsx":
                case ".ppt":
                case ".pptm":
                case ".pptx":
                case ".ppz":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".ps":
                    return "application/postscript";
                case ".wp":
                case ".wp5":
                case ".wp6":
                case ".wpd":
                    return "application/workdperfect";
                case ".png":
                    return "image/png";
                case ".mht":
                    return "message/rfc822";
                case ".rtx":
                    return "text/richtext";
                case ".xml":
                case ".xsd":
                case ".xsl":
                    return "text/xml";
                default:
                    return null;
            }
        }
    }
}