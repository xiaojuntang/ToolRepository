using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Request
{
    public class Book
    {
        public string BookId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class UserService
    {
        public static void Login(SortedDictionary<string, string> paramter)
        {
            var a = DeserializeHelper.DeserializeJson<List<Book>>(ApiInvoke.BeginExec("/books/getbooks", paramter));
        }
    }
}
