
using System;
using System.Collections.Generic;
using System.Data;

namespace Climb.Core
{
    #region 查询对象 Query
    /// <summary>
    /// 查询对象
    /// </summary>
    public class Query : IQuery
    {
        private QueryName _name;
        private IList<Criterion> _criteria;
        private IList<Operator> _queryOperator = new List<Operator>();
        private IList<OrderByItem> _orderByClause = new List<OrderByItem>();

        public Query()
            : this(QueryName.Dynamic, new List<Criterion>())
        { }
        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="name">查询类型</param>
        /// <param name="criteria">规则List</param>
        public Query(QueryName name, IList<Criterion> criteria)
        {
            _name = name;
            _criteria = criteria;
        }

        /// <summary>
        /// 创建查询类对象
        /// </summary>
        /// <returns></returns>
        public QueryName Name
        {
            get { return _name; }
        }
        /// <summary>
        /// 是否基础SQL
        /// </summary>
        /// <returns></returns>
        public bool IsNamedQuery()
        {
            return Name != QueryName.Dynamic;
        }

        /// <summary>
        /// 规则类集合
        /// </summary>
        public IEnumerable<Criterion> Criteria
        {
            get { return _criteria; }
        }
        /// <summary>
        /// 添加规范
        /// </summary>
        /// <param name="criterion"></param>
        public void Add(Criterion criterion)
        {
            if (!IsNamedQuery())
            {
                _criteria.Add(criterion);
            }
            else
                throw new ApplicationException(
                    "You cannot add additionalcriteria to named queries");
        }

        /// <summary>  
        /// 添加条件连接符  
        /// </summary>  
        /// <param name="queryOperator"></param>  
        public void AddQueryOperator(Operator queryOperator)
        {
            QueryOperator.Add(queryOperator);
        }

        /// <summary>  
        /// 添加排序属性  
        /// </summary>  
        /// <param name="orderByItem"></param>  
        public void AddOrderByItem(OrderByItem orderByItem)
        {
            OrderByClause.Add(orderByItem);
        }
        /// <summary>
        /// 连接操作符List
        /// </summary>
        public IList<Operator> QueryOperator
        {
            get { return _queryOperator; }
            set { _queryOperator = value; }
        }
        /// <summary>
        /// 排序操作符list
        /// </summary>
        public IList<OrderByItem> OrderByClause
        {
            get { return _orderByClause; }
            set { _orderByClause = value; }
        }

        private string _selectFileds = string.Empty;
        /// <summary>
        /// 查询字段串
        /// </summary>
        public string SelectFileds
        {
            get { return _selectFileds; }
            set { _selectFileds = value; }
        }
    }

    /// <summary>
    /// 查询对象接口
    /// </summary>
    public interface IQuery
    {
        IList<OrderByItem> OrderByClause { get; set; }
        IList<Operator> QueryOperator { get; set; }
        string SelectFileds { get; set; }
        /// <summary>
        /// 是否为查询
        /// </summary>
        /// <returns></returns>
        bool IsNamedQuery();
        /// <summary>
        /// 添加规则
        /// </summary>
        /// <param name="criterion"></param>
        void Add(Criterion criterion);
        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="queryOperator"></param>
        void AddQueryOperator(Operator queryOperator);
        /// <summary>
        /// 添加排序规则
        /// </summary>
        /// <param name="orderByItem"></param>
        void AddOrderByItem(OrderByItem orderByItem);
    }

    #endregion

    #region 更新对象 Update

    public class Update : IUpdate
    {
        private IList<Criterion> _criteria;
        private IList<Operator> _queryOperator = new List<Operator>();

        public Update()
            : this(new List<Criterion>())
        { }

        public Update(IList<Criterion> criteria)
        {
            _criteria = criteria;
        }

        /// <summary>
        /// 创建更新类对象
        /// </summary>
        /// <returns></returns>
        public static Update Create()
        {
            return new Update();
        }
        /// <summary>
        /// 规则实体
        /// </summary>
        public IEnumerable<Criterion> Criteria
        {
            get { return _criteria; }
        }
        /// <summary>
        ///  更新操作
        /// </summary>
        public IList<Operator> UpdateOperator
        {
            get { return _queryOperator; }
            set { _queryOperator = value; }
        }

        /// <summary>
        /// 添加规则
        /// </summary>
        /// <param name="criterion"></param>
        public void Add(Criterion criterion)
        {
            _criteria.Add(criterion);
        }

        /// <summary>  
        /// 添加条件连接符  
        /// </summary>  
        /// <param name="queryOperator"></param>  
        public void AddUpdateOperator(Operator queryOperator)
        {
            UpdateOperator.Add(queryOperator);
        }
    }

    public interface IUpdate
    {
        IEnumerable<Criterion> Criteria { get; }

        IList<Operator> UpdateOperator { get; set; }

        /// <summary>
        /// 添加标准
        /// </summary>
        /// <param name="criterion"></param>
        void Add(Criterion criterion);

        /// <summary>
        /// 加入操作符
        /// </summary>
        /// <param name="queryOperator"></param>
        void AddUpdateOperator(Operator queryOperator);
    }

    #endregion

    #region 通用部分结构与类

    #region 枚举

    /// <summary>
    /// 操作枚举
    /// </summary>
    public enum CriteriaOperator
    {
        /// <summary>
        /// 等于=
        /// </summary>
        Equal,              //=  
        /// <summary>
        /// 不等于<>
        /// </summary>
        NotEquale,          //<>  
        /// <summary>
        /// 小于等于<=  
        /// </summary>
        LessThanOrEqual,    //<=  
        /// <summary>
        /// 小于< 
        /// </summary>
        LessThan,           //<  
        /// <summary>
        /// 大于> 
        /// </summary>
        MoreThan,           //>  
        /// <summary>
        /// 大于等于>= 
        /// </summary>
        MoreThanOrEqual,    //>=  
        In,                 //IN()  
        NotIn,              //NOT IN ()  
        Like                //%  
    }

    /// <summary>
    /// 操作连接符
    /// </summary>
    public enum Operator
    {
        And,
        Or
    }

    public enum QueryName
    {
        /// <summary>
        /// 普通SQL，其他存储过程
        /// </summary>
        Dynamic = 0,//普通SQL，其他存储过程
        Getuserlist
    }

    public enum UpdateType
    {
        /// <summary>
        /// 修改的字段
        /// </summary>
        SetFiled = 1,
        /// <summary>
        /// 修改字段需要的条件
        /// </summary>
        WhereFiled = 2
    }

    #endregion

    #region 结构常量

    /// <summary>
    /// 常量左右括号
    /// </summary>
    public struct Bracket
    {
        /// <summary>
        /// 左括号(
        /// </summary>
        public const string LeftBracket = "(";

        /// <summary>
        /// // 右括号)
        /// </summary>
        public const string RightBracket = ")";
    }

    #endregion

    #region 规则类

    /// <summary>
    /// 规则类
    /// </summary>
    public class Criterion
    {
        private string _propertyName;
        private object _value;
        private CriteriaOperator _criteriaOperator;
        private ParameterDirection _direction = ParameterDirection.Input;
        private string _bracket;

        private UpdateType _updateType;

        /// <summary>
        /// 规则
        /// </summary>
        /// <param name="propertyName">字段名</param>
        /// <param name="value">值</param>
        /// <param name="updateType">更新类型（枚举）</param>
        public Criterion(string propertyName, object value, UpdateType updateType
           )
        {
            _propertyName = propertyName;
            _value = value;
            _updateType = updateType;
        }
        /// <summary>
        /// 更新类型
        /// </summary>
        public UpdateType updateType
        {
            get { return _updateType; }
            set { _updateType = value; }
        }

        /// <summary>
        /// 规范类
        /// </summary>
        /// <param name="propertyName">字段</param>
        /// <param name="value">值</param>
        /// <param name="criteriaOperator">条件操作符（等于、小于等）</param>
        public Criterion(string propertyName, object value,
            CriteriaOperator criteriaOperator)
        {
            _propertyName = propertyName;
            _value = value;
            _criteriaOperator = criteriaOperator;
        }

        /// <summary>
        /// 规范类
        /// </summary>
        /// <param name="propertyName">字段</param>
        /// <param name="value">值</param>
        /// <param name="criteriaOperator">操作符</param>
        /// <param name="direction">ParameterDirection Type（input、output）</param>
        public Criterion(string propertyName, object value,
            CriteriaOperator criteriaOperator, ParameterDirection direction)
        {
            _propertyName = propertyName;
            _value = value;
            _criteriaOperator = criteriaOperator;
            _direction = direction;
        }

        /// <summary>
        /// 规范类
        /// </summary>
        /// <param name="propertyName">字段</param>
        /// <param name="value">值</param>
        /// <param name="criteriaOperator">条件操作符（等于、小于等）</param>
        /// <param name="bracket">括号</param>
        public Criterion(string propertyName, object value,
            CriteriaOperator criteriaOperator, string bracket)
        {
            _propertyName = propertyName;
            _value = value;
            _criteriaOperator = criteriaOperator;
            _bracket = bracket;
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return _value; }
        }
        /// <summary>
        /// 操作枚举
        /// </summary>
        public CriteriaOperator CriteriaOperator
        {
            get { return _criteriaOperator; }
        }
        /// <summary>
        /// DataSet 的参数的类型
        /// </summary>
        public ParameterDirection Direction
        {
            get { return _direction; }
        }
        /// <summary>
        /// 括号
        /// </summary>
        public string Bracket
        {
            get { return _bracket; }
        }

    }

    #endregion

    #region 排序类

    /// <summary>
    /// 查询排序类
    /// </summary>
    public class OrderByItem
    {
        private string _propertyName;
        private bool _desc;

        /// <summary>
        /// 排序构造函数
        /// </summary>
        /// <param name="propertyName">字段</param>
        /// <param name="desc">排序方式</param>
        public OrderByItem(string propertyName, bool desc)
        {
            PropertyName = propertyName;
            Desc = desc;
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }
        /// <summary>
        /// 排序bool值
        /// </summary>
        public bool Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }
    }

    #endregion

    #endregion
}
