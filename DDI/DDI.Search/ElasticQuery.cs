using System;
using System.Collections.Generic;
using System.IO;
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
        #region Private Fields

        private BoolQuery _boolQuery;
        private List<QueryContainer> _must, _should, _mustNot;
        private Dictionary<ElasticQuery<T>, NestedQueryDescriptor<T>> _nestedQueryDict;
        private bool _isFinalized = false;
        private List<SortField> _sortFields;

        #endregion

        #region Constructors

        public ElasticQuery()
        {
            _boolQuery = new BoolQuery();
            _must = new List<QueryContainer>();
            _should = new List<QueryContainer>();
            _mustNot = new List<QueryContainer>();
            _nestedQueryDict = new Dictionary<ElasticQuery<T>, NestedQueryDescriptor<T>>();
            _sortFields = new List<SortField>();
        }

        #endregion

        #region Public Properties

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

        /// <summary>
        /// Add a "Must Not" query term that if satisified, causes a document to be excluded.
        /// </summary>
        public BoolQueryTerm MustNot
        {
            get
            {
                return new BoolQueryTerm(this, BoolQueryTermType.MustNot);
            }
        }

        /// <summary>
        /// Add a "Should" query term that if satisfied, causes a document's score to be boosted.
        /// </summary>
        public BoolQueryTerm Should
        {
            get
            {
                return new BoolQueryTerm(this, BoolQueryTermType.Should);
            }
        }

        /// <summary>
        /// Add a sort field to the query with ascending sort order.
        /// </summary>
        public void OrderBy(Expression<Func<T,object>> predicate)
        {
            _sortFields.Add(new SortField() { Field = new Field(predicate), Order = SortOrder.Ascending });
        }

        public void OrderByScore()
        {
            _sortFields.Add(new SortField() { Field = "_score", Order = SortOrder.Descending });
        }

        /// <summary>
        /// Add a sort field to the query with descending sort order.
        /// </summary>
        public void OrderByDescending(Expression<Func<T, object>> predicate)
        {
            _sortFields.Add(new SortField() { Field = new Field(predicate), Order = SortOrder.Descending });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Build the search request
        /// </summary>
        /// <returns></returns>
        public ISearchRequest BuildSearchRequest()
        {

            FinalizeQuery();           

            return new SearchDescriptor<T>().Query(q => q.Bool(b => _boolQuery)).Index(IndexHelper.GetIndexName<T>()).Sort(p => ApplySorting(p));
        }

        public QueryContainer GetQuery()
        {
            FinalizeQuery();

            return new QueryContainer(_boolQuery);
            //return _boolQuery;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Finalize the query by populating the Bool query's Must, MustNot and Should properties and recursively processing nested queries.
        /// </summary>
        private void FinalizeQuery()
        {
            if (!_isFinalized)
            {
                foreach (var kvp in _nestedQueryDict)
                {
                    kvp.Value.Query(p => kvp.Key.GetQuery());
                }

                _boolQuery.Must = _must;
                _boolQuery.Should = _should;
                _boolQuery.MustNot = _mustNot;
            }
            
            _isFinalized = true;
        }

        /// <summary>
        /// Apply all sorting entries to a SortDescriptor.
        /// </summary>
        /// <param name="sortDescriptor"></param>
        private SortDescriptor<T> ApplySorting(SortDescriptor<T> sortDescriptor)
        {
            foreach (var entry in _sortFields)
            {
                sortDescriptor = sortDescriptor.Field(entry.Field, entry.Order.Value);
            }
            return sortDescriptor;
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

        #endregion

        #region Enums

        public enum BoolQueryTermType
        {
            Must, MustNot, Should
        }

        #endregion

        #region Internal Classes

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

            /// <summary>
            /// Specify a boost factor for calcuting the score for this term.
            /// </summary>
            public BoolQueryTerm Boost(double boost)
            {
                _boost = boost;
                return this;
            }
                       
            /// <summary>
            /// Full text query term: Match all words
            /// </summary>
            /// <param name="value">Text to match</param>
            /// <param name="predicates">Paths to properties to match.</param>
            public ElasticQuery<T> MatchAll(string value, params Expression<Func<T, object>>[] predicates)
            {
                if (predicates.Length == 1)
                {
                    GetQueryContainer().Add(new QueryContainerDescriptor<T>().Match(m => m.Field(predicates[0]).Query(value).Lenient(true).Boost(_boost).Operator(Operator.And)));
                }
                else if (predicates.Length > 1)
                {
                    GetQueryContainer().Add(new QueryContainerDescriptor<T>().MultiMatch(m => m.Fields(predicates).Query(value).Lenient(true).Boost(_boost).Operator(Operator.And)));
                }
                return _query;
            }

            /// <summary>
            /// Full text query term: Match any words
            /// </summary>
            /// <param name="value">Text to match</param>
            /// <param name="predicates">Paths to properties to match.</param>
            public ElasticQuery<T> Match(string value, params Expression<Func<T, object>>[] predicates)
            {
                if (predicates.Length == 1)
                {
                    GetQueryContainer().Add(new QueryContainerDescriptor<T>().Match(m => m.Field(predicates[0]).Query(value).Lenient(true).Boost(_boost)));
                }
                else if (predicates.Length > 1)
                {
                    GetQueryContainer().Add(new QueryContainerDescriptor<T>().MultiMatch(m => m.Fields(predicates).Query(value).Lenient(true).Boost(_boost)));
                }
                return _query;
            }

            /// <summary>
            /// Query string term.
            /// </summary>
            /// <param name="queryString">Elasticsearch query string</param>
            /// <param name="predicate">Path to default property.</param>
            /// <returns></returns>
            public ElasticQuery<T> QueryString(string queryString, params Expression<Func<T, object>>[] predicates)
            {
                if (predicates == null || predicates.Length == 0)
                {
                    GetQueryContainer().Add(new QueryContainerDescriptor<T>().QueryString(m => m.Query(queryString).Boost(_boost).Lenient(true).DefaultOperator(Operator.And)));
                }
                else if (predicates.Length == 1)
                {
                    GetQueryContainer().Add(new QueryContainerDescriptor<T>().QueryString(m => m.DefaultField(predicates[0]).Query(queryString).Boost(_boost).Lenient(true).DefaultOperator(Operator.And)));
                }
                else
                {
                    GetQueryContainer().Add(new QueryContainerDescriptor<T>().QueryString(m => m.Query(queryString).Fields(predicates).Boost(_boost).Lenient(true).DefaultOperator(Operator.And)));
                }
                return _query;
            }

            /// <summary>
            /// Exact value query term.
            /// </summary>
            /// <param name="value">Value to match</param>
            /// <param name="predicates">Path to property.</param>
            public ElasticQuery<T> Equal(object value, Expression<Func<T, object>> predicate)
            {
                GetQueryContainer().Add(new QueryContainerDescriptor<T>().Term(m => m.Field(predicate).Value(value).Boost(_boost)));
                return _query;
            }

            /// <summary>
            /// Query term for a list of values. (Exact match).
            /// </summary>
            /// <param name="value">Values to match.  Values can be separated by | or &.</param>
            /// <param name="predicates">Path to property.</param>
            public ElasticQuery<T> BeInList(string list, Expression<Func<T, object>> predicate)
            {
                list = _query.ConvertListToQueryString(list);
                if (!string.IsNullOrWhiteSpace(list))
                {
                    GetQueryContainer().Add(new QueryContainerDescriptor<T>().QueryString(q => q.Fields(f => f.Field(predicate)).Query(list).Boost(_boost)));
                }
                return _query;
            }

            /// <summary>
            /// Prefix (starts with) query term.
            /// </summary>
            /// <param name="value">Prefix value.</param>
            /// <param name="predicates">Path to property.</param>
            public ElasticQuery<T> Prefix(string value, Expression<Func<T, object>> predicate)
            {
                GetQueryContainer().Add(new QueryContainerDescriptor<T>().Prefix(m => m.Field(predicate).Value(value).Boost(_boost)));
                return _query;
            }

            /// <summary>
            /// Range query term.
            /// </summary>
            /// <param name="lowValue">Lower end of range.</param>
            /// <param name="highValue">Upper end of range.</param>
            /// <param name="predicate">Path to property.</param>
            public ElasticQuery<T> Range(string lowValue, string highValue, Expression<Func<T, object>> predicate)
            {
                GetQueryContainer().Add(new QueryContainerDescriptor<T>().TermRange(m => m.Field(predicate).GreaterThanOrEquals(lowValue).LessThanOrEquals(highValue).Boost(_boost)));
                return _query;
            }

            /// <summary>
            /// Range query term.
            /// </summary>
            /// <param name="lowValue">Lower end of range.</param>
            /// <param name="highValue">Upper end of range.</param>
            /// <param name="predicate">Path to property.</param>
            public ElasticQuery<T> Range(double lowValue, double highValue, Expression<Func<T, object>> predicate)
            {
                GetQueryContainer().Add(new QueryContainerDescriptor<T>().Range(m => m.Field(predicate).GreaterThanOrEquals(lowValue).LessThanOrEquals(highValue).Boost(_boost)));
                return _query;
            }

            /// <summary>
            /// Range query term.
            /// </summary>
            /// <param name="lowValue">Lower end of range.</param>
            /// <param name="highValue">Upper end of range.</param>
            /// <param name="predicate">Path to property.</param>
            public ElasticQuery<T> Range(DateTime lowValue, DateTime highValue, Expression<Func<T, object>> predicate)
            {
                GetQueryContainer().Add(new QueryContainerDescriptor<T>().DateRange(m => m.Field(predicate).GreaterThanOrEquals(lowValue).LessThanOrEquals(highValue).Boost(_boost)));
                return _query;
            }

            /// <summary>
            /// Nested query term.
            /// </summary>
            /// <param name="predicate">Path to child object being queried.</param>
            /// <param name="nestedQuery">Nested ElasticQuery object</param>
            /// <returns></returns>
            public ElasticQuery<T> Nested(Expression<Func<T, object>> predicate, ElasticQuery<T> nestedQuery)
            {
                NestedQueryDescriptor<T> nested = new NestedQueryDescriptor<T>().Path(predicate);

                GetQueryContainer().Add(new QueryContainerDescriptor<T>().Nested(p => p.Path(predicate).Query(q => nestedQuery.GetQuery())));
                return _query;
            }

        }

        #endregion

    }
}
