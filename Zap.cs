using Microsoft.Xna.Framework;

namespace match_3
{
    public class Zap
    {
        public Point point;
        public bool ver;
        public Zap(Point point, bool ver)
        {
            this.point = point;
            this.ver = ver;
        }
    }
}