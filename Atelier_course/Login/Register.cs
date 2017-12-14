using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Atelier_course.Login
{
    public partial class Register : Form
    {
        public MySqlConnection conn;
        List<int> Type = new List<int>();
        List<int> Office = new List<int>();
        List<string> Users = new List<string>();

        public Register()
        {
            InitializeComponent();
            Config.DataBase.DataBaseInfo dataBase = new Config.DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();

            groupBox3.Visible = false;
        }

        private void Register_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Items.Clear();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT UType_id, UType_name FROM User_type;")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Type.Add(reader.GetInt32(0));
                    comboBox1.Items.Add(reader.GetString(1));
                }
                reader.Close();

                comboBox2.Items.Clear();

                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Office_id, Office_name FROM Office;")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Office.Add(reader.GetInt32(0));
                    comboBox2.Items.Add(reader.GetString(1));
                }
                reader.Close();

                ////////////Users
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Login FROM Users;")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Users.Add(reader.GetString(0));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "User")
            {
                groupBox3.Visible = false;
            }
            else
            {
                groupBox3.Visible = true;
            }
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in Users)
            {
                if (textBox3.Text == item)
                {
                    label12.Text = "Already exists";
                    label12.ForeColor = Color.Red;
                }
                else
                {
                    label12.Text = "Confirm";
                    label12.ForeColor = Color.Green;
                }
            }
        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.Length < 7)
            {
                label14.Text = "So short!";
                label14.ForeColor = Color.Red;
            }
            else
            {
                label14.Text = "Confirm";
                label14.ForeColor = Color.Green;
            }
        }

        private void TextBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text == textBox6.Text && textBox5.Text.Length >= 8)
            {
                label13.Text = "Confirm";
                label13.ForeColor = Color.Green;
            }
            else
            {
                label13.Text = "Do not match!";
                label13.ForeColor = Color.Red;
            }
        }

        private void TextBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text.Contains('@'))
            {
                label15.Text = "Confirm";
                label15.ForeColor = Color.Green;
            }
            else
            {
                label15.Text = "No mail!";
                label15.ForeColor = Color.Red;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "User")
            {
                if (label12.ForeColor == Color.Green && label14.ForeColor == Color.Green &&
                       label13.ForeColor == Color.Green && label15.ForeColor == Color.Green)
                {
                    if (textBox1.Text != string.Empty && textBox2.Text != string.Empty &&
                    textBox3.Text != string.Empty && textBox4.Text != string.Empty &&
                    textBox5.Text != string.Empty && textBox6.Text != string.Empty &&
                    textBox10.Text != string.Empty && comboBox1.Text != string.Empty)
                    {
                        try
                        {
                            string pass = GetHashSha256(textBox6.Text);

                            string sql = "insert into Users(User_id, UType_id, Office_id," +
                                         " Login, Password, Email, U_name, U_surname, U_fname) values (null, '" +
                                           Type[comboBox1.SelectedIndex] + "', null , '"
                                         + textBox3.Text + "', '" + pass + "', '"
                                         + textBox10.Text + "', '" + textBox1.Text + "', '"
                                         + textBox2.Text + "', '" + textBox4.Text + "')";
                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Registration was successful!");
                            conn.Close();
                            LogIn lg = new LogIn();
                            lg.Show();
                            this.Close();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        MessageBox.Show("Set all the fields!");
                    }
                }
                else
                {
                    MessageBox.Show("Check the data!");
                }
            }
            else
            {
                if (label12.ForeColor == Color.Green && label14.ForeColor == Color.Green &&
                    label13.ForeColor == Color.Green && label15.ForeColor == Color.Green)
                {
                if (textBox1.Text != string.Empty && textBox2.Text != string.Empty &&
                    textBox3.Text != string.Empty && textBox4.Text != string.Empty &&
                    textBox5.Text != string.Empty && textBox6.Text != string.Empty &&
                    textBox10.Text != string.Empty && comboBox1.Text != string.Empty &&
                    comboBox2.Text != string.Empty)
                {
                    try
                    {
                        string pass = GetHashSha256(textBox6.Text);

                        string sql = "insert into Users(User_id, UType_id, Office_id," +
                                     " Login, Password, Email, U_name, U_surname, U_fname) values (null, '" +
                                       Type[comboBox1.SelectedIndex] + "', " +Office[comboBox2.SelectedIndex]+" , '"
                                     + textBox3.Text + "', '" + pass + "', '"
                                     + textBox10.Text + "', '" + textBox1.Text + "', '"
                                     + textBox2.Text + "', '" + textBox4.Text + "')";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Registration was successful!");
                        conn.Close();
                        LogIn lg = new LogIn();
                        lg.Show();
                        this.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    MessageBox.Show("Set all the fields!");
                    }

                }
                else
                {

                    MessageBox.Show("Check the data!");
                }
            }
        }

        public static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}
