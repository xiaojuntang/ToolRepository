using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppDI
{
    class DIKid
    {
    }

    public interface IFoodSupplier
    {
       string GetFood();
    }

    public class Kid
    {
        private readonly IFoodSupplier _foodSupplier;
        public Kid(IFoodSupplier foodSupplier)
        {
            _foodSupplier = foodSupplier;
        }
        public void HaveAMeal()
        {
            var food = _foodSupplier.GetFood();        //eat
        }
    }
}
