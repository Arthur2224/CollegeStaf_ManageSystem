using CollegeStaff_ManageSystem.dataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CollegeStaff_ManageSystem
{
    public partial class InputData_teachers : Base_InputForm_subjects
    {
        public InputData_teachers()
        {
            InitializeComponent();
            label1.Text = "Имя";
            label2.Text = "Фамилия";

        }

        private void InputData_teachers_Load(object sender, EventArgs e)
        {

        }
        protected override void button1_Click(object sender, EventArgs e)
        {   //some validation need to be here
            string query = $@"INSERT INTO teachers (name, lastName, otherName, address, positionID, characteristic, workHours) 
                VALUES 
                ('{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', '{textBox4.Text}', 
                 (SELECT positionID FROM positions WHERE positionName = '{textBox5.Text}'), '{textBox8.Text}', {maskedTextBox1.Text});

                INSERT INTO teacherSubjects (teacherID, subjectID)
                SELECT last_insert_rowid(), subjectID FROM subjects WHERE subjectName = '{textBox6.Text}';";

            DataBaseQueryProvider queryProvider = new DataBaseQueryProvider();
            queryProvider.AddNewEntity(query);
            
            textBox1.Text = null; 
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox6.Text = null;
            textBox8.Text = null;
            maskedTextBox1.Text = null;


        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
