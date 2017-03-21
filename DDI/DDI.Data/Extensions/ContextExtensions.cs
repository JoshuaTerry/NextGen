using EntityFramework.MappingAPI.Extensions;
using System.Data.Entity;


public static class ContextExtensions
{
    public static string GetTableName<T>(this DbContext context) where T : class
    {        
        return context.Db<T>().TableName;        
    }
}

