using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CriptoScrapingLibrary
{
    public class CriptoScraping
    {
        private string UrlScraping = string.Empty;
        public CriptoScraping(string _urlScraping)
        {
            UrlScraping = _urlScraping;
        }

        public Dictionary<string, double> GetCriptosValues( string url = "")
        {

            HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            //HtmlDocument doc = web.Load("https://es.investing.com/crypto/");
            if (string.IsNullOrEmpty(url))
            {
                url = UrlScraping;
            }
            HtmlDocument doc = web.Load(url);
            List<HtmlNode> listNode = new List<HtmlNode>();
            Dictionary<string, double> dictionaryCoin = new Dictionary<string, double>();



            listNode = doc.DocumentNode.Descendants("table").Where(node => !node.GetAttributeValue("class", "").Contains("left bold elp name cryptoName first js-currency-name")).ToList();

            var rowsTr = listNode[0].InnerHtml.Split("<tr").Where(x => x.Contains(@" i=")).ToList();

            dictionaryCoin = ConvertToDictionaryCoin(rowsTr);

            
            return dictionaryCoin;

        }
        public Dictionary<string, Tuple<string, string>> GetCriptosDropList()
        {

            HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            //HtmlDocument doc = web.Load("https://es.investing.com/crypto/");
            HtmlDocument doc = web.Load(UrlScraping);
            List<HtmlNode> listNode = new List<HtmlNode>();
            Dictionary<string, Tuple<string, string>> dictionaryCoin = new Dictionary<string, Tuple<string, string>>();



            listNode = doc.DocumentNode.Descendants("table").Where(node => !node.GetAttributeValue("class", "").Contains("left bold elp name cryptoName first js-currency-name")).ToList();

            var rowsTr = listNode[2].InnerHtml.Split("<tr").Where(x => x.Contains(@"cmc-table-row")).ToList();
            //var rowsTr = listNode[2].InnerHtml.Split("<tr").ToList();

            dictionaryCoin = ConvertDictioraryDropList(rowsTr);


            return dictionaryCoin;

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
        private  Dictionary<string, Tuple<string,string>> ConvertDictioraryDropList(List<string> rowsTr)
        {
            Dictionary<string, Tuple<string, string>> dictionaryCoin = new Dictionary<string, Tuple<string, string>>();
            try
            {
                foreach (string item in rowsTr)
            {
                var columTd = item.Split("<td").ToList();
               string columTdFilter = columTd.Where(x => x.Contains("cmc-table__cell cmc-table__cell--sticky cmc-table__cell--sortable cmc-table__cell--left cmc-table__cell--sort-by__name")).FirstOrDefault();
                if (! string.IsNullOrEmpty(columTdFilter)) { 
                    dictionaryCoin.Add(GetCoinDropKey(columTdFilter), new Tuple<string, string>(GetCoinDropName(columTdFilter), GetCoinDropImg(columTdFilter)));
                 }
                }
            }
            catch (Exception ex)
            {
                dictionaryCoin = null;
            }

            dictionaryCoin = CleanDictionaryDropList(dictionaryCoin);
            return dictionaryCoin;
        }

        private  Dictionary<string, Tuple<string, string>> CleanDictionaryDropList(Dictionary<string, Tuple<string, string>> dictionaryCoin)
        {
            Dictionary<string, double> dicOrigin = GetCriptosValues(url:"https://es.investing.com/crypto/");
            Dictionary<string, Tuple<string, string>> dictionaryCoinAux = new Dictionary<string, Tuple<string, string>>();

            foreach (string originKey in dicOrigin.Keys)
            {
                if (dictionaryCoin.Keys.Contains(originKey))
                {
                    dictionaryCoinAux.Add(originKey, dictionaryCoin.GetValueOrDefault(originKey));
                }
            }
            return (dictionaryCoinAux);
        }

        private static string GetCoinName(List<string> columTd)
        {
            string td = columTd.Where(x => x.Contains("symbol")).FirstOrDefault();
            td = td.Substring(td.IndexOf(">") + 1);
            td = td.Replace("</td>", string.Empty);
            td = td.Replace("\n", string.Empty);
            return td;
        }

        private static double GetCoinValue(List<string> columTd)
        {
            double result = 0;
            string td = columTd.Where(x => x.Contains("price")).FirstOrDefault();
            td = td.Replace(@"</a></td>", string.Empty);

            td = td.Substring(td.LastIndexOf(">") + 1);
            td = td.Replace("\n", string.Empty);
            if (!double.TryParse(td, out result))
            {
                result = 0;
            }
            return result;
        }
        private static string GetCoinDropImg(string columTd)
        {
            columTd = columTd.Substring(columTd.IndexOf("img src") + 9);
            columTd = columTd.Substring(0, columTd.IndexOf(".png") + 4);
            columTd = columTd.Replace("</td>", string.Empty);
            return columTd;
        }
        private static string GetCoinDropName(string columTd)
        {
            columTd = columTd.Substring(columTd.IndexOf("title=") + 7);
            columTd = columTd.Substring(0, columTd.IndexOf("class") - 2);
          
            return columTd;
        }
        private static string GetCoinDropKey(string columTd)
        {
            columTd = columTd.Substring(columTd.IndexOf("symbol cmc-link") + 17);
            columTd = columTd.Substring(0, columTd.IndexOf("<"));
            return columTd;
        }

    }
}
