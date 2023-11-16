using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using AreaRoomsAPI.Geometric;
using AreaRoomsAPI.Info;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using NUnit.Framework.Constraints;

namespace AreaRoomsAPI.Algorithm
{
    internal class AreaChromosome : ChromosomeBase
    {
        private readonly int cellsCount;
        public readonly int cellsCountWidth;
        public readonly int cellsCountHeight;
        private readonly double cellSize;
        public readonly double Width;
        public readonly double Height;
        private readonly double minCellSize;
        private readonly AreaInfo areaInfo;
        private List<Point>[] roomsPoints;

        public AreaChromosome(AreaInfo areaInfo, double minCellSize, int roomsCount) : base(roomsCount)
        {
            this.areaInfo = areaInfo;
            Width = areaInfo.Width;
            Height = areaInfo.Height;
            this.minCellSize = minCellSize;
            roomsPoints = new List<Point>[roomsCount];
            for (int i = 0; i < roomsCount; i++)
            {
                roomsPoints[i] = new List<Point>();
            }

            var nod = FindNOD(Width, Height);
            while (nod / 2 > minCellSize)
            {
                nod /= 2;
            }

            cellSize = nod;
            cellsCountWidth = (int)(Width / cellSize);
            cellsCountHeight = (int)(Height / cellSize);

            var coordinatesX = RandomizationProvider.Current.GetUniqueInts(roomsCount, 0, cellsCountWidth);
            var coordinatesY = RandomizationProvider.Current.GetUniqueInts(roomsCount, 0, cellsCountHeight);

            for (int i = 0; i < roomsCount; i++)
            {
                ReplaceGene(i, new Gene(new RoomGene(new Point(coordinatesX[i], coordinatesY[i]), cellSize)));
            }

            cellsCount = cellsCountWidth * cellsCountHeight;
        }

        private double FindNOD(double a, double b)
        {
            if (b <= 0.05)
            {
                return a;
            }

            if (a < b)
            {
                return FindNOD(b, a);
            }

            return FindNOD(b, a % b);
        }

        public override IChromosome CreateNew()
        {
            return new AreaChromosome(areaInfo, minCellSize, Length);
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(new RoomGene(
                    new Point
                    (
                        RandomizationProvider.Current.GetInt(0, cellsCountWidth),
                        RandomizationProvider.Current.GetInt(0, cellsCountHeight)
                    ), 
                    cellSize));
        }

        public override IChromosome Clone()
        {
            var clone = base.Clone() as AreaChromosome;

            return clone;
        }

        public void AddCell(int geneIndex, Point point)
        {
            roomsPoints[geneIndex].Add(point);
        }

        public void ClearCells()
        {
            for (int i = 0; i < roomsPoints.Length; i++)
            {
                roomsPoints[i].Clear();
            }
        }

        public double GetCellSize()
        {
            return cellSize;
        }

        public IList<PointD> ConvertPointListToPointDList(IList<Point> points, PointD basePoint)
        {
            return points.Select(p => new PointD(basePoint.X + p.X * cellSize + cellSize, basePoint.Y + p.Y * cellSize + cellSize)).ToList();
        }

        public IList<IList<Point>> GetRoomsBorders()
        {
            return roomsPoints.Select(x => GetPointsMinimalConvexHull(x)).ToList();
        }

        private static IList<Point> GetPointsMinimalConvexHull(IList<Point> pointsList)
        {
            var points = new List<Point>(pointsList);
            var basePoint = points.OrderBy(point => point.Y).ThenByDescending(point => point.X).First();
            points.Remove(basePoint);
            var pointsSortedByAtan2 = points.OrderBy(point => Math.Atan2(point.Y - basePoint.Y, point.X - basePoint.X)).ToList();

            var convexHull = new List<Point>();
            convexHull.Add(basePoint);
            convexHull.Add(pointsSortedByAtan2.First());

            for (var i = 1; i < pointsSortedByAtan2.Count; i++)
            {
                while (!isLeftRotate(convexHull[convexHull.Count-2], convexHull[convexHull.Count-1], pointsSortedByAtan2[i]))
                {
                    convexHull.RemoveAt(convexHull.Count-1);
                }
                convexHull.Add(pointsSortedByAtan2[i]);
            }
            return convexHull;
        }

        private static bool isLeftRotate (Point A, Point B, Point C)
        {
            return (B.X - A.X) * (C.Y - B.Y) - (B.Y - A.Y) * (C.X - B.X) >= 0;
        }
    }
}
