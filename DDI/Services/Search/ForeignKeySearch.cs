using System;

namespace DDI.Services.Search
{
    public class ForeignKeySearch : PageableSearch
    {
        public Guid? Id { get; set; }
    }
}
