using Microsoft.Xna.Framework;

namespace match_3
{
    public class Bomb
    {
        public bool IsReplaced{get; set;}
        public Point point {get; set;}
        public Bomb()
        {
            IsReplaced = false;
            point = new Point();
        }
    }
}