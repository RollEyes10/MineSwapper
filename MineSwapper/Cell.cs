using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSwapper
{
    public class Cell
    {
        public int X; // pozice X v poli
        public int Y; // pozice Y v poli
        public bool mined = false; // default
        
        public int minesArround; // pocet min okolo cell 

        public enum Cellstate
        {
            hidden,    //0
            revealed, //1
            flagged  //2
        }

        //default
        public Cellstate state;

        public Cell(int X, int Y)  
        {
            this.X = X;
            this.Y = Y;
           
        
        }
    }
}
