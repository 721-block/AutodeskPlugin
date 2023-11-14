using AreaRoomsAPI.Geometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Algorithm
{
    public class RoomGene
    {
        public Point Point;
        private int minXCell;
        private int minYCell;
        private int maxXCell;
        private int maxYCell;
        public double Area { get; private set; }

        private double cellSize;

        private List<Point> cells = new List<Point>();

        public RoomGene(Point point, double cellSize) 
        { 
            Point = point;
            this.cellSize = cellSize;
        }

        public void AddCell(Point point)
        {
            cells.Add(point);
            minXCell = Math.Min(minXCell, point.X);
            minYCell = Math.Min(minYCell, point.Y);
            maxXCell = Math.Max(maxXCell, point.X);
            maxYCell = Math.Max(maxYCell, point.Y);
            Area += cellSize * cellSize;
        }

        public double GetWidth()
        {
            return (maxXCell - minXCell + 1) * cellSize;
        }

        public double GetHeight()
        {
            return (maxYCell - minYCell + 1) * cellSize;
        }

        public void ClearCells()
        {
            cells.Clear();
            Area = 0;
            minXCell = int.MaxValue;
            minYCell = int.MaxValue;
            maxXCell = -1;
            maxYCell = -1;
        }
    }
}
