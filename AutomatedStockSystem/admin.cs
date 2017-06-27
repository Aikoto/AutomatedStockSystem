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
    public partial class admin : Form {
        public admin() {
            InitializeComponent();
        }
        DataLayer DL = DataLayer.Instance();

        private void Create_Stock_Click(object sender, EventArgs e) {
            DL.addNewStock(textBox1.Text, int.Parse( textBox2.Text), int.Parse( textBox3.Text ));
        }

        private void button1_Click(object sender, EventArgs e) {
            comboBox2.Items.Clear();
            //Load stock liste.
            DataSet ds = DL.GetAllStocks();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                comboBox2.Items.Add(ds.Tables[0].Rows[i][0].ToString() + " | " + ds.Tables[0].Rows[i][1].ToString());
            }
        }

       
        private void Delete_Stock_Click(object sender, EventArgs e) {
            string[] stock;
            stock = comboBox2.SelectedItem.ToString().Split('|');
            DL.DelectStock(stock[0].Trim());
            comboBox2.Items.Clear();
            comboBox2.SelectedText = "";
        }
    }
}
