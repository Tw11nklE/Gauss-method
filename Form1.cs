using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya_rabota
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dalee_Click(object sender, EventArgs e)
        {
            int numberOfUnknowns = Convert.ToInt32(comboBox1.SelectedItem);
            NewForm newForm = new NewForm(numberOfUnknowns);
            newForm.Show();
            this.Hide();
        }
    }
}
