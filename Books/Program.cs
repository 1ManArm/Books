using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace Books
{
    internal class Program
    {
        static void Main(string[] args)
        {
            XDocument xmlDoc = XDocument.Load("C:\\Users\\LysyyKiller\\Documents\\books.xml");
            var books = xmlDoc.Root.Elements("book");

            var sortedBooks = books.OrderBy(book => book.Element("title").Value);
            Console.WriteLine("По названию: ");
            foreach ( var book in sortedBooks )
            {
                Console.WriteLine(book.Element("title").Value);
            }

            var genreCounts = books
                .GroupBy(book => book.Element("genre").Value)
                .Select(group => new 
                { 
                    Genre = group.Key,
                    Count = group.Count() 
                });
            Console.WriteLine("\nПо жанрам: ");
            foreach(var genreCount in genreCounts )
            {
                Console.WriteLine($"{genreCount.Genre}: {genreCount.Count}");
            }

            var oldAuthors = books
                .Where(book => int.Parse(book.Element("year").Value)<1900)
                .Select(book => book.Element("year").Value)
                .Distinct();
            Console.WriteLine("\nАвторы с книгами до 1900 года: ");
            foreach(var author in oldAuthors)
            {
                Console.WriteLine(author);
            }

            var prolificAuthors = books
                .GroupBy(book => book.Element("author").Value)
                .Where(group => group.Count() >= 2)
                .Select(group => group.Key);
            Console.WriteLine("\nДве или более книги: ");
            foreach (var author in prolificAuthors)
            {
                Console.WriteLine(author);
            }

            var multiwordBooks = books
                .Where(book => book.Element("title").Value.Split(' ').Length > 1)
                .Select(book => new
                {
                    Title = book.Element("title").Value,
                    Author = book.Element("author").Value,
                    Year = int.Parse(book.Element("year").Value),
                    Genre = book.Element("genre").Value
                });
            Console.WriteLine("\nКниги с многосложными названиями: ");
            Console.WriteLine($"Количество: {multiwordBooks.Count()}");
            foreach(var book in multiwordBooks)
            {
                Console.WriteLine($"{book.Title} ({book.Author}, {book.Year}, {book.Genre}");
            }

            var authorsAndBooks = books
                .Where(book =>
                {
                    int year = int.Parse(book.Element("year").Value);
                    return year >= 1940 && year <= 2000;
                })
                .Select(book => new
                {
                    Author = book.Element("author").Value,
                    Title = book.Element("title").Value
                });
            Console.WriteLine("\nМежду 1940 и 2000: ");
            foreach(var item in authorsAndBooks) 
            {
                Console.WriteLine($"{item.Author}: {item.Title}");
            }
        }
    }
}