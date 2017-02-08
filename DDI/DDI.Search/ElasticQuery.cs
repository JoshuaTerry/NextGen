using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;
using Nest;

namespace DDI.Search
{
    /// <summary>
    /// A strongly-typed Elasticsearch query for a single document type.
    /// </summary>
    /// <typeparam name="T">Search document type</typeparam>
    public class ElasticQuery<T> where T : class, ISearchDocument
    {
        private BoolQuery _boolQuery;
        private List<QueryContainer> _must, _should, _mustNot;

        public ElasticQuery()
        {
            _boolQuery = new BoolQuery();
            _must = new List<QueryContainer>();
            _should = new List<QueryContainer>();
            _mustNot = new List<QueryContainer>();

        }

        /// <summary>
        /// Add a "Must" full-text query term.
        /// </summary>
        /// <param name="predicate">Path to the property being queried</param>
        /// <param name="value">String value</param>
        public ElasticQuery<T> MustMatch(Expression<Func<T, object>> predicate, string value)
        {
            _must.Add(new QueryContainerDescriptor<T>().Match(m => m.Field(predicate).Query(value)));
            return this;
        }

        /// <summary>
        /// Add a "Must" exact-match query term.
        /// </summary>
        /// <param name="predicate">Path to the property being queried</param>
        /// <param name="value">Object value</param>
        public ElasticQuery<T> MustEqual(Expression<Func<T, object>> predicate, object value)
        {
            _must.Add(new QueryContainerDescriptor<T>().Term(m => m.Field(predicate).Value(value)));
            //new QueryContainerDescriptor<T>().ConstantScore(cs => cs.Filter(f => f.Term(m => m.Field(predicate).Value(value)))));
            return this;
        }

        /// <summary>
        /// Add a "Must" query term for a delimited list of codes or tags.
        /// </summary>
        /// <param name="predicate">Path to the property being queried</param>
        /// <param name="list">Delimited list of strings</param>
        public ElasticQuery<T> MustBeInList(Expression<Func<T, object>> predicate, string list)
        {
            list = ConvertListToQueryString(list);
            if (!string.IsNullOrWhiteSpace(list))
            {
                _must.Add(new QueryContainerDescriptor<T>().QueryString(q => q.Fields(f => f.Field(predicate)).Query(list)));
            }
            return this;
        }

        /// <summary>
        /// Add a "Must Not" full-text query term.  (Matching documents are always excluded)
        /// </summary>
        /// <param name="predicate">Path to the property being queried</param>
        /// <param name="value">String value</param>
        public ElasticQuery<T> MustNotMatch(Expression<Func<T, object>> predicate, string value)
        {
            _mustNot.Add(new QueryContainerDescriptor<T>().Match(m => m.Field(predicate).Query(value)));
            return this;
        }

        /// <summary>
        /// Add a "Must Not" exact-match query term.  (Matching documents are always excluded)
        /// </summary>
        /// <param name="predicate">Path to the property being queried</param>
        /// <param name="value">Object value</param>
        public ElasticQuery<T> MustNotEqual(Expression<Func<T, object>> predicate, object value)
        {
            _mustNot.Add(new QueryContainerDescriptor<T>().Term(m => m.Field(predicate).Value(value)));
            return this;
        }


        /// <summary>
        /// Add a "Must Not" query term for a delimited list of codes or tags.  (Matching documents are always excluded)
        /// </summary>
        /// <param name="predicate">Path to the property being queried</param>
        /// <param name="list">Delimited list of strings</param>
        public ElasticQuery<T> MustNotBeInList(Expression<Func<T, object>> predicate, string list)
        {
            list = ConvertListToQueryString(list);
            if (!string.IsNullOrWhiteSpace(list))
            {
                _mustNot.Add(new QueryContainerDescriptor<T>().QueryString(q => q.Fields(f => f.Field(predicate)).Query(list)));
            }
            return this;
        }

        /// <summary>
        /// Add a "Should" full-text query term.  (Matching documents are scored higher.)
        /// </summary>
        /// <param name="predicate">Path to the property being queried</param>
        /// <param name="value">String value</param>
        public ElasticQuery<T> ShouldMatch(Expression<Func<T, object>> predicate, string value)
        {
            _should.Add(new QueryContainerDescriptor<T>().Match(m => m.Field(predicate).Query(value)));
            return this;
        }

        /// <summary>
        /// Add a "Should" exact-match query term.  (Matching documents are scored higher.)
        /// </summary>
        /// <param name="predicate">Path to the property being queried</param>
        /// <param name="value">Object value</param>
        public ElasticQuery<T> ShouldEqual(Expression<Func<T, object>> predicate, object value)
        {
            _should.Add(new QueryContainerDescriptor<T>().Term(m => m.Field(predicate).Value(value)));
            return this;
        }

        /// <summary>
        /// Build the search request
        /// </summary>
        /// <returns></returns>
        public ISearchRequest BuildSearchRequest()
        {
            _boolQuery.Must = _must;
            _boolQuery.Should = _should;
            _boolQuery.MustNot = _mustNot;            

            return new SearchDescriptor<T>().Query(q => q.Bool(b => _boolQuery));
        }

        /// <summary>
        /// Convert a list of terms like "AA,BB+CC,DD" to a Elasticsearch query string like (AA OR BB AND CC OR DD)
        /// </summary>
        /// <param name="list"></param>
        private string ConvertListToQueryString(string list)
        {
            StringBuilder sb = new StringBuilder();

            // Standardize chars used for AND and OR: convert comma and space to | and plus to &
            list = list.Replace(',', '|').Replace(' ', '|').Replace('+', '&');
            bool firstOrTerm = true;
            foreach (var orTerm in list.Split('|'))
            {
                if (string.IsNullOrWhiteSpace(orTerm))
                {
                    continue;
                }

                bool firstAndTerm = true;

                foreach (var andTerm in orTerm.Split('&'))
                {
                    string term = andTerm.Trim();
                    if (string.IsNullOrWhiteSpace(term))
                    {
                        continue;
                    }

                    if (firstAndTerm)
                    {
                        firstAndTerm = false;
                        if (!firstOrTerm)
                        {
                            sb.Append(" OR ");
                        }
                        firstOrTerm = false;
                    }
                    else
                    {
                        sb.Append(" AND ");
                    }
                    // Escape terms that look like reserved words AND, OR by prefixing with a backslash.
                    if (string.Compare(term, "AND", true) == 0 || string.Compare(term, "OR", true) == 0)
                    {
                        sb.Append('\\');
                    }

                    sb.Append(term);
                }
            }
            if (sb.Length > 0)
            {
                // Wrap entire string in parentheses.
                sb.Insert(0, '(');
                sb.Append(')');
            }
            return sb.ToString();
        }

    }
}
