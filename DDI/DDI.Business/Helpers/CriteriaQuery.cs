using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using DDI.Business.Extensions;
using DDIX.Module.Helpers;
using LinqKit;

namespace DDI.Business.Helpers
{
    public class CriteriaQuery<TDatabaseType, TSearchModel> : LinqQuery<TDatabaseType>
    {
        private TSearchModel _model;
        public TSearchModel Model => _model;

        public CriteriaQuery(IQueryable<TDatabaseType> query, TSearchModel model)
            : base(query)
        {
            _model = model;
        }

        public CriteriaQuery<TDatabaseType, TSearchModel> And(Expression<Func<TDatabaseType, bool>> expression)
        {
            if (expression == null)
            {
                return this;
            }

            PredicateAnd(expression);

            return this;
        }

        public CriteriaQuery<TDatabaseType, TSearchModel> IfAtLeast<V>(Expression<Func<TSearchModel, V>> test, Expression<Func<TDatabaseType, V>> expression)
        {
            var testValue = GetTestValue(test);

            if (testValue != null && !(testValue.Equals(typeof(V).DefaultValue())))
            {
                var constantExpression = Expression.Constant(testValue, typeof(V));
                var compExpression = Expression.GreaterThanOrEqual(expression.Body, constantExpression);
                var lambda = (Expression<Func<TDatabaseType, bool>>) Expression.Lambda(compExpression, expression.Parameters[0]);

                PredicateAnd(lambda);
            }

            return this;
        }

        public CriteriaQuery<TDatabaseType, TSearchModel> IfAtMost<V>(Expression<Func<TSearchModel, V>> test, Expression<Func<TDatabaseType, V>> expression)
        {
            var testValue = GetTestValue(test);

            if (testValue != null && !(testValue.Equals(typeof(V).DefaultValue())))
            {
                var constantExpression = Expression.Constant(testValue, typeof(V));
                var compExpression = Expression.LessThanOrEqual(expression.Body, constantExpression);
                var lambda = (Expression<Func<TDatabaseType, bool>>) Expression.Lambda(compExpression, expression.Parameters[0]);

                PredicateAnd(lambda);
            }

            return this;
        }

        public CriteriaQuery<TDatabaseType, TSearchModel> IfBetween<V>(Expression<Func<TSearchModel, V>> lowerBoundTest, Expression<Func<TSearchModel, V>> upperBoundTest, Expression<Func<TDatabaseType, V>> expression)
        {
            IfAtLeast(lowerBoundTest, expression);
            IfAtMost(upperBoundTest, expression);
            return this;
        }

        public CriteriaQuery<TDatabaseType, TSearchModel> IfContains<TypeOfProperty>(Expression<Func<TSearchModel, TypeOfProperty>> test, Expression<Func<TDatabaseType, TypeOfProperty>> expression)
        {
            IfCheckMethod("Contains", test, expression);
            return this;
        }

        public CriteriaQuery<TDatabaseType, TSearchModel> IfEquals<V>(Expression<Func<TSearchModel, V>> test, Expression<Func<TDatabaseType, V>> expression)
        {
            //if (HasValue(test))
            var testValue = GetTestValue(test);

            if (!IsNullOrBlankOrDefault<V>(testValue))
            {
                var constantExpression = Expression.Constant(testValue, typeof(V));
                var equalExpression = Expression.Equal(expression.Body, constantExpression);
                var lambda = (Expression<Func<TDatabaseType, bool>>) Expression.Lambda(equalExpression, expression.Parameters[0]);

                PredicateAnd(lambda);
            }

            return this;
        }

        public CriteriaQuery<TDatabaseType, TSearchModel> IfHasValue<V>(Expression<Func<TSearchModel, V>> test, Expression<Func<TDatabaseType, bool>> expression)
        {
            var testValue = test.Compile().Invoke(Model);
            if (testValue != null && !(testValue.Equals(typeof(V).DefaultValue())))
            {
                PredicateAnd(expression);
            }

            return this;
        }

        public CriteriaQuery<TDatabaseType, TSearchModel> IfStartsWith<TypeOfProperty>(Expression<Func<TSearchModel, TypeOfProperty>> test, Expression<Func<TDatabaseType, TypeOfProperty>> expression)
        {
            IfCheckMethod("StartsWith", test, expression);
            return this;
        }

        public CriteriaQuery<TDatabaseType, TSearchModel> IfThen(Expression<Func<TSearchModel, bool>> condition, Expression<Func<TDatabaseType, bool>> expression)
        {
            if (condition.Compile().Invoke(Model))
            {
                And(expression);
            }

            return this;
        }

        public bool IsNull(Expression<Func<TSearchModel, bool>> propertyExpression)
        {
            return propertyExpression.Compile().Invoke(Model);
        }

        private TValue GetTestValue<TValue>(Expression<Func<TSearchModel, TValue>> test)
        {
            TValue testValue;

            try
            {
                testValue = test.Compile().Invoke(Model);
            }
            catch
            {
                testValue = default(TValue);
            }

            return testValue;
        }

        private CriteriaQuery<TDatabaseType, TSearchModel> IfCheckMethod<TypeOfProperty>(string method, Expression<Func<TSearchModel, TypeOfProperty>> test, Expression<Func<TDatabaseType, TypeOfProperty>> expression)
        {
            var testValue = GetTestValue(test);

            if (!IsNullOrBlankOrDefault<TypeOfProperty>(testValue))
            {
                var constantExpression = Expression.Constant(testValue, typeof(TypeOfProperty));
                MethodInfo contains = typeof(TypeOfProperty).GetMethod(method, new Type[] { typeof(TypeOfProperty) });
                var expressionBody = Expression.Call(expression.Body, contains, constantExpression);
                var lambda = (Expression<Func<TDatabaseType, bool>>) Expression.Lambda(expressionBody, expression.Parameters[0]);

                PredicateAnd(lambda);
            }

            return this;
        }

        private bool IsNullOrBlankOrDefault<V>(object testValue)
        {
            if (testValue == null)
                return true;
            if (testValue.Equals(typeof(V).DefaultValue()))
                return true;
            if (testValue is string && string.IsNullOrWhiteSpace((string)testValue))
                return true;
            return false;
        }
    }
}