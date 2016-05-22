using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Artech.Transactions
{
    public abstract class Transaction : IDisposable
    {
        [ThreadStatic]
        private static Transaction current;

        public bool Completed { get; private set; }
        public DbTransactionWrapper DbTransactionWrapper { get; protected set; }
        protected Transaction() { }
        public void Rollback()
        {
            this.DbTransactionWrapper.Rollback();
        }
        public DependentTransaction DependentClone()
        {
            return new DependentTransaction(this);
        }
        public void Dispose()
        {
            this.DbTransactionWrapper.Dispose();
        }
        public static Transaction Current
        {
            get { return current; }
            set { current = value; }
        }
    }
}
