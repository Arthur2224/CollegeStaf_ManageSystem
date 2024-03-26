using CollegeStaff_ManageSystem.dataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace CollegeStaff_ManageSystem
{
    public partial class Base_InputForm_subjects : Form
    {
        protected Label label1;
        protected Label label2;
        protected TextBox textBox1;
        protected TextBox textBox2;
      
        public Base_InputForm_subjects()
        {
            InitializeComponent();
        }

        protected virtual void button1_Click(object sender, EventArgs e)
        {
            string query = $@"INSERT INTO subjects (subjectName, information) 
                             VALUES ('{textBox1.Text}','{textBox2.Text}');";
            DataBaseQueryProvider queryProvider = new DataBaseQueryProvider();
            queryProvider.AddNewEntity(query);
            textBox1.Text = null; textBox2.Text=null;
           
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Base_InputForm_subjects_Load(object sender, EventArgs e)
        {

        }
    }
}
