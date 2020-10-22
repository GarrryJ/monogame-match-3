using System;

namespace match_3
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Match3Game())
                game.Run();
        }
    }
}
