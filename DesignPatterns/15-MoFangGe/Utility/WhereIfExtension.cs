using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Climb.Utility
{
    /// <summary>
    /// where if 的扩展
    /// </summary>
    /*
    例如
    public IQueryable<Person> Query(IQueryable<Person> source, string name, string code, string address)
{
   return source
       .WhereIf(p => p.Name.Contains(name), string.IsNullOrEmpty(name) == false)
       .WhereIf(p => p.Code.Contains(code), string.IsNullOrEmpty(code) == false)
       .WhereIf(p => p.Code.Contains(address), string.IsNullOrEmpty(address) == false);
}
    */
    public static class WhereIfExtension
    {
        /// <summary>
        /// WhereIf 扩展返回IQueryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
        /// <summary>
        /// WhereIf 扩展返回IQueryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, int, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
        /// <summary>
        /// obj where if 的扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
        /// <summary>
        /// obj where if 的扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, int, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
