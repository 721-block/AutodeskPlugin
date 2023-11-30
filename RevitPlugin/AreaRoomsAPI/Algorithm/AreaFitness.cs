using AreaRoomsAPI.Geometric;
using AreaRoomsAPI.Info;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Algorithm
{
    public class AreaFitness : IFitness
    {
        private readonly AreaRoomsFormatsInfo formats;
        private readonly RoomType[] priority;
        private readonly Direction[] directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray();
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
            var queue = new Queue<int>();
            var hashSet = new HashSet<Point>();
            var genes = chromosome.GetGenes();
            var roomGenes = new RoomGene[chromosome.Length];

            for (int i = 0; i < chromosome.Length; i++)
            {
                var roomGene = (RoomGene)genes[i].Value;
                roomGene.ClearCells();
                roomGenes[i] = roomGene;
                queue.Enqueue(i);
                hashSet.Add(roomGene.Point);
            }
            areaChromosome.ClearCells();

            while (queue.Count > 0)
            {
                var geneIndex = queue.Dequeue();
                var gene = roomGenes[geneIndex];
                (var width, var height) = (gene.CellsWidth,  gene.CellsHeight);

                foreach (var nextPoint in directions.OrderBy(x => (int)x % 2 == 1 ? height : width).ThenBy(x => x))
                {
                    queue.Enqueue(geneIndex);
                }
            }

            for (var i = 0; i < roomGenes.Length; i++)
            {
                var rectangleArea = roomGenes[i].GetWidth() * roomGenes[i].GetHeight();
                areaChromosome.ReplaceGene(i, new Gene(roomGenes[i]));
                fitness -= Math.Abs(rectangleArea - roomGenes[i].Area);
            }


            var diff = roomGenes.Length - roomGenes.Distinct().Count();
            fitness -= diff * 1000;
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
