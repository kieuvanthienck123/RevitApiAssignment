using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Autodesk.Revit.UI;



namespace GetAreaValue
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public List<WallParams> data;

        public Form1(ExternalCommandData commandData)
        {
            InitializeComponent();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bricksTotal = 0;
            for (int i = 0; i < data.Count; i++)
            {
                var lg = Double.Parse(textBox3.Text) / 1000;
                var bg = Double.Parse(textBox4.Text) / 1000;
                var doubleValue = data[i].area / (lg * bg);
                data[i].numberOfBrick = Convert.ToInt32(doubleValue);
                bricksTotal += data[i].numberOfBrick;
            }

            var bindingList = new BindingList<WallParams>(data);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;

            textBox1BrickTotal.Text = bricksTotal.ToString();

            dataGridView1.Columns[1].HeaderText = "Volume (m3)";
            dataGridView1.Columns[2].HeaderText = "Area (m2)";
            dataGridView1.Columns[3].HeaderText = "Thickness (mm)";
        }

        public void setDataGrid(List<WallParams> data)
        {
            this.data = data;
        }

    }
}
