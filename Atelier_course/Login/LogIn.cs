using MySql.Data.MySqlClient;
using System;
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

        }
    }
}
