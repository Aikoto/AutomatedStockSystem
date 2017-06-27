using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedStockSystem {
    class DataLayer {

        MySqlConnection Conn = new MySqlConnection("Server=mysql32.unoeuro.com;Database=hydracon_dk_db;Uid=hydracon_dk;Pwd=Zone6940;");
        //Private construktor
        private DataLayer() {
        }

        private static DataLayer instance;

        public static DataLayer Instance() {
            if (instance == null) {
                instance = new DataLayer();
            }
            return instance;
        }
        public DataSet GetAllStocks() {
            try {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM FFL_Stocks", Conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                //Fill opens and closes the connection automatic.
                da.Fill(ds);

                return ds;
            } catch (Exception) {
                return null;
            }
        }
        public string getStockValue(string stockname) {
            try {
                MySqlCommand cmd = new MySqlCommand("SELECT BuyPrice, SellPrice, ID FROM FFL_Stocks WHERE Name ='" + stockname + "';", Conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                //Fill opens and closes the connection automatic.
                da.Fill(ds);

                return "BuyPrice " + ds.Tables[0].Rows[0][0].ToString() + " SellPrice" + ds.Tables[0].Rows[0][1].ToString();

            } catch (Exception e) {
                return "Cannot get stock information" + "  " + stockname;
            }
        }
        public void EditStockUp(string stockname, float buyPrice, float sellPrice, string id) {
            try {
                MySqlCommand cmd = new MySqlCommand("Update FFL_Stocks SET BuyPrice=" + buyPrice + ", Sellprice=" + sellPrice + " WHERE Name = '" + stockname + "';", Conn);
                MySqlCommand cmd2 = new MySqlCommand("INSERT INTO FFL_StockHistory(ID, DateTime, BuyPrice, SellPrice) VALUES('" + id + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + buyPrice + "','" + sellPrice + "');", Conn);
                Conn.Open();
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();

            } catch (Exception e) {
            } finally {
                Conn.Close();
            }
        }
        public DataSet getSingleStockInfo(string stockname) {
            try {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM FFL_Stocks WHERE Name ='" + stockname + "';", Conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                //Fill opens and closes the connection automatic.
                da.Fill(ds);

                return ds;

            } catch (Exception e) {
                return null;
            }
        }

        public void addNewStock(string stockname, float buyPrice, float sellPrice) {
            try {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO FFL_Stocks(Name, BuyPrice, Sellprice) VALUES('" + stockname + "','" + buyPrice + "','" + sellPrice + "');", Conn);
                Conn.Open();
                cmd.ExecuteNonQuery();

            } catch (Exception e) {
            } finally {
                Conn.Close();
            }
        }
        public void DelectStock(string stockname) {
            try {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM FFL_Stocks WHERE ID =" + stockname + ";", Conn);
                MySqlCommand cmd2 = new MySqlCommand("DELETE FROM FFL_StockHistory WHERE ID =" + stockname + ";", Conn);
                Conn.Open();
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();

            } catch (Exception e) {
                string ex = e.Message;
            } finally {
                Conn.Close();
            }
        }
    }
}
