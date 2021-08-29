using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    class Program
    {

        public void Main()
        {
            Console.WriteLine("Comenzar");

            Console.ReadLine();
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://es.investing.com/crypto/");

            var HeaderNames = doc.DocumentNode.SelectNodes("//a[@class='genTbl js-top-crypto-table mostActiveStockTbl crossRatesTbl allCryptoTlb wideTbl elpTbl elp15 ']").ToList();
            foreach (var inten in HeaderNames)
            {
                Console.WriteLine(inten.InnerText);
            }

            Console.ReadLine();
        }
    }
}
