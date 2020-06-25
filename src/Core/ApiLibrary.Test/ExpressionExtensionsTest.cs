using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibrary.Test.Mocks.Models;
using Xunit;
using System.Linq.Expressions;
using ApiLibrary.Core.Extensions;

namespace ApiLibrary.Test
{
    public class ExpressionExtensionsTest
    {

        [Fact]
        public async Task constant_with_decimal()
        {
            var parameter = Expression.Parameter(typeof(ModelTest), "x");
            var property = Expression.Property(parameter, "Decimal");
            var value = 1.1;

            var result = property.Constant(value.ToString());

            Assert.Equal(value.ToString(), result.ToString());
        }

        [Fact]
        public async Task constant_with_integer()
        {
            var parameter = Expression.Parameter(typeof(ModelTest), "x");
            var property = Expression.Property(parameter, "Integer");
            var value = 1;

            var result = property.Constant(value.ToString());

            Assert.Equal(value.ToString(), result.ToString());
        }

        [Fact]
        public async Task constant_with_datetime()
        {
            var parameter = Expression.Parameter(typeof(ModelTest), "x");
            var property = Expression.Property(parameter, "Date");
            var value = new DateTime(2000, 8, 25);

            var result = property.Constant("25/08/2000");

            Assert.Equal(value.ToString(), result.ToString());
        }

        [Fact]
        public async Task constant_with_string()
        {
            var parameter = Expression.Parameter(typeof(ModelTest), "x");
            var property = Expression.Property(parameter, "String");
            var value = "Test";

            var result = property.Constant(value);

            Assert.Equal(string.Concat(new[] { "\"", value, "\"" }), result.ToString());
        }

        private Expression ConstantExpression(object value) =>
            Expression.Constant(value , value.GetType());
    }
}
