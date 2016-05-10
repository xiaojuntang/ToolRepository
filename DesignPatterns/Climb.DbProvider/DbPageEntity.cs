using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Climb.DbProvider
{
    public abstract class DbPageEntity
    {
        protected DbPageEntity(string selectFiled, string orderbyFiled, string tableNameWhere, int pageSize = 10, int pageCount = 0, params IDataParameter[] dataParameters)
        {
            this.PageCount = pageCount;
            this.PageSize = pageSize;
            this.SelectFiled = selectFiled;
            this.TableNameAndWhere = tableNameWhere;
            this.OrderByFiled = orderbyFiled;
            this.DbParameters = dataParameters;
        }

        public abstract string GetComandCountSqlStr();
        public abstract string GetComandSqlStr(PageEnum sqlEnum);
        public abstract IDataParameter[] GetPageParams(PageEnum sqlEnum);

        public IDataParameter[] DbParameters { get; set; }

        public string OrderByFiled { get; private set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public string SelectFiled { get; private set; }

        public string TableNameAndWhere { get; private set; }
    }

    public enum PageEnum : byte
    {
        Id = 1,
        Nomal = 0
    }
}
