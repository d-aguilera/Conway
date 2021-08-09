using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Conway.Engine
{
    public class Board
    {
        public delegate void CellEventHandler(object sender, CellEventArgs e);

        public event CellEventHandler CellsDied;
        public event CellEventHandler CellsBorn;

        private readonly SortedDictionary<string, Cell> cells;

        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();

        public Board()
        {
            cells = new SortedDictionary<string, Cell>();
        }

        public IList<Cell> Cells
        {
            get
            {
                sync.EnterReadLock();
                var list = cells.Select(entry => entry.Value).ToList();
                sync.ExitReadLock();
                return list;
            }
        }

        private string GetKey(Cell cell) => GetKey(cell.X, cell.Y);

        private string GetKey(int x, int y) => $"{x},{y}";

        public Cell GetAt(int x, int y)
        {
            sync.EnterReadLock();
            var cell = GetAtInternal(x, y);
            sync.ExitReadLock();
            return cell;
        }

        private Cell GetAtInternal(int x, int y)
        {
            var key = GetKey(x, y);
            return cells.ContainsKey(key) ? cells[key] : null;
        }

        public Cell AddAt(int x, int y)
        {
            var cell = new Cell(x, y);
            Add(cell);
            return cell;
        }

        public void AddAt(int x, int y, IEnumerable<string> component)
        {
            var row = y;
            foreach (var line in component)
            {
                var col = x;
                foreach (char c in line)
                {
                    if (c != ' ')
                    {
                        Add(new Cell(col, row));
                    }
                    col++;
                }
                row++;
            }
        }

        public void Add(Cell cell)
        {
            sync.EnterWriteLock();
            AddInternal(cell);
            sync.ExitWriteLock();
            CellsBorn?.Invoke(this, new CellEventArgs(cell));
        }

        private void AddInternal(Cell cell)
        {
            cells.Add(GetKey(cell), cell);
        }

        public void Remove(Cell cell)
        {
            sync.EnterWriteLock();
            RemoveInternal(cell);
            sync.ExitWriteLock();
            CellsDied?.Invoke(this, new CellEventArgs(cell));
        }

        private void RemoveInternal(Cell cell)
        {
            cells.Remove(GetKey(cell));
        }

        public int Run()
        {
            var factory = new TaskFactory();
            var tasks = new Task[2];

            Cell[] deaths = null;
            tasks[0] = factory.StartNew(() =>
            {
                sync.EnterReadLock();
                deaths = ProcessDeaths().ToArray();
                sync.ExitReadLock();
            });

            Cell[] newBorns = null;
            tasks[1] = factory.StartNew(() =>
            {
                sync.EnterReadLock();
                newBorns = ProcessNewBorns().ToArray();
                sync.ExitReadLock();
            });

            var task2 = factory.ContinueWhenAll(tasks, _ =>
            {
                sync.EnterWriteLock();
                foreach (var cell in deaths)
                {
                    RemoveInternal(cell);
                }
                foreach (var cell in newBorns)
                {
                    AddInternal(cell);
                }
                sync.ExitWriteLock();
            });
            
            var task3 = factory.ContinueWhenAll(tasks, _ =>
            {
                if (deaths.Length > 0)
                {
                    CellsDied?.Invoke(this, new CellEventArgs(deaths));
                }
                if (newBorns.Length > 0)
                {
                    CellsBorn?.Invoke(this, new CellEventArgs(newBorns));
                }
            });

            Task.WaitAll(task2, task3);

            return deaths.Length + newBorns.Length;
        }

        private IEnumerable<Cell> ProcessDeaths()
        {
            foreach (var entry in cells)
            {
                var cell = entry.Value;
                var neighbors = GetNeighbors(cell.X, cell.Y).Take(4).Count();
                if (neighbors < 2 || neighbors > 3)
                {
                    yield return cell;
                }
            }
        }

        private IEnumerable<Cell> ProcessNewBorns()
        {
            var empty = new SortedDictionary<string, Cell>();

            foreach (var entry in cells)
            {
                var cell = entry.Value;
                for (var dx = -1; dx < 2; dx++)
                {
                    var x = cell.X + dx;
                    for (var dy = -1; dy < 2; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        var y = cell.Y + dy;
                        var neighbor = GetAtInternal(x, y);
                        if (neighbor != null) continue;
                        var key = GetKey(x, y);
                        if (!empty.ContainsKey(key))
                        {
                            empty.Add(key, new Cell(x, y));
                        }
                    }
                }
            }

            foreach (var entry in empty)
            {
                var cell = entry.Value;
                var neighbors = GetNeighbors(cell.X, cell.Y).Take(4).Count();
                if (neighbors == 3)
                {
                    yield return cell;
                }
            }
        }

        private IEnumerable<Cell> GetNeighbors(int x, int y)
        {
            for (var dx = -1; dx < 2; dx++)
            {
                for (var dy = -1; dy < 2; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    var neighbor = GetAtInternal(x + dx, y + dy);
                    if (neighbor != null)
                        yield return neighbor;
                }
            }
        }
    }
}
