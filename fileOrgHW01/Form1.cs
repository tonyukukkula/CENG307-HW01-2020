using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static fileOrgHW01.tableDataStructres;


namespace fileOrgHW01
{
    public partial class Form1 : Form
    {

        hashingFunctions func = new hashingFunctions();
        table tableREISCH;
        table tableBLISCH;
        table tableLICH;
        table tableEISCH;

        public int[] ParentLich_hashing(int[] keys, table table, int primaryLine, ListBox box)
        {
            box.Items.Clear();
            int probeNUM = 0;
            int[] outP = new int[2];
            int CheckPrimaryLine = primaryLine;
            table.modAmount = (int)(table.modAmount * 0.9);
            for (int i=0; i < keys.Length; i++) {
                int probenum = func.lich_hashing(keys[i], table);
                if (probenum != -1)
                {
                    probeNUM += probenum;
                    outP[1] = table.modAmount;
                    box.Items.Add(keys[i] + "->" + probenum);
                }
                else
                {
                    if (table.modAmount != 0)
                    {
                        box.Items.Clear();
                        table.TableFormat();
                        table.R = table.rDown;
                        CheckPrimaryLine--;
                        table.modAmount--;
                        outP[1] = 0;
                        i = 0;
                        probeNUM = 0;
                    }
                    else {
                        i = i + keys.Length;
                        MessageBox.Show("The integer keys are not compatible..");
                    }

                }
            }
                    

            outP[0] = probeNUM;
            return outP;

        }
        public Form1()
        {
            InitializeComponent();

        }
        void tableRefresh(table table, ListBox box) {
            box.Items.Clear();
            for (int i = 0; i < table.entity.Length; i++) {
                box.Items.Add(i + "-) " + table.entity[i].key + "-->" + table.entity[i].link);
            }
        }
        void tableRefreshforLICH(table table, ListBox box)
        {
            box.Items.Clear();
            for (int i = 0; i < table.entity.Length; i++)
            {
                if (i == table.modAmount)
                {
                    box.Items.Add("#OVERFLOW AREA#");
                }
                box.Items.Add(i + "-) " + table.entity[i].key + "-->" + table.entity[i].link);
            }
        }




        void labelsRefresh(int keyAmount, int rowAmount) {
            label2.Text = Convert.ToString(func.packingFactorCalculate(keyAmount, rowAmount));
            label3.Text = Convert.ToString(func.packingFactorCalculate(keyAmount, rowAmount));
            label4.Text = Convert.ToString(func.packingFactorCalculate(keyAmount, rowAmount));
        }
        public void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            try
            {
                int keyAmount = Convert.ToInt32(textBox1.Text);
                int rowAmount = (10 * keyAmount) / 9;
                int primaryLine = (int)(rowAmount * 0.86);
                int[] keys = new int[keyAmount];

                tableEISCH = new table(rowAmount);
                tableLICH = new table(rowAmount);
                tableBLISCH = new table(rowAmount);
                tableREISCH = new table(rowAmount);

                for (int i = 0; i < keys.Length; i++)
                {
                    int randomNumber = rnd.Next(0, 999);
                    keys[i] = randomNumber;
                }


                listBox5.Items.Clear();
                for (int i = 0; i < keys.Length; i++)
                {
                    int probenum = func.eisch_hashing(keys[i], tableEISCH);
                    tableEISCH.probeNumber += probenum;
                    listBox5.Items.Add(keys[i] + "->" + probenum);
                }


                listBox6.Items.Clear();
                for (int i = 0; i < keys.Length; i++)
                {
                    int probenum = func.blisch_hashing(keys[i], tableBLISCH);
                    tableBLISCH.probeNumber += probenum;
                    listBox6.Items.Add(keys[i] + "->" + probenum);
                }


                listBox7.Items.Clear();
                for (int i = 0; i < keys.Length; i++)
                {
                    int probenum = func.reisch_hashing(keys[i], tableREISCH);
                    tableREISCH.probeNumber += probenum;
                    listBox7.Items.Add(keys[i] + "->" + probenum);
                }

                int[] temptemp = ParentLich_hashing(keys, tableLICH, primaryLine, listBox8);
                tableLICH.modAmount = temptemp[1];
                tableLICH.probeNumber += temptemp[0];


                tableRefresh(tableEISCH, listBox1);
                tableRefresh(tableBLISCH, listBox2);
                tableRefresh(tableREISCH, listBox3);
                tableRefreshforLICH(tableLICH, listBox4);

                labelsRefresh(keyAmount, rowAmount);
                double packLich = func.packingFactorCalculateForLich(primaryLine, tableLICH);
                label5.Text = Convert.ToString(packLich);
                if (packLich < 80.0) {
                    MessageBox.Show("I don't like these numbers\n so could you create one more?\n just press create button ^_^");
                }


                String AVGprobeBlisch = Convert.ToString(Convert.ToDouble((double)tableBLISCH.probeNumber / (double)keyAmount));
                String AVGprobeLich = Convert.ToString(Convert.ToDouble((double)tableLICH.probeNumber / (double)keyAmount));
                String AVGprobeReisch = Convert.ToString(Convert.ToDouble((double)tableREISCH.probeNumber / (double)keyAmount));
                String AVGprobeEisch = Convert.ToString(Convert.ToDouble((double)tableEISCH.probeNumber / (double)keyAmount));

                label10.Text = AVGprobeEisch;
                label11.Text = AVGprobeBlisch;
                label12.Text = AVGprobeReisch;
                label13.Text = AVGprobeLich;
                label14.Text = "Average probe number:\n";/* + "EISCH PROBE NUMBER:"+tableEISCH.probeNumber + "\n" +
                "BLISCH PROBE NUMBER:" + tableBLISCH.probeNumber + "\n" +
                "REISCH PROBE NUMBER:" + tableREISCH.probeNumber + "\n" +
                "LICH PROBE NUMBER:" + tableLICH.probeNumber + "\n";*/

            }
            catch (FormatException) { MessageBox.Show("I know you'll try this :)\nbut i can't allow it :( "); }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox9.Items.Clear();
            try
            {
                int deger = Convert.ToInt32(textBox2.Text);
                listBox9.Items.Add("----IN EISCH TABLE----");
                listBox9.Items.Add(func.search(deger, tableEISCH));
                listBox9.Items.Add("\n---IN BLISCH TABLE----");
                listBox9.Items.Add(func.search(deger, tableBLISCH));
                listBox9.Items.Add("\n---IN REISCH TABLE----");
                listBox9.Items.Add(func.search(deger, tableREISCH));
                listBox9.Items.Add("\n-----IN LICH TABLE----");
                listBox9.Items.Add(func.search(deger, tableLICH));
            } catch(FormatException) { MessageBox.Show("I know you'll try this :)\nbut i can't allow it :( "); }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
