using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace match_3
{
    public class Lightning
    {
        public List<Zap> zapList;
        public bool IsBoom {get; set;}
        private int ticCounter;

        public bool TicForScore()
        {
            return ticCounter == 4;
        }

        public Lightning()
        {
            zapList = new List<Zap>(64);
            IsBoom = false;
            ticCounter = 1;
        }
        private void Tic()
        {
            if (ticCounter > 8)
            {
                ticCounter = 1;
                IsBoom = false;
            }
            else
                ticCounter++;
        }

        //1024 x 512
        public Rectangle TextureRect(bool ver)
        {
            Tic();
            if (ver)
            {
                Point point = new Point(ticCounter * 140, 0);
                return new Rectangle(point, new Point(128, 512));
            }
            else
            {
                Point point = new Point(0, ticCounter * 140);
                return new Rectangle(point, new Point(512, 128));
            }
        }

    }
}