using ApiLibrary.Core.Entity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace ApiLibrary.Core.Extensions
{
    public static class IQuerableExtensions
    {

        public static IQueryable<T> Range<T>(this IQueryable<T> query, int start, int count)
        {
            return query.Skip(start).Take(count);
        }

       /* public static IOrderedQueryable<T> SortAsc<T, TKey>(this IQueryable<T> query, string propertyName) =>
            query.Expression.Type == typeof(IOrderedQueryable<T>) ? ((IOrderedQueryable<T>)query).ThenBy(keySelector) : query.OrderBy(keySelector);

        public static IOrderedQueryable<T> SortDesc<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector) =>
            query.Expression.Type == typeof(IOrderedQueryable<T>) ? ((IOrderedQueryable<T>)query).ThenByDescending(keySelector) : query.OrderByDescending(keySelector);*/

        /*public static IOrderedQueryable<T> OrderBy<T> (this IOrderedQueryable<T> query, string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            
            var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(T), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { query, lambda });


            return (IOrderedQueryable<T>)result;
        }*/


        public static IEnumerable<dynamic> SelectDynamic<T>(this IQueryable<T> query, string fields)
        {
            var tab = fields.Split(',');
            var parameter = Expression.Parameter(typeof(T), "x");
            var bindings = tab.Select(x => Expression.PropertyOrField(parameter, x)).Select(x => Expression.Bind(x.Member, x));
            var exp = Expression.MemberInit(Expression.New(typeof(T)), bindings);

            var lambda = Expression.Lambda<Func<T, object>>(exp, parameter);

            return query.Select(lambda).ToList().Select(x => SelectObject(x, fields));
        }

        public static object SelectObject(object value, string fields)
        {
            dynamic data = new ExpandoObject();
            var dataDictionary = (IDictionary<string, object>)data;
            var props = value.GetType().GetRuntimeProperties();
            var properties = fields.Split(',');
            foreach (var item in properties)
            {
                dataDictionary.Add(item, props.Single(x => x.Name.ToLower() == item.ToLower()).GetValue(value));
            }
            return data;
        }



        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propName)
        {
            return source.Expression.Type == typeof(IOrderedQueryable<T>) ? ((IOrderedQueryable<T>)source).ThenBy(ToLambda<T>(propName)) : source.OrderBy(ToLambda<T>(propName));
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propName)
        {
            return source.Expression.Type == typeof(IOrderedQueryable<T>) ? ((IOrderedQueryable<T>)source).ThenByDescending(ToLambda<T>(propName)) : source.OrderByDescending(ToLambda<T>(propName));
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propName)
        {
            return source.ThenBy(ToLambda<T>(propName));
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propName)
        {
            return source.ThenByDescending(ToLambda<T>(propName));
        }

    }
}
