using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObserverPattern
{
    /// <summary>
    /// 利用事件和委托来实现Observer模式
    /// 观察者模式定义了一种一对多的依赖关系，让多个观察者对象同时监听某一个主题对象，
    /// 这个主题对象在状态发生变化时，会通知所有观察者对象，使它们能够自动更新自己的行为。
    /// 一个是观察者对象，另一个就是主题对象
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Cat cat = new Cat("Tom");
            Mouse mouse1 = new Mouse("Jerry");
            Mouse mouse2 = new Mouse("Jack");
            Mouse mouse3 = new Mouse("Ma");
            Mouse mouse4 = new Mouse("Skey");
            cat.CatShout += mouse1.Run;
            cat.CatShout += new Cat.CatShoutEventHandler(mouse2.Run);
            cat.CatShout += new Cat.CatShoutEventHandler(mouse3.Run);
            cat.CatShout += new Cat.CatShoutEventHandler(mouse4.Run);
            cat.OnCatShout();
            //可用于群发短信和邮件


            Console.ReadLine();

            Customer subject1 = new Customer();
            for (int i = 0; i < 50; i++)
            {
                UserInfo user = new UserInfo("用户" + i);
                subject1.Update += new CustomerEventHandler(user.AcceptMsg);
            }
            subject1.Notify();


            Customer subject = new Customer();

            Accountant account = new Accountant();
            Cashier cashier = new Cashier();
            Dilliveryman dilliver = new Dilliveryman();

            subject.Update += new CustomerEventHandler(account.GiveInvoice);
            subject.Update += new CustomerEventHandler(cashier.Recoded);
            subject.Update += new CustomerEventHandler(dilliver.Dilliver);
            subject.Notify();

            Console.ReadLine();
        }
    }
}
