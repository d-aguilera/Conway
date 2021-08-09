using Conway.Engine;

namespace Conway.ConsoleApp
{
    public class SimkinGliderGun : BoardComponent
    {
        public SimkinGliderGun(Board board) : base(board)
        {
        }

        public override void DeployAt(int x, int y)
        {
            Board.AddAt(x - 2, y, new[]
            {
                "  BB     BB                          ",
                "  BB     BB                          ",
                "                                     ",
                "      BB                             ",
                "      BB                             ",
                "                                     ",
                "                                     ",
                "                                     ",
                "                                     ",
                "                        BB BB        ",
                "                       B     B       ",
                "                       B      B  BB  ",
                "                       BBB   B   BB  ",
                "                            B        ",
            //  "                                     ",
            //  "                                     ",
            //  "                                     ",
            //  "                      BB             ",
            //  "                      B              ",
            //  "                       BBB           ",
            //  "                         B           ",
            });
        }
    }
}
