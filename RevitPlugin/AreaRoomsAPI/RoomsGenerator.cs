﻿using AreaRoomsAPI.Algorithm;
using AreaRoomsAPI.Info;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Infrastructure.Framework.Threading;

namespace AreaRoomsAPI
{
    public class RoomsGenerator
    {
        private readonly Dictionary<RoomType, int> roomsPriority = new Dictionary<RoomType, int>()
        {
            { RoomType.Kitchen, 3 },
            { RoomType.Default, 3 },
            { RoomType.Wardrobe, 1 },
            { RoomType.Toilet, 1 },
            { RoomType.Bathroom, 2 },
            { RoomType.Loggia, 2 },
            { RoomType.Corridor, 2 },
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

        public List<GeneratedArea> GenerateAreas(int areasCount)
        {
            var result = new List<GeneratedArea>();

            var chromosome = new AreaChromosome(areaInfo, formatsInfo[RoomType.Corridor].RecWidth,
                areaInfo.RoomTypes.Count);
            var fitness = new AreaFitness(formatsInfo, areaInfo.RoomTypes, roomsPriority, chromosome.cellsCountWidth,
                chromosome.cellsCountHeight);
            var population = new TplPopulation(80, 120, chromosome);
            var selection = new AreaTournamentSelection(15);
            var crossover = new OrderedCrossover();
            var mutation = new ReverseSequenceMutation();
            var termination = new GenerationNumberTermination(300);
            var taskExecutor = new TplTaskExecutor();
            var reinsertion = new AreaReinsertion();

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.TaskExecutor = taskExecutor;
            ga.Termination = termination;
            ga.Reinsertion = reinsertion;

            ga.GenerationRan += (sender, e) =>
            {
                if (ga.GenerationsNumber == 175)
                {
                }
            };

            ga.Start();

            for (int i = 0; i < areasCount; i++)
            {
                var chromosomes = ga.Population.CurrentGeneration.Chromosomes;
                var currentChromosome = (AreaChromosome)chromosomes[i];
                var genes = currentChromosome.GetGenes().Select(x => (RoomGene)x.Value).ToArray();

                var basePoint = areaInfo.Points.OrderBy(p => p.X).ThenBy(p => p.Y).First();

                var ans = currentChromosome.GetRoomsBorders()
                    .Select(x => currentChromosome.ConvertPointListToPointDList(x, basePoint)).ToList();

                var list = ans.Select(x => (RoomType.Default, x)).ToList();
                result.Add(new GeneratedArea(list));
            }

            return result;
        }
    }
}