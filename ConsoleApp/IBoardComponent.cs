using Conway.Engine;

namespace Conway.ConsoleApp
{
    public interface IBoardComponent
    {
        void DeployAt(int x, int y);
    }
}