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

        public float GetWidth()
        {
            return (int)cellWidth.Value;
        }

        public void SetWidth(float width)
        {
            cellWidth.Value = (int)width;
        }

        public float GetHeight()
        {
            return (int)cellHeight.Value;
        }

        public void SetHeight(float height)
        {
            cellHeight.Value = (int)height;
        }

        public int GetTimer()
        {
            return (int)timerSetting.Value;
        }

        public void SetTimer(int time)
        {
            timerSetting.Value = time;
        }
    }
}
