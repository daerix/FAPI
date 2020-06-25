using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApiLibrary.Core.Entities;
using System.Net.Http.Headers;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Dynamic;

namespace ApiLibrary.Core.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Pagination<T>(this IQueryable<T> query, long from, long to)
        {
            return query;
        }

        public static IQueryable<T> Range<T>(this IQueryable<T> query, long from, long to) =>
            query.Skip((int)from).Take((int)(to - from + 1));


        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string[] field)
        {
            foreach (var item in field)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, item.Split(" ")[0]);
                var propAsObject = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<T, object>>(propAsObject, parameter);

                if (item.EndsWith(" desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = query.SortDesc(lambda);
                }
                else
                {
                    query = query.SortAsc(lambda);
                }
            }
            return query;
        }

        public static IQueryable<dynamic> Field<T>(this IQueryable<T> query, string[] fields)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var bindings = fields.Select(x => Expression.PropertyOrField(parameter, x)).Select(x => Expression.Bind(x.Member, x));
            var lambda = Expression.Lambda<Func<T, object>>(Expression.MemberInit(Expression.New(typeof(T)), bindings), parameter);
            return query.Select(lambda).Select(x => ToDynamicObject(x, fields));
        }

        public static IQueryable<T> Search<T>(this IQueryable<T> query, string field, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);
            var constante = property.Constant(value.Replace("*", ""));

            Expression expression = property;
            if (property.Type != typeof(string))
            {
                expression = expression.ToStringExpression();
                constante = constante.ToStringExpression();
            }

            if (value.StartsWith("*") && value.EndsWith("*"))
            {
                expression = expression.Contains(constante);
            }
            else if (value.StartsWith("*"))
            {
                expression = expression.EndsWith(constante);
            }
            else
            {
                expression = expression.StartsWith(constante);
            }
            var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);
            return query.Where(lambda);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, string field, string[] values)
        {
            Expression expression = null;
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);
            foreach (var value in values)
            {
                var constante = property.Constant(value);
                var condition = Expression.Equal(property, constante);
                expression = expression.OrCondition(condition);
            }
            var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);
            return query.Where(lambda);
        }

        public static IQueryable<T> Fork<T>(this IQueryable<T> query, string field, string[] values)
        {
            Expression expression = null;
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);
            foreach (var value in values)
            {
                Expression condition;
                var index = Array.IndexOf(values, value);
                var boundary = value.Replace("[", "").Replace("]", "");
                if (!string.IsNullOrWhiteSpace(boundary))
                {
                    var constante = property.Constant(boundary);
                    if (index == 0)
                    {

                        condition = Expression.GreaterThanOrEqual(property, constante);
                    }
                    else
                    {
                        condition = Expression.LessThanOrEqual(property, constante);
                    }
                    expression = expression.AndCondition(condition);
                }
            }
            var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);
            return query.Where(lambda);
        }

        private static IOrderedQueryable<T> SortAsc<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector) =>
            query.Expression.Type == typeof(IOrderedQueryable<T>) ? ((IOrderedQueryable<T>)query).ThenBy(keySelector) : query.OrderBy(keySelector);

        private static IOrderedQueryable<T> SortDesc<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector)
            => query.Expression.Type == typeof(IOrderedQueryable<T>) ? ((IOrderedQueryable<T>)query).ThenByDescending(keySelector) : query.OrderByDescending(keySelector);

        public static object ToDynamicObject(object value, string[] fields)
        {
            dynamic data = new ExpandoObject();
            var dataDictionary = (IDictionary<string, object>)data;
            var props = value.GetType().GetRuntimeProperties();
            foreach (var item in fields)
            {
                dataDictionary.Add(item, props.Single(x => x.Name.ToLower() == item.ToLower()).GetValue(value));
            }
            return data;
        }
    }
}
