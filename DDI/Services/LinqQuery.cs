using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using DDI.Shared;

namespace DDI.Services
{
    public class LinqQuery<TEntity>
    {
        #region Private Fields

        private bool _hasPredicate = false;
        private ExpressionStarter<TEntity> _predicate = PredicateBuilder.New<TEntity>();

        private List<string> _orderBy = new List<string>();

        public List<string> OrderBy
        {
            get { return _orderBy; }
            set { _orderBy = value; }
        }

        private IQueryable<TEntity> _query;

        #endregion Private Fields

        #region Public Properties

        public int Offset { get; set; }
        public int Limit { get; set; }

        public ExpressionStarter<TEntity> Predicate
        {
            get { return _predicate; }
        }

        #endregion Public Properties

        #region Public Constructors

        public LinqQuery(IQueryable<TEntity> query)
        {
            _query = query;
        }

        #endregion Public Constructors

        #region Public Methods

        public IQueryable<TEntity> GetQueryable()
        {
            var query = _query?.AsExpandable();
            if (_hasPredicate)
            {
                query = query.Where(_predicate);
            }
            query = AddSorting(query);
            query = AddPaging(query);

            return query;
        }

        public LinqQuery<TEntity> Or(Expression<Func<TEntity, bool>> expression)
        {
            if (expression != null)
            {
                _hasPredicate = true;
                _predicate = _predicate.Or(expression);
            }

            return this;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void PredicateAnd(Expression<Func<TEntity, bool>> expression)
        {
            if (expression != null)
            {
                _hasPredicate = true;
                _predicate = _predicate.And(expression);
            }
        }

        #endregion Protected Methods

        private IQueryable<TEntity> AddPaging(IQueryable<TEntity> query)
        {
            if (Offset > 0)
            {
                query = query.Skip(Offset * Limit);
            }
            if (Limit > 0)
            {
                query = query.Take(Limit);
            }
            return query;
        }

        private IQueryable<TEntity> AddSorting(IQueryable<TEntity> query)
        {
            if (_orderBy?.Count > 0)
            {
                int orderNumber = 0;
                foreach (string orderByColumn in _orderBy)
                {
                    orderNumber++;
                    string propertyName = orderByColumn;
                    bool descending = false;
                    if (propertyName.StartsWith("-"))
                    {
                        descending = true;
                        propertyName = propertyName.TrimStart('-');
                    }
                    query = query.DynamicOrderBy(propertyName, descending, orderNumber==1);
                }
            }
            else if (Offset > 0 || Limit > 0)
            {
                query = query.DynamicOrderBy("Id", true, true);
            }
            return query;
        }
    }
}
