using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2021
{

    class Day05 : ASolution
    {

        public Day05() : base(05, 2021, "")
        {
            
        }

        protected override string SolvePartOne()
        {
            Setup();
            // only horizontal or verticals
            Lines = Lines.Where(l => (l.a.x == l.b.x) || (l.a.y == l.b.y)).ToList();

            foreach (var line in Lines)
            {
                Diagram = ApplyLine(Diagram, line);
            }

            var counter = GetOverlapsOver(1);

            return counter.ToString();
        }

        protected override string SolvePartTwo()
        {
            Setup();
            
            foreach (var line in Lines)
            {
                Diagram = ApplyLine(Diagram, line);
            }

            var counter = GetOverlapsOver(1);

            return counter.ToString();
        }

        private void Setup()
        {
            var diagramSize = 0;
            Lines.Clear();
            foreach (var line in this.Input.SplitByNewline())
            {
                var coordinates = line.Replace(" -> ", ",").Split(',').Select(n => int.Parse(n)).ToArray();
                var l = new Line(
                    new Coordinate(coordinates[0], coordinates[1]), 
                    new Coordinate(coordinates[2], coordinates[3]));
                Lines.Add(l);
                // track largest coordinate value
                diagramSize = coordinates.Append(diagramSize).Max();
            }
            Diagram = new int[diagramSize+1, diagramSize+1];
        }

        private int GetOverlapsOver(int overlapCount)
        {
            var counter = 0;
            for (var i = 0; i < Diagram.GetLength(0); i++)
            {
                for (var j = 0; j < Diagram.GetLength(1); j++)
                {
                    if (Diagram[i,j]>overlapCount) counter++;
                }
            }
            return counter;
        }

        private int[,] ApplyLine(int[,] diagram, Line line)
        {
            Line temp = line with { };
            while (temp.a != temp.b)
            {
                diagram[temp.a.x, temp.a.y]++;   
                temp = temp.Next();
            }
            diagram[temp.a.x, temp.a.y]++;  
            return diagram;
        }

        private int[,] Diagram;
        private List<Line> Lines = new List<Line>();
        
    }

    record Coordinate(int x, int y);

    record Line(Coordinate a, Coordinate b)
    {
        public Line Next()
        {
            int x1 = a.x, y1 = a.y;

            // a calc
            if (a.x > b.x)
            {
                x1--;
            }
            else if (a.x < b.x)
            {
                x1++;
            }

            if (a.y > b.y)
            {
                y1--;
            }
            else if (a.y < b.y)
            {
                y1++;
            }
            return new Line(new Coordinate(x1, y1), b);
        }
    }
}
