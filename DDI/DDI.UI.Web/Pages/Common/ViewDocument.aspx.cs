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
                    Response.ContentType = document.FileType;
                    Response.BinaryWrite(document.Data);
                    Response.End();
                }
            }
        }
    }
}