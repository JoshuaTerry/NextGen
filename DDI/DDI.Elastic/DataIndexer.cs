using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace DDI.Elastic
{
	public class DataIndexer
	{
		#region Private Fields

		private string _url;

		#endregion Private Fields

		#region Public Constructors

		public DataIndexer(string url)
		{
			_url = url;
		}

		#endregion Public Constructors

		public string IndexObject(object o, string id)
		{
			JObject result;
			var client = new RestClient(_url);
			
			//var request = new RestRequest($"{o.GetType().Name}/{id}", Method.PUT);
			var request = new RestRequest("crm/{index}/{id}", Method.POST);
			request.AddUrlSegment("index", o.GetType().Name.ToLower());
			request.AddUrlSegment("id", id);
			//request.AddObject(o);
			request.AddJsonBody(o);

			IRestResponse response = client.Execute(request);
			result = JObject.Parse(response.Content);

			return result.ToString();
		}

		public string GetIndex(string index)
		{
			JObject result;
			var client = new RestClient(_url);
			var request = new RestRequest(index, Method.GET);
			IRestResponse response = client.Execute(request);
			result = JObject.Parse(response.Content);
			//result = new JObject(response.Content);

			//return response.Content;
			return result.ToString();
		}
	}

	public class User
	{
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
	}
}
