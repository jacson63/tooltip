using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintFileLineProject
{
    class PrintFileLine
    {
        // [ファイル名,表示行数]のcsvを取り込んで、outputFileに出力する
        // arg1:csv
        // arg2:表示行数より前の部分を表示する行数
        // arg3:表示行数よりうしろの部分を表示する行数
        // arg4:結果ファイル名
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("usage:");
                Console.WriteLine(" args1:listFileName");
                Console.WriteLine(" args2:prevLineNum");
                Console.WriteLine(" args3:followLineNum");
                Console.WriteLine(" args4:outputFile");
                return;
            }

            PrintFileLineFactory fa = new PrintFileLineFactory();
            AbstractPrintFileLine abPfl = fa.create("Excel");

            String fileName = args[0];
            abPfl.prevLineNum = int.Parse(args[1]);
            abPfl.followLineNum = int.Parse(args[2]);
            String outputFile = args[3];

            abPfl.outputFile = outputFile;
            abPfl.listToPrint(fileName);
        }
    }
}
