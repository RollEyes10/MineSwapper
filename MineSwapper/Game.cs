using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSwapper
{
    public class Game
    {
        public List<Cell> cells= new List<Cell>();

        public int gridSizeX = 10; // pocet bunek na sirku
        public int gridSizeY = 10; // vyska herniho pole
        public int mineCount = 15;
        public bool running = true; 
        public bool gameOver = false; // konec hry
        public int timeLeft = 0; // prenastavit

        public enum GameLevel
        {
            beginner,
            advanced,
            expert
        }
        public GameLevel level = GameLevel.beginner; // aktualni herni level

        public void CreateGame() 
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int x = 0; x < gridSizeX; x++)
                {

                    cells.Add(new Cell(x, y));
                }
            }
        }
        

        public void PrepareGame()
        {
            PlaceMines();
            foreach (Cell c in cells)
            {
                c.minesArround = CountMinesArround(c.X, c.Y);
            }
        }

        public void SetGameParameters(GameLevel level)
        {
            switch (level) 
            {
                case GameLevel.beginner:
                    StartNewGame(7, 7, 5,60);
                    break;
                case GameLevel.advanced:
                    StartNewGame(10, 12, 20, 120);
                    break;
                case GameLevel.expert:
                    StartNewGame(30,20 ,100, 180);
                    break;

            }    
            
        }
        public void StartNewGame(int gridX, int gridY, int mineCount, int timeLeft)
        {
            gridSizeX = gridX;
            gridSizeY = gridY;
            this.mineCount = mineCount;
            this.timeLeft = timeLeft;
            running = true;
            gameOver = false;
            cells.Clear();

            CreateGame();
            PrepareGame();

        }
        public void PlaceMines()
        {
            Random rnd = new Random();
            int placed = 0;

            // pozor, for nepomuze, protoze to vygeneruje nektere miny na stejne misto
            do
            {
               int rndIndex = rnd.Next(gridSizeX * gridSizeY);
                if (cells[rndIndex].mined == false)
                {
                    cells[(rndIndex)].mined = true;
                    placed++;
                }

                
            }
            while (placed <mineCount);
            

        }


        public int CountMinesArround (int x, int y)
        {
            int count = 0;
            List<Cell> neighborous = FindNeighbours(x, y);
            foreach(Cell c in neighborous)
            {
                if(c.mined)
                    count++;
            }

            return count;
        } 

        public List<Cell> FindNeighbours(int x, int y)
        {
            List<Cell> neighbours = new List<Cell>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    int newX = x + i;
                    int newY = y + j;
                    if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY)
                    {
                        neighbours.Add(cells[Form1.ConvertXYtoIndex(newX, newY)]);
                    }
                }
            }
            return neighbours;
        }
        public bool TestOfVictory()
        {
            int revealedCount = 0;
            foreach (Cell c in cells)
            {
                if (c.state == Cell.Cellstate.revealed)
                {
                    revealedCount++;
                }
            }
            if (gridSizeX * gridSizeY - revealedCount == mineCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RevealCell(Cell c)
        {
            if (c.state == Cell.Cellstate.revealed || c.state == Cell.Cellstate.flagged)
                return;

            c.state = Cell.Cellstate.revealed;

            // Rekurzivně odhal sousedy, pokud není kolem žádná mina
            if (c.minesArround == 0)
            {
                List<Cell> neighbours = FindNeighbours(c.X, c.Y);
                foreach (Cell neighbour in neighbours)
                {
                    if (neighbour.state == Cell.Cellstate.hidden)
                    {
                        RevealCell(neighbour);
                    }
                }
            }
        }

        public Game()
        {
            
        }

    }
}
