using System;

namespace DDI.Services.Search
{
    public class AccountNumberSearch : PageableSearch
    {
        public string AccountNumber { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public string QuickSearch { get; set; }
        public string Fields { get; set; }
    }

}
