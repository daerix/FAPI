using ApiLibrary.Core.Extensions;
using ApiLibrary.Test.Mocks.Models;
using System;
using System.Linq.Expressions;
using Xunit;

namespace ApiLibrary.Test
{
    public class ExpressionExtensionsTest
    {

        [Fact]
        public void constant_with_decimal()
        {
            var parameter = Expression.Parameter(typeof(ModelTest), "x");
            var property = Expression.Property(parameter, "Decimal");
            var value = 1.1M;

            var result = property.Constant(value.ToString());

            Assert.Equal(value.ToString(), result.ToString());
        }

        [Fact]
        public void constant_with_integer()
        {
            var parameter = Expression.Parameter(typeof(ModelTest), "x");
            var property = Expression.Property(parameter, "Integer");
            var value = 1;

            var result = property.Constant(value.ToString());

            Assert.Equal(value.ToString(), result.ToString());
        }

        [Fact]
        public void constant_with_datetime()
        {
            var parameter = Expression.Parameter(typeof(ModelTest), "x");
            var property = Expression.Property(parameter, "Date");
            var value = new DateTime(2000, 8, 25);

            var result = property.Constant(value.ToString());

            Assert.Equal(value.ToString(), result.ToString());
        }

        [Fact]
        public void constant_with_string()
        {
            var parameter = Expression.Parameter(typeof(ModelTest), "x");
            var property = Expression.Property(parameter, "String");
            var value = "Test";

            var result = property.Constant(value);

            Assert.Equal(string.Concat(new[] { "\"", value, "\"" }), result.ToString());
        }
    }
}
