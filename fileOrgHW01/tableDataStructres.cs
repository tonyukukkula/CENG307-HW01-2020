using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileOrgHW01
{
    public class tableDataStructres
    {
        public struct row
        {
            public int? key, link;
            //avamlar bilmez "int?" C#9.0 veya .NET 5.0 özelliğiydi sanırım
            //"null olabilen int moruQ"
        }
        public struct table
        {
            public int probeNumber;
            public int R;
            public int rUp;
            public int rDown;
            public row[] entity;
            public int modAmount;
            public table(int rowAmount)
            {
                this.entity = new row[rowAmount];
                modAmount = rowAmount;
                R = modAmount - 1;
                rUp = 0;
                rDown = modAmount-1;
                probeNumber=1;
            }

            public void tableCreate(int rowAmount) {
                this.entity = new row[rowAmount];
                modAmount = rowAmount;
                R = modAmount - 1;
                rUp = 0;
                rDown = modAmount-1;
                probeNumber++;
            }
            public void probeNumberAdd(int temp) {
                probeNumber += temp;
            }
            public void probeNumberAdd()
            {
                probeNumber += 1;
            }
            public void TableFormat()
            {
                for (int i = 0; i < entity.Length; i++)
                {
                    entity[i].key = null;
                    entity[i].link = null;
                }
                probeNumber = 0;
            }
        }
    }
}