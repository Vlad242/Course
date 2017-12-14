using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atelier_course.Worker
{
    public partial class CompleteOrder : Form
    {
        private int Id;
        public MySqlConnection conn;

        public CompleteOrder(int id)
        {
            InitializeComponent();
            Id = id;
            Config.DataBase.DataBaseInfo dataBase = new Config.DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void CompleteOrder_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT Orders.Order_id, Service_type.Type_name, Services.Service_name, Orders.Order_date, Status.Status_name FROM Orders INNER JOIN Services on(Orders.Service_id=Services.Service_id) INNER JOIN Service_type on(Service_type.Type_id=Services.Type_id) INNER JOIN Status on(Orders.Status_id=Status.Status_id) WHERE Orders.Worker_id=" + Id + ";", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Orders");
                dataGridView1.DataSource = ds.Tables["Orders"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Service type";
                dataGridView1.Columns[2].HeaderText = "Service name";
                dataGridView1.Columns[3].HeaderText = "Order date";
                dataGridView1.Columns[4].HeaderText = "Status name";

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {

            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "UPDATE Orders SET Status_id=" +
                  3 + " WHERE Order_id=" +
                (int)dataGridView1.CurrentRow.Cells[0].Value + ";";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Order " + dataGridView1.CurrentRow.Cells[0].Value + " complete!");

                CompleteOrder_Load(null,null);
            }
            catch (Exception)
            {
            }
        }

        private void Send()
        {
            try
            {
                string userMail = "";
                MySqlCommand cmd1 = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Users.Email FROM Orders INNER JOIN Users on(Orders.User_id=Users.User_id) WHERE Orders.Order_id='" + dataGridView1.CurrentRow.Cells[0].Value + "';")
                };
                MySqlDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    userMail = reader.GetString(0);
                }
                reader.Close();

                Mailer.Generator generator = new Mailer.Generator();
                string body = generator.GenerateCompleteOrderBody((int)dataGridView1.CurrentRow.Cells[0].Value, "SunComp");
                string subject = generator.GenerateSubject("SunComp", (int)dataGridView1.CurrentRow.Cells[0].Value);
                Mailer.Mailer mailer = new Mailer.Mailer();
                mailer.SendMail(userMail, "example@gmail.com", "", subject, body);
            }
            catch (Exception ex)
            {
            }

        }

        private void CompleteOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            WorkerRoom room = new WorkerRoom(Id);
            room.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
