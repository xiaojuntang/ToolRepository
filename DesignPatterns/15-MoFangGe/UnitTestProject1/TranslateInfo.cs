using Climb.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public abstract class TranslateInfo<T> 
        //where T : class
    {
        #region Query转换部分
        /// <summary>
        /// 转换成SQL语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="command"></param>
        /// <param name="tableName"></param>
        public void TranslateInto(Query query, MySqlCommand command, string tableName)
        {
            string fileds = string.IsNullOrEmpty(query.SelectFileds) ? "*" : query.SelectFileds;
            string sql = string.Format("select {0} from {1}", fileds, tableName);

            //生成存储过程  
            if (query.IsNamedQuery())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format("{0}{1}", sql, query.Name);

                //生成参数  
                foreach (Criterion criterion in query.Criteria)
                {
                    command.Parameters.AddWithValue("?" + criterion.PropertyName, criterion.Value);
                    command.Parameters["?" + criterion.PropertyName].Direction = criterion.Direction;
                }
                return;
            }

            string sqlQuery = TranslateWhere(query, command);
            sqlQuery += TranslateOrderBy(query);

            command.CommandType = CommandType.Text;
            command.CommandText = sql + sqlQuery;
        }

        /// <summary>  
        /// 生成OrderBy语句  
        /// </summary>  
        /// <param name="query"></param>  
        /// <returns></returns>  
        public string TranslateOrderBy(Query query)
        {
            return TranslateOrderBy(query, "");
        }

        /// <summary>  
        /// 生成OrderBy语句  
        /// </summary>  
        /// <param name="query"></param>  
        /// <param name="defaultValue"></param>  
        /// <returns></returns>  
        public string TranslateOrderBy(Query query, string defaultValue)
        {
            string orderBy = GenerateOrderByClauseFrom(query.OrderByClause);
            return string.IsNullOrEmpty(orderBy) ? defaultValue : orderBy;
        }

        /// <summary>  
        /// 生成条件语句和条件语句中的参数  
        /// </summary>  
        /// <param name="query"></param>  
        /// <param name="command"></param>  
        /// <returns></returns>  
        public string TranslateWhere(Query query, MySqlCommand command)
        {
            StringBuilder sqlQuery = new StringBuilder(query.Criteria.Count() > 0 ? " WHERE " : "");

            //检查条件语句个数和连接词个数是否匹配  
            if (query.QueryOperator.Count() != query.Criteria.Count() - 1)
            {
                throw new Exception("条件语句个数和连接词个数不匹配。");
            }

            bool _isNotfirstFilterClause = false;//不是第一个条件  

            int n = 0, m = 0;
            foreach (Criterion criterion in query.Criteria)
            {

                //如果不是第一个条件，则加上连接符  
                if (_isNotfirstFilterClause)
                {
                    sqlQuery.Append(GetQueryOperator(query, n)); n++;
                }

                //生成语句,同时生成参数  
                sqlQuery.Append(AddFilterClauseFrom(criterion, command, m));

                _isNotfirstFilterClause = true;
                m++;
            }

            return sqlQuery.ToString();
        }

        /// <summary>  
        /// 将排序语句清空  
        /// </summary>  
        /// <param name="query"></param>  
        public void ResetOrderBy(Query query)
        {
            query.OrderByClause.Clear();
        }

        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="criterion"></param>
        /// <param name="command"></param>
        /// <param name="m"></param>
        /// <returns></returns> 
        private string AddFilterClauseFrom(Criterion criterion, MySqlCommand command, int m)
        {
            string sql = string.Empty;
            if (criterion.CriteriaOperator == CriteriaOperator.In
                || criterion.CriteriaOperator == CriteriaOperator.NotIn)
            {
                string paras = string.Empty;
                if (criterion.Value is System.Int32[])
                {
                    var d = criterion.Value as int[];
                    for (int i = 0; i < d.Length; i++)
                    {
                        paras += string.Format("?p{0}_{1}{2}", m, i, i == d.Length - 1 ? "" : ",");
                        command.Parameters.AddWithValue(string.Format("?p{0}_{1}", m, i), d[i]);
                    }
                }
                else
                {
                    var d = criterion.Value as string[];
                    for (int i = 0; i < d.Length; i++)
                    {
                        paras += string.Format("?p{0}_{1}{2}", m, i, i == d.Length - 1 ? "" : ",");
                        command.Parameters.AddWithValue(string.Format("?p{0}_{1}", m, i), d[i]);
                    }
                }

                switch (criterion.Bracket)
                {
                    case Bracket.LeftBracket:
                        sql = string.Format(" ( {0} {1} ({2}) ", criterion.PropertyName, FindSQLOperatorFor(criterion.CriteriaOperator), paras);
                        break;
                    case Bracket.RightBracket:
                        sql = string.Format("{0} {1} ({2}) ) ", criterion.PropertyName, FindSQLOperatorFor(criterion.CriteriaOperator), paras);
                        break;
                    default:
                        sql = string.Format("{0} {1} ({2}) ", criterion.PropertyName, FindSQLOperatorFor(criterion.CriteriaOperator), paras);
                        break;
                }
                return sql;
            }

            command.Parameters.AddWithValue("?p" + m, criterion.Value);
            command.Parameters["?p" + m].Direction = criterion.Direction;

            switch (criterion.Bracket)
            {
                case Bracket.LeftBracket:
                    sql = string.Format("( {0} {1} ?p{2} ", criterion.PropertyName, FindSQLOperatorFor(criterion.CriteriaOperator), m);
                    break;
                case Bracket.RightBracket:
                    sql = string.Format("{0} {1} ?p{2} )", criterion.PropertyName, FindSQLOperatorFor(criterion.CriteriaOperator), m);
                    break;
                default:
                    sql = string.Format("{0} {1} ?p{2} ", criterion.PropertyName, FindSQLOperatorFor(criterion.CriteriaOperator), m);
                    break;
            }

            return sql;
        }

        /// <summary>
        /// 依据属性条款生成OrderBy
        /// </summary>
        /// <param name="orderByClauses"></param>
        /// <returns></returns>
        private string GenerateOrderByClauseFrom(IList<OrderByItem> orderByClauses)
        {
            if (orderByClauses.Count() == 0) return "";

            string orderBy = " ORDER BY ";
            int n = 0, m = orderByClauses.Count() - 1;
            foreach (var item in orderByClauses)
            {
                orderBy += string.Format("{0} {1}{2}",
                    item.PropertyName, item.Desc ? "DESC" : "ASC", n == m ? "" : ",");
                n++;
            }
            return orderBy;
        }

        /// <summary>
        /// 连接符
        /// </summary>
        /// <param name="query"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetQueryOperator(Query query, int index)
        {
            Operator queryOperator = query.QueryOperator[index];

            switch (queryOperator)
            {
                case Climb.Core.Operator.And:
                    return " AND ";
                case Climb.Core.Operator.Or:
                    return " OR ";
                default:
                    throw new ApplicationException("No QueryOperator defined.");
            }
        }

        /// <summary>
        /// 操作符
        /// </summary>
        /// <param name="criteriaOperator"></param>
        /// <returns></returns>
        private string FindSQLOperatorFor(CriteriaOperator criteriaOperator)
        {
            switch (criteriaOperator)
            {
                case CriteriaOperator.Equal:
                    return "=";
                case CriteriaOperator.NotEquale:
                    return "<>";
                case CriteriaOperator.LessThanOrEqual:
                    return "<=";
                case CriteriaOperator.LessThan:
                    return "<";
                case CriteriaOperator.MoreThan:
                    return ">";
                case CriteriaOperator.MoreThanOrEqual:
                    return ">=";
                case CriteriaOperator.Like:
                    return "LIKE";
                case CriteriaOperator.In:
                    return "IN";
                case CriteriaOperator.NotIn:
                    return "NOT IN";
                default:
                    throw new ApplicationException("No operator defined.");
            }
        }

        #endregion

        #region 更新转换部分

        /// <summary>
        /// 转换成SQL语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="command"></param>
        /// <param name="tableName"></param>
        public void TranslateInto(Update Update, MySqlCommand command, string tableName)
        {
            string setStr = string.Empty;
            string whereStr = string.Empty;

            TranslateWhere(Update, command, out setStr, out whereStr);

            string sql = string.Format("update {0} {1} {2} ", tableName, setStr, whereStr);
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
        }

        private void TranslateWhere(Update Update, MySqlCommand command, out string setStr, out string whereStr)
        {
            StringBuilder set = new StringBuilder(" SET ");
            StringBuilder where = new StringBuilder(" WHERE ");
            bool _isNotfirstFilterClause = false;//不是第一个条件  

            int n = 0, m = 0, o = 0;
            foreach (Criterion criterion in Update.Criteria)
            {
                if (criterion.updateType != UpdateType.SetFiled)  // 条件
                {
                    //如果不是第一个条件，则加上连接符  
                    if (_isNotfirstFilterClause)
                    {
                        where.Append(GetOperator(Update, n)); n++;
                    }

                    //生成语句,同时生成参数  
                    where.Append(AddFilterClauseFrom(criterion, command, m));

                    _isNotfirstFilterClause = true;
                    m++;

                }
                else// 修改的字段
                {
                    set.Append(AddSetFileds(criterion, command, o)).Append(",");
                    o++;
                }
            }

            setStr = set.ToString().TrimEnd(',');
            whereStr = where.ToString();
        }

        private string AddSetFileds(Criterion criterion, MySqlCommand command, int o)
        {
            command.Parameters.AddWithValue("?pp" + o, criterion.Value);
            command.Parameters["?pp" + o].Direction = criterion.Direction;
            string sql = string.Format(" {0} = ?pp{1} ", criterion.PropertyName, o);

            return sql;
        }

        /// <summary>
        /// 连接符
        /// </summary>
        /// <param name="query"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetOperator(Update u, int index)
        {
            Operator queryOperator = u.UpdateOperator[index];

            switch (queryOperator)
            {
                case Climb.Core.Operator.And:
                    return " AND ";
                case Climb.Core.Operator.Or:
                    return " OR ";
                default:
                    throw new ApplicationException("No QueryOperator defined.");
            }
        }
        #endregion
    }
}
