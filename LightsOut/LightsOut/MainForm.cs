using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GridOffset = 25; // Distance from upper-left side of window

        private int gridLength = 200; // Size in pixels of grid
        private int numCells = 3; // Number of cells in grid
        private int cellLength = 0;
        private bool[,] grid; // Stores on/off state of cells in grid
        private Random rand; // Used to generate random numbers

        public MainForm()
        {
            InitializeComponent();

            rand = new Random(); // Initializes random number generator

            loadNewGrid();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int r = 0; r < numCells; r++)
            {
                for (int c = 0; c < numCells; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid section
                    Brush brush;
                    Pen pen;
                    if (grid[r, c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White; // On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black; // Off
                    }
                    // Determine (x,y) coord of row and col to draw rectangle
                    int x = c * cellLength + GridOffset;
                    int y = r * cellLength + GridOffset;
                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, cellLength, cellLength);
                    g.FillRectangle(brush, x + 1, y + 1, cellLength - 1, cellLength - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Make sure click was inside the grid
            if (e.X < GridOffset || e.X > cellLength * numCells + GridOffset ||
            e.Y < GridOffset || e.Y > cellLength * numCells + GridOffset)
                return;
            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / cellLength;
            int c = (e.X - GridOffset) / cellLength;
            // Invert selected box and all surrounding boxes
            for (int i = r - 1; i <= r + 1; i++)
                for (int j = c - 1; j <= c + 1; j++)
                    if (i >= 0 && i < numCells && j >= 0 && j < numCells)
                        grid[i, j] = !grid[i, j];
            // Redraw grid
            this.Invalidate();
            // Check to see if puzzle has been solved
            if (PlayerWon())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool PlayerWon()
        {
            bool result = true;

            for (var i = 0; i < numCells; i++)
            {
                for (var j = 0; j < numCells; j++)
                {
                    if (grid[i, j])
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // Fill grid with either white or black
            for (int r = 0; r < numCells; r++)
                for (int c = 0; c < numCells; c++)
                    grid[r, c] = rand.Next(2) == 1;
            // Redraw grid
            this.Invalidate();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGameButton_Click(sender, e);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void X3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = true;
            x4ToolStripMenuItem.Checked = false;
            x5ToolStripMenuItem.Checked = false;

            numCells = 3;
            loadNewGrid();
        }

        private void X4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = false;
            x4ToolStripMenuItem.Checked = true;
            x5ToolStripMenuItem.Checked = false;

            numCells = 4;
            loadNewGrid();
        }

        private void X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = false;
            x4ToolStripMenuItem.Checked = false;
            x5ToolStripMenuItem.Checked = true;

            numCells = 5;
            loadNewGrid();
        }

        private void loadNewGrid()
        {
            cellLength = gridLength / numCells;
            grid = new bool[numCells, numCells];

            // Turn entire grid on
            for (int r = 0; r < numCells; r++)
                for (int c = 0; c < numCells; c++)
                    grid[r, c] = true;
            this.Invalidate();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            gridLength = this.Height - 119;
            if (this.Width - 2 * GridOffset < gridLength)
            {
                gridLength = this.Width - 2 * GridOffset;
            }
            cellLength = gridLength / numCells;
            this.Invalidate();
        }
    }
}
