using AutoMapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = new Book();
            book.Title = "seeeeeeeee";
            book.Description = "ddddddddddd";
            book.Language = "LLLL";
            book.Authors = new List<Author>();
            book.Authors.Add(new Author()
            {
                Name = "1",
                Description = "2",
                ContactInfo = new ContactInfo()
                {
                    Blog = "q",
                    Email = "12@",
                    Twitter = "t"
                }
            });
            book.Authors.Add(new Author()
            {
                Name = "12",
                Description = "22",
                ContactInfo = new ContactInfo()
                {
                    Blog = "q2",
                    Email = "12@2",
                    Twitter = "t2"
                }
            });

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Book, BookDto>();
               
            });

            var exp = Mapper.Map<Book, BookDto>(book);


            // Construct a ConcurrentDictionary
            ConcurrentDictionary<int, int> cd = new ConcurrentDictionary<int, int>();

            // Bombard the ConcurrentDictionary with 10000 competing AddOrUpdates
            Parallel.For(0, 10000, i =>
            {
                // Initial call will set cd[1] = 1.  
                // Ensuing calls will set cd[1] = cd[1] + 1
                cd.AddOrUpdate(1, 1, (key, oldValue) => oldValue + 1);
            });

            Console.WriteLine("After 10000 AddOrUpdates, cd[1] = {0}, should be 10000", cd[1]);

            // Should return 100, as key 2 is not yet in the dictionary
            int value = cd.GetOrAdd(2, (key) => 100);
            Console.WriteLine("After initial GetOrAdd, cd[2] = {0} (should be 100)", value);

            // Should return 100, as key 2 is already set to that value
            value = cd.GetOrAdd(2, 10000);
            Console.WriteLine("After second GetOrAdd, cd[2] = {0} (should be 100)", value);
        }
    }


    #region E

    public class Book
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public decimal Price { get; set; }

        public List<Author> Authors { get; set; }

        public DateTime? PublishDate { get; set; }

        public Publisher Publisher { get; set; }

        public int? Paperback { get; set; }

    }

    public class Publisher
    {
        public string Name { get; set; }

    }

    public class Author
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public ContactInfo ContactInfo { get; set; }

    }

    public class ContactInfo
    {
        public string Email { get; set; }

        public string Blog { get; set; }

        public string Twitter { get; set; }

    }
    #endregion

    public class BookDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public decimal Price { get; set; }

        public DateTime? PublishDate { get; set; }

        public string Publisher { get; set; }

        public int? Paperback { get; set; }

        public string FirstAuthorName { get; set; }

        public string FirstAuthorDescription { get; set; }

        public string FirstAuthorEmail { get; set; }

        public string FirstAuthorBlog { get; set; }

        public string FirstAuthorTwitter { get; set; }

        public string SecondAuthorName { get; set; }

        public string SecondAuthorDescription { get; set; }

        public string SecondAuthorEmail { get; set; }

        public string SecondAuthorBlog { get; set; }

        public string SecondAuthorTwitter { get; set; }

    }
}
