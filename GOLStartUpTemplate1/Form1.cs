using System;
using System.CodeDom;
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
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[50, 50];
        bool[,] scratchPad = new bool[50, 50];
        int uWidth;
        int uHeight;

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running

            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            uHeight = Properties.Settings.Default.GridHeight;
            uWidth = Properties.Settings.Default.GridWidth;
            bool[,] tempGrid = new bool[uWidth, uHeight];
            universe = tempGrid;
            scratchPad = tempGrid;
            //optionsMenu.TimerSetting = Properties.Settings.Default.TimeSet;
        }
        #region Next Gen Logic
        // Calculate the next generation of cells
        private void NextGeneration()
        {
            Array.Clear(scratchPad, 0, scratchPad.Length);
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    bool currentCell = universe[x, y];
                    int neighbor = CountNeighborsFinite(x, y);
                    if (currentCell == true && neighbor < 2)
                    {
                        scratchPad.SetValue(false, x, y);
                    }
                    if (currentCell == true && neighbor > 3)
                    {
                        scratchPad.SetValue(false, x, y);
                    }
                    if (currentCell == true && neighbor == 2 || neighbor == 3)
                    {
                        scratchPad.SetValue(true, x, y);
                    }
                    if (currentCell == false && neighbor == 3)
                    {
                        scratchPad.SetValue(true, x, y);
                    }
                }
            }
            //copy/swap from sctach pag to universe
            bool[,] swap = universe;
            universe = scratchPad;
            scratchPad = swap;

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            //call invalidate for play button.
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Timer Logic
        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }
        #endregion

        #region Grid Paint
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            //change cell width and height to floats!!!!
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = RectangleF.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }
        #endregion

        #region Cell Click
        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = (int)(e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = (int)(e.Y / cellHeight);

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        #region Exit Button
        private void ExitButton(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region New File Buttons
        private void NewGridMenu(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                    generations = 0;
                    toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
                    timer.Enabled = false;
                }
            }
            graphicsPanel1.Invalidate();
        }

        private void NewToolBar(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                    generations = 0;
                    toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
                    timer.Enabled = false;
                }
            }
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Reset Option
        private void ResetGrid(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            Properties.Settings.Default.Reset();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            uHeight = Properties.Settings.Default.GridHeight;
            uWidth = Properties.Settings.Default.GridWidth;
            bool[,] resetGrid = new bool[uWidth, uHeight];
            bool[,] resetGrid2 = new bool[uWidth, uHeight];
            universe = resetGrid;
            scratchPad = resetGrid2;
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Reload Options
        private void reloadGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
        }
        #endregion

        #region Play/Pause/NextGen buttons
        private void PlayButton(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void PauseButton(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void NextGenButton(object sender, EventArgs e)
        {
            NextGeneration();
        }
        private void PlayButtonMenu(object sender, EventArgs e)
        {

        }

        private void PauseButtonMenu(object sender, EventArgs e)
        {

        }

        private void NextGenButtonMenu(object sender, EventArgs e)
        {

        }
        #endregion

        private void StopAtButtonMenu(object sender, EventArgs e)
        {

        }
        #region Finite
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        #endregion

        #region Toroidal
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xLen = -1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yLen = -1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xLen = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yLen = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        #endregion 

        #region Grid Toggle option
        private void ToroidalGrid(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    CountNeighborsToroidal(x, y);
                }
            }
        }

        private void FiniteGrid(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    CountNeighborsFinite(x, y);
                }
            }
        }
        #endregion

        #region Right Click Color
        private void backroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = graphicsPanel1.BackColor;
            if ( DialogResult.OK == colorDialog.ShowDialog())
            {
                graphicsPanel1.BackColor = colorDialog.Color;
                graphicsPanel1.Invalidate();
            }

        }

        private void gridColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = gridColor;
            if (DialogResult.OK == colorDialog.ShowDialog())
            {
                gridColor = colorDialog.Color;
                graphicsPanel1.Invalidate();
            }
        }
        
        private void cellColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = cellColor;
            if (DialogResult.OK == colorDialog.ShowDialog())
            {
                cellColor = colorDialog.Color;
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        private void modalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        #region Option Menu
        private void Options(object sender, EventArgs e)
        {
            OptionsMenu optionsMenu = new OptionsMenu();
            optionsMenu.Height = universe.GetLength(1);
            optionsMenu.Width = universe.GetLength(0);
            optionsMenu.TimerSetting = timer.Interval;
            if (DialogResult.OK == optionsMenu.ShowDialog())
            {
                bool[,] temp = new bool[optionsMenu.Width, optionsMenu.Height];
                bool[,] temp2 = new bool[optionsMenu.Width, optionsMenu.Height];
                scratchPad = temp;
                universe = temp2;
                uWidth = optionsMenu.Width;
                uHeight = optionsMenu.Height;
                timer.Interval = optionsMenu.TimerSetting;
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        #region User Setting Saves
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.PanelColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.GridHeight = uHeight;
            Properties.Settings.Default.GridWidth = uWidth;
            //Properties.Settings.Default.TimeSet = TimerSetting;
            Properties.Settings.Default.Save();
        }
        #endregion

    }
}
