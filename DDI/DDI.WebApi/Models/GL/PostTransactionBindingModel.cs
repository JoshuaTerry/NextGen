using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDI.Shared.Models.Client.GL;

namespace DDI.WebApi.Models.GL
{
    public class PostTransactionBindingModel
    {
         public int TotalCount { get; set; }
         public IEnumerable<PostedTransaction> PostedTransactions { get; set; } 
         public int GroupCount { get; set; }
    }
}