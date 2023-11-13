using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AreaRoomsAPI.Geometric;
using AreaRoomsAPI.Info;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;

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

        public AreaChromosome(AreaInfo areaInfo, double minCellSize, int roomsCount) : base(roomsCount)
        {
            this.areaInfo = areaInfo;
            Width = areaInfo.Width;
            Height = areaInfo.Height;
            this.minCellSize = minCellSize;
            var nod = FindNOD(Width, Height);

            while (nod / 2 > minCellSize)
            {
                nod /= 2;
            }

            cellSize = nod;
            cellsCountWidth = (int)(Width / cellSize);
            cellsCountHeight = (int)(Height / cellSize);

            var coordinatesX = RandomizationProvider.Current.GetInts(roomsCount, 0, cellsCountWidth);
            var coordinatesY = RandomizationProvider.Current.GetInts(roomsCount, 0, cellsCountHeight);

            for (int i = 0; i < roomsCount; i++)
            {
                ReplaceGene(i, new Gene(new RoomGene(new Point(coordinatesX[0], coordinatesY[0]), cellSize)));
            }

            cellsCount = cellsCountWidth * cellsCountHeight;
        }

        private double FindNOD(double a, double b)
        {
            if (b == 0)
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
    }
}
