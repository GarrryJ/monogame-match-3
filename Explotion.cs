using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace match_3
{
    public class Explotion
    {
        public List<Point> boomList;
        public bool IsBoom {get; set;}
        private int ticCounter;

        public bool TicForScore()
        {
            return ticCounter == 4;
        }

        public Explotion()
        {
            boomList = new List<Point>(64);
            IsBoom = false;
            ticCounter = 1;
        }
        private void Tic()
        {
            if (ticCounter > 8)
            {
                ticCounter = 1;
                IsBoom = false;
                boomList.Clear();
            }
            else
                ticCounter++;
        }

        public Rectangle TextureRect()
        {
            Point point = new Point((ticCounter % 4) * 140, (ticCounter / 4 - 1) * 140);
            Tic();
            return new Rectangle(point, new Point(140, 140)); 
        }

    }
}