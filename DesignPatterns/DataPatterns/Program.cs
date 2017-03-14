using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataPatterns
{
    class Program
    {
        static void Main(string[] args)
        {

            //using (TransactionScope scope = new TransactionScope())
            //{
            //    //SqlConnection connection1 = new SqlConnection(connectString1)
            //    //SqlConnection connection2 = new SqlConnection(connectString2)
            //    scope.Complete();
            //}
        }

        public void GetUserIncomeDtos(IQueryable<User> users, IQueryable<Income> incomes)
        {
            incomes.AsQueryable().GroupBy(p => new { p.Id, p.Year, p.Month }).Select(p => new { p.Key.Id, p.Key.Year, p.Key.Month, a = p.Sum(m => m.Amount) });
        }


    }


    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Income
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class UserIncomeDto
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Income { get; set; }
    }

    /// <summary>
    /// 表达式树 Func<>
    /// </summary>
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public void Validate1()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new Exception("please enter a name for the product");
            }
            if (string.IsNullOrEmpty(this.Description))
            {
                throw new Exception("product description is required");
            }
        }

        public void Validate2()
        {
            this.Require(x => x.Name, "please enter a name for the product");
            this.Require(x => x.Description, "product description is required");
        }

        public void Require(Expression<Func<Product, string>> model, string error)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new Exception(error);
            }
        }
    }
}
