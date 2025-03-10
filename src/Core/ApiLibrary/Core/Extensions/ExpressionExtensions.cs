﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ApiLibrary.Core.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression AndCondition(this Expression expression, Expression newExpression) =>
            expression is null ? newExpression : Expression.And(expression, newExpression);

        public static Expression OrCondition(this Expression expression, Expression newExpression) =>
            expression is null ? newExpression : Expression.Or(expression, newExpression);

        public static Expression ToStringExpression(this Expression expression) =>
            Expression.Call(expression, typeof(object).GetMethod("ToString", new Type[] { }));

        public static Expression ToLowerString(this Expression expression) =>
            Expression.Call(expression, typeof(string).GetMethod("ToLower", 0, new Type[] { }));

        public static Expression ToUpperString(this Expression expression) =>
            Expression.Call(expression, typeof(string).GetMethod("ToUpper", new Type[] { }));

        public static Expression Contains(this Expression expression, Expression constant) =>
            Expression.Call(expression, typeof(string).GetMethod("Contains", new[] { typeof(string) }), constant);

        public static Expression StartsWith(this Expression expression, Expression constant) =>
            Expression.Call(expression, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), constant);

        public static Expression EndsWith(this Expression expression, Expression constant) =>
            Expression.Call(expression, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), constant);

        public static Expression Constant(this MemberExpression expression, string value)
        {
            var obj = TypeDescriptor.GetConverter(expression.Type).ConvertFromString(value);
            return Expression.Constant(obj, expression.Type);
        }
    }
}
