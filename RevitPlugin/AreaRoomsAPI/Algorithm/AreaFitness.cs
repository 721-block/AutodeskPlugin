using AreaRoomsAPI.Geometric;
using AreaRoomsAPI.Info;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Algorithm
{
    public class AreaFitness : IFitness
    {
        private readonly AreaRoomsFormatsInfo formats;
        private readonly RoomType[] priority;
        public AreaFitness(AreaRoomsFormatsInfo formats, IList<RoomType> roomTypes, Dictionary<RoomType, int> roomPriority) 
        {
            this.formats = formats;
            priority = roomTypes.OrderByDescending(x => roomPriority[x]).ToArray();
        }

        public double Evaluate(IChromosome chromosome)
        {
            var fitness = 0d;
            var areaChromosome = (AreaChromosome)chromosome;
            var cellsCountWidth = areaChromosome.cellsCountWidth;
            var cellsCountHeight = areaChromosome.cellsCountHeight;
            var queue = new Queue<(int index, Point point)>();
            var hashSet = new HashSet<Point>();
            var genes = chromosome.GetGenes();
            var roomGenes = new RoomGene[chromosome.Length];

            for (int i = 0; i < chromosome.Length; i++)
            {
                var roomGene = (RoomGene)genes[i].Value;
                roomGene.ClearCells();
                roomGenes[i] = roomGene;
                queue.Enqueue((i, roomGene.Point));
            }

            while (queue.Count > 0)
            {
                var pointInfo = queue.Dequeue();

                foreach (var nextPoint in GetDirections(pointInfo.point)
                    .Where(nextPoint => !hashSet.Contains(nextPoint) && 
                    nextPoint.X >= 0 && nextPoint.X < cellsCountWidth && 
                    nextPoint.Y >= 0 && nextPoint.Y < cellsCountHeight))
                {
                    roomGenes[pointInfo.index].AddCell(nextPoint);
                    queue.Enqueue((pointInfo.index, nextPoint));
                    hashSet.Add(nextPoint);
                }
            }

            foreach (var roomGene in roomGenes)
            {
                var rectangleArea = roomGene.GetWidth() * roomGene.GetHeight();
                fitness -= Math.Abs(rectangleArea - roomGene.Area);
            }

            areaChromosome.Fitness = fitness;
            return fitness;
        }

        public IEnumerable<Point> GetDirections(Point point)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 ^ y == 0)
                    {
                        yield return new Point(point.X + x, point.Y + y);
                    }
                }
            }
        }
    }
}
