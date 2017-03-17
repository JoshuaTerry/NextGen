using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;
namespace DDI.Shared.Statics
{
    public static class FieldLists
    {
        /// <summary>
        /// Fields for Code Entities (ICodeEntity): "Id,DisplayName,IsActive"
        /// </summary>
        public static string CodeFields => $"{nameof(IEntity.Id)},{nameof(IEntity.DisplayName)},{nameof(ICodeEntity.IsActive)}";

    }
}
