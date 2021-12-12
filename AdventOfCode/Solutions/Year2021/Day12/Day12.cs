using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2021
{

    class Day12 : ASolution
    {
        private Dictionary<string, List<string>> Caves = new Dictionary<string, List<string>>();
        private List<string> ValidRoutes = new List<string>();
        public Day12() : base(12, 2021, "")
        {

        }

        protected override string SolvePartOne()
        {
            Caves = ParseInput(this.Input);
            var visitSmallCaveCount = 1;
            var startingPaths = Caves.GetValueOrDefault("start");
            foreach (var path in startingPaths)
            {
                var route = new List<string>() { "start" };
                RecursivelyFollowCave(path, route, visitSmallCaveCount);
            }
            return ValidRoutes.Count.ToString();
        }

        protected override string SolvePartTwo()
        {
            ValidRoutes.Clear();
            Caves = ParseInput(this.Input);
            var visitSmallCaveCount = 2;
            var startingPaths = Caves.GetValueOrDefault("start");
            foreach (var path in startingPaths)
            {
                var route = new List<string>() { "start" };
                RecursivelyFollowCave(path, route, visitSmallCaveCount);
            }
            return ValidRoutes.Count.ToString();
        }

        private Dictionary<string, List<string>> ParseInput(string input)
        {
            var caves = new Dictionary<string, List<string>>();
            foreach (var parts in input.SplitByNewline().Select(p => p.Split('-')))
            {
                if (caves.ContainsKey(parts[0]))
                    caves[parts[0]].Add(parts[1]);
                else
                    caves.TryAdd(parts[0], new List<string> { parts[1] });

                if (caves.ContainsKey(parts[1]))
                    caves[parts[1]].Add(parts[0]);
                else
                    caves.TryAdd(parts[1], new List<string> { parts[0] });
            }
            return caves;
        }

        private bool IsBigCave(string caveName) => 
            char.IsUpper(caveName[0]);
        
        private void RecursivelyFollowCave(string caveName, List<string> currentRoute, int maxSmallCaveVisitCount = 1)
        {
            currentRoute.Add(caveName);
            foreach (var cave in Caves[caveName].Where(n => n != "start" && RouteCanInclude(n, currentRoute, maxSmallCaveVisitCount)))
            {
                if (cave == "end")
                    ValidRoutes.Add(string.Join(",", new List<string>(currentRoute) { cave }));
                else
                    RecursivelyFollowCave(cave, new List<string>(currentRoute), maxSmallCaveVisitCount);
            }
            return;
        }

        private bool RouteCanInclude(string caveName, List<string> currentRoute, int maxSmallCaveVisitCount = 1)
        {
            if (caveName == "start") return false;
            if (!currentRoute.Contains(caveName)) return true;

            var smallCavesCount = 0;
            var smallCavesVisited =
                currentRoute.Where(r => r != "start" && r != "end" && !IsBigCave(r)).ToList();
            if (smallCavesVisited.Any())
            {
                smallCavesCount = smallCavesVisited
                    .GroupBy(r => r)
                    .Max(g => g.Count());
            }
            return IsBigCave(caveName) || smallCavesCount < maxSmallCaveVisitCount;
        }
    }
}
