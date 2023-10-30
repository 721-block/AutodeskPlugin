using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    public class Shape
    {
        public IList<PointD> Points { get; protected set; }

        public double Width => GetWidth();

        private double GetWidth()
        {
            var sum = 0d;

            sum += Math.Abs(Points[0].X - Points[Points.Count - 1].X);
            for (var i = 1; i < Points.Count; i++)
            {
                sum += Math.Abs(Points[i].X - Points[i - 1].X);
            }

            return sum / 2;
        }

        public double Height => GetHeight();

        private double GetHeight()
        {
            var sum = 0d;

            sum += Math.Abs(Points[0].Y - Points[Points.Count - 1].Y);
            for (var i = 1; i < Points.Count; i++)
            {
                sum += Math.Abs(Points[i].Y - Points[i - 1].Y);
            }

            return sum / 2;
        }

        public Shape(IList<PointD> points)
        {
            Points = points.ToList();
        }

        public Shape(IList<Wall> walls)
        {
            var points = new PointD[walls.Count];
            Points = points;
            //TODO: возможно надо будет реализовать сортировку стенок(углов)
            for (int i = 0; i < walls.Count; i++)
            {
                points[i] = walls[i].startPoint;
            }
        }

        public double GetSquare()
        {
            double area = default;

            for (int i = 0; i < Points.Count - 1; i++)
            {
                area += Points[i].X * Points[i + 1].Y - Points[i].Y * Points[i + 1].X;
            }

            area += Points[Points.Count - 1].X * Points[0].Y - Points[Points.Count - 1].Y * Points[0].X;

            return Math.Abs(area) / 2;
        }

        public (IList<Shape>, IList<PointD> connection) SplitInHalf(bool isSplitByY)
        {
            var shapes = new Shape[2];

            var leftSide = Points.
                OrderByDescending(x => isSplitByY ? x.Y : -x.X).
                ThenBy(point => isSplitByY ? point.X : point.Y).
                Take(2).
                ToArray();
            var rightSide = Points.
                OrderBy(point => isSplitByY ? point.Y : -point.X).
                ThenByDescending(points => isSplitByY ? points.X : points.Y).
                Take(2).
                ToArray();

            var center = (isSplitByY ? Height : Width) / 2;
            var connectionLine = isSplitByY ? 
                new List<PointD>() { new PointD(leftSide[1].X, leftSide[1].Y - center), new PointD(leftSide[0].X, leftSide[0].Y - center) } : 
                new List<PointD>() { new PointD(rightSide[1].X - center, rightSide[1].Y), new PointD(rightSide[0].X - center, rightSide[0].Y) };

            if (isSplitByY)
            {
                shapes[0] = GetYShape(leftSide, -center);
                shapes[1] = GetYShape(rightSide, center);
            }
            else
            {
                shapes[0] = GetXShape(rightSide, -center);
                shapes[1] = GetXShape(leftSide, center);
            }

            Shape GetYShape(PointD[] side, double bias)
            {
                return new Shape(new PointD[4]
                {
                    side[0],
                    side[1],
                    new PointD(side[1].X, side[1].Y + bias),
                    new PointD(side[0].X, side[0].Y + bias)
                });
            }

            Shape GetXShape(PointD[] side, double bias)
            {
                return new Shape(new PointD[4]
                {
                    side[0],
                    side[1],
                    new PointD(side[1].X + bias, side[1].Y),
                    new PointD(side[0].X + bias, side[0].Y)
                });
            }

            return (shapes, connectionLine);
        }
    }
}
