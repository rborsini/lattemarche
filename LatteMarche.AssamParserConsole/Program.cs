using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.AssamParserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var reports = ExcelParser.Parse(@"asset\LatteBovinoAnalisiconMedie_C832.xls");
            Console.ReadKey();
        }
    }
}
