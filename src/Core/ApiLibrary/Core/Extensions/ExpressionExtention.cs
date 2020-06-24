using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace ApiLibrary.Core.Extentions
{
    public static class ExpressionExtention
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
            if (expression.Type != typeof(string) && expression.Type != typeof(DateTime))
            {
                value = value.Replace(".", ",");
            }
            var obj = TypeDescriptor.GetConverter(expression.Type).ConvertFromString(value);
            return Expression.Constant(obj, expression.Type);
        }
    }
}
