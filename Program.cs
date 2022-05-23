﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Music_task
{
    class Program
    {
        const string UP = "UP";
        const string DOWN = "DOWN";
        const string LEFT = "LEFT";
        const string RIGHT = "RIGHT";
        const int xLength = 480, yLength = 127;
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            const string resultsFile = "results.txt";

            Random random = new Random();

            int[][] Map = new int[yLength][];
            for (int i = 0; i < yLength; i++)
            {
                Map[i] = new int[xLength];
            }

            Point A = new Point(0, 69);
            Point C = new Point(479, random.Next(0, 80));
            Point B = new Point(121, random.Next(C.Y + 20, Map.Length - 1));

            Stack<Point> stack1 = DrawIterativeToC(Map, random, A, C);
            Stack<Point> stack2 = DrawIterativeToB(Map, random, C, B);

            string[] AllLines = new string[yLength];
            for (int i = 0; i < yLength; i++)
            {
                string line = "";//String.Format("Y = {0}| ", i);
                for (int j = 0; j < xLength; j++)
                {
                    line += (Map[i][j] + " ");
                }
                AllLines[i] = line;
            }
            File.WriteAllLines(resultsFile, AllLines);
            Console.WriteLine(String.Format("Point A: ({0};{1})  Point B: ({2};{3})  Point C: ({4};{5})", A.X, A.Y, B.X, B.Y, C.X, C.Y));

            OutputStacks(stack1, stack2);
        }



        static Stack<Point> DrawIterativeToC(int[][] Map, Random random, Point A, Point C)
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(A);
            Point currentPoint = stack.Peek();
            Map[currentPoint.Y][currentPoint.X] = 2;
            Point lastPoint = currentPoint;
            int counter = 0;
            ; while (!currentPoint.Equals(C))
            {
                string decision;
                decision = DecisionMaker(random, "FIRST", currentPoint.Y);
                currentPoint = VectorManagment(Map, random, currentPoint, decision);
                Console.WriteLine("->C " + currentPoint + "    " + lastPoint);
                if (lastPoint.Equals(currentPoint))
                {
                    counter++;
                }
                if (lastPoint != null && counter > 10)
                {
                    Point holder = lastPoint;
                    while (stack.Count != 0 && (lastPoint.Equals(currentPoint) || Math.Abs(lastPoint.X - currentPoint.X) <= 16 || Math.Abs(lastPoint.Y - currentPoint.Y) <= 16))
                    {
                        currentPoint = stack.Pop();
                        CleanMap(Map, currentPoint, holder);
                        holder = currentPoint;
                    }
                    CleanMap(Map, currentPoint, holder);
                }
                if (!lastPoint.Equals(currentPoint))
                {
                    stack.Push(currentPoint);
                    counter = 0;
                }
                lastPoint = currentPoint;
            }
            return stack;
        }

        static Stack<Point> DrawIterativeToB(int[][] Map, Random random, Point C, Point B)
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(C);
            Point currentPoint = stack.Peek();
            Map[currentPoint.Y][currentPoint.X] = 2;
            Point lastPoint = currentPoint;
            int counter = 0;
            ; while (!currentPoint.Equals(B))
            {
                string decision;
                decision = DecisionMaker(random, "SECOND", currentPoint.Y);
                currentPoint = VectorManagment(Map, random, currentPoint, decision);
                Console.WriteLine("->B " + currentPoint + "    " + lastPoint);
                if (lastPoint.Equals(currentPoint))
                {
                    counter++;
                }
                if (lastPoint != null && counter > 5)
                {
                    Point holder = lastPoint;
                    while (stack.Count != 0 && (lastPoint.Equals(currentPoint) || Math.Abs(lastPoint.X - currentPoint.X) <= 16 || Math.Abs(lastPoint.Y - currentPoint.Y) <= 16))
                    {
                        currentPoint = stack.Pop();
                        CleanMap(Map, currentPoint, holder);
                        holder = currentPoint;
                    }
                    CleanMap(Map, currentPoint, holder);
                }
                if (!lastPoint.Equals(currentPoint) && !currentPoint.Equals(B))
                {
                    stack.Push(currentPoint);
                    counter = 0;
                }
                lastPoint = currentPoint;
            }
            return stack;
        }
        static void CleanMap(int[][] Map, Point A, Point B)
        {
            if (Math.Abs(A.X - B.X) != 0)
            {
                if (A.X > B.X)
                {
                    for (int i = B.X; i <= A.X; i++)
                    {
                        Map[A.Y][i] = 0;
                    }
                }
                else
                {
                    int val = Map[A.Y][A.X];
                    for (int i = A.X; i <= B.X; i++)
                    {
                        Map[A.Y][i] = 0;
                    }
                }
            }

            else if (Math.Abs(A.Y - B.Y) != 0)
            {
                if (A.Y > B.Y)
                {
                    for (int i = B.Y; i <= A.Y; i++)
                    {
                        Map[i][A.X] = 0;
                    }
                }
                else
                {
                    for (int i = A.Y; i <= B.Y; i++)
                    {
                        Map[i][A.X] = 0;
                    }
                }
            }

        }


        static string DecisionMaker(Random random, string direction, int y)
        {
            switch (direction)
            {
                case "FIRST":
                    if (y > 81)
                    {
                        return UP;
                    }
                    int decision = random.Next(7);
                    if (decision == 0 || decision == 1)
                    {
                        return UP;
                    }
                    else if (decision == 2 || decision == 3)
                    {
                        return RIGHT;
                    }
                    else if (decision == 4 || decision == 5)
                    {
                        return DOWN;
                    }
                    else return LEFT;

                case "SECOND":
                    decision = random.Next(7);
                    if (decision == 0 || decision == 1)
                    {
                        return DOWN;
                    }
                    else if (decision == 2 || decision == 3)
                    {
                        return LEFT;
                    }
                    else if (decision == 4 || decision == 5)
                    {
                        return UP;
                    }
                    else return RIGHT;


            }
            return null;
        }

        static Point VectorManagment(int[][] Map, Random random, Point point, string decision)
        {
            Point newPoint = point;
            switch (decision)
            {
                case UP:
                    newPoint = VectorUp(Map, newPoint.X, newPoint.Y, random.Next(1, 16));
                    break;
                case RIGHT:
                    newPoint = VectorRight(Map, newPoint.X, newPoint.Y, random.Next(1, 16));
                    break;
                case DOWN:
                    newPoint = VectorDown(Map, newPoint.X, newPoint.Y, random.Next(1, 16));
                    break;
                case LEFT:
                    newPoint = VectorLeft(Map, newPoint.X, newPoint.Y, random.Next(1, 16));
                    break;
            }
            return newPoint;
        }

        static Point VectorUp(int[][] Map, int x, int y, int count)
        {
            int counter = 0;
            int lastY = y;
            while (counter < count && lastY - 2 >= 0 && Map[lastY - 2][x] != 2)
            {
                Map[lastY--][x] = 2;
                counter++;
            }
            return new Point(x, lastY);
        }

        static Point VectorDown(int[][] Map, int x, int y, int count)
        {
            int counter = 0;
            int lastY = y;
            while (counter < count && lastY + 2 < Map.Length && Map[lastY + 2][x] != 2)
            {
                Map[lastY++][x] = 2;
                counter++;
            }
            return new Point(x, lastY);
        }

        static Point VectorLeft(int[][] Map, int x, int y, int count)
        {
            int counter = 0;
            int lastX = x;
            while (counter < count && lastX - 2 >= 0 && Map[y][lastX - 2] != 2 || counter < count && lastX - 1 == 0)
            {
                Map[y][lastX--] = 2;
                counter++;
            }
            return new Point(lastX, y);
        }
        static Point VectorRight(int[][] Map, int x, int y, int count)
        {
            int counter = 0;
            int lastX = x;
            while (counter < count && lastX + 2 < Map[0].Length && Map[y][lastX + 2] != 2 || counter < count && lastX + 1 == Map[0].Length - 1)
            {
                Map[y][lastX++] = 2;
                counter++;
            }
            return new Point(lastX, y);
        }

        static List<Point> SortByX(Point[] arr1, Point[] arr2)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < arr1.Length - 2; i++)
            {
                string direction = Point.FindDirection(arr1[i], arr1[i + 1]);
                if (direction == "LEFT")
                {
                    arr1[i].X = arr1[i].X - Point.FindDistance(arr1[i], arr1[i + 1]);
                    arr1[i].direction = direction;
                    points.Add(arr1[i]);
                }
                else
                {
                    arr1[i].direction = direction;
                    points.Add(arr1[i]);
                }
            }
            for (int i = 0; i < arr2.Length - 2; i++)
            {
                string direction = Point.FindDirection(arr1[i], arr1[i + 1]);
                if (direction == "LEFT")
                {
                    Point holder = arr2[i];
                    holder.X = holder.X - Point.FindDistance(arr2[i], arr2[i + 1]);
                    holder.direction = direction;
                    points.Add(holder);
                }
                else
                {
                    arr2[i].direction = direction;
                    points.Add(arr2[i]);
                }
            }
            List<Point> sortedPoints = points.OrderBy(point => point.X).ToList();
            return sortedPoints;
        }

        static void OutputStacks(Stack<Point> stack1, Stack<Point> stack2)
        {
            Point[] arr1 = new Point[stack1.Count];
            int counter = stack1.Count - 1;
            while (stack1.Count != 0)
            {
                arr1[counter--] = stack1.Pop();
            }

            Point[] arr2 = new Point[stack2.Count];
            counter = stack2.Count - 1;
            while (stack2.Count != 0)
            {
                arr2[counter--] = stack2.Pop();
            }
            List<Point> sorted = SortByX(arr1, arr2);

            using (StreamWriter output = new StreamWriter("rezultatai.txt"))
            {
                Point lastLeft = null;
                for (int i = 1; i < sorted.Count() - 1; i++)
                {
                    if (sorted[i].direction == LEFT)
                    {
                        lastLeft = sorted[i];
                    }
                    output.WriteLine(String.Format("{0} {1} {2};", (sorted[i].X - sorted[i - 1].X) * 250, sorted[i].direction.ToLower(), 300 + yLength - 1 - sorted[i].Y));
                }
                string[] stopLines = new string[4];
                stopLines[0] = String.Format("{0} right 0;", 0);
                stopLines[1] = String.Format("{0} left 0;", 0);
                stopLines[2] = String.Format("{0} up 0;", 0);
                stopLines[3] = String.Format("{0} down 0;", 0);
                for (int i = 0; i < stopLines.Length; i++)
                {
                    output.WriteLine(stopLines[i]);
                }
            }
        }
    }

    public class Point
    {
        public int X;
        public int Y;
        public string direction;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return String.Format("X: {0}  Y: {1}", this.X, this.Y);
        }

        public override bool Equals(Object obj)
        {
            return this.X == ((Point)obj).X && this.Y == ((Point)obj).Y;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        public static Point ReturnPointFromArray(Point[] arr, Point a)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == a)
                {
                    return a;
                }
            }
            return null;
        }

        public static int FindDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) != 0 ? Math.Abs(a.X - b.X) : Math.Abs(a.Y - b.Y);
        }

        public static string FindDirection(Point a, Point b)
        {
            if (a.X - b.X < 0)
            {
                return "RIGHT";
            }
            else if (a.X - b.X > 0)
            {
                return "LEFT";
            }
            else if (a.Y - b.Y < 0)
            {
                return "DOWN";
            }

            else
            {
                return "UP";
            }
        }
    }
}