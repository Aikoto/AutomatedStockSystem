using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomatedStockSystem {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        DataLayer DL = DataLayer.Instance();
        private static int updatetimerseconds =  5;
        private static int current = updatetimerseconds;
        private void Form1_Load(object sender, EventArgs e) {
            //We need to load all the stocks here.
            DataSet ds = DL.GetAllStocks();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                comboBox1.Items.Add(ds.Tables[0].Rows[i][1].ToString());
            }
            progressBar1.Maximum = updatetimerseconds;
            progressBar1.Value = updatetimerseconds;
            timer1.Start();

        }
        ComboBox com;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            //Get data regarding the selected stock
            com = (ComboBox)sender;
            label4.Text = DL.getStockValue(com.SelectedItem.ToString());
        }

        private void button1_Click(object sender, EventArgs e) {
            DataSet ds = DL.getSingleStockInfo(com.SelectedItem.ToString());
            string id = ds.Tables[0].Rows[0][0].ToString();
            string stockName = ds.Tables[0].Rows[0][1].ToString();
            string buyPrice = ds.Tables[0].Rows[0][2].ToString();
            string sellPrice = ds.Tables[0].Rows[0][3].ToString();

            trent = 4;

            float procent = setTrent();
            float buyprice = procent * float.Parse(buyPrice);
            float sellprice = procent * float.Parse(sellPrice);

            //Turn the value up for next update on the selected stock
            DL.EditStockUp(stockName, buyprice, sellprice, id);
            //Show the new value
            label4.Text = DL.getStockValue(com.SelectedItem.ToString());

            //Set it back to random.
            trent = 1;

        }

        private void button2_Click(object sender, EventArgs e) {
            //Turn the value down for next update on the selected stock
            DataSet ds = DL.getSingleStockInfo(com.SelectedItem.ToString());
            string id = ds.Tables[0].Rows[0][0].ToString();
            string stockName = ds.Tables[0].Rows[0][1].ToString();
            string buyPrice = ds.Tables[0].Rows[0][2].ToString();
            string sellPrice = ds.Tables[0].Rows[0][3].ToString();

            trent = 5;

            float procent = setTrent();
            float buyprice = procent * float.Parse(buyPrice);
            float sellprice = procent * float.Parse(sellPrice);

            //Turn the value up for next update on the selected stock
            DL.EditStockUp(stockName, buyprice, sellprice, id);
            //Show the new value
            label4.Text = DL.getStockValue(com.SelectedItem.ToString());

            //Set it back to random.
            trent = 1;
        }
        int trent = 1;
        private void button4_Click(object sender, EventArgs e) {
            //Golden age, Increase all stocks
            Selection.Text = "GoldenAge Update";
            trent = 4;
        }

        private void button3_Click(object sender, EventArgs e) {
            //Dump all stocks value
            Selection.Text = "Crashing the marked.";
            trent = 5;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if(isrunning == true) { 
            if (current > -1) {
                lbl_Time.Text = current + " Seconds left to update";
                progressBar1.Value = current;
                current--;
            } else {

                updateStocks();
                current = updatetimerseconds;
            }
            } else {
                //do nothing;
            }
        }

        private float setTrent() {
            //Trent 1.Random 2.Up 3.Down 4.Goldenage 5. Crash the marked.
            Random ran = new Random();
            double randomValue = 0;
            if (trent == 1) {
                randomValue = 0.95 + (1.05 - 0.95) * ran.NextDouble();
            } else if (trent == 2) {
                randomValue = 1 + (1.4 - 1) * ran.NextDouble();
            } else if (trent == 3) {
                randomValue = 0.6 + (1 - 0.6) * ran.NextDouble();
            } else if (trent == 4) {
                randomValue = 1.2 + (1.5 - 1.2) * ran.NextDouble();
            } else if (trent == 5) {
                randomValue = 0.4 + (0.9 - 0.4) * ran.NextDouble();
            } else {
                //Revert back to random default values if there isn't anything else.
                randomValue = 0.95 + (1.05 - 0.95) * ran.NextDouble();
            }
            return (float)randomValue;
        }


        //Trent 1.Random 2.Up 3.Down 4.Goldenage 5. Crash the marked.
        private void updateStocks() {
            lbl_Time.Text = " Updating";
            DataSet ds;
            ds = DL.GetAllStocks();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                float procent = setTrent();

                string id = ds.Tables[0].Rows[i][0].ToString();
                string stockName = ds.Tables[0].Rows[i][1].ToString();
                string buyPrice = ds.Tables[0].Rows[i][2].ToString();
                string sellPrice = ds.Tables[0].Rows[i][3].ToString();

                float buyprice = procent * float.Parse(buyPrice);
                float sellprice = procent * float.Parse(sellPrice);

                DL.EditStockUp(stockName, buyprice, sellprice, id);

            }
            Selection.Text = "Random";
            //Set it back to random.
            trent = 1;
        }

        private void btn_Admin_Click(object sender, EventArgs e) {
            admin ad = new admin();
            ad.Show();
        }
        bool isrunning = false;
        private void button5_Click(object sender, EventArgs e) {
            //Pause the marked.
            if (isrunning == true) {
                isrunning = false;
                button5.Text = "Click to Start it";
            } else {
                button5.Text = "Click to Pause it";
                isrunning = true;
            }
            
        }
    }
}
