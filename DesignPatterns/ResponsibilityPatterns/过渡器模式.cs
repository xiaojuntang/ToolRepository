using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponsibilityPatterns
{
    /// <summary>
    /// 过渡器模式
    /// 我们在给用户做订单催付通知的时候，会有这样的一种场景，用户在系统后台设置一组可以催付的规则，比如说订单金额大于xx元，非黑名单用户，来自
    /// 哪个地区，已购买过某个商品，指定某个营销活动的人等等这样的条件，如果这时用户在淘宝上下了一个订单，那程序要判断的就是看一下此订单是否满足这
    /// 些规则中的某一个，如果满足，我们给他发送催付通知，这种场景是很多做CRM的同学都会遇到的问题，那针对这种场景，如何更好的规划业务逻辑呢？
    /// </summary>
    public class 过渡器模式
    {
        public void M1()
        {
            var regulars = new List<Regulars>();

            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则1", AnalysisConditons = "xxxx" });
            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则2", AnalysisConditons = "xxxx" });
            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则3", AnalysisConditons = "xxxx" });
            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则4", AnalysisConditons = "xxxx" });
            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则5", AnalysisConditons = "xxxx" });

            var filters = FilterRegularID(regulars);
            filters = FilterRegularName(filters);
            filters = FilterCondtions(filters);

            //... 后续逻辑


            var filterList = new IFilter[3] {
                new RegularIDFilter(),
                new RegularNameFilter(),
                new RegularCondtionFilter()
            };

            var andCritteria = new AndFilter(filterList.ToList());

            andCritteria.Filter(regulars);
        }

        static List<Regulars> FilterRegularID(List<Regulars> persons)
        {
            //过滤 “姓名” 的逻辑
            return null;
        }

        static List<Regulars> FilterRegularName(List<Regulars> persons)
        {
            //过滤 “age” 的逻辑
            return null;
        }

        static List<Regulars> FilterCondtions(List<Regulars> persons)
        {
            //过滤 “email” 的逻辑
            return null;
        }
    }

    public class Regulars
    {
        public Regulars()
        {
        }

        public string AnalysisConditons { get; set; }
        public int RegularID { get; set; }
        public string RegularName { get; set; }
    }









    public interface IFilter
    {
        List<Regulars> Filter(List<Regulars> regulars);
    }

    public class RegularIDFilter : IFilter
    {
        public List<Regulars> Filter(List<Regulars> regulars)
        {
            return null;
        }
    }
    public class RegularNameFilter : IFilter
    {
        /// <summary>
        /// regularName的过滤方式
        /// </summary>
        /// <param name="regulars"></param>
        /// <returns></returns>
        public List<Regulars> Filter(List<Regulars> regulars)
        {
            return null;
        }
    }

    public class RegularCondtionFilter : IFilter
    {
        /// <summary>
        /// Condition的过滤条件
        /// </summary>
        /// <param name="regulars"></param>
        /// <returns></returns>
        public List<Regulars> Filter(List<Regulars> regulars)
        {
            return null;
        }
    }

    /// <summary>
    /// filter的 And 模式
    /// </summary>
    public class AndFilter : IFilter
    {
        List<IFilter> filters = new List<IFilter>();

        public AndFilter(List<IFilter> filters)
        {
            this.filters = filters;
        }

        public List<Regulars> Filter(List<Regulars> regulars)
        {
            var regularlist = new List<Regulars>(regulars);

            foreach (var criteriaItem in filters)
            {
                regularlist = criteriaItem.Filter(regularlist);
            }

            return regularlist;
        }
    }

    /// <summary>
    /// filter的 Or 模式
    /// </summary>
    public class OrFilter : IFilter
    {
        List<IFilter> filters = null;

        public OrFilter(List<IFilter> filters)
        {
            this.filters = filters;
        }

        public List<Regulars> Filter(List<Regulars> regulars)
        {
            //用hash去重
            var resultHash = new HashSet<Regulars>();

            foreach (var filter in filters)
            {
                var smallPersonList = filter.Filter(regulars);

                foreach (var small in smallPersonList)
                {
                    resultHash.Add(small);
                }
            }

            return resultHash.ToList();
        }
    }
}
