using AreaRoomsAPI.Geometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Algorithm
{
    public struct RoomGene
    {
        public Point Point;
        private int minXCell;
        private int minYCell;
        private int maxXCell;
        private int maxYCell;
        public double Area { get; private set; }

        private double cellSize;

        public RoomGene(Point point, double cellSize) 
        { 
            Point = point;
            minXCell = point.X;
            minYCell = point.Y;
            maxXCell = point.X;
            maxYCell = point.Y;
            Area = cellSize * cellSize;
            this.cellSize = cellSize;
        }

        public void AddCell(Point point)
        {
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
            Area = cellSize * cellSize;
            minXCell = Point.X;
            minYCell = Point.Y;
            maxXCell = Point.X;
            maxYCell = Point.Y;
        }

        public static bool operator==(RoomGene left, RoomGene right)
        {
            return left.Point == right.Point;
        }

        public static bool operator !=(RoomGene left, RoomGene right)
        {
            return left.Point != right.Point;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoomGene roomGene))
            {
                return false;
            }

            return roomGene == this;
        }
    }
}
