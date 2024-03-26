using CollegeStaff_ManageSystem.dataBase;
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
    public partial class MainMenu : Form
    {   Color mainYellowClr=Color.FromArgb(255, 207, 72);
        private string connectionString= @"Data Source=..\..\\Files\\CollegeStaff.db;Version=3;";
        private string selectedTableName = "teachers";
        private string selectedTablePKName = "teacherId";
        private DataBaseQueryProvider queryProvider;

        public MainMenu()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            queryProvider = new DataBaseQueryProvider();
             LoadData("teachers");

            label1.Text = "Преподаватели колледжа";
            label1.Location=new Point(this.Width/2-label1.Size.Width/2,40);

            panel1.Location = new Point( this.Width/6, this.Height/5);
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

            inputFormButton.Location=new Point(dataGridView1.Location.X, dataGridView1.Location.Y+dataGridView1.Size.Height+50);
            inputFormButton.Text="Ввести";

            deleteDataButton.Location = new Point(inputFormButton.Location.X+inputFormButton.Size.Width, dataGridView1.Location.Y + dataGridView1.Size.Height + 50);
            deleteDataButton.Text = "Удалить";



        }
        private void LoadData(string tableName)
        {
            
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
        private void SaveData()
        {
            using (SQLiteConnection connection = new SQLiteConnection())
            {
                connection.Open();

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
            selectedTableName = "teachers";
            selectedTablePKName = "teacherId";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            LoadData("subjects");
            selectedTableName = "subjects";
            selectedTablePKName = "subjectId";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            LoadData("positions");
            selectedTableName = "positions";
            selectedTablePKName= "positionId";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string selectedTablePkValue;
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    selectedTablePkValue = dataGridView1.SelectedRows[i].Cells[selectedTablePKName].Value.ToString();
                    queryProvider.DeleteRecord(selectedTableName, selectedTablePKName, selectedTablePkValue);
                }
                LoadData(selectedTableName);
            }
            else
            {
                MessageBox.Show("Не выбрана ни одна строка для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {   if(radioButton1.Checked)
            {


                Form InputData_teachers = new InputData_teachers();
                InputData_teachers.Show();

            }
            else if(radioButton2.Checked)
            {
                Form inputSubjectsForm = new Base_InputForm_subjects();
                inputSubjectsForm.Show();
            }
            else if (radioButton3.Checked)
            {
               
                Form inputPositionsForm = new Inherite_InputForm_positions();
                inputPositionsForm.Show();
            }
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           dataGridView1.DataSource= queryProvider.GetAverageWorkHours();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                dataGridView1.DataSource = queryProvider.GetTeachersByCity(textBox1.Text);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите город.");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = queryProvider.GetSubjectsForTeachers();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                dataGridView1.DataSource = queryProvider.GetTeachersByLastNameInitial(textBox2.Text);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите инициал фамилии.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int hourlyRate;
            if (int.TryParse(maskedTextBox1.Text, out hourlyRate))
            {
                dataGridView1.DataSource = queryProvider.GetTeachersByHourlyRate(hourlyRate);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректную почасовую ставку.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox3.Text) && !string.IsNullOrWhiteSpace(maskedTextBox3.Text))
            {
                dataGridView1.DataSource = queryProvider.QueryWithParameters(textBox3.Text, Convert.ToDecimal(maskedTextBox3.Text));
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = queryProvider.FetchLastNamesAndInitials();
        }

        

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = queryProvider.ChangeCaseOfPositionName("UPPER");
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = queryProvider.ChangeCaseOfPositionName("LOWER");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = queryProvider.GetMaxHourlyRate();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = queryProvider.GetTotalWorkHours();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = queryProvider.CountSubjectsPerTeacher();
        }
    }
}
