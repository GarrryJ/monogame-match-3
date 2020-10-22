using Microsoft.Xna.Framework;

namespace match_3
{
    public class Piece
    {
        public Type type {get; set;}
        public Point point;
        public bool ver {get; set;}
        public bool hor {get; set;}
        
        public Piece(Type type, Point point, bool ver, bool hor)
        {
            this.point = point;
            this.hor = hor;
            this.ver = ver;
            this.type = type;
        }
    }
}