using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObserverPattern
{
    class CatOrMuse
    {
    }

    public class Cat
    {
        private string name;

        public Cat(string name)
        {
            this.name = name;
        }

        public delegate void CatShoutEventHandler(object sender, CatShoutEventArgs args);

        public event CatShoutEventHandler CatShout;

        public virtual void OnCatShout()
        {
            Console.WriteLine("喵，我是{0}.", name);
            if (CatShout != null)
            {
                CatShoutEventArgs e = new CatShoutEventArgs();
                e.Name = this.name;
                CatShout(this, e);
            }
        }
    }

    public class Mouse
    {
        private string name;

        public Mouse(string name)
        {
            this.name = name;
        }

        public void Run(object sender, CatShoutEventArgs args)
        {
            Console.WriteLine("老猫{1}来了，{0}快跑！", name, args.Name);
        }
    }

    public class CatShoutEventArgs : EventArgs
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
