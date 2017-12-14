using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Atelier_course.Worker
{
    public partial class CreateModel : Form
    {
        private int Id;
        public MySqlConnection conn;

        public CreateModel(int id)
        {
            InitializeComponent();
            Id = id;
            Config.DataBase.DataBaseInfo dataBase = new Config.DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();

        }

        private void CreateModel_Load(object sender, EventArgs e)
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

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            label1.ForeColor = Color.Green;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ModelGenerate generate = new ModelGenerate(Id, (int)dataGridView1.CurrentRow.Cells[0].Value);
            generate.Show();
            conn.Close();
            this.Dispose();
        }

        private void CreateModel_FormClosing(object sender, FormClosingEventArgs e)
        {
            WorkerRoom room = new WorkerRoom(Id);
            room.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
