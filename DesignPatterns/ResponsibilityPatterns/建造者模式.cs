using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponsibilityPatterns
{
    /// <summary>
    /// http://www.banzg.com/archives/812.html
    /// </summary>
    public class 建造者模式
    {
        public void Main1()
        {
            MealBuilder mealBuilder = new MealBuilder();

            Meal vegMeal = mealBuilder.prepareVegMeal();
            Console.WriteLine("Veg Meal");
            vegMeal.ShowItem();
            Console.WriteLine("Total Cost: " + vegMeal.getCost());

            Meal nonVegMeal = mealBuilder.prepareNonVegMeal();
            Console.WriteLine("\n\nNon-Veg Meal");
            nonVegMeal.ShowItem();
            Console.WriteLine("Total Cost: " + nonVegMeal.getCost());
        }
    }

    /// <summary>
    /// 食物
    /// </summary>
    public interface Item
    {
        /// <summary>
        /// 食物名称
        /// </summary>
        string name();
        /// <summary>
        /// 包装容器（纸盒、瓶）
        /// </summary>
        Packing packing();
        /// <summary>
        /// 价格
        /// </summary>
        float price();
    }

    /// <summary>
    /// 包装容器
    /// </summary>
    public interface Packing
    {
        string pack();
    }

    /// <summary>
    /// 纸盒 容器
    /// </summary>
    public class Wrapper : Packing
    {
        public string pack()
        {
            return "Wrapper";
        }
    }

    /// <summary>
    /// 瓶 容器
    /// </summary>
    public class Bottle : Packing
    {
        public string pack()
        {
            return "Bottle";
        }
    }

    /// <summary>
    /// 汉堡 对象
    /// </summary>
    public abstract class Burger : Item
    {
        public abstract string name();

        public Packing packing()
        {
            return new Wrapper();
        }

        public abstract float price();
    }

    /// <summary>
    /// 冷饮
    /// </summary>
    public abstract class ColdDrink : Item
    {
        public abstract string name();

        public Packing packing()
        {
            return new Bottle();
        }

        public abstract float price();
    }

    /// <summary>
    /// veg 汉堡
    /// </summary>
    public class VegBurger : Burger
    {
        public override string name()
        {
            return "VegBurger";
        }

        public override float price()
        {
            return 25.0f;
        }
    }

    /// <summary>
    /// 鸡肉汉堡
    /// </summary>
    public class ChickenBurger : Burger
    {
        public override string name()
        {
            return "ChickenBurger";
        }

        public override float price()
        {
            return 35.0f;
        }
    }

    /// <summary>
    /// Coca-Cola口乐
    /// </summary>
    public class Coke : ColdDrink
    {
        public override string name()
        {
            return "Coke";
        }

        public override float price()
        {
            return 55f; ;
        }
    }

    /// <summary>
    /// 百事可乐
    /// </summary>
    public class Pepsi : ColdDrink
    {
        public override string name()
        {
            return "Pepsi";
        }

        public override float price()
        {
            return 65f; ;
        }
    }

    /// <summary>
    /// 餐
    /// </summary>
    public class Meal
    {
        private List<Item> items = new List<Item>();

        public void AddItem(Item item)
        {
            items.Add(item);
        }
        public float getCost()
        {
            float cost = 0.0f;
            items.ForEach(p=> {
                cost += p.price();
            });           
            return cost;
        }

        public void ShowItem()
        {
            items.ForEach(p => {
                Console.WriteLine("{0}-{1}-{2}", 
                    p.name(), 
                    p.packing().pack(), 
                    p.price());
            });
        }
    }

    public class MealBuilder
    {
        /// <summary>
        /// 套餐一
        /// </summary>
        /// <returns></returns>
        public Meal prepareVegMeal()
        {
            Meal meal = new Meal();
            meal.AddItem(new VegBurger());
            meal.AddItem(new Coke());
            return meal;
        }

        /// <summary>
        /// 套餐二
        /// </summary>
        /// <returns></returns>
        public Meal prepareNonVegMeal()
        {
            Meal meal = new Meal();
            meal.AddItem(new ChickenBurger());
            meal.AddItem(new Pepsi());
            return meal;
        }
    }
}
