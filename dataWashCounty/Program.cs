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
            var searchGroup2 = p.InitSearchGroup();

            for(int i = 0; i < searchGroup2.Count(); i++)
            {
                for (int j = 0; j < searchGroup2[i].Count; j++)
                {
                    Console.WriteLine(searchGroup2[i][j]);
                }
            }
            Console.WriteLine("\nEnd of search term list.");
            Console.WriteLine("\nPress Enter to extract taxlotID html");
            Console.ReadLine();

            string searchTerm1 = "1";
            IEnumerable<string> taxLotIDList = p.SearchID(searchTerm1);
            foreach(var entry1 in taxLotIDList)
            {
                Console.WriteLine(entry1);
            }

            Console.WriteLine("\nEnd. Press Enter to Exit.");
            Console.ReadLine();
        }
    }

}
