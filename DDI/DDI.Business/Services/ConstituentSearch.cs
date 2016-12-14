using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.Business.Services
{
    public class ConstituentSearch : IPageable
    {
        public string QuickSearch { get; set; }
        public string Name { get; set; }
        public int? ConstituentNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string OrderBy { get; set; }

        public string ToQueryString()
        {
            string baseUrl = @"api/v1/constituents?";
            string querystring = string.Empty;

            querystring += (!string.IsNullOrWhiteSpace(QuickSearch)) ? $"&quickSearch={QuickSearch}" : string.Empty;
            querystring += (!string.IsNullOrWhiteSpace(Name)) ? $"&name={Name}" : string.Empty;
            querystring += (ConstituentNumber != null) ? $"&constituentNumber={ConstituentNumber}" : string.Empty;
            querystring += (!string.IsNullOrWhiteSpace(Address)) ? $"&address={Address}" : string.Empty;
            querystring += (!string.IsNullOrWhiteSpace(City)) ? $"&city={City}" : string.Empty;
            querystring += (!string.IsNullOrWhiteSpace(State)) ? $"&state={State}" : string.Empty;
            querystring += (!string.IsNullOrWhiteSpace(Zip)) ? $"&zip={Zip}" : string.Empty;

            querystring = baseUrl + querystring.TrimStart('&');
            
            return querystring;
        }
    }
}