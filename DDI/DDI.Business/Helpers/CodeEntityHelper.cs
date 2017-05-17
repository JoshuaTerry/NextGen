using DDI.Shared;
using DDI.Shared.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace DDI.Business.Helpers
{
    /// <summary>
    /// Static helper class for ICodeEntity (Entities that have Code and Name properties).
    /// </summary>
    public static class CodeEntityHelper 
    {
        #region Private Fields
        #endregion

        #region Constructors 

        #endregion

        #region Public Methods
        /// <summary>
        /// Convert a code into a Guid.  If the code is already a Guid string, it's Guid value is returned.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="code">Code to convert</param>
        /// <param name="predicate">"Where" predicate</param>        
        public static Guid? ConvertToGuid<T>(IUnitOfWork unitOfWork, string code, Expression<Func<T, bool>> predicate = null) where T : class, ICodeEntity, IEntity
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            Guid id;
            if (Guid.TryParse(code, out id))
            {
                return id;
            }

            var query = unitOfWork.GetEntities<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query.FirstOrDefault(p => p.Code == code)?.Id;
        }

        /// <summary>
        /// Convert a delimited list of codes into a delimited list of Guid strings.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="text">Text to convert</param>
        /// <param name="predicate">"Where" predicate</param>        
        public static string ConvertCodeListToGuidList<T>(IUnitOfWork unitOfWork, string text, Expression<Func<T, bool>> predicate = null) where T : class, ICodeEntity, IEntity
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            StringBuilder result = new StringBuilder();
            foreach (var token in Regex.Split(text, "([ ,+&|])"))
            {
                if (string.IsNullOrEmpty(token))
                {
                    continue;
                }
                else if (token == " " || token == "," || token == "+" || token == "&" || token == "|")
                {
                    result.Append(token);
                }
                else
                {
                    Guid? id = ConvertToGuid<T>(unitOfWork, token, predicate);
                    if (id != null)
                    {
                        result.Append(id.ToString());
                    }
                }
            }
            return result.ToString();
        }

        #endregion

        #region Private Methods

        #endregion  

    }
}
