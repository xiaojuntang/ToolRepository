using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artech.Transactions
{
    public class DbTransactionWrapper : IDisposable
    {
        public DbTransactionWrapper(DbTransaction transaction)
        {
            this.DbTransaction = transaction;
        }
        public DbTransaction DbTransaction { get; private set; }
        public bool IsRollBack { get; set; }
        public void Rollback()
        {
            if (!this.IsRollBack)
            {
                this.DbTransaction.Rollback();
            }
        }
        public void Commit()
        {
            this.DbTransaction.Commit();
        }
        public void Dispose()
        {
            this.DbTransaction.Dispose();
        }
    }
}
