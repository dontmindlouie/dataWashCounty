using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace dataWashCounty
{
    public class ExtractTaxlot
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
        public Tuple<IEnumerable<string>,bool> SearchID(string searchTerm)
        {
            //Parameters    The search term to put into the website's search function to obtain a list of valid tax lot IDs
            //Functions     Access the website and use the search term to access list of valid tax lot IDs
            //              Extract list of valid tax lot IDs
            //              Figure out if the search result reached maximum capacity
            //Returns       List of valid tax lot IDs as an IEnumerable
            //              Status of the search's maximum capacity. True if capacity is reached, False if not.
            Console.WriteLine("\nTerm to search: " + searchTerm);
            string rawHtmlIDList = findTaxLotID(searchTerm).Result;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(rawHtmlIDList);
            HtmlNodeCollection taxLotIDNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table//tr/td/table//tr//td//a");
            var IDNumber = taxLotIDNode.Select(node => node.InnerText);
            Console.WriteLine("First valid taxlot ID of thi search: " + IDNumber.ElementAt(0));
            Console.WriteLine("Count of valid taxlot IDs of this search: " + IDNumber.Count());
            bool resultCap = false;
            resultCap = rawHtmlIDList.Contains("Search exceeded the maximum return limit.");
            Console.WriteLine("Result Capacity Reached: " + resultCap.ToString());
            return new Tuple<IEnumerable<string>,bool>(IDNumber,resultCap);
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
                Console.WriteLine("Accessed site");
                responseString = await response.Content.ReadAsStringAsync();
            }
            return responseString;
        }
        public List<Dictionary<string,string>> CreateTaxLotTempData(IEnumerable<string> validTaxLotIDs)
        {
            List<Dictionary<string, string>> taxLotTempData = new List<Dictionary<string, string>>();
            return taxLotTempData;
        }
        public List<Dictionary<string,string>> ExtractTaxLotData(IEnumerable<string> validTaxLotIDs, List<Dictionary<string,string>> taxLotTempData)
        {
            //Parameters    List of valid taxlot ids to extract data from 
            //              Sets of previous data to put data into
            //Functions     Access website with the tax lot ID
            //              Extract data from the website
            //              Adds another key/value pair 
            //Returns       The list of key/value pairs that contains the tax lot data
            Console.WriteLine("TaxLot ID to extract data from: " + validTaxLotIDs.ElementAt(0));
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument htmlDoc = hw.Load("http://washims.co.washington.or.us/GIS/index.cfm?id=30&sid=3&IDValue=" + validTaxLotIDs.ElementAt(0));
            Console.WriteLine("Accessed site\n");
            HtmlNodeCollection siteAddressNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[3]//tr[2]/td[2]");
            var siteAddress = siteAddressNode.Select(node => node.InnerText);
            taxLotTempData.Add(new Dictionary<string, string>());
            taxLotTempData[0].Add("TaxLotID", validTaxLotIDs.ElementAt(0));
            taxLotTempData[0].Add("SiteAddress", siteAddress.ElementAt(0));
            return taxLotTempData;
        }

    }
}
