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
        private int cellLength = 0;

        private LightsOutGame _game = new LightsOutGame();

        public MainForm()
        {
            InitializeComponent();
            
            loadNewGrid();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int r = 0; r < _game.GridSize; r++)
            {
                for (int c = 0; c < _game.GridSize; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid section
                    Brush brush;
                    Pen pen;
                    if (_game.GetGridValue(r, c))
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
            if (e.X < GridOffset || e.X > cellLength * _game.GridSize + GridOffset ||
            e.Y < GridOffset || e.Y > cellLength * _game.GridSize + GridOffset)
                return;
            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / cellLength;
            int c = (e.X - GridOffset) / cellLength;
            _game.Move(r, c);
            // Redraw grid
            this.Invalidate();
            // Check to see if puzzle has been solved
            if (_game.IsGameOver())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.NewGame();
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

            _game.GridSize = 3;
            loadNewGrid();
        }

        private void X4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = false;
            x4ToolStripMenuItem.Checked = true;
            x5ToolStripMenuItem.Checked = false;

            _game.GridSize = 4;
            loadNewGrid();
        }

        private void X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = false;
            x4ToolStripMenuItem.Checked = false;
            x5ToolStripMenuItem.Checked = true;

            _game.GridSize = 5;
            loadNewGrid();
        }

        private void loadNewGrid()
        {
            cellLength = gridLength / _game.GridSize;

            _game.NewGame();
            this.Invalidate();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            gridLength = this.Height - 119;
            if (this.Width - 2 * GridOffset < gridLength)
            {
                gridLength = this.Width - 2 * GridOffset;
            }
            cellLength = gridLength / _game.GridSize;
            this.Invalidate();
        }
    }
}
