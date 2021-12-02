using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static fileOrgHW01.tableDataStructres;
// kodu direk kopyalayıp yapıştırmayın, okuyun araştırın az kültürlenin


namespace fileOrgHW01
{
    public class hashingFunctions
    {
        public double packingFactorCalculate(int keyAmount, int rowAmount)
        {
            return Convert.ToDouble((keyAmount * 100.0) / rowAmount);
        }

        public double packingFactorCalculateForLich(int primaryLine, table table)
        {
            int primaryspace = 0;
            for(int i=0; i <= primaryLine; i++)
                if (table.entity[i].key == null)
                    primaryspace++;
            
            return ((primaryLine - primaryspace)*100) / primaryLine;
        }
        public int Rfinder(table table)
        {
            int tempR = table.R;
            while (table.entity[tempR].key != null ) { tempR--; }
            table.R = tempR;
            return table.R;
        }
        public int RfinderForREISCH(table table) {
            Random rand = new Random();
            int R = table.R;
            while (table.entity[R].key != null) { R = rand.Next(0, table.modAmount); }
            table.R = R;
            return table.R;
        }
        public void insertTheTable(int key, int rowNumber, table table)
        {
            table.entity[rowNumber].key = key;
        }
        public void linkTheTable(int rowNumber, int linkAddress, table table)
        {
            table.entity[rowNumber].link = linkAddress;
        }
        public int eisch_hashing(int key, table table)
        {
            int rowNumber = key % table.modAmount;
            int R;
            int temp = 0;
            if (table.entity[rowNumber].key == null)
            {
                insertTheTable(key, rowNumber, table);
                temp++;
            }
            else
            {
                if (table.entity[rowNumber].link == null)
                {
                    R = Rfinder(table);
                    insertTheTable(key, R, table);
                    linkTheTable(rowNumber, R, table);
                    temp+=2;
                }
                else
                {
                    R = Rfinder(table);
                    int tempLink = (int)table.entity[rowNumber].link;
                    insertTheTable(key, R, table);
                    linkTheTable(rowNumber, R, table);
                    linkTheTable(R, tempLink, table);
                    temp +=3;//arayı 1 açıyoruz homeaddressten dönen değer 2den fazlaysa
                }
            }
            return temp;
        }
        public int lich_hashing(int key, table table)
        {
            int gecici = 0;
            try {
                int TemprowN = key % table.modAmount;
                int R;
                row temp = table.entity[TemprowN];
                if (temp.key == null)
                {
                    insertTheTable(key, TemprowN, table);
                    gecici++;
                }
                else
                {
                    if (temp.link == null)
                    {
                        R = Rfinder(table);
                        if (R >= table.modAmount)
                        {
                            insertTheTable(key, R, table);
                            linkTheTable(TemprowN, R, table);
                            gecici += 2;
                        }
                        else
                        {
                            gecici = -1;
                        }
                    }
                    else
                    {
                        gecici = -1;
                    }
                }
            }
            catch { }

            return gecici;
        }

        public int reisch_hashing(int key, table table)
        {
            int rowNumber = key % table.modAmount;
            Random rand = new Random();
            int R = rand.Next(0, table.modAmount-1);
            int gecici = 0;
            if (table.entity[rowNumber].key == null)
            {
                insertTheTable(key, rowNumber, table);
                gecici++;
            }
            else
            {
                if (table.entity[rowNumber].link == null)
                {
                    R = RfinderForREISCH(table);
                    insertTheTable(key, R, table);
                    linkTheTable(rowNumber, R, table);
                    gecici+=2;
                }
                else
                {
                    R = RfinderForREISCH(table);
                    int tempLink = (int)table.entity[rowNumber].link;
                    insertTheTable(key, R, table);
                    linkTheTable(rowNumber, R, table);
                    linkTheTable(R, tempLink, table);
                    gecici+=3;
                }
            }
            return gecici;
        }
        public int blisch_hashing(int key, table table)
        {
            int TemprowN = key % table.modAmount;
            int f = 0;
            int gecici = 0;
            row temp = table.entity[TemprowN];
            while (f == 0)
            {
                if (temp.key == null)
                {
                    insertTheTable(key, TemprowN, table);
                    gecici++;
                    f = 1;
                }
                else
                {
                    if (temp.link != null)
                    {
                        TemprowN = (int)temp.link;
                        temp = table.entity[(int)temp.link];
                        gecici++;
                    }
                    else
                    {
                        int flag = 0;
                        while (flag != 1)
                        {
                            if (table.entity[table.rUp].key == null)
                            {
                                insertTheTable(key, table.rUp, table);
                                linkTheTable(TemprowN, table.rUp, table);
                                gecici++;
                                flag = 1;
                            }
                            else
                            {
                                table.rUp++;
                                if (table.entity[table.rDown].key == null)
                                {
                                    insertTheTable(key, table.rDown, table);
                                    linkTheTable(TemprowN, table.rDown, table);
                                    gecici++;
                                    flag = 1;
                                }
                                else
                                {
                                    table.rDown--;
                                }
                            }
                        }
                        f = 1;
                    }
                }
            }
            return gecici;
        }
        public string search(int key, table table) {
            string mesaj = "I can't see any value that you want :(";
            
            int rowNumber = key % table.modAmount;
            if (table.entity[rowNumber].key == key)
            {
                mesaj = key + " Found->Row No:   " + rowNumber;
            }else{
                int c = 1;
                try {
                    row temp = table.entity[(int)table.entity[rowNumber].link];
                    int rowNumberTemp = (int)table.entity[rowNumber].link;
                    while (c == 1)
                    {
                        if (temp.key == key)
                        {
                            mesaj = key + " Found->Row No::   " + rowNumberTemp;
                            c = 0;
                        }
                        else
                        {
                            if (temp.link != null)
                            {
                                rowNumberTemp = (int)temp.link;
                                temp = table.entity[(int)temp.link];
                                
                            }
                            else
                            {
                                mesaj = "I can't see any value that you want :(";
                                c = 0;
                            }
                        }
                    }
                } catch (System.InvalidOperationException) { }
                
            }
            return mesaj;
        }


    }
}