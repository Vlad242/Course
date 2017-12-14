using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Atelier_course.Login
{
    public partial class LogIn : Form
    {
        public MySqlConnection conn;

        public LogIn()
        {
            InitializeComponent();
            Config.DataBase.DataBaseInfo dataBase = new Config.DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Items.Clear();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Login FROM Users;")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string res1 = reader.GetString(0);
                    comboBox1.Items.Add(res1);
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                conn.Close();
                Register register = new Register();
                register.Show();
                this.Hide();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != string.Empty && textBox2.Text != string.Empty)
            {
                string Login = comboBox1.Text;
                string Password = textBox2.Text;

                bool flag = false;
                foreach (string item in comboBox1.Items)
                {
                    if (Login == item)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    if (!SearchPass(Login, Password))
                    {
                        MessageBox.Show("Password error!");
                    }
                    else
                    {
                        if (GetUserType(Login, Password) == 1)
                        {
                           
                            MessageBox.Show("Authorization was successful!WELCOME USER " + Login + "!");
                            User.UserRoom user = new User.UserRoom(GetUserId(Login,Password));
                            user.Show();
                            conn.Close();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Authorization was successful!WELCOME WORKER " + Login + "!");
                            Worker.WorkerRoom room = new Worker.WorkerRoom(GetUserId(Login, Password));
                            room.Show();
                            conn.Close();
                            this.Hide();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("This user is not registered on the system!");
                }
            }
            else
            {
                MessageBox.Show("Fill in all the logon fields!");
            }
        }

        public bool SearchPass(string Login, string password)
        {
            password = GetHashSha256(password);
            int result = 0;
            try
            {
                MySqlCommand cmd2 = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("Select COUNT(User_id) FROM Users Where Login = '" + Login + "' and password = '" + password + "';")
                };
                MySqlDataReader reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetUserType(string Login, string password)
        {
            int result = 0;
            try
            {
                password = GetHashSha256(password);

                MySqlCommand cmd2 = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("Select UType_id FROM Users Where Login = '" + Login + "' and password = '" + password + "';")
                };
                MySqlDataReader reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return result;
        }

        public int GetUserId(string Login, string password)
        {
            int result = 0;
            try
            {
                password = GetHashSha256(password);

                MySqlCommand cmd2 = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("Select User_id FROM Users Where Login = '" + Login + "' and password = '" + password + "';")
                };
                MySqlDataReader reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return result;
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

        private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                conn.Close();
                this.Dispose();
                Application.ExitThread();
                Application.Exit();
            }
            catch (Exception)
            {
                Application.Exit();
            }

        }
    }
}
