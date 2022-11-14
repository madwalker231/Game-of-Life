using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLStartUpTemplate1
{
    public partial class RandomBox : Form
    {
        public RandomBox()
        {
            InitializeComponent();
            numericUpDown1.Minimum = -2147483647;
            numericUpDown1.Maximum = 2147483647;
        }

        public int Random
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        private void RandomizeButton(object sender, EventArgs e)
        {
            numericUpDown1.Value = new Random ().Next(int.MinValue, int.MaxValue);
        }
    }
}
