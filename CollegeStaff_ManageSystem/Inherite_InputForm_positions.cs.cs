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

namespace CollegeStaff_ManageSystem
{
    public partial class Inherite_InputForm_positions : Base_InputForm_subjects
    {
        public Inherite_InputForm_positions()
        {
            InitializeComponent();
            label1.Text = "Название должности";
            label2.Text = "Информация о ней";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
        protected override void button1_Click(object sender, EventArgs e)
        {

  
            string query = $@"INSERT INTO positions (positionName, hourPaid, information) 
                VALUES 
                    ('{textBox1.Text}',{textBox3.Text}, '{textBox2.Text}')";


            DataBaseQueryProvider queryProvider = new DataBaseQueryProvider();
            queryProvider.AddNewEntity(query);
            textBox1.Text = null; textBox2.Text=null;
            textBox3.Text = null;
        }
    }
}
