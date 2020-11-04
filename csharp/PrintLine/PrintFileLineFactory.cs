using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintFileLineProject
{
    class PrintFileLineFactory
    {
        public AbstractPrintFileLine create(String clazzName)
        {
            AbstractPrintFileLine retClazz = null;
            switch (clazzName)
            {
                case "Excel":
                    retClazz = new PrintFileLineExcel();
                    break;
                default:
                    break;
            }

            return retClazz;
        }
    }
}
