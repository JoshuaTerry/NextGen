using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models
{
    /// <summary>
    /// Interface for Elasticsearch documents.
    /// </summary>
    public interface ISearchDocument
    {
        Guid Id { get; set; }
    }
}
