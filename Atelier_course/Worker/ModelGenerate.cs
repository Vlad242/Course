using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Atelier_course.Worker
{
    public partial class ModelGenerate : Form
    {
        private int Id;
        private int OrderId;
        public MySqlConnection conn;
        int Price = 0;
        int model_id = 0;
        string forMessage = "";
        List<int> DetailId = new List<int>();
        List<string> DetailName = new List<string>();
        List<string> DetailPrice = new List<string>();
        List<string> DetailParse = new List<string>();

        public ModelGenerate(int id, int orderid)
        {
            InitializeComponent();
            Id = id;
            OrderId = orderid;
            Config.DataBase.DataBaseInfo dataBase = new Config.DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void ModelGenerate_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Items.Clear();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT * FROM Detail;")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DetailId.Add(reader.GetInt32(0));
                    DetailName.Add(reader.GetString(1));
                    DetailPrice.Add(reader.GetString(2));
                    comboBox1.Items.Add(reader.GetString(1)); 
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(comboBox1.Text + "-" + numericUpDown1.Value);
        }

        private void ListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox1.Items.Count != 0)
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                try
                {
                    listBox1.Items.Remove(textBox1.Text);
                    textBox1.Clear();
                }
                catch (Exception)
                {
                    MessageBox.Show("Matches not found!");
                    textBox1.Clear();
                }
               
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0)
            {
                ///////////Add price
                foreach (var item in listBox1.Items)
                {
                    DetailParse.Add(item.ToString());
                }

                forMessage += "Components: + <br>";
                for (int i = 0; i < DetailParse.Count; i++)
                {
                    for (int j = 0; j < DetailName.Count; j++)
                    {
                        if (DetailName[j] == DetailParse[i].Substring(0, DetailParse[i].IndexOf('-')))
                        {
                            int uno = Convert.ToInt32(DetailParse[i].Substring(DetailParse[i].IndexOf('-') + 1));
                            int dos = Convert.ToInt32(DetailPrice[j]);
                            Price += uno * dos;
                        }
                    }
                    forMessage += DetailParse[i] + "<br>";
                }

                forMessage += "<br> General component price: " + Price + "<br>";

                /////////Create new model
                try
                {
                    string sql = "insert into Model(Model_id, M_price) values (null, " + Price + ")";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = string.Format("SELECT MAX(Model_id) FROM Model;")
                    };
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        model_id = reader.GetInt32(0);
                    }
                    reader.Close();

                    ///////////////////////add info into consist
                    for (int i = 0; i < DetailParse.Count; i++)
                    {
                        for (int j = 0; j < DetailName.Count; j++)
                        {
                            if (DetailName[j] == DetailParse[i].Substring(0, DetailParse[i].IndexOf('-')))
                            {
                                int count = Convert.ToInt32(DetailParse[i].Substring(DetailParse[i].IndexOf('-') + 1));

                                sql = "insert into consists(Detail_id, Model_id, count) values (" + DetailId[j] +","+ model_id
                                    + "," + count + ")";
                                cmd = new MySqlCommand(sql, conn);
                                cmd.ExecuteNonQuery();

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                int current_price = 0;
                /////////////////Orders block
                try
                {
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = string.Format("SELECT Price FROM orders WHERE Order_id=" + OrderId+";")
                    };
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        current_price = reader.GetInt32(0);
                    }
                    reader.Close();

                    string sql = "UPDATE Orders SET Price=" +
                    (current_price + Price) + " WHERE Order_id=" + OrderId + ";";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    sql = "UPDATE Orders SET Model_id=" +
                    model_id + " WHERE Order_id=" + OrderId + ";";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    sql = "UPDATE Orders SET Status_id=" +
                    2 + " WHERE Order_id=" + OrderId + ";";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                    Send(forMessage);

                    CreateModel model = new CreateModel(Id);
                    model.Show();
                    conn.Close();
                    this.Dispose();

                }
                catch (Exception ex)
                {
                }

            }
            else
            {
                MessageBox.Show("Parts List is empty, add at least one piece!");
            }
        }

        private void Send(string detail)
        {
            try
            {
                string userMail = "";
                MySqlCommand cmd1 = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Users.Email FROM Orders INNER JOIN Users on(Orders.User_id=Users.User_id) WHERE Orders.Order_id='" + OrderId + "';")
                };
                MySqlDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    userMail = reader.GetString(0);
                }
                reader.Close();

                Mailer.Generator generator = new Mailer.Generator();
                string body = generator.GenerateActiveBody(detail, OrderId, "SunComp");
                string subject = generator.GenerateSubject("SunComp", OrderId);
                Mailer.Mailer mailer = new Mailer.Mailer();
                mailer.SendMail(userMail, "example@gmail.com", "", subject, body);
            }
            catch (Exception ex)
            {
            }

        }

        private void ModelGenerate_FormClosing(object sender, FormClosingEventArgs e)
        {
            CreateModel model = new CreateModel(Id);
            model.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
