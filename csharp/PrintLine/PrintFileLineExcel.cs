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
        private const string noneStr = "NONE" + separator;

        public override void printformated(CsvParser parser, List<string> buf, int startLine)
        {
            if ( buf == null )
            {
                base.getWriterInstance().WriteLine(noneStr);
                return;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(parser.fileName);
            builder.Append(separator);
            builder.Append(String.Format("{0}", parser.baseLine));
            builder.Append(separator);

            if (buf.Count != 1)
            {
                builder.Append("\"");
            }

            int cnt = startLine;
            foreach (string line in buf)
            {
                builder.Append(cnt);
                builder.Append("|");
                builder.Append(line.Replace("\t","    ").Replace("\"", "\"\""));
                builder.Append("\r\n");
                cnt++;
            }

            if (buf.Count != 1)
            {
                builder.Append("\"");
            }
            //base.fileWrite(builder.ToString());
            base.getWriterInstance().WriteLine(builder.ToString());
        }
    }
}
