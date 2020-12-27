using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintObjdumpTree
{
    class Tree
    {
        protected int depth = 0;
        protected Tree parent { get; set; } = null;
        List<Tree> child { get; set; }
        public String nodeValue { get; private set; } = "";

        public Tree(string currentStr)
        {
            nodeValue = currentStr;
            child = new List<Tree>();
        }
        
        public void addChild(Tree childTree)
        {
            childTree.depth = this.depth + 1;
            childTree.parent = this;
            child.Add(childTree);
        }

        /// <summary>
        /// 標準出力用の階層分tabを挿入した文字列を返す
        /// </summary>
        /// <returns></returns>
        public string nodeStr()
        {
            return new string('\t', depth) + nodeValue;
        }

        public IEnumerable<Tree> getChild()
        {
            foreach (Tree node in child)
            {
                yield return node;
            }
        }

        public Boolean isChild()
        {
            if ( child.Count() <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
