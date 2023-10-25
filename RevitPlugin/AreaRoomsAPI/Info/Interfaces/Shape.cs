﻿using System;
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
            for ( var i = 1; i < Points.Count; i++)
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
    }
}
