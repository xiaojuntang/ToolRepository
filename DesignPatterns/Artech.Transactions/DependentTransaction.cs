using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artech.Transactions
{
    public class DependentTransaction : Transaction
    {
        public Transaction InnerTransaction { get; private set; }
        internal DependentTransaction(Transaction innerTransaction)
        {
            this.InnerTransaction = innerTransaction;
            this.DbTransactionWrapper = this.InnerTransaction.DbTransactionWrapper;
        }
    }
}
