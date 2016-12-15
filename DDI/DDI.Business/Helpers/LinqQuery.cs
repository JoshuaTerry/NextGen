using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

namespace DDIX.Module.Helpers
{
    public class LinqQuery<TEntity>
    {
        #region Private Fields

        private ExpressionStarter<TEntity> _predicate = PredicateBuilder.New<TEntity>();

        private IQueryable<TEntity> _query;

        #endregion Private Fields

        #region Public Properties

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

        public IQueryable<TEntity> GetQueryable() => _query?.AsExpandable().Where(_predicate);

        public LinqQuery<TEntity> Or(Expression<Func<TEntity, bool>> expression)
        {
            if (expression != null)
            {
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
                _predicate = _predicate.And(expression);
            }
        }

        #endregion Protected Methods
    }
}
