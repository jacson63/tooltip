using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace PrintObjdumpTree
{
    class Program
    {
        static Tree treeData;
        static Dictionary<string, List<string>> elementData;

        //option file
        static Boolean op_file = false;
        static string op_filePath = "";

        //option midFile
        //static Boolean op_midfile = false;
        //static string op_midfilePath = "";

        //option findkeys
        static Boolean op_findKeys = false;
        static List<string> op_findKeysVal = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">
        ///   [0] = objdumpファイルパス
        ///   [1] = 検索関数
        /// </param>
        static void Main(string[] args)
        {
            if (!parseOptions(args))
            {
                return;
            }

            //if (op_midfile)
            //{
            //    //未実装
            //}
            //else if (op_file)
            if (op_file)
            {
                // objdumpファイル読み込み⇒Tree情報作成
                elementData = readfile(op_filePath);
                treeData = elementToTree(elementData);

                // Tree情報を中間ファイルとして出力
                //・・・
            }

            PrintTree(treeData, op_findKeysVal);
        }

        static Boolean parseOptions(string[] args)
        {
            if (args.Count() <= 0)
            {
                Console.WriteLine("usage:");
                Console.WriteLine("  -file <objdumpFilePath>");
                Console.WriteLine("  -findkeys <findFunctionName, ...>");
                return false;
            }

            for (int cnt = 0; cnt < args.Length; cnt++)
            {
                switch (args[cnt])
                {
                    case "-file":
                        op_file = true;
                        op_filePath = args[cnt + 1];
                        cnt++;
                        break;
                    //case "-midfile":
                    //    op_midfile = true;
                    //    op_midfilePath = args[cnt + 1];
                    //    cnt++;
                    //    break;
                    case "-findkeys":
                        op_findKeys = true;
                        op_findKeysVal = new List<string>(args[cnt + 1].Split(',').Select(s => s.Trim()));
                        break;
                }
            }

            return true;
        }

        // 親―子のelement作成
        static Dictionary<string, List<string>> readfile(string filePath)
        {
            const string KEY_REG = @"(^[0-9a-fA-F]+ <[\S]+>)";
            const string VALUE_REG = @"call[ ]+([0-9a-fA-F]+ <[\S]+>)";

            Dictionary<string, List<string>> element = new Dictionary<string, List<string>>();

            using( StreamReader sr = new StreamReader(filePath) )
            {
                String line;
                String key = "";
                while ( (line = sr.ReadLine()) != null)
                {
                    if ( Regex.IsMatch(line, KEY_REG) )
                    {
                        key = Regex.Match(line, KEY_REG).Groups[1].Value;
                    }

                    if (Regex.IsMatch(line, VALUE_REG))
                    {
                        if (key == "")
                        {
                            continue;
                        }

                        string value = Regex.Match(line, VALUE_REG).Groups[1].Value;
                        value = "0" + value;
                        List<string> values;
                        if (element.ContainsKey(key))
                        {
                            values = element[key];
                        }
                        else
                        {
                            values = new List<String>();
                        }

                        values.Add(value);
                        element[key] = values;
                    }
                }
            }

            return element;
        }

        static Tree elementToTree(Dictionary<string, List<string>> element)
        {
            Tree tree = new Tree("");
            Stack<Tree> execStack = new Stack<Tree>();

            //初期データセット(main関数探す）
            foreach(string key in element.Keys)
            {
                if ( key.Contains("<main>"))
                {
                    tree = new Tree(key);
                    execStack.Push(tree);
                    break;
                }
            }

            // stackの中身がなくなるまで実行
            while (true)
            {
                if ( execStack.Count() <= 0)
                {
                    // 全件終了
                    break;
                }

                // 1件取り出し
                Tree currentTree = execStack.Pop();

                // 子要素をpush
                foreach(string value in element[currentTree.nodeValue])
                {
                    Tree childTree = new Tree(value);
                    currentTree.addChild(childTree);

                    if (element.ContainsKey(value)) {
                        // element内に存在する子ノードの場合、スタックに積む
                        // ※printfなど存在しないものがあるため
                        execStack.Push(childTree);
                    }
                }
            }

            return tree;
        }

        static void PrintTree(Tree printTreeData, List<string> findList)
        {
            List<Tree> outputBuf = new List<Tree>();
            Stack<Tree> execStack = new Stack<Tree>();

            // 初期ノードセット
            execStack.Push(printTreeData);

            while (true)
            {
                if (execStack.Count() <= 0)
                {
                    // 全件終了
                    break;
                }

                // 1件取り出し
                Tree currentTree = execStack.Pop();

                // 子要素をpush
                foreach(Tree childTree in currentTree.getChild())
                {
                    execStack.Push(childTree);
                }
               
                if (!currentTree.isChild())
                {
                    // Tree先端に来た場合、標準出力
                    outputBuf.Add(currentTree);
                    _PrintTree(outputBuf, findList);
                    outputBuf = new List<Tree>();
                }
                else
                {
                    // 出力バッファに追加
                    outputBuf.Add(currentTree);
                }
            }
        }

        static void _PrintTree(List<Tree> outputBuf, List<string> findList)
        {
            if (findList.Count() <= 0)
            {
                // 全件出力
                _PrintTree(outputBuf);
                return;
            }

            //以下条件付き出力
            string baseStr = String.Join(",", outputBuf.Select(s => s.nodeValue));
            foreach (string findkey in findList)
            {
                if (baseStr.Contains(findkey))
                {
                    _PrintTree(outputBuf);
                    break;
                }
            }
        }

        static void _PrintTree(List<Tree> outputBuf)
        {
            foreach(Tree currentTree in outputBuf)
            {
                Console.WriteLine(currentTree.nodeStr());
            }
        }
    }
}
