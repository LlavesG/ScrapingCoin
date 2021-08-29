using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ReadLine();
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://es.investing.com/crypto/");

            var programmerLinks = doc.DocumentNode.Descendants("table");

           

            Console.ReadLine();
        }
    
    }
}
