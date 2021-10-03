using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using VibeQuest.Dto;

namespace VibeQuest.Service.Helper
{
    public class PagedList<T>
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
        }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<T> Items { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, PaginationParams userParams) 
        {
            var count = await source.CountAsync();
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                if (userParams.OrderBy != string.Empty && (userParams.Order == "" || userParams.Order == "asc")){
                    source = OrderBy(source, userParams.OrderBy, "OrderBy");
                }
                else
                {
                    source = OrderBy(source, userParams.OrderBy);
                }
            }
            var items = await source.Skip((userParams.PageNumber - 1) * userParams.PageSize).Take(userParams.PageSize).ToListAsync();
            var dta = new PagedList<T>(items, count, userParams.PageNumber, userParams.PageSize);
            return dta;
        }

        public static IOrderedQueryable<T> OrderBy(IQueryable<T> query, string propertyName, string order = "OrderByDescending") 
        {
            var entityType = typeof(T);

            // Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo.DeclaringType != entityType)
            {
                propertyInfo = propertyInfo.DeclaringType.GetProperty(propertyName);
            }

            // If we try to order by a property that does not exist in the object return the list
            if (propertyInfo == null)
            {
                return (IOrderedQueryable<T>)query;
            }

            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.MakeMemberAccess(arg, propertyInfo);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            // Get System.Linq.Queryable.OrderBy() method.
            var method = typeof(Queryable).GetMethods()
                 .Where(m => m.Name == order && m.IsGenericMethodDefinition)
                 .Where(m => m.GetParameters().ToList().Count == 2) // ensure selecting the right overload
                 .Single();

            //The linq's OrderBy<T, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /* Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it. */
            return (IOrderedQueryable<T>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
        }
    }
}
