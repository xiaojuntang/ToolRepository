using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrategyPattern
{
    /// <summary>
    /// http://www.cnblogs.com/promise-7/articles/2524357.html
    /// 策略模式并不负责做这个决定。换言之，应当由客户端自己决定在什么情况下使用什么具体策略角色。
    /// 策略模式仅仅封装算法，提供新算法插入到已有系统中，以及老算法从系统中"退休"的方便，策略模式并不决定在何时使用何种算法
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            #region MyRegion
            // Three contexts following different strategies
            Context c = new Context(new ConcreteStrategyA());
            c.ContextInterface();

            Context d = new Context(new ConcreteStrategyB());
            d.ContextInterface();

            Context e = new Context(new ConcreteStrategyC());
            e.ContextInterface(); 
            #endregion

            #region MyRegion
            SortedList studentRecords = new SortedList();
            studentRecords.Add("Samual");
            studentRecords.Add("Jimmy");
            studentRecords.Add("Sandra");
            studentRecords.Add("Anna");
            studentRecords.Add("Vivek");

            studentRecords.SetSortStrategy(new QuickSort());
            studentRecords.Sort();
            studentRecords.Display(); 
            #endregion

            Console.ReadLine();
        }
    }

    /// <summary>
    /// 抽象策略的角色
    /// </summary>
    public abstract class Strategy
    {
        abstract public void AlgorithmInterface();
    }

    #region 具体策略
    /// <summary>
    /// 具体策略ConcreteStrategyA
    /// </summary>
    public class ConcreteStrategyA : Strategy
    {
        public override void AlgorithmInterface()
        {
            Console.WriteLine("Called ConcreteStrategyA.AlgorithmInterface()");
        }
    }

    /// <summary>
    /// 具体策略ConcreteStrategyB
    /// </summary>
    public class ConcreteStrategyB : Strategy
    {
        public override void AlgorithmInterface()
        {
            Console.WriteLine("Called ConcreteStrategyB.AlgorithmInterface()");
        }
    }

    /// <summary>
    /// 具体策略ConcreteStrategyC
    /// </summary>
    public class ConcreteStrategyC : Strategy
    {
        public override void AlgorithmInterface()
        {
            Console.WriteLine("Called ConcreteStrategyC.AlgorithmInterface()");
        }
    }
    #endregion

    public class Context
    {
        Strategy strategy;
        public Context(Strategy strategy)
        {
            this.strategy = strategy;
        }

        public void ContextInterface()
        {
            strategy.AlgorithmInterface();
        }
    }


    /// <summary>
    /// 排序算法策略----------------------------------------------------------------------------------------------------------------------
    /// </summary>
    abstract class SortStrategy
    {
        abstract public void Sort(ArrayList list);
    }

    /// <summary>
    /// 排序算法1
    /// </summary>
    class QuickSort : SortStrategy
    {
        public override void Sort(ArrayList list)
        {
            list.Sort();
            Console.WriteLine("QuickSorted list ");
        }
    }

    /// <summary>
    ///  排序算法2
    /// </summary>
    class ShellSort : SortStrategy
    {
        public override void Sort(ArrayList list)
        {
            //list.ShellSort();
            Console.WriteLine("ShellSorted list ");
        }
    }

    /// <summary>
    ///  排序算法3
    /// </summary>
    class MergeSort : SortStrategy
    {
        public override void Sort(ArrayList list)
        {
            Console.WriteLine("MergeSorted list ");
        }
    }

    /// <summary>
    ///  排序算法Context
    /// </summary>
    class SortedList
    {
        // Fields
        private ArrayList list = new ArrayList();
        private SortStrategy sortstrategy;

        // Constructors
        public void SetSortStrategy(SortStrategy sortstrategy)
        {
            this.sortstrategy = sortstrategy;
        }

        // Methods
        public void Sort()
        {
            sortstrategy.Sort(list);
        }

        public void Add(string name)
        {
            list.Add(name);
        }

        public void Display()
        {
            foreach (string name in list)
                Console.WriteLine(" " + name);
        }
    }
}
