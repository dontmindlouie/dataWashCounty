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
            Console.WriteLine("Begin searching for valid taxLotIDs");

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

            Console.WriteLine("Addresses of extracted tax Lots\n");
            Console.WriteLine(taxLotTempData[0]["SiteAddress"]);
            Console.WriteLine(taxLotTempData[0]["PropertyID"]);
            Console.WriteLine(taxLotTempData[0]["PropertyClass"]);
            Console.WriteLine(taxLotTempData[0]["NeighCode"]);
            Console.WriteLine(taxLotTempData[0]["LatLong"]);
            Console.WriteLine(taxLotTempData[0]["RollDate"]);
            Console.WriteLine(taxLotTempData[0]["TaxCode"]);
            Console.WriteLine(taxLotTempData[0]["MarketLandValue"]);
            Console.WriteLine(taxLotTempData[0]["MarketBldgValue"]);
            Console.WriteLine(taxLotTempData[0]["SpecialMarketValue"]);
            Console.WriteLine(taxLotTempData[0]["MarketTotalValue"]);
            Console.WriteLine(taxLotTempData[0]["TaxableAssessedValue"]);
            Console.WriteLine(taxLotTempData[0]["Legal"]);
            Console.WriteLine(taxLotTempData[0]["LotSize"]);
            Console.WriteLine(taxLotTempData[0]["BldgSqFt"]);
            Console.WriteLine(taxLotTempData[0]["YearBuilt"]);

            //Save data into database
            p.SaveTaxLotData(taxLotTempData);

            Console.WriteLine("\nEnd. Press Enter to Exit.");
            Console.ReadLine();
        }
    }

}
