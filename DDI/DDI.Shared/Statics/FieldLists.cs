using DDI.Shared.Models;
namespace DDI.Shared.Statics
{
    public static class FieldLists
    {
        /// <summary>
        /// Fields for Code Entities (ICodeEntity): "Id,DisplayName,IsActive"
        /// </summary>
        public static string CodeFields => $"{nameof(IEntity.Id)},{nameof(IEntity.DisplayName)},{nameof(ICodeEntity.IsActive)}";

        public static string AllFields => "all";

    }
}
