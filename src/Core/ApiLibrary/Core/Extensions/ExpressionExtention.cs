using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;

namespace ApiLibrary.Core.Extentions
{
    public static class ExpressionExtention
    {
        public static Expression AndEqual<T>(this Expression expression, Expression newExpression) =>
            expression is null ? expression : Expression.And(expression, newExpression);

        public static Expression OrEqual<T>(this Expression expression, Expression newExpression) =>
            expression is null ? newExpression : Expression.Or(expression, newExpression);

    }
}
