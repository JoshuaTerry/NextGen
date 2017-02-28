﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.EFAudit.Filter
{
    public class UnknownTypeException : Exception
    {
        public string TypeName;
        public IEnumerable<Exception> TypeLoadExceptions;

        public UnknownTypeException(string typeName, IEnumerable<Exception> typeLoadExceptions)
            : base(message(typeName, typeLoadExceptions))
        {
            TypeName = typeName;
            TypeLoadExceptions = typeLoadExceptions;
        }

        private static string message(string typeName, IEnumerable<Exception> typeLoadExceptions)
        {
            var message = string.Format("An error occurred trying to log a property belonging to an object of type '{0}'.", typeName);

            if (typeLoadExceptions.Any())
            {
                message += string.Format("The following {0} exceptions occurred when scanning for types:\n{1}", typeLoadExceptions.Count(), string.Join("\n", typeLoadExceptions));
            }

            return message;
        }
    }
}
