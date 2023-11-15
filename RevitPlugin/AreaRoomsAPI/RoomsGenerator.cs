using AreaRoomsAPI.Algorithm;
using AreaRoomsAPI.Info;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Terminations;
using System.Runtime.Remoting.Channels;

namespace AreaRoomsAPI
{
    public class RoomsGenerator
    {
        private readonly Dictionary<RoomType, int> roomsPriority = new Dictionary<RoomType, int>()
        {
            {RoomType.Kitchen, 3 },
            {RoomType.Default, 3 },
            {RoomType.Wardrobe, 1 },
            {RoomType.Toilet, 1 },
            {RoomType.Bathroom, 2 },
            {RoomType.Loggia, 2 },
            {RoomType.Corridor, 2 },
        };


        private readonly AreaRoomsFormatsInfo formatsInfo;

        private readonly double roomMargin;

        private readonly AreaInfo areaInfo;

        public RoomsGenerator(AreaInfo areaInfo, AreaRoomsFormatsInfo formatsInfo)
        {
            this.areaInfo = areaInfo;
            roomMargin = areaInfo.Margin / 2;
            this.formatsInfo = formatsInfo;
        }

        public GeneratedArea GenerateArea()
        {
            var list = new List<(RoomType, IList<PointD>)>();
            var fitness = new AreaFitness(formatsInfo, areaInfo.RoomTypes, roomsPriority);
            var chromosome = new AreaChromosome(areaInfo, formatsInfo[RoomType.Corridor].RecommendedWidth / 2, areaInfo.RoomTypes.Count);
            var population = new Population(50, 100, chromosome);
            var selection = new TournamentSelection(3);
            var crossover = new OrderedCrossover();
            var mutation = new ReverseSequenceMutation();
            var termination = new GenerationNumberTermination(200);
            
            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = termination;

            ga.GenerationRan += (sender, e) =>
            {
                if (ga.GenerationsNumber == 99)
                {

                }
            };

            ga.Start();

            

            var bestChromosome = (AreaChromosome)ga.BestChromosome;
            var genes = bestChromosome.GetGenes().Select(x => (RoomGene)x.Value).ToArray();
            


            return new GeneratedArea(list);
        }
    }
}
