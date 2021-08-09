using System.Collections.Generic;

namespace Conway.Engine
{
    public class CellEventArgs
    {
        public CellEventArgs(params Cell[] cells)
        {
            Cells = cells;
        }

        public IEnumerable<Cell> Cells { get; }
    }
}