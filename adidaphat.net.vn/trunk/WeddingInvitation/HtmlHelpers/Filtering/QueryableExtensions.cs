// (c) Copyright 2002-2010 Telerik 
// This source is subject to the GNU General Public License, version 2
// See http://www.gnu.org/licenses/gpl-2.0.html. 
// All other rights reserved.

namespace Telerik.Web.Mvc.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using Infrastructure.Implementation.Expressions;
    using Telerik.Web.Mvc;
    using Telerik.Web.Mvc.Infrastructure;
    using Telerik.Web.Mvc.Infrastructure.Implementation;
    using System.Reflection;

    public static class QueryableExtensions
    { 
        private static IQueryable CallQueryableMethod(this IQueryable source, string methodName, LambdaExpression selector)
        {
            IQueryable query = source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { source.ElementType, selector.Body.Type },
                    source.Expression,
                    Expression.Quote(selector)));

            return query;
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are the result of invoking a 
        /// projection selector on each element of <paramref name="source" />.
        /// </returns>
        /// <param name="source"> A sequence of values to project. </param>
        /// <param name="selector"> A projection function to apply to each element. </param>
        public static IQueryable Select(this IQueryable source, LambdaExpression selector)
        {
            return source.CallQueryableMethod("Select", selector);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <param name="source"> An <see cref="IQueryable" /> whose elements to group.</param>
        /// <param name="keySelector"> A function to extract the key for each element.</param>
        /// <returns>
        /// An <see cref="IQueryable"/> with <see cref="IGrouping{TKey,TElement}"/> items, 
        /// whose elements contains a sequence of objects and a key.
        /// </returns>
        public static IQueryable GroupBy(this IQueryable source, LambdaExpression keySelector)
        {
            return source.CallQueryableMethod("GroupBy", keySelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted according to a key.
        /// </returns>
        /// <param name="source">
        /// A sequence of values to order.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from an element.
        /// </param>
        public static IQueryable OrderBy(this IQueryable source, LambdaExpression keySelector)
        {
            return source.CallQueryableMethod("OrderBy", keySelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted in descending order according to a key.
        /// </returns>
        /// <param name="source">
        /// A sequence of values to order.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from an element.
        /// </param>
        public static IQueryable OrderByDescending(this IQueryable source, LambdaExpression keySelector)
        {
            return source.CallQueryableMethod("OrderByDescending", keySelector);
        }
        
        /// <summary> 
        /// Filters a sequence of values based on a collection of <see cref="IFilterDescriptor"/>. 
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="filterDescriptors">The filter descriptors.</param>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains elements from the input sequence 
        /// that satisfy the conditions specified by each filter descriptor in <paramref name="filterDescriptors" />.
        /// </returns>
        public static IQueryable Where(this IQueryable source, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                var parameterExpression = Expression.Parameter(source.ElementType, "item");

                var expressionBuilder = new FilterDescriptorCollectionExpressionBuilder(parameterExpression, filterDescriptors);
                expressionBuilder.Options.LiftMemberAccessToNull = source.Provider.IsLinqToObjectsProvider();
                var predicate = expressionBuilder.CreateFilterExpression();
                return source.Where(predicate);
            }

            return source;
        }

        public static object Sum(this IQueryable source, string member)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (member == null) throw new ArgumentNullException("member");

            // Properties
            PropertyInfo property = source.ElementType.GetProperty(member);
            ParameterExpression parameter = Expression.Parameter(source.ElementType, "s");
            Expression selector = Expression.Lambda(Expression.MakeMemberAccess(parameter, property), parameter);
            // We've tried to find an expression of the type Expression<Func<TSource, TAcc>>,
            // which is expressed as ( (TSource s) => s.Price );

            // Method
            MethodInfo sumMethod = typeof(Queryable).GetMethods().First(
                m => m.Name == "Sum"
                    && m.ReturnType == property.PropertyType // should match the type of the property
                    && m.IsGenericMethod);

            return source.Provider.Execute(
                Expression.Call(
                    null,
                    sumMethod.MakeGenericMethod(new[] { source.ElementType }),
                    new[] { source.Expression, Expression.Quote(selector) }));
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains the specified number 
        /// of elements from the start of <paramref name="source" />.
        /// </returns>
        /// <param name="source"> The sequence to return elements from.</param>
        /// <param name="count"> The number of elements to return. </param>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null. </exception>
        public static IQueryable Take(this IQueryable source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Take",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence 
        /// and then returns the remaining elements.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains elements that occur 
        /// after the specified index in the input sequence.
        /// </returns>
        /// <param name="source">
        /// An <see cref="IQueryable" /> to return elements from.
        /// </param>
        /// <param name="count">
        /// The number of elements to skip before returning the remaining elements.
        /// </param>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null.</exception>
        public static IQueryable Skip(this IQueryable source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Skip",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        /// <summary> Returns the element at a specified index in a sequence.</summary>
        /// <returns> The element at the specified position in <paramref name="source" />.</returns>
        /// <param name="source"> An <see cref="IQueryable" /> to return an element from.</param>
        /// <param name="index"> The zero-based index of the element to retrieve.</param>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero.</exception>
        public static object ElementAt(this IQueryable source, int index)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (index < 0) throw new ArgumentOutOfRangeException("index");

            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable),
                    "ElementAt",
                    new Type[] { source.ElementType },
                    source.Expression,
                    Expression.Constant(index)));
        }

        /// <summary>
        /// Creates a <see cref="IList{T}" /> from an <see cref="IQueryable" /> where T is <see cref="IQueryable.ElementType"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        /// <param name="source">
        /// The <see cref="IQueryable" /> to create a <see cref="List{T}" /> from.
        /// </param>
        /// <exception cref="ArgumentNullException"> 
        /// <paramref name="source" /> is null.
        /// </exception>
        public static IList ToIList(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(source.ElementType));

            foreach (var item in source)
            {
                list.Add(item);
            }

            return list;
        }

        internal static bool IsBindableType(Type type)
        {
            if ((!type.IsPrimitive &&
                (type != typeof(string))) &&
                (type != typeof(DateTime)) &&
                (type != typeof(TimeSpan)) &&
                (type != typeof(decimal)) &&
                (type != typeof(Guid)) &&
                (!type.IsEnum))
            {
                return type.IsValueType &&
                    type.IsGenericType &&
                    type.GetGenericArguments().Length == 1 &&
                        IsBindableType(type.GetGenericArguments()[0]);
            }

            return true;
        }
    }
}
