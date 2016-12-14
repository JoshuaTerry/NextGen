using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

namespace DDIX.Module.Helpers
{
    public class LinqQuery<T>
    {
        #region Private Fields

        private ExpressionStarter<T> _predicate = PredicateBuilder.New<T>();

        private IQueryable<T> _query;

        #endregion Private Fields

        #region Public Properties

        public ExpressionStarter<T> Predicate
        {
            get { return _predicate; }
        }

        #endregion Public Properties

        #region Public Constructors

        public LinqQuery(IQueryable<T> query)
        {
            _query = query;
        }

        #endregion Public Constructors

        #region Public Methods

        public IQueryable<T> GetQueryable() => _query?.AsExpandable().Where(_predicate);

        public LinqQuery<T> Or(Expression<Func<T, bool>> expression)
        {
            if (expression != null)
            {
                _predicate = _predicate.Or(expression);
            }

            return this;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void PredicateAnd(Expression<Func<T, bool>> expression)
        {
            if (expression != null)
            {
                _predicate = _predicate.And(expression);
            }
        }

        #endregion Protected Methods
    }
}
