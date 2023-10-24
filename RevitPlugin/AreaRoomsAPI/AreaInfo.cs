using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI
{
    public class AreaInfo
    {
        public readonly IList<Wall> Walls;
        public readonly double Square;
        public readonly IList<PointD> Points;


        public AreaInfo(IList<Wall> walls)
        {
            Walls = walls;

            var points = new PointD[walls.Count];
            Points = points;
            //TODO: возможно надо будет реализовать сортировку стенок(углов)
            for (int i = 0; i < walls.Count; i++)
            {
                points[i] = walls[i].startPoint;
            }

            Square = GetSquare(points);
        }


        private double GetSquare(IList<PointD> points)
        {
            double area = default;

            for (int i = 0; i < points.Count - 1; i++)
            {
                area += points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X;
            }
            
            area += points[points.Count - 1].X * points[0].Y - points[points.Count - 1].Y * points[0].X;

            return Math.Abs(area) / 2;
        }
    }
}
