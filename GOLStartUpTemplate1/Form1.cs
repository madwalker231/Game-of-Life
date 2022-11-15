using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLStartUpTemplate1
{
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[30, 30];
        bool[,] scratchPad = new bool[30, 30];
        int uWidth;
        int uHeight;
        int rando;
        int aliveCells;
        bool isFinite = true;
        bool NeighborCountOn = true;

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
            bool[,] tempGrid2 = new bool[uWidth, uHeight];
            universe = tempGrid;
            scratchPad = tempGrid2;
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
                    int neighbor;
                    if(isFinite)
                    {
                        neighbor = CountNeighborsFinite(x, y);
                    }
                    else
                    {
                        neighbor = CountNeighborsToroidal(x, y);
                    }
                    bool currentCell = universe[x, y];
                    
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
            LivingCell();

            
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

                    Font font = new Font("Arial", 7f);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;                    
                    if (NeighborCountOn)
                    {
                        int group;
                        if (isFinite)
                        {
                            group = CountNeighborsFinite(x, y);
                        }
                        else
                        {
                            group = CountNeighborsToroidal(x, y);
                        }
                        e.Graphics.DrawString(group.ToString(), font, Brushes.Red, cellRect, stringFormat);
                    }
                    //if (!NeighborCountOn)
                    //{
                    //    int group;
                    //    if (isFinite)
                    //    {
                    //        group = CountNeighborsFinite(x, y);
                    //    }
                    //    else
                    //    {
                    //        group = CountNeighborsToroidal(x, y);
                    //    }
                    //    //e.Graphics.DrawString(group.ToString(), font, Brushes.Red, cellRect, stringFormat);
                    //}
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

                LivingCell();

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
                        xCheck = xLen - 1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        #endregion 

        #region Grid Toggle option
        private void ToroidalGrid_Click(object sender, EventArgs e)
        {
            isFinite = false;
            FiniteGrid.Checked = false;
            FiniteRClick.Checked = false;
            ToroidalGrid.Checked = true;
            ToroidalRClick.Checked = true;
            graphicsPanel1.Invalidate();
        }

        private void FiniteGrid_Click(object sender, EventArgs e)
        {
            isFinite = true;
            FiniteGrid.Checked = true;
            FiniteRClick.Checked = true;
            ToroidalGrid.Checked = false;
            ToroidalRClick.Checked = false;
            graphicsPanel1.Invalidate();
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

        #region Random
        private void RandomizeTool(object sender, EventArgs e)
        {
            Random randy = new Random(4);
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    randy.Next(0, 2);
                    if(randy.Next() == 0) 
                    {
                        graphicsPanel1.Invalidate();
                    }
                }
            }
        }

        private void RandomTime()
        {
            Random randyTime = new Random();
            int a;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    a = randyTime.Next(0, 2);
                    if (a == 0) 
                    {
                        universe.SetValue(true, x, y);
                    }
                    else
                    {
                        universe.SetValue(false, x, y);
                    }
                }
            }
        }

        private void RandomSeed ()
        {
            Random randySeed = new Random(rando);
            int a;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    a = randySeed.Next(0, 2);
                    if (a == 0)
                    {
                        universe.SetValue(true, x, y);
                    }
                    else
                    {
                        universe.SetValue(false, x, y);
                    }
                }
            }
        }

        private void RandoTime_Click(object sender, EventArgs e)
        {
            RandomTime();
            graphicsPanel1.Invalidate();
        }

        private void RandoSeed_Click(object sender, EventArgs e)
        {
            RandomBox seeds = new RandomBox();
            seeds.Random = rando;
            if (DialogResult.OK == seeds.ShowDialog()) 
            {
                rando = seeds.Random;
                RandomSeed();
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        #region Save File
        private void SafeFileButton(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!Your saved universe.");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        if (universe[x, y] == true)
                        {
                            currentRow += 'O';
                        }

                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        else if (universe[x, y] == false)
                        {
                            currentRow += '.';
                        }
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }
        #endregion

        #region Open File
        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;
            int yPos = 0;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if(row.StartsWith("!"))
                    {
                        continue;
                    }

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    else
                    {
                        maxHeight++;
                    }

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                bool[,] uniFile = new bool[maxWidth, maxHeight];
                bool[,] scratchFile = new bool[maxWidth, maxHeight];
                universe = uniFile;
                scratchPad = scratchFile;
                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.StartsWith("!")) 
                    {
                        continue;
                    }

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.
                        if (row[xPos] == 'O')
                        {
                            universe.SetValue(true, xPos, yPos);
                        }

                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                        if (row[xPos] == '.')
                        {
                            universe.SetValue(false, xPos, yPos);
                        }
                    }
                    yPos++;
                }

                // Close the file.
                reader.Close();
                graphicsPanel1.Invalidate();
            }

        }
        #endregion

        #region Cell Count
        private void LivingCell()
        {
            int livingCell = aliveCells;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x,y] == true)
                    {
                        livingCell++;
                    }
                }
            }
            aliveCells = livingCell;
            LivingCells.Text = "Current Living Cells = " + aliveCells.ToString();
            aliveCells = aliveCells - aliveCells;
        }


        #endregion

        #region Cell Neighbor Count

        #endregion

        private void heighborCountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NeighborCountOn)
            {
                heighborCountsToolStripMenuItem.Checked = false;
                NeighborCountOn = false;
            }
            else
            {
                heighborCountsToolStripMenuItem.Checked = true;
                NeighborCountOn = true;
            }
            graphicsPanel1.Invalidate();
        }
    }
}
