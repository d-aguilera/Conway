using Conway.Engine;

namespace Conway.ConsoleApp
{
    public abstract class BoardComponent : IBoardComponent
    {
        public BoardComponent(Board board)
        {
            Board = board;
        }

        public Board Board { get; }

        public abstract void DeployAt(int x, int y);
    }
}