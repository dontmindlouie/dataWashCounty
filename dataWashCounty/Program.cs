using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using HtmlAgilityPack;

//  Find all TaxLotIDs
//    HtmlClient to extract raw html of taxLotID list
//    HtmlAgilityPack to parse raw html into taxLotIDs
//  Extract Taxlot data
//    HtmlClient to extract raw html of each property data
//    HtmlAgilityPack to parse raw html into lists
//  Save all data to database

namespace dataWashCounty
{
    class Program
    {
        static void Main(string[] args)
        {
            ExtractTaxlot p = new ExtractTaxlot();

            //Initialize possible tax lot ID combinations
            var searchGroup = p.InitSearchGroup();
            string searchTerm = searchGroup[0][0];

            //Run through possible tax lot ID combinations
            Tuple<IEnumerable<string>, bool> taxLotIDList = p.SearchID(searchTerm);

            Console.WriteLine("\nFinished Searching for TaxLotIDs");
            Console.WriteLine("\nNow extracting data from valid taxlot IDs\n");

            //Create temporary tax lot data storage
            var taxLotTempData = p.CreateTaxLotTempData(taxLotIDList.Item1);

            //Extract data from specified valid tax lot ID
            taxLotTempData = p.ExtractTaxLotData(taxLotIDList.Item1, taxLotTempData);


            Console.WriteLine("\nSite Address of taxLots extracted: " + taxLotTempData[0]["SiteAddress"]);
            Console.WriteLine("\nEnd. Press Enter to Exit.");
            Console.ReadLine();
        }
    }

}
