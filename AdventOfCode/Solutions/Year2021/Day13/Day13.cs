using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2021
{

    class Day13 : ASolution
    {
        private List<(int x, int y)> Coordinates = new List<(int x, int y)>();
        private List<(char axis, int position)> Folds = new List<(char axis, int position)>();
        public Day13() : base(13, 2021, "", false)
        {
            ParseInput(this.Input);
        }

        protected override string SolvePartOne()
        {
            var foldState = Fold(this.Coordinates, Folds.First());
            return $"{foldState.Count()}";
        }

        protected override string SolvePartTwo()
        {
            var foldState = new List<(int x, int y)>(Coordinates);
            foreach (var fold in Folds)
                foldState = new List<(int x, int y)>(Fold(foldState, fold));
            PrintDots(foldState);
            return $"{foldState.Count()}";
        }

        private List<(int x, int y)> Fold(List<(int x, int y)> dots, (char axis, int location) foldInstruction)
        {
            var foldedDots = new List<(int y, int x)>();
            foreach (var dot in dots)
            {
                switch (foldInstruction.axis) {
                    case 'x':
                        if (dot.x > foldInstruction.location) {
                            foldedDots.Add((foldInstruction.location - (dot.x - foldInstruction.location), dot.y));
                        } else {
                            foldedDots.Add(dot);
                        }
                        break;
                    case 'y':
                        if (dot.y > foldInstruction.location) {
                            foldedDots.Add((dot.x, foldInstruction.location - (dot.y - foldInstruction.location)));
                        } else {
                            foldedDots.Add(dot);
                        }
                        break;
                    default:
                        break;
                }
            }
            return foldedDots.Distinct().ToList();
        }

        private void ParseInput(string input)
        {
            var inputArray = input.SplitByNewline();
            var i = 0;
            var inputLine = inputArray[i];
            while(!string.IsNullOrWhiteSpace(inputArray[i]))
            {
                var s = inputArray[i].Split(',');
                if (s.Length != 2) break;
                Coordinates.Add(
                    (int.Parse(inputArray[i].Split(',')[0]), 
                    int.Parse(inputArray[i].Split(',')[1]))
                );
                i++;
            }
            while(i < inputArray.Length && !string.IsNullOrWhiteSpace(inputArray[i]))
            {
                var instruction = inputArray[i].Split(' ').Last().Split('=');
                Folds.Add(
                    (instruction[0][0], int.Parse(instruction[1]))
                );
                i++;
            }
        }

        private void PrintDots(List<(int x, int y)> dots)
        {
            var maxX = dots.Max(d => d.x);
            var maxY = dots.Max(d => d.y);
            var dotMatrix = new int[maxY, maxX];
            for (var i = 0; i <= dotMatrix.GetLength(0); i++)
            {
                for (var j = 0; j <= dotMatrix.GetLength(1); j++)
                {
                    if (dots.Contains((j, i))) {
                        Console.Write("#");
                    } else {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

    }
}