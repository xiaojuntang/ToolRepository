using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artech.Transactions
{
    public class CommittableTransaction : Transaction
    {
        public CommittableTransaction(DbTransaction dbTransaction)
        {
            this.DbTransactionWrapper = new DbTransactionWrapper(dbTransaction);
        }
        public void Commit()
        {
            this.DbTransactionWrapper.Commit();
        }
    }
}
