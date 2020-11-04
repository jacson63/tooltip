using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintFileLineProject
{
    class PrintFileLine
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("usage:");
                Console.WriteLine(" args1:listFileName");
                Console.WriteLine(" args2:outputFile");
                return;
            }

            String fileName = args[0];
            String outputFile = args[1];

            PrintFileLineFactory fa = new PrintFileLineFactory();
            AbstractPrintFileLine abPfl = fa.create("Excel");

            abPfl.outputFile = outputFile;
            abPfl.listToPrint(fileName);
        }
    }
}
