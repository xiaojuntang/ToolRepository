using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Climb.ExpressionHelper
{
    public class ExpHelper<T> where T : class
    {

        private ParameterExpression param;

        private BinaryExpression filter;

        public ExpHelper()
        {

            param = Expression.Parameter(typeof(T), "c");

            //1==1

            Expression left = Expression.Constant(1);

            filter = Expression.Equal(left, left);

        }

        public Expression<Func<T, bool>> GetExpression()
        {

            return Expression.Lambda<Func<T, bool>>(filter, param);

        }

        public void Equal(string propertyName, object value)
        {

            try
            {
                Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));

                Expression right = Expression.Constant(value, value.GetType());

                Expression result = Expression.Equal(left, right);

                filter = Expression.And(filter, result);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void Contains(string propertyName, string value)
        {

            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));

            Expression right = Expression.Constant(value, value.GetType());

            Expression result = Expression.Call(left, typeof(string).GetMethod("Contains"), right);

            filter = Expression.And(filter, result);

        }

        public void Contains(string propertyName, int value)
        {

            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));

            Expression right = Expression.Constant(value.ToString(), typeof(string));

            Expression result = Expression.Call(left, typeof(string).GetMethod("Contains"), right);

            filter = Expression.And(filter, result);

        }
        /// <summary>
        /// 大于或者等于
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void Greater(string propertyName, object value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.GreaterThanOrEqual(left, right);
            filter = Expression.And(filter, result);
        }
        /// <summary>
        /// 小于或等于
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void Less(string propertyName, object value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.LessThanOrEqual(left, right);
            filter = Expression.And(filter, result);
        }


        public void OrWithContains(params Tuple<string, object>[] conditionPair)
        {
            if (conditionPair.Length == 0)
                return;
            var expList = new List<Expression>();
            foreach (var item in conditionPair)
            {
                Expression left = Expression.Property(param, typeof(T).GetProperty(item.Item1));
                Expression right = Expression.Constant(item.Item2, item.Item2.GetType());
                expList.Add(Expression.Call(left, typeof(string).GetMethod("Contains"), right));
            }

            if (expList.Count > 1)
            {
                Expression ex = expList.First();
                for (var i = 1; i < expList.Count; i++)
                {
                    ex = Expression.Or(ex, expList[i]);
                }
                filter = Expression.And(filter, ex);
            }
            else
                filter = Expression.And(filter, expList[0]);
        }

        public void IsNotNull(string propertyName)
        {
            Expression exp = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression result = Expression.NotEqual(exp, Expression.Constant(""));
            filter = Expression.And(filter, result);
        }

        /// <summary>
        /// 判断一个值是否包含在一个集合中
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="values"></param>
        public void In(string propertyName, object[] values)
        {
            Expression left = Expression.Constant(1);
            BinaryExpression bexp = null;


            for (int i = 0; i < values.Length; i++)
            {
                object obj = values[i];
                string ty = typeof(T).GetProperty(propertyName).ToString();
                string te = "System." + ty.Substring(0, ty.Length - propertyName.Length - 1);
                Type ty1 = Type.GetType(te);
                var _obj = Convert.ChangeType(obj, ty1);
                Expression exp = Expression.Property(param, typeof(T).GetProperty(propertyName));
                if (i == 0)
                {
                    bexp = Expression.Equal(exp, Expression.Constant(_obj));
                    continue;
                }
                else
                {
                    Expression result = Expression.Equal(exp, Expression.Constant(_obj));
                    bexp = Expression.Or(bexp, result);
                }
            }
            filter = Expression.And(filter, bexp);
        }

        /// <summary>
        /// 等于Null或者等于“”
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="values"></param>
        public void isNullOrNulls(string propertyName)
        {
            Expression result = null;
            Expression exp = Expression.Property(param, typeof(T).GetProperty(propertyName));
            result = Expression.Equal(exp, Expression.Constant(null));
            Expression rig = Expression.Equal(exp, Expression.Constant(""));
            var t = Expression.Or(result, rig);
            filter = Expression.And(filter, t);

        }
        /// <summary>
        /// 不等于Null并且不等“”
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="values"></param>
        public void isNotNullAndNulls(string propertyName)
        {
            Expression result = null;
            Expression exp = Expression.Property(param, typeof(T).GetProperty(propertyName));
            result = Expression.NotEqual(exp, Expression.Constant(null));
            Expression rig = Expression.NotEqual(exp, Expression.Constant(""));
            var t = Expression.And(result, rig);
            filter = Expression.And(filter, t);
        }
        /// <summary>
        /// 判断一个属性值是否包含在一个泛型集合中
        /// </summary>
        /// <typeparam name="M">泛型元素的类型</typeparam>
        /// <param name="properName">属性名</param>
        /// <param name="arr">泛型集合</param>
        public void In<M>(string properName, List<M> arr)
        {
            Expression left = Expression.Property(Expression.Parameter(typeof(T), "c"), properName);
            Type ty = typeof(T).GetProperty(properName).PropertyType;

            BinaryExpression resul = null;
            Type ty1 = typeof(M);
            if (ty != ty1) //如果属性的type和泛型元素的type不等，则元素肯定不包含在集合中，返回一个1！=1的表达式
            {
                Expression ex = Expression.Constant(1);
                if (filter == null)
                    filter = Expression.NotEqual(ex, ex);
                else
                    filter = Expression.And(filter, Expression.NotEqual(ex, ex));
            }
            else
            {
                foreach (M ob in arr)
                {

                    BinaryExpression rig = Expression.Equal(left, Expression.Constant(ob));
                    if (resul == null)
                        resul = rig;
                    else
                        resul = Expression.Or(resul, rig);
                }
                if (resul != null && filter != null)
                    filter = Expression.And(filter, resul);
                else
                    filter = resul;
            }

        }
        /// <summary>
        /// 判断属性不为null和"",与！String.IsNullOrEmpty()用法类似
        /// </summary>
        /// <param name="properName"></param>
        public void NotNullOrEmpty(string properName)
        {
            BinaryExpression res = null;
            Expression left = Expression.Property(Expression.Parameter(typeof(T), "c"), properName);
            res = Expression.NotEqual(left, Expression.Constant(""));
            res = Expression.And(res, Expression.NotEqual(left, Expression.Constant(null)));
            filter = Expression.And(filter, res);

        }
        /// <summary>
        /// 判断属性为null或者"",与String.IsNullOrEmpty()用法类似
        /// </summary>
        /// <param name="properName"></param>
        public void IsNullOrEmpty(string properName)
        {
            BinaryExpression res = null;
            Expression left = Expression.Property(Expression.Parameter(typeof(T), "c"), properName);
            res = Expression.Equal(left, Expression.Constant(""));
            res = Expression.Or(res, Expression.Equal(left, Expression.Constant(null)));
            filter = Expression.And(filter, res);

        }
        /// <summary>
        /// 比较时间或者数字在某个范围内
        /// </summary>
        /// <param name="properName">属性名</param>
        /// <param name="starValu">范围的开始值</param>
        /// <param name="endValu">范围的结束值</param>
        public void Between(string properName, object starValu, object endValu, EncloseFlag flag)
        {
            Type t = typeof(T).GetProperty(properName).PropertyType;
            if (!t.IsValueType)
            {
                throw new Exception("属性\"" + properName + "\"的类型为:" + t + "不支持Between比较");

            }
            BinaryExpression exr = null, exr2 = null;
            Expression left = Expression.Property(Expression.Parameter(typeof(T), "c"), properName);
            object star = Convert.ChangeType(starValu, t);
            object end = Convert.ChangeType(endValu, t);

            Expression righ1 = Expression.Constant(star, t);
            Expression righ2 = Expression.Constant(end, t);

            switch (flag)
            {
                case EncloseFlag.BothClose:
                    {
                        exr = Expression.GreaterThanOrEqual(left, righ1);
                        exr2 = Expression.LessThanOrEqual(left, righ2);
                        exr = Expression.And(exr, exr2);
                        break;
                    }
                case EncloseFlag.LeftClose:
                    {
                        exr = Expression.GreaterThanOrEqual(left, righ1);
                        exr2 = Expression.LessThan(left, righ2);
                        exr = Expression.And(exr, exr2);

                        break;
                    }
                case EncloseFlag.RihtClose:
                    {
                        exr = Expression.GreaterThan(left, righ1);
                        exr2 = Expression.LessThanOrEqual(left, righ2);
                        exr = Expression.And(exr, exr2);
                        break;
                    }
                case EncloseFlag.NotClose:
                    {
                        exr = Expression.GreaterThan(left, righ1);
                        exr2 = Expression.LessThanOrEqual(left, righ2);
                        exr = Expression.And(exr, exr2);
                    }
                    break;
                default:
                    break;
            }


            if (filter != null)
                filter = Expression.And(filter, exr);
            else
                filter = exr;
        }
    }
    public enum EncloseFlag
    {
        /// <summary>
        /// 左右闭合
        /// </summary>
        BothClose,
        /// <summary>
        /// 左闭右开
        /// </summary>
        LeftClose,
        /// <summary>
        /// 左开右闭
        /// </summary>
        RihtClose,
        /// <summary>
        /// 左右不闭合
        /// </summary>
        NotClose
    }
}
