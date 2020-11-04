using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintFileLineProject
{
    public class CsvParser
    {
        public String fileName { get; private set; }
        public int baseLine { get; private set; }
        public int prevLine { get; private set; }
        public int followLine { get; private set; }

        private Char _separator;

        public CsvParser()
        {
            _separator = ',';
        }

        public void parse(String csvLine)
        {
            String[] splitedStr = csvLine.Split(_separator);

            if (splitedStr.Length != 4)
            {
                throw new FormatException("csv format error.[fileName, baseLine, startLine, endLine]");
            }

            fileName = splitedStr[0];
            baseLine = int.Parse(splitedStr[1]);
            prevLine = int.Parse(splitedStr[2]);
            followLine = int.Parse(splitedStr[3]);
        }
    }
}
