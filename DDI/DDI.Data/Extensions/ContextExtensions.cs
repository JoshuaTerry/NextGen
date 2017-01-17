using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EntityFramework.MappingAPI.Extensions;


public static class ContextExtensions
{
    public static string GetTableName<T>(this DbContext context) where T : class
    {        
        return context.Db<T>().TableName;        
    }
}

