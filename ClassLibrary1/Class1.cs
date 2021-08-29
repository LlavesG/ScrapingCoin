using System;
using System.Linq;

namespace ClassLibrary1
{
    public class Class1
    {
        public void Main() {
            Console.WriteLine("Comenzar");

            Console.ReadLine();
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
        HtmlAgilityPack.HtmlDocument doc = web.Load("https://es.investing.com/crypto/");

            var HeaderNames = doc.DocumentNode.SelectNodes("//a[@class='genTbl js-top-crypto-table mostActiveStockTbl crossRatesTbl allCryptoTlb wideTbl elpTbl elp15 ']").ToList();
            foreach(var inten in HeaderNames)
            {
                Console.WriteLine(inten.InnerText);
            }

            Console.ReadLine();
        }
    }
}
