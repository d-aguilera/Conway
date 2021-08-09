using Conway.Engine;

namespace Conway.ConsoleApp
{
    public class GosperGliderGun : BoardComponent
    {
        public GosperGliderGun(Board board) : base(board)
        {
        }

        public override void DeployAt(int x, int y)
        {
            Board.AddAt(x, y + 4);
            Board.AddAt(x + 1, y + 4);
            Board.AddAt(x, y + 5);
            Board.AddAt(x + 1, y + 5);

            Board.AddAt(x + 4, y + 4);
            Board.AddAt(x + 5, y + 4);
            Board.AddAt(x + 4, y + 5);
            Board.AddAt(x + 5, y + 5);
            Board.AddAt(x + 4, y + 6);
            Board.AddAt(x + 5, y + 6);

            Board.AddAt(x + 9, y + 2);
            Board.AddAt(x + 9, y + 3);
            Board.AddAt(x + 10, y + 3);
            Board.AddAt(x + 10, y + 4);
            Board.AddAt(x + 11, y + 4);
            Board.AddAt(x + 10, y + 5);
            Board.AddAt(x + 11, y + 5);
            Board.AddAt(x + 12, y + 5);
            Board.AddAt(x + 10, y + 6);
            Board.AddAt(x + 11, y + 6);
            Board.AddAt(x + 9, y + 7);
            Board.AddAt(x + 10, y + 7);
            Board.AddAt(x + 9, y + 8);

            Board.AddAt(x + 26, y);
            Board.AddAt(x + 26, y + 1);
            Board.AddAt(x + 28, y + 1);
            Board.AddAt(x + 27, y + 2);
            Board.AddAt(x + 29, y + 2);
            Board.AddAt(x + 27, y + 3);
            Board.AddAt(x + 30, y + 3);
            Board.AddAt(x + 27, y + 4);
            Board.AddAt(x + 29, y + 4);
            Board.AddAt(x + 26, y + 5);
            Board.AddAt(x + 28, y + 5);
            Board.AddAt(x + 26, y + 6);

            Board.AddAt(x + 34, y + 2);
            Board.AddAt(x + 35, y + 2);
            Board.AddAt(x + 34, y + 3);
            Board.AddAt(x + 35, y + 3);
        }
    }
}
