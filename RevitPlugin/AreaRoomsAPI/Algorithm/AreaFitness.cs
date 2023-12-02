using AreaRoomsAPI.Geometric;
using AreaRoomsAPI.Info;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Algorithm
{
    public class AreaFitness : IFitness
    {
        private readonly AreaRoomsFormatsInfo formats;
        private readonly RoomType[] priority;
        private readonly Direction[] directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray();
        private readonly int cellWidthCount;
        private readonly int cellHeightCount;
        private readonly bool[,] hashSet;
        public AreaFitness(
            AreaRoomsFormatsInfo formats, 
            IList<RoomType> roomTypes, 
            Dictionary<RoomType, int> roomPriority, 
            int cellWidthCount, 
            int cellHeightCount) 
        {
            this.formats = formats;
            priority = roomTypes.OrderByDescending(x => roomPriority[x]).ToArray();
            hashSet = new bool[cellHeightCount, cellWidthCount];
            this.cellWidthCount = cellWidthCount;
            this.cellHeightCount = cellHeightCount;

        }

        public double Evaluate(IChromosome chromosome)
        {
            var fitness = 0d;
            var areaChromosome = (AreaChromosome)chromosome;
            var queue = new Queue<int>();
            var genes = chromosome.GetGenes();
            var roomGenes = new RoomGene[chromosome.Length];

            for (int i = 0; i < chromosome.Length; i++)
            {
                var roomGene = (RoomGene)genes[i].Value;
                roomGene.ClearCells();
                roomGenes[i] = roomGene;
                queue.Enqueue(i);
                hashSet[roomGene.Point.Y, roomGene.Point.X] = true;
            }
            areaChromosome.ClearCells();

            while (queue.Count > 0)
            {
                var geneIndex = queue.Dequeue();
                var gene = roomGenes[geneIndex];
                (var width, var height) = (gene.CellsWidth,  gene.CellsHeight);

                foreach (var side in directions.OrderBy(x => (int)x % 2 == 0 ? height : width).ThenBy(x => x))
                {
                    List<Point> result;
                    bool isLegit;
                    if ((int)side % 2 == 0)
                    {
                        isLegit = TryFindWidthCells(gene, side, out result);
                    }
                    else
                    {
                        isLegit = TryFindHeightCells(gene, side, out result);
                    }

                    if (isLegit)
                    {
                        areaChromosome.AddCells(geneIndex, result);
                        roomGenes[geneIndex].AddCells(result);
                        
                        queue.Enqueue(geneIndex);
                        break;
                    }
                }           
            }

            for (var i = 0; i < roomGenes.Length; i++)
            {
                areaChromosome.ReplaceGene(i, new Gene(roomGenes[i]));
            }

            var diff = roomGenes.Length - roomGenes.Distinct().Count();
            fitness -= diff * 1000;
            areaChromosome.Fitness = fitness;
            SetToDefault(hashSet);
            return fitness;
        }

        private void SetToDefault(bool[,] hashSet)
        {
            for (int i = 0; i < cellHeightCount; i++)
            {
                for (int j = 0; j < cellWidthCount; j++)
                {
                    hashSet[i, j] = false;
                }
            }
        }

        private void SetCells(List<Point> cells)
        {
            foreach (var cell in cells)
            {
                hashSet[cell.Y, cell.X] = true;
            }
        }

        private bool TryFindWidthCells(RoomGene roomGene, Direction direction, out List<Point> result)
        {
            result = new List<Point>(roomGene.CellsWidth);
            var minWidth = roomGene.minXCell;
            var maxWidth = roomGene.maxXCell + 1;
            var currentPos = direction == Direction.Top ? roomGene.maxYCell + 1 : roomGene.minYCell - 1;
            if (currentPos < 0 || currentPos >= cellHeightCount)
            {
                return false;
            }

            for (int i = minWidth; i < maxWidth; i++)
            {
                var nextPoint = new Point(i, currentPos);
                if (hashSet[nextPoint.Y, nextPoint.X])
                {
                    return false;
                }

                result.Add(nextPoint);
            }

            return true;
        }

        private bool TryFindHeightCells(RoomGene roomGene, Direction direction, out List<Point> result)
        {
            result = new List<Point>(roomGene.CellsHeight);
            var minHeight = roomGene.minYCell;
            var maxHeight = roomGene.maxYCell + 1;
            var currentPos = direction == Direction.Left ? roomGene.minXCell - 1 : roomGene.maxXCell + 1;
            if (currentPos < 0 || currentPos >= cellWidthCount)
            {
                return false;
            }

            for (int i = minHeight; i < maxHeight; i++)
            {
                var nextPoint = new Point(currentPos, i);
                if (hashSet[nextPoint.Y, nextPoint.X])
                {
                    return false;
                }

                result.Add(nextPoint);
            }

            return true;
        }
    }
}
