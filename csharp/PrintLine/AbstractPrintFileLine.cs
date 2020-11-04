using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace PrintFileLineProject
{
    public abstract class AbstractPrintFileLine
    {
        public string outputFile { set; get; }
        CsvParser _parser = new CsvParser();
        protected string outputEncoding { get; set; }

        public AbstractPrintFileLine()
        {
            outputEncoding = "UTF-8";
        }

        public void listToPrint(string csvListFile)
        {
            foreach (string csvLine in File.ReadLines(csvListFile))
            {
                _parser.parse(csvLine);
                printFileRange();
            }
        }

        private void printFileRange()
        {
            List<string> buf = new List<string>();
            int startLine = _parser.baseLine - _parser.prevLine;
            int endLine = _parser.baseLine + _parser.followLine;
            int endCount = endLine - startLine;

            foreach (String line in File.ReadLines(_parser.fileName).Skip(startLine).Take(endCount))
            {
                buf.Add(line);
            }

            printformated(_parser, buf);
        }

        public abstract void printformated(CsvParser parser, List<String> buf);

        protected void fileWrite(String str)
        {
            using (StreamWriter writer = new StreamWriter(outputFile, true, System.Text.Encoding.GetEncoding(outputEncoding)))
            {
                writer.WriteLine(str);
            }
        }
    }
}
