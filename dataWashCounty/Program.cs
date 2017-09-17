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
            Console.WriteLine("\nPress enter to cycle through taxLotIDs");
            Console.ReadLine();
            ExtractTaxlot p = new ExtractTaxlot();
            var searchGroup = p.InitSearchGroup(); //initialize list of possible search term combinations
            Console.WriteLine("\nEnd of search term list.");

            Console.WriteLine("\nPress Enter to extract taxlotID html");
            Console.ReadLine();
            bool resultCap = false;
            string searchTerm = searchGroup[0][0];
            Tuple<IEnumerable<string>,bool> taxLotIDList = p.SearchID(searchTerm);
            //foreach(var entry in taxLotIDList){
            //    Console.WriteLine(entry);
            //}
            Console.WriteLine(taxLotIDList.Item2);
            Console.WriteLine("\nEnd. Press Enter to Exit.");
            Console.ReadLine();
        }
    }

}
