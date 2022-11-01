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
    public partial class OptionsMenu : Form
    {
        public OptionsMenu()
        {
            InitializeComponent();
        }

        public int Width
        {
            get { return (int)cellWidth.Value; }
            set { cellWidth.Value = value; }
        }

        public int Height
        {
            get { return (int)cellHeight.Value; }
            set { cellHeight.Value = value; }
        }

        public int Timer
        {
            get { return (int)timerSetting.Value; }
            set { timerSetting.Value = value; }
        }
    }
}
