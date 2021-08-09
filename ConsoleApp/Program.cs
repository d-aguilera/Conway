using System;
using System.Threading;
using Conway.Engine;

namespace Conway.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowSize(Console.BufferWidth, Console.BufferHeight);

            var board = new Board();
            board.CellsBorn += Board_CellsBorn;
            board.CellsDied += Board_CellsDied;

            new SimkinGliderGun(board).DeployAt(20, 20);

            for (int i = 0; i < 1000; i++)
            {
                Wait();
                var changes = board.Run();
                if (changes == 0) break;
            }

            Console.SetCursorPosition(0, 0);
        }

        private static void Wait()
        {
            // Console.ReadKey();
            Thread.Sleep(10);
        }

        private static void Board_CellsDied(object sender, CellEventArgs e)
        {
            foreach (var cell in e.Cells)
            {
                if (FallsOutside(cell.X, cell.Y)) continue;
                Console.SetCursorPosition(cell.X * 2, cell.Y);
                Console.Write("  ");
            }
        }

        private static void Board_CellsBorn(object sender, CellEventArgs e)
        {
            foreach (var cell in e.Cells)
            {
                if (FallsOutside(cell.X, cell.Y)) continue;
                Console.SetCursorPosition(cell.X * 2, cell.Y);
                Console.Write("██");
            }
        }

        private static bool FallsOutside(int x, int y)
        {
            if (x * 2 < 0 || x * 2 + 2 > Console.WindowWidth) return true;
            if (y < 0 || y + 1 > Console.WindowHeight) return true;
            return false;
        }
    }
}
