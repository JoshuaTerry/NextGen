﻿using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.IO;
using DDI.Shared;

namespace DDI.WebApi.Controllers.Core
{
    public class ImportController : GenericController<ImportFile>
    {
        public ImportController(IService<ImportFile> service) : base(service) { }

        [HttpPost]
        [Route("api/v1/import")] 
        public IHttpActionResult Post([FromBody] ImportFile item)
        {
            var serviceResult = Service.Add(item);
             
            Stream stream = new MemoryStream(serviceResult.Data.File.Data);
            string[] columns;
            using (var streamReader = new StreamReader(stream))
            {
                columns = streamReader.ReadLine().Split(',');                
            }

            if (!item.ContainsHeaders)
            {
                int count = columns.Length;
                for (int x = 1; x < count; x++)
                    columns[x - 1] = $"Column {x}";
            }

            var response = new DataResponse<object>();
            var result = new { FileId = serviceResult.Data.Id, Columns = columns };
            response.Data = result;

            return Ok(response);
        } 

    }
}