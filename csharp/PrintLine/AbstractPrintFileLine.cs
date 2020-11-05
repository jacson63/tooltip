using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace PrintFileLineProject
{
    public abstract class AbstractPrintFileLine
    {
        public int prevLineNum { set; get; }
        public int followLineNum { set; get; }
        public string outputFile { set; get; }

        protected string outputEncoding { get; set; }
        CsvParser _parser = new CsvParser();
        StreamWriter _writer = null;

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

            //デストラクタでstream閉じるとエラーになるので、とりあえずここで閉じる
            CloseStream();
        }

        private void printFileRange()
        {
            List<string> buf = null;
            int startLine = getStartLine(_parser.baseLine, prevLineNum);
            int endLine = _parser.baseLine + followLineNum;
            int endCount = endLine - startLine + 1;

            if (!File.Exists(_parser.fileName))
            {
                //csv記載のファイルが存在しない
                printformated(_parser, buf, startLine);
                return;
            }

            buf = new List<string>();
            foreach (String line in File.ReadLines(_parser.fileName).Skip(startLine - 1).Take(endCount))
            {
                buf.Add(line);
            }

            printformated(_parser, buf, startLine);
        }

        public abstract void printformated(CsvParser parser, List<String> buf, int startLine);

        protected StreamWriter getWriterInstance()
        {
            if (_writer == null)
            {
                _writer = new StreamWriter(outputFile, false, System.Text.Encoding.GetEncoding(outputEncoding));
            }
            return _writer;
        }

        private void CloseStream()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Dispose();
            }
        }

        private int getStartLine(int baseLine, int prevLineNum)
        {
            if ( (baseLine - prevLineNum) < 1)
            {
                return 1;
            }

            return (baseLine - prevLineNum);
        }

        //protected void fileWrite(String str)
        //{
        //    using (StreamWriter writer = new StreamWriter(outputFile, true, System.Text.Encoding.GetEncoding(outputEncoding)))
        //    {
        //        writer.WriteLine(str);
        //    }
        //}
    }
}
