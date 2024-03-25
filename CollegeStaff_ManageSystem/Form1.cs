using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollegeStaff_ManageSystem
{
    public partial class Form1 : Form
    {   Color mainYellowClr=Color.FromArgb(255, 207, 72);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             LoadData("teachers");

            label1.Text = "Преподаватели колледжа";
            label1.Location=new Point(this.Width/2-label1.Size.Width/2,40);

            panel1.Location = new Point( this.Width/6, this.Height/6);
            panel1.Size = new Size(this.Width / 2+this.Width/5, this.Height / 2 + this.Height / 5);
            panel1.BackColor =mainYellowClr;
            
           
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.Location = new Point(panel1.Width / 10, panel1.Height / 6);
            dataGridView1.BackgroundColor = mainYellowClr;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            radioButtonPanel.Location = new Point(dataGridView1.Location.X+dataGridView1.Width+100,panel1.Height/6);
            radioButton1.Checked = true;
            radioButton1.Text = "Таблицы с преподавателями";
            radioButton2.Text = "Таблицы с предметами";
            radioButton3.Text = "Таблицы с должностями";



        }
        private void LoadData(string tableName)
        {
            string connectionString = @"Data Source=..\..\\Files\\CollegeStaff.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = $"SELECT * FROM {tableName};";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable; 

                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            LoadData("teachers");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            LoadData("subjects");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            LoadData("positions");
        }
    }
}
