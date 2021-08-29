using System;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace ScrapingConsole
{
    class ScrapingProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Comenzamos");
            Console.ReadLine();
            
            HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlDocument doc = web.Load("https://es.investing.com/crypto/");
            List<HtmlNode> listNode = new List<HtmlNode>();
            Dictionary<string, double> dictionaryCoin = new Dictionary<string, double>();
             

            
            listNode = doc.DocumentNode.Descendants("table") .Where(node => !node.GetAttributeValue("class", "").Contains("left bold elp name cryptoName first js-currency-name")).ToList();

            var rowsTr = listNode[0].InnerHtml.Split("<tr").Where(x => x.Contains(@" i=")).ToList();

            dictionaryCoin = ConvertToDictionaryCoin(rowsTr);

            printDictionary(dictionaryCoin);

            Console.ReadLine();

        }

        private static void printDictionary(Dictionary<string, double> dictionaryCoin)
        {
            Console.WriteLine("Resultados -->");
           foreach (var keydic in dictionaryCoin.Keys)
            {
                Console.WriteLine("Coin: " + keydic + " Precio: " + dictionaryCoin.GetValueOrDefault(keydic));
            }
            Console.WriteLine("Fin del proceso");
        }

        private static Dictionary<string, double> ConvertToDictionaryCoin(List<string> rowsTr)
        {
            Dictionary<string, double> dictionaryCoin = new Dictionary<string, double>();

            foreach (string item in rowsTr)
            {
                var columTd = item.Split("<td").ToList();
                
                dictionaryCoin.Add(GetCoinName(columTd), GetCoinValue(columTd));
            }
            return dictionaryCoin;
        }
        private static string GetCoinName(List<string> columTd)
        {
            string td = columTd.Where(x => x.Contains("symbol")).FirstOrDefault();
            td = td.Substring(td.IndexOf(">") + 1);
            td = td.Replace("</td>", string.Empty);
            return td;
        }

        private static double GetCoinValue(List<string> columTd)
        {
            double result = 0;
            string td = columTd.Where(x => x.Contains("price")).FirstOrDefault();
            td = td.Replace(@"</a></td>", string.Empty);
            
            td = td.Substring(td.LastIndexOf(">") + 1);
            td = td.Replace("\n", "");
            if (! double.TryParse(td, out result))
            {
                result = 0;
            }
            return result;
        }

       
    }
}
