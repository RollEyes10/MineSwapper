using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSwapper

{
    public partial class Form1 : Form
    {

        public static int cellSize = 30; //jednotna delka do vsech stran
        public static Game game; // game = instance tridy

        public static string rootDir = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static string imgDir = rootDir + "/images/"; // lomitka??
        public static string[] imgPaths = Directory.GetFiles(imgDir);
        public static Image[] images = new Image[imgPaths.Length]; // velikost pole podle pole imgPath
        public static Color[] colorsLetters =
        {
            Color.Black,     // 0
            Color.Blue,      // 1
            Color.Green,     // 2
            Color.Red,       // 3
            Color.Orange,    // 4
            Color.Purple,    // 5
            Color.LightBlue, // 6
            Color.Brown,     // 7
            Color.Pink,      // 8
        };

        public int margin = 10;

        public Form1()
        {
            InitializeComponent();
            game = new Game();
            

            pictureBox1.Refresh();

            LoadImages();
            pictureBoxHeader.Image = images[7];
            game.SetGameParameters(Game.GameLevel.beginner);
            SetWindow();
        }

        void SetWindow()
        {
            // velikost herniho pole - pictureBox1
            pictureBox1.Width = game.gridSizeX*cellSize;
            pictureBox1.Height = game.gridSizeY*cellSize;

            // nastaveni okna
            this.Width = pictureBox1.Width+margin * 4;
            this.Height = pictureBox1.Height+margin + 120; // nahore menu a smajlik
            // umisteni dalsich komponent v okne
            pictureBoxHeader.Left= this.Width/2 - pictureBoxHeader.Width/2;
            pictureBox1.Left = this.Width / 2 - pictureBox1.Width / 2;
            labelCountMines.Left= this.Width - labelCountMines.Width - margin*2;

            // aktivace zobrazeni hry
            timer1.Start();
            pictureBox1.Refresh();
            pictureBoxHeader.Refresh();

            // nastaveni timeru a poctu odhalenych min
            labelTime.Text = game.timeLeft.ToString();
            labelCountMines.Text=game.mineCount.ToString();
        }

        public static int ConvertXYtoIndex(int x, int y)
        {
            return y * game.gridSizeX + x;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show($"Souradnice kliknuti: X={e.X}, Y={e.Y}");

            int cellX = e.X/cellSize;
            int cellY = e.Y/cellSize;  
            int cellIndex = ConvertXYtoIndex(cellX, cellY);
            Cell c = game.cells[cellIndex];

            //levy klik
            if (e.Button == MouseButtons.Left)
            {
                //game.cells[cellIndex].state = Cell.CellState.revealed;
                game.RevealCell(c);

                if (game.TestOfVictory())
                {
                    game.running = false;
                    game.gameOver = false;
                }

                if (c.mined)

                {
                    // Handle game over (e.g., reveal all cells and disable clicks)
                    game.running = false;
                    game.gameOver = true;
                    //GameOver();
                }
            }

            // pravy klik
            if (e.Button == MouseButtons.Right)
            {
                if (c.state == Cell.Cellstate.hidden)
                {
                    c.state = Cell.Cellstate.flagged;
                }
                else if (c.state == Cell.Cellstate.flagged)
                {   
                    c.state = Cell.Cellstate.hidden;
                }
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g= e.Graphics;


            // projdi seznam bunek a kazdou na jejich souradnici vykresli i s cislem ktere obsahuje
            foreach (Cell c in game.cells)
            {
                //Přidat podmínku pro vykreslení políčka

                // skryte policka
                if (c.state == Cell.Cellstate.hidden)
                {
                    g.DrawImage(images[2], c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
                }

                // policka s vlajkou
                else if (c.state == Cell.Cellstate.flagged)
                {
                    g.DrawImage(images[4], c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
                }
                else
                {   
                    // cislo 0
                    if (c.minesArround == 0)
                    {
                        g.DrawImage(images[3], c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
                    }
                    // vypis cisel okolnych min
                    else
                    {
                        g.DrawImage(images[3], c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
                        g.DrawString(c.minesArround.ToString(), new Font("Arial", 16), new SolidBrush(colorsLetters[c.minesArround]), c.X * cellSize, c.Y * cellSize);
                    }
                    // vykresleni min
                    if (c.mined == true)
                    {
                        g.DrawImage(images[1], c.X * cellSize, c.Y * cellSize, cellSize, cellSize);
                    }
                }
            }
            if (!game.running)
            {
                timer1.Stop();
                if (game.gameOver)
                {
                    // Vykreslení nápisu pro prohru
                    g.DrawString("GAME OVER", new Font("Arial", 20, FontStyle.Bold),
                                 new SolidBrush(Color.Red), new PointF(50, 50));
                }
                else
                {
                    // Vykreslení nápisu pro výhru (pokud budeš chtít přidat)
                    g.DrawString("YOU WIN!", new Font("Arial", 36, FontStyle.Bold),
                                 new SolidBrush(Color.Green), new PointF(50, 50));
                }
            }
        }

        void LoadImages()
        {
            try
            {
                for (int i = 0; i < imgPaths.Length; i++)
                {
                    images[i] = Image.FromFile(imgPaths[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání obrázků: {ex.Message}");
            }
        }

        private void TestImages()
        {
            var g = pictureBox1.CreateGraphics();
            for (int i = 0; i < imgPaths.Length; i++)
            {
                g.DrawImage(images[i], i*cellSize, i*cellSize); 

            }
        }

        private void GameOver()
        {
            // Handle the end of the game (show all cells, disable further clicks)
            foreach (var cell in game.cells)
            {
                if (cell.state == Cell.Cellstate.hidden)
                {
                    cell.state = Cell.Cellstate.revealed;
                }
            }

            MessageBox.Show("Game Over!");
        }

        public void StartNewGame()
        {

        }
        private void pictureBoxHeader_Click(object sender, EventArgs e)
        {
            
        }

        private void beginnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.level = Game.GameLevel.beginner;
            game.SetGameParameters(game.level);
            // start nove hry + nastaveni okna
            SetWindow();
        }

        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.level = Game.GameLevel.advanced;
            game.SetGameParameters(game.level);
            // start nove hry + nastaveni okna
            SetWindow();
        }

        private void expertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.level = Game.GameLevel.expert;
            game.SetGameParameters(game.level);
            // start nove hry + nastaveni okna
            SetWindow();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            game.timeLeft--;
            labelTime.Text = game.timeLeft.ToString();

            // VYPRSENI CASU - kontrola a osetreni
            if (game.timeLeft < 1)
            {
                game.running = false;
                game.gameOver = true;
                

                foreach(Cell cell in game.cells)
                {
                    cell.state= Cell.Cellstate.revealed;
                }
                pictureBoxHeader.Refresh();
                pictureBox1.Refresh();
            }
        }

        private void pictureBoxHeader_Paint(object sender, PaintEventArgs e)
        {
            if (!game.running)
            {
                pictureBoxHeader.BackgroundImage = images[4];
            }
            else 
            {
                if (game.gameOver)
                {
                    pictureBoxHeader.BackgroundImage = images[7];
                }
                else 
                {
                    pictureBoxHeader.BackgroundImage = images[6];
                }
            }
        }
    }
}