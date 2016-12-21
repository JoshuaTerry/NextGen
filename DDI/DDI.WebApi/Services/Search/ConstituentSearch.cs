using System;
using System.Reflection;
using DDI.WebApi.Extensions;

namespace DDI.WebApi.Services.Search
{
    public class ConstituentSearch : PageableSearch
    {
        public string QuickSearch { get; set; }
        public Guid? ConstituentTypeId { get; set; }
        public string AlternateId { get; set; }
        public string Name { get; set; }
        public int? ConstituentNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipFrom { get; set; }
        public string ZipTo { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }

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

            querystring = baseUrl + querystring.TrimStart('&');
            
            return querystring;
        }

        public string GenericToQueryString<T>(T searchObject)
        {
            string queryString = string.Empty;
            var classType = typeof (T);

            PropertyInfo[] properties = classType.GetProperties();
            foreach (var property in properties)
            {
                switch (property.Name)
                {
                    case "Limit":
                    case "OrderBy":
                    case "Offset":
                        continue;
                }

                var camelName = property.Name.ConvertProperCaseToCamelCase();
                var searchValue = searchObject.GetType().GetProperty(property.Name).GetValue(searchObject);
                if (searchValue != null)
                {
                    queryString += $"&{camelName}={searchValue}";
                }
            }

            queryString = queryString.TrimStart('&');
            return queryString;
        }
    }
}