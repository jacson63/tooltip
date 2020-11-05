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
        int _baseLine;
        public int baseLine {
            get { return _baseLine; }
            private set {
                if (value < 1)
                {
                    _baseLine = 1;
                    return;
                }
                _baseLine = value;
            }
        }

        private Char _separator;

        public CsvParser()
        {
            _separator = ',';
        }

        public void parse(String csvLine)
        {
            String[] splitedStr = csvLine.Split(_separator);

            if (splitedStr.Length != 2)
            {
                throw new FormatException("csv format error.[fileName, baseLine]");
            }

            fileName = splitedStr[0];
            baseLine = int.Parse(splitedStr[1]);
        }
    }
}
