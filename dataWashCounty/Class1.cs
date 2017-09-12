using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace dataWashCounty
{
    class ExtractTaxlot
    {
        public List<List<string>> InitSearchGroup()
        {
            List<List<string>> searchGroup = new List<List<string>>
            {
                new List<string> {"1","2","3" },
                new List<string> { "N", "S" },
                new List<string> { "0", "1", "2", "3", "4", "5", "6" },
                new List<string> { "0", "1", "2", "3" },
                new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" },
                new List<string> { "0", "A", "B", "C", "D" },
                new List<string> { "0", "A", "B", "C", "D" },
                new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" },
                new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }
            };
            return searchGroup;
        }
        public IEnumerable<string> SearchID(string searchTerm)
        {
            string rawHtmlIDList = findTaxLotID(searchTerm).Result;
            //Console.WriteLine(rawHtmlIDList);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(rawHtmlIDList);
            HtmlNodeCollection taxLotIDNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table//tr/td/table//tr//td//a");
            var IDNumber = taxLotIDNode.Select(node => node.InnerText);
            return IDNumber;
        }
        public static async Task<string> findTaxLotID(string searchTerm)
        {
            var responseString = "";
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    {"theTLNO", searchTerm},
                    {"theAddress", ""},
                    {"theRnum", ""},
                    {"OwnerName", ""},
                    {"btnSubmit", "Search"}
                };
                client.BaseAddress = new Uri(@"http://washims.co.washington.or.us");
                var content = new FormUrlEncodedContent(values);
                //HttpRequestHeaders atPostHeader = client.DefaultRequestHeaders; // what does this even do?
                var response = await client.PostAsync(@"/GIS/index.cfm?id=20&sid=2", content);
                responseString = await response.Content.ReadAsStringAsync();
            }
            return responseString;
        }

    }
}
