using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Atelier_course.User
{
    public partial class NewOrder : Form
    {
        private int Id;
        public MySqlConnection conn;
        List<int> SType = new List<int>();
        List<int> Service = new List<int>();
        List<int> Office = new List<int>();
        List<int> Worker_id = new List<int>();
        List<string> Worker_name = new List<string>();
        List<int> Worker_count = new List<int>();
        int worker = 0;
        int price = 0;

        public NewOrder(int id)
        {
            InitializeComponent();
            Id = id;
            Config.DataBase.DataBaseInfo dataBase = new Config.DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();

            textBox2.Text = DateTime.Now.ToString("yyyy:MM:dd");
            textBox2.ReadOnly = true;
            textBox1.ReadOnly = true;
        }

        private void NewOrder_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Items.Clear();
                comboBox3.Items.Clear();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Type_id, Type_name FROM Service_type;")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SType.Add(reader.GetInt32(0));
                    comboBox1.Items.Add(reader.GetString(1));
                }
                reader.Close();

                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Office_id, Office_name, Address FROM Office;")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Office.Add(reader.GetInt32(0));
                    comboBox3.Items.Add(reader.GetString(1) + "(" + reader.GetString(2) + ")");
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox2.Items.Clear();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Service_id, Service_name FROM Services WHERE Type_id=" +SType[comboBox1.SelectedIndex]+ ";")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Service.Add(reader.GetInt32(0));
                    comboBox2.Items.Add(reader.GetString(1));
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Worker_id.Clear();
                Worker_name.Clear();
                Worker_count.Clear();

                //////////////////////////////////////WORKER
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Users.User_id, COUNT(Orders.Order_id), Users.U_surname, Users.U_name FROM Users INNER JOIN Orders on(Users.User_id=Orders.Worker_id) WHERE Users.UType_id=2 and Users.Office_id=" +Office[comboBox3.SelectedIndex] +" GROUP BY Users.User_id;")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        Worker_id.Add(reader.GetInt32(0));
                        Worker_count.Add(reader.GetInt32(1));
                        Worker_name.Add(reader.GetString(2) + " " + reader.GetString(3));
                    }
                    catch (Exception ex)
                    {

                    }

                }
                reader.Close();

                int index = 0;
                int min = Worker_count.Min();
                for (int i = 0; i < Worker_count.Count; i++)
                {
                    if (Worker_count[i] == min)
                    {
                        index = i;
                        break;
                    }
                }
                textBox1.Text = Worker_name[index];
                worker = Worker_id[index];
            }
            catch (Exception)
            {
                textBox1.Clear();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "Insert into Orders (Order_id, User_id, " +
                      "Service_id, Status_id, Worker_id, Model_id, Order_date, " +
                      "Descriptions, Price) values (null," +
                      Id + "," +Service[comboBox2.SelectedIndex] + "," + 1 + "," +
                      worker+ ", null ,'" + textBox2.Text + "','" +
                      richTextBox1.Text + "'," + price + ");";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Order added!Order status - Processing!");

                UserRoom room = new UserRoom(Id);
                room.Show();
                conn.Close();
                this.Dispose();
            }
            catch (Exception ex)
            {
            }
           
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Service_price FROM Services WHERE Service_id=" + Service[comboBox2.SelectedIndex] + ";")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    price = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
