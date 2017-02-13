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
        private Dictionary<ElasticQuery<T>, NestedQueryDescriptor<T>> _nestedQueryDict;

        public ElasticQuery()
        {
            _boolQuery = new BoolQuery();
            _must = new List<QueryContainer>();
            _should = new List<QueryContainer>();
            _mustNot = new List<QueryContainer>();
            _nestedQueryDict = new Dictionary<ElasticQuery<T>, NestedQueryDescriptor<T>>();
        }

        /// <summary>
        /// Add a "Must" query term that must be satisfied in order for a document to be included.
        /// </summary>
        public BoolQueryTerm Must 
        {
            get
            {
                return new BoolQueryTerm(this, BoolQueryTermType.Must);
            }
        }

        public BoolQueryTerm MustNot
        {
            get
            {
                return new BoolQueryTerm(this, BoolQueryTermType.MustNot);
            }
        }

        public BoolQueryTerm Should
        {
            get
            {
                return new BoolQueryTerm(this, BoolQueryTermType.Should);
            }
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
            return this;
        }

        /// <summary>
        /// Add a "Must" exact-match query term.
        /// </summary>
        /// <param name="predicate">Path to the property being queried</param>
        /// <param name="value">Object value</param>
        public ElasticQuery<T> MustPrefix(Expression<Func<T, object>> predicate, string value)
        {
            _must.Add(new QueryContainerDescriptor<T>().Prefix(m => m.Field(predicate).Value(value)));
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

            foreach (var kvp in _nestedQueryDict)
            {
                kvp.Value.Query(p => kvp.Key.BuildQuery());
            }

            _boolQuery.Must = _must;
            _boolQuery.Should = _should;
            _boolQuery.MustNot = _mustNot;            


            return new SearchDescriptor<T>().Query(q => q.Bool(b => _boolQuery)).Index(IndexHelper.GetIndexName<T>());
        }

        public BoolQuery BuildQuery()
        {
            foreach (var kvp in _nestedQueryDict)
            {
                kvp.Value.Query(p => kvp.Key.BuildQuery());
            }

            _boolQuery.Must = _must;
            _boolQuery.Should = _should;
            _boolQuery.MustNot = _mustNot;

            return _boolQuery;
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

        public enum  BoolQueryTermType
        {
            Must, MustNot, Should
        }

        public class BoolQueryTerm
        {
            private ElasticQuery<T> _query;
            private BoolQueryTermType _type;
            private double? _boost;

            public BoolQueryTerm(ElasticQuery<T> query, BoolQueryTermType type)
            {
                _query = query;
                _type = type;
                _boost = null;
            }

            private IList<QueryContainer> GetQueryContainer()
            {
                switch(_type)
                {
                    case BoolQueryTermType.Must: return _query._must;
                    case BoolQueryTermType.MustNot: return _query._mustNot;
                    case BoolQueryTermType.Should: return _query._should;
                    default: return _query._must;
                }
            }

            public BoolQueryTerm Boost(int boost)
            {
                _boost = boost;
                return this;
            }

            public ElasticQuery<T> Match(Expression<Func<T, object>> predicate, string value)
            {
                GetQueryContainer().Add(new QueryContainerDescriptor<T>().Match(m => m.Field(predicate).Query(value).Boost(_boost)));
                return _query;
            }

            public ElasticQuery<T> Equal(Expression<Func<T, object>> predicate, object value)
            {
                GetQueryContainer().Add(new QueryContainerDescriptor<T>().Term(m => m.Field(predicate).Value(value).Boost(_boost)));
                return _query;
            }

            public ElasticQuery<T> BeInList(Expression<Func<T, object>> predicate, string list)
            {
                list = _query.ConvertListToQueryString(list);
                if (!string.IsNullOrWhiteSpace(list))
                {
                    GetQueryContainer().Add(new QueryContainerDescriptor<T>().QueryString(q => q.Fields(f => f.Field(predicate)).Query(list).Boost(_boost)));
                }
                return _query;
            }

            public ElasticQuery<T> Prefix(Expression<Func<T, object>> predicate, string value)
            {
                GetQueryContainer().Add(new QueryContainerDescriptor<T>().Prefix(m => m.Field(predicate).Value(value).Boost(_boost)));
                return _query;
            }

            public ElasticQuery<T> Nested(Expression<Func<T, object>> predicate, ElasticQuery<T> nestedQuery)
            {
                NestedQueryDescriptor<T> nested = new NestedQueryDescriptor<T>().Path(predicate);

                GetQueryContainer().Add(new QueryContainerDescriptor<T>().Nested(p => p.Path(predicate).Query(q => nestedQuery.BuildQuery())));
                return _query;
            }

        }

    }
}
