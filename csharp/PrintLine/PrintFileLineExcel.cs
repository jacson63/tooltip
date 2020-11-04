using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintFileLineProject
{
    class PrintFileLineExcel : AbstractPrintFileLine
    {
        private const string separator = "\t";

        public override void printformated(CsvParser parser, List<string> buf)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(parser.fileName);
            builder.Append(separator);
            builder.Append(String.Format("({0})", parser.baseLine));
            builder.Append(separator);
            builder.Append("\"");

            int cnt = parser.prevLine;
            foreach (string line in buf)
            {
                builder.Append(cnt);
                builder.Append("|");
                builder.Append(line.Replace("\t","    ").Replace("\"", "\"\""));
                builder.Append("\r\n");
                cnt++;
            }

            builder.Append("\"");
            base.fileWrite(builder.ToString());
        }
    }
}
