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
            HtmlNodeCollection propertyIDNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[3]//tr[4]/td[2]");
            var propertyID = propertyIDNode.Select(node => node.InnerText);
            HtmlNodeCollection propertyClassNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[3]//tr[5]/td[2]/text()");
            var propertyClass = propertyClassNode.Select(node => node.InnerText);
            HtmlNodeCollection neighCodeNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[3]//tr[6]/td[2]");
            var neighCode = neighCodeNode.Select(node => node.InnerText);
            HtmlNodeCollection latLongNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[3]//tr[7]/td[2]");
            var latLong = latLongNode.Select(node => node.InnerText);
            HtmlNodeCollection saleDateNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[4]//tr[3]/td[1]");
            var saleDate = saleDateNode.Select(node => node.InnerText);
            HtmlNodeCollection saleInstrNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[4]//tr[3]/td[2]");
            var saleInstr = saleInstrNode.Select(node => node.InnerText);
            HtmlNodeCollection saleDeedNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[4]//tr[3]/td[3]");
            var saleDeed = saleDeedNode.Select(node => node.InnerText);
            HtmlNodeCollection salePriceNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[4]//tr[3]/td[4]");
            var salePrice = salePriceNode.Select(node => node.InnerText);
            HtmlNodeCollection rollDateNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[2]/td[2]");
            var rollDate = rollDateNode.Select(node => node.InnerText);
            HtmlNodeCollection taxCodeNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[3]/td[2]");
            var taxCode = taxCodeNode.Select(node => node.InnerText);
            HtmlNodeCollection marketLandValueNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[4]/td[2]");
            var marketLandValue = marketLandValueNode.Select(node => node.InnerText);
            HtmlNodeCollection marketBldgValueNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[5]/td[2]");
            var marketBldgValue = marketBldgValueNode.Select(node => node.InnerText);
            HtmlNodeCollection specialMarketValueNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[6]/td[2]");
            var specialMarketValue = specialMarketValueNode.Select(node => node.InnerText);
            HtmlNodeCollection marketTotalValueNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[7]/td[2]");
            var marketTotalValue = marketTotalValueNode.Select(node => node.InnerText);
            HtmlNodeCollection taxableAssessedValueNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[8]/td[2]");
            var taxableAssessedValue = taxableAssessedValueNode.Select(node => node.InnerText);
            HtmlNodeCollection legalNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[9]/td[2]");
            var legal = legalNode.Select(node => node.InnerText);
            HtmlNodeCollection lotSizeNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[10]/td[2]");
            var lotSize = lotSizeNode.Select(node => node.InnerText);
            HtmlNodeCollection bldgSqFtNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[11]/td[2]");
            var bldgSqFt = bldgSqFtNode.Select(node => node.InnerText);
            HtmlNodeCollection yearBuiltNode = htmlDoc.DocumentNode.SelectNodes("/html/body/table[3]//tr/td[3]/table[5]//tr[12]/td[2]");
            var yearBuilt = yearBuiltNode.Select(node => node.InnerText);
            taxLotTempData.Add(new Dictionary<string, string>());
            taxLotTempData[0].Add("TaxLotID", validTaxLotIDs.ElementAt(0));
            taxLotTempData[0].Add("SiteAddress", siteAddress.ElementAt(0));
            taxLotTempData[0].Add("PropertyID", propertyID.ElementAt(0));
            taxLotTempData[0].Add("PropertyClass", propertyClass.ElementAt(0));
            taxLotTempData[0].Add("NeighCode", neighCode.ElementAt(0));
            taxLotTempData[0].Add("LatLong", latLong.ElementAt(0));
            taxLotTempData[0].Add("SaleDate", saleDate.ElementAt(0));
            taxLotTempData[0].Add("SaleInstr", saleInstr.ElementAt(0));
            taxLotTempData[0].Add("SaleDeed", saleDeed.ElementAt(0));
            taxLotTempData[0].Add("SalePrice", salePrice.ElementAt(0));
            taxLotTempData[0].Add("RollDate", rollDate.ElementAt(0));
            taxLotTempData[0].Add("TaxCode", taxCode.ElementAt(0));
            taxLotTempData[0].Add("MarketLandValue", marketLandValue.ElementAt(0));
            taxLotTempData[0].Add("MarketBldgValue", marketBldgValue.ElementAt(0));
            taxLotTempData[0].Add("SpecialMarketValue", specialMarketValue.ElementAt(0));
            taxLotTempData[0].Add("MarketTotalValue", marketTotalValue.ElementAt(0));
            taxLotTempData[0].Add("TaxableAssessedValue", taxableAssessedValue.ElementAt(0));
            taxLotTempData[0].Add("Legal", legal.ElementAt(0));
            taxLotTempData[0].Add("LotSize", lotSize.ElementAt(0));
            taxLotTempData[0].Add("BldgSqFt", bldgSqFt.ElementAt(0));
            taxLotTempData[0].Add("YearBuilt", yearBuilt.ElementAt(0));

            return taxLotTempData;
        }
        public void SaveTaxLotData(List<Dictionary<string,string>> taxLotTempData) {
            string connectstring = "Data Source=DESKTOP-5LVATAU\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";
            //string insertQuery = "I"

        }

    }
}
