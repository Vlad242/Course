using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Atelier_course.Worker
{
    public partial class WorkerRoom : Form
    {
        private int Id;
        public MySqlConnection conn;
        public string Search = "";

        List<string> indexes = new List<string>();
        List<string> Searces = new List<string>();

        public WorkerRoom(int id)
        {
            InitializeComponent();
            Id = id;
            Config.DataBase.DataBaseInfo dataBase = new Config.DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyy:MM:dd";
            dateTimePicker1.Visible = false;
        }

        private void WorkerRoom_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT Orders.Order_id, Service_type.Type_name, Services.Service_name, Orders.Order_date, Orders.Price, Status.Status_name FROM Orders INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Service_type on(Services.Type_id=Service_type.Type_id) INNER JOIN Status on(Status.Status_id=Orders.Status_id) WHERE Orders.Worker_id=" + Id + ";", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Service");
                dataGridView1.DataSource = ds.Tables["Service"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Service type";
                dataGridView1.Columns[2].HeaderText = "Service name";
                dataGridView1.Columns[3].HeaderText = "Order date";
                dataGridView1.Columns[4].HeaderText = "Order price";
                dataGridView1.Columns[5].HeaderText = "Status name";

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                ReColorGrid();
            }
            catch (Exception)
            {

            }
        }

        private void ReColorGrid()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                switch (dataGridView1.Rows[i].Cells[5].Value)
                {
                    case "Processing":
                        {
                            dataGridView1.Rows[i].Cells[5].Style.BackColor = System.Drawing.Color.Orange;
                            break;
                        }
                    case "Active":
                        {
                            dataGridView1.Rows[i].Cells[5].Style.BackColor = System.Drawing.Color.Cyan;
                            break;
                        }
                    case "Completed":
                        {
                            dataGridView1.Rows[i].Cells[5].Style.BackColor = System.Drawing.Color.Green;
                            break;
                        }
                }
            }
        }

        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ReColorGrid();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                ReSearch();
                ////////////////////////////////
                dataGridView1.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT Orders.Order_id, Service_type.Type_name, Services.Service_name, Orders.Order_date, Orders.Price, Status.Status_name FROM Orders INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Service_type on(Services.Type_id=Service_type.Type_id) INNER JOIN Status on(Status.Status_id=Orders.Status_id) WHERE Orders.Worker_id=" + Id + " " + Search + ";", conn);
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
                dataGridView1.Columns[4].HeaderText = "Order price";
                dataGridView1.Columns[5].HeaderText = "Status name";

                ReColorGrid();
            }
            catch (Exception ex)
            {
            }
           
        }

        public void ReSearch()
        {
            string listText = "";
            string addSearch = "";
            if (comboBox1.Text != "" && textBox2.Text != "")
            {
                string operation = "";
                switch (comboBox2.Text)
                {
                    case ">":
                        operation = ">";
                        break;
                    case "<":
                        operation = "<";
                        break;
                    case "=":
                        operation = "=";
                        break;
                    case ">=":
                        operation = ">=";
                        break;
                    case "<=":
                        operation = "<=";
                        break;
                    case "!=":
                        operation = "!=";
                        break;
                    default:
                        operation = " LIKE ";
                        break;
                }
                switch (comboBox1.Text)
                {
                    case "ID":
                        {
                            addSearch += "and Orders.Order_id" + operation + "" + textBox2.Text + " ";
                            listText = "ID " + operation + " " + textBox2.Text;
                            break;
                        }
                    case "Service type":
                        {
                            addSearch += "and Service_type.Type_name" + operation + "'" + textBox2.Text + "' ";
                            listText = "Service type " + operation + " " + textBox2.Text;
                            break;
                        }
                    case "Service name":
                        {
                            addSearch += "and Services.Service_name" + operation + "'" + textBox2.Text + "' ";
                            listText = "Service name " + operation + " " + textBox2.Text;
                            break;
                        }
                    case "Order date":
                        {
                            addSearch += "and Orders.Order_date" + operation + "'" + textBox2.Text + "' ";
                            listText = "Order date " + operation + " " + textBox2.Text;
                            break;
                        }
                    case "Order price":
                        {
                            addSearch += "and Orders.Price='" + textBox2.Text + "' ";
                            listText = "Order price = " + operation + " " + textBox2.Text;
                            break;
                        }
                    case "Status name":
                        {
                            addSearch += "and Status.Status_name" + operation + "'" + textBox2.Text + "' ";
                            listText = "Status name " + operation + " " + textBox2.Text;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                bool flag = true;
                foreach (var item in listBox1.Items)
                {
                    if (item.ToString() == listText)
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    Search += addSearch;
                    Searces.Add(addSearch);
                    indexes.Add(listText);
                    listBox1.Items.Add(listText);
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Searces.Clear();
            listBox1.Items.Clear();
            Search = "";

            WorkerRoom_Load(null, null);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.Items.Count != 0)
                {
                    for (int i = 0; i < indexes.Count; i++)
                    {
                        if (indexes[i] == listBox1.SelectedItem.ToString())
                        {
                            int index = Search.IndexOf(Searces[i]);
                            int lenght = Searces[i].Length;
                            int ln = Search.Length;
                            Search = Search.Remove(index, lenght);
                            indexes.Remove(indexes[i]);
                            Searces.Remove(Searces[i]);
                            listBox1.Items.Remove(listBox1.SelectedItem);
                        }
                    }
                }
                comboBox1.Text = "";
                textBox2.Text = "";
                button2.PerformClick();
            }
            catch (Exception)
            {

            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Text = dataGridView1.CurrentCell.Value.ToString();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Order date")
            {
                dateTimePicker1.Visible = true;
            }
            else
            {
                dateTimePicker1.Visible = false;
            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = dateTimePicker1.Text;
        }

        private void WorkerRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            Login.LogIn logIn = new Login.LogIn();
            conn.Close();
            logIn.Show();
            this.Dispose();
        }

        private void ToolStripButton7_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            string path = "";

            if (s.ShowDialog() == DialogResult.OK)
            {
                path = s.FileName;
            }

            try
            {
                FileStream fs = new FileStream(@path, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fs);


                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    streamWriter.Write(dataGridView1.Columns[j].HeaderText + " ");
                }
                streamWriter.WriteLine();

                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    for (int i = 0; i < dataGridView1.Rows[j].Cells.Count; i++)
                    {
                        streamWriter.Write(dataGridView1.Rows[j].Cells[i].Value + " ");
                    }

                    streamWriter.WriteLine();
                }

                streamWriter.Close();
                fs.Close();

                MessageBox.Show("File saved successfully");
            }
            catch
            {
                MessageBox.Show("Saving file Error!");
            }
        }

        private void ToolStripButton8_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog
            {
                Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*"
            };
            string path = "";

            if (s.ShowDialog() == DialogResult.OK)
            {
                path = s.FileName;
            }

            try
            {
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT Orders.Order_id, Service_type.Type_name, Services.Service_name, Orders.Order_date, Orders.Price, Status.Status_name FROM Orders INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Service_type on(Services.Type_id=Service_type.Type_id) INNER JOIN Status on(Status.Status_id=Orders.Status_id) WHERE Orders.Worker_id=" + Id + " " + Search + ";", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Orders");

                iTextSharp.text.Document doc = new iTextSharp.text.Document();

                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));

                doc.Open();

                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

                PdfPTable table = new PdfPTable(ds.Tables["Orders"].Columns.Count);

                PdfPCell cell = new PdfPCell(new Phrase(" " + "Orders" + " ", font))
                {
                    Colspan = ds.Tables["Orders"].Columns.Count,
                    HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                    Border = 0
                };
                table.AddCell(cell);
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    cell = new PdfPCell(new Phrase(new Phrase(dataGridView1.Columns[j].HeaderText, font)))
                    {
                        BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY
                    };
                    table.AddCell(cell);
                }

                for (int j = 0; j < ds.Tables["Orders"].Rows.Count; j++)
                {
                    for (int k = 0; k < ds.Tables["Orders"].Columns.Count; k++)
                    {
                        table.AddCell(new Phrase(ds.Tables["Orders"].Rows[j][k].ToString(), font));
                    }
                }
                doc.Add(table);

                doc.Close();

                MessageBox.Show("Pdf-document saved!");
            }
            catch (Exception)
            {
                MessageBox.Show("Problems with saving to PDF format!");
            }
        }

        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog
            {
                Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*"
            };
            string path = "";

            if (s.ShowDialog() == DialogResult.OK)
            {
                path = s.FileName;
            }
            try
            {
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);

                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Orders"
                };
                sheets.Append(sheet);
                spreadsheetDocument.Close();

                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                ExcelApp.Application.Workbooks.Open(path);
                ExcelApp.Columns.ColumnWidth = 15;

                ExcelApp.Cells[1, 1] = "ID";
                ExcelApp.Cells[1, 2] = "Service type";
                ExcelApp.Cells[1, 3] = "Service name";
                ExcelApp.Cells[1, 4] = "Order date";
                ExcelApp.Cells[1, 5] = "Order price";
                ExcelApp.Cells[1, 6] = "Status name";

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    for (int j = 0; j < dataGridView1.RowCount - 1; j++)
                    {
                        ExcelApp.Cells[j + 2, i + 1] = (dataGridView1.Rows[j].Cells[i].Value).ToString();
                    }
                }
                ExcelApp.Quit();
            }
            catch (Exception)
            {

            }
        }

        private void ToolStripButton9_Click(object sender, EventArgs e)
        {
            try
            {
                bmp = new Bitmap(dataGridView1.Size.Width + 10, dataGridView1.Size.Height + 10);
                dataGridView1.DrawToBitmap(bmp, dataGridView1.Bounds);
                if ((dataGridView1.Size.Width + 10) > 210)
                {
                    printDocument1.DefaultPageSettings.Landscape = true;
                }
                else
                {
                    printDocument1.DefaultPageSettings.Landscape = false;
                }
                printDocument1.DefaultPageSettings.Color = false;

                printPreviewDialog1.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Printing Error!");
            }
        }

        Bitmap bmp;
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void CreateModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateModel model = new CreateModel(Id);
            model.Show();
            conn.Close();
            this.Dispose();
        }

        private void CompliteOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompleteOrder order = new CompleteOrder(Id);
            order.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
