namespace DDI.Shared.Statics.GL
{
    public static class UserMessagesGL
{
    public static string NameIsRequired => "The Business Unit Name is required";
    public static string CodeIsRequired => "The Business Unit Code is required.";
    public static string CodeMaxLengthError => "The Business Unit Code is greater than 8 characters.";
    public static string CodeAlphaNumericRequired => "The Business Unit Code can only contain Letters and Numbers";
    public static string CodeIsNotUnique => "The Business Unit Code must be unique.";
    public static string BusinessUnitTypeNotEditable => "The Business Unit Type is not editable.";
    public static string DuplicateGroupName => "Group names must be unique.";
    public static string BlankGroupName => "Group names are required for each level.";
    public static string DuplicateBudgetName => "Budget names must be unique.";
    public static string BlankBudgetName => "Budget names are required.";

}
}
