using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20
{
    class Program
    {

        static void Main(string[] args)
        {
        one:
            string path = @"D:\matrix.txt";
            string currLine = "";
            List<int[]> matrixOfDistances = new List<int[]>();
            char[] mtrx = new char[100];
            int iter = 0;
            int length = 0;
            int num = 0;

            int numOfClients = 0;
            int numOfCastles = 0;



            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {

                while ((currLine = sr.ReadLine()) != null)
                {
                    string[] m = currLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    matrixOfDistances.Add(new int[m.Length]);
                    for (int i = 0; i < m.Length; i++)
                    {
                        matrixOfDistances[iter][i] = Convert.ToInt32(m[i]);

                    }
                    iter++;
                }
            }

            for (int i = 0; i < iter; i++)
            {
                for (int j = 0; j < matrixOfDistances[0].Length; j++)
                {
                    Console.Write(matrixOfDistances[i][j] + " ");
                }
                Console.WriteLine();
            }
            numOfClients = iter;
            numOfCastles = matrixOfDistances[0].Length;
            Console.WriteLine();

            Console.WriteLine("Press any button if you don't want to change matrixOfDistances or ENTER if you want to");


            if (Console.ReadKey().Key.Equals(ConsoleKey.Enter))
            {

                Console.WriteLine("Enter the amount of castles");

                while (!Int32.TryParse(Console.ReadLine(), out numOfCastles) || numOfCastles <= 1)
                {
                    Console.WriteLine("Incorrect input, try again");
                }


                Console.WriteLine("Enter the amount of clients");
                while (!Int32.TryParse(Console.ReadLine(), out numOfClients) || numOfClients <= 1)
                {
                    Console.WriteLine("Incorrect input, try again");
                }

                Console.WriteLine("Distances");

                matrixOfDistances.RemoveRange(0, iter);

                for (int i = 0; i < numOfClients; i++)
                {
                    matrixOfDistances.Add(new int[numOfCastles]);
                    for (int j = 0; j < numOfCastles; j++)
                    {
                        matrixOfDistances[i][j] = Convert.ToInt32(Console.ReadLine());


                    }

                }

                File.WriteAllText(path, "");
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    for (int i = 0; i < numOfClients; i++)
                    {
                        for (int j = 0; j < numOfCastles; j++)
                        {

                            sw.Write(matrixOfDistances[i][j] + " ");
                        }
                        sw.Write("\n");

                    }
                }
                goto one;
            }

      
            Console.WriteLine("\nGreedy alg:");
            int diference;
            var solution = GreedyAlg(matrixOfDistances, out diference);
            Console.Write("\nSolution: ");
            foreach (var item in solution)
            {
                Console.Write(item);
            }
            Console.Write($"\nDiference: {diference}");

            Console.WriteLine();
            int numOfTries = 0;
            Console.WriteLine("Enter the amount of tries");
            while (!Int32.TryParse(Console.ReadLine(), out numOfTries) || numOfTries <= 1)
            {
                Console.WriteLine("Incorrect input, try again");
            }

            //int optimal = 1;

            //if (numOfTries % 2 == 0)
            //{
            //    optimal = 0;
            //}

            int optimal = 0;

            var randd = new Random();
            int[,] build = new int[numOfTries, numOfCastles];
            for (int i = 0; i < build.GetLength(0); i++)
            {
                for (int j = 0; j < build.GetLength(1); j++)
                {
                    build[i, j] = randd.Next(2);
                }
            }

            int iterations = 0;

        here:

            if (iterations > 100000)
            {
                goto no;
            }
            int[] differences = new int[numOfTries];
            int[,] numOfMinsInCastles = new int[numOfTries, numOfCastles];

            int currMin = matrixOfDistances[0][0];
            int indexI = 0;
            int indexJ = 0;
            int nextMin = 0;
            Random rand = new Random();
            for (int k = 0; k < numOfTries; k++)
            {
                for (int i = 0; i < numOfClients; i++)
                {
                    currMin = 10;
                    indexI = i;
                    indexJ = 0;
                    nextMin = 10;
                    for (int j = 0; j < numOfCastles; j++)
                    {
                        if (build[k, j] == 1)
                        {
                            if (matrixOfDistances[i][j] < currMin)
                            {
                                currMin = matrixOfDistances[i][j];
                                indexI = i;
                                indexJ = j;
                            }
                        }
                    }
                    numOfMinsInCastles[k, indexJ]++;

                }
            }

            int min = numOfMinsInCastles[0, 0];
            int max = 0;


            int z = 0;
            int q = 0;
            for (int i = 0; i < numOfMinsInCastles.GetLength(0); i++)
            {

                max = 0;

                for (int j = 0; j < numOfMinsInCastles.GetLength(1); j++)
                {
                    if (build[i, j] == 1)
                    {
                        q++;
                        if (z == 0)
                        {
                            min = 10;
                            z++;
                        }
                        if (numOfMinsInCastles[i, j] < min) min = numOfMinsInCastles[i, j];
                        if (numOfMinsInCastles[i, j] > max) max = numOfMinsInCastles[i, j];
                    }
                }
                z = 0;
                if (q == 1)
                {
                    min = 0;
                }
                q = 0;
                differences[i] = max - min;

            }

            string empt = "";
            if (iterations > 5000) optimal = 1;
            int checkOptimal = 0;
            int[] indexes = new int[numOfTries];
            for (int i = 0; i < numOfTries; i++)
            {
                if (differences[i] == optimal)
                {
                    checkOptimal++;
                    indexes[i] = 1;
                }

            }

            string emptAAO = "";

            if (checkOptimal != 0)
            {

                for (int i = 0; i < numOfTries; i++)
                {
                    if (indexes[i] == 1)
                    {

                        for (int j = 0; j < numOfCastles; j++)
                        {
                            emptAAO += build[i, j];
                            empt += 0;

                        }
                        if (empt.Equals(emptAAO))
                        {
                            optimal = 1;
                            goto here;
                        }
                        emptAAO += " ";


                    }


                }
                var result = string.Join(" ", emptAAO.Split(' ').Distinct());
                Console.WriteLine("Solution:");
                Console.WriteLine(result);
                Console.WriteLine("Diference:");
                Console.WriteLine(optimal);
                Console.WriteLine(iter);
                goto first;
            }

            int bestChoose = differences[0];
            int bestIndex = 0;
            int randChoose = 0;
            int randIndex = 0;


            for (int i = 0; i < differences.Length; i++)
            {
                if (differences[i] < bestChoose)
                {
                    bestChoose = differences[i];
                    bestIndex = i;
                }
            }

            while (randIndex == bestIndex)
            {
                randIndex = rand.Next(0, differences.Length - 1);
            }

            randChoose = differences[randIndex];

            //СЮююююЮЮДАаа СХРЕЩУВАННЯ АОФЫДЛАФВТсчсысОВТФОЛ
            int numOfPoints = 0;
            numOfPoints = rand.Next(1, differences.Length / 2);

            int[] firstChild = new int[numOfCastles];
            int[] secondChild = new int[numOfCastles];

            for (int i = 0; i < numOfCastles; i++)
            {
                if (i < numOfPoints) firstChild[i] = build[bestIndex, i];
                else if (i >= numOfPoints && i < numOfCastles - numOfPoints) firstChild[i] = build[randIndex, i];
                else if (i >= numOfCastles - numOfPoints) firstChild[i] = build[bestIndex, i];

            }


            for (int i = 0; i < numOfCastles; i++)
            {
                if (i < numOfPoints) secondChild[i] = build[randIndex, i];
                else if (i > numOfPoints && i < numOfCastles - numOfPoints) secondChild[i] = build[bestIndex, i];
                else if (i > numOfCastles - numOfPoints) secondChild[i] = build[randIndex, i];
            }

            int numOfMutations = 1;
            int secondMutationIndex = 0;
            int mutationIndex = rand.Next(0, numOfCastles - 1);
            if (firstChild[mutationIndex] == 0) firstChild[mutationIndex] = 1;
            else if (firstChild[mutationIndex] == 1) firstChild[mutationIndex] = 0;

            secondMutationIndex = rand.Next(0, numOfCastles - 1);
            while (secondMutationIndex == mutationIndex)
            {
                secondMutationIndex = rand.Next(0, numOfCastles - 1);
            }
            if (firstChild[secondMutationIndex] == 0) firstChild[secondMutationIndex] = 1;
            else if (firstChild[secondMutationIndex] == 1) firstChild[secondMutationIndex] = 0;

            int firstDifference = 0;
            int secondDifference = 0;

            min = 0;
            max = 0;



            int[,] buildTwo = new int[2, numOfCastles];
            for (int i = 0; i < 2; i++)
            {

                for (int j = 0; j < numOfCastles; j++)
                {
                    if (i == 0)
                    {
                        buildTwo[i, j] = firstChild[j];
                    }
                    else if (i == 1)
                    {
                        buildTwo[i, j] = secondChild[j];
                    }
                }

            }

            int[,] numOfMinsInTwo = new int[2, numOfCastles];
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < numOfClients; i++)
                {
                    currMin = 10;
                    indexI = i;
                    indexJ = 0;
                    for (int j = 0; j < numOfCastles; j++)
                    {
                        if (buildTwo[k, j] == 1)
                        {

                            if (matrixOfDistances[i][j] < currMin)
                            {
                                currMin = matrixOfDistances[i][j];
                                indexI = i;
                                indexJ = j;
                            }
                        }
                    }
                    numOfMinsInTwo[k, indexJ]++;
                }
            }

            int[] differencesTwo = new int[2];
            for (int i = 0; i < numOfMinsInTwo.GetLength(0); i++)
            {
                min = 10;
                max = 0;
                for (int j = 0; j < numOfMinsInTwo.GetLength(1); j++)
                {
                    if (buildTwo[0, j] == 1)
                    {
                        if (numOfMinsInTwo[i, j] < min) min = numOfMinsInTwo[i, j];
                        if (numOfMinsInTwo[i, j] > max) max = numOfMinsInTwo[i, j];
                    }
                }

                differencesTwo[i] = max - min;

            }


            int firstIndexChange = 0, secondIndexchange = 0;
            max = 0;
            firstDifference = differencesTwo[0];
            secondDifference = differencesTwo[1];

            int cycle = 1;
            max = 0;
            for (int i = 0; i < differences.Length; i++)
            {
                if (differences[i] > max)
                {
                    max = differences[i];
                    firstIndexChange = i;
                }

            }
            if (max > firstDifference)
            {

                for (int j = 0; j < build.GetLength(1); j++)
                {
                    build[firstIndexChange, j] = firstChild[j];
                }

                cycle = 0;
                iterations = 0;
                differences[firstIndexChange] = firstDifference;


            }


            max = 0;
            for (int i = 0; i < differences.Length; i++)
            {
                if (differences[i] > max)
                {
                    max = differences[i];
                    secondIndexchange = i;
                }


            }
            if (max > secondDifference)
            {

                for (int j = 0; j < build.GetLength(1); j++)
                {
                    build[secondIndexchange, j] = secondChild[j];
                }

                cycle = 0;
                iterations = 0;
                differences[secondIndexchange] = secondDifference;
            }

            if (cycle == 1)
            {
                iterations++;
            }



            goto here;

        first:
            //Console.WriteLine("Получается изи чи как");
            Console.ReadLine();

        no:
           Console.WriteLine("No result");
           Console.ReadLine();


        }


        static int[,] RandNumOfTries(int[,] matrixOfTries)
        {
        here:
            int num = 0;
            Random rand = new Random();
            Console.WriteLine("Tries");
            for (int i = 0; i < matrixOfTries.GetLength(0); i++)
            {
                for (int j = 0; j < matrixOfTries.GetLength(1); j++)
                {
                    matrixOfTries[i, j] = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(matrixOfTries[i, j] + " ");

                }
                Console.WriteLine();
            }

            for (int i = 0; i < matrixOfTries.GetLength(0); i++)
            {
                for (int j = 0; j < matrixOfTries.GetLength(1); j++)
                {
                    if (matrixOfTries[i, j] == 1) num++;
                }
                if (num == matrixOfTries.GetLength(1)) goto here;
                num = 0;
            }


            return matrixOfTries;
        }

        static int[,] RandDistances(int[,] distances)
        {

            Console.WriteLine("Distances");
            Random rand = new Random();
            for (int i = 0; i < distances.GetLength(0); i++)
            {
                for (int j = 0; j < distances.GetLength(1); j++)
                {
                    distances[i, j] = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(distances[i, j]);
                }
                Console.WriteLine();
            }

            return distances;
        }

        static int[] GreedyAlg(List<int[]> matrixOfDistances, out int diferenceValue)
        {
            int minDistance = 2;
            int countOfClients = matrixOfDistances.Count();
            int countOfTowers = matrixOfDistances[0].Length;
            var diference = int.MaxValue;
            var solution = new List<int>();
            while (true)
            {
                var lessThenMinDistanceCount = TowersWithLessDistance(matrixOfDistances, minDistance);
               
                var avarage = (int)Math.Ceiling((decimal)lessThenMinDistanceCount.Sum() / lessThenMinDistanceCount.Count());

                var vectorOfSolution = lessThenMinDistanceCount.Select(x => x == avarage ? 1 : 0).ToList();

                if (vectorOfSolution.Where(x => x == 1).Count() > 1)
                {
                    var currentDiderence = CountDiference(matrixOfDistances,ref vectorOfSolution);
                    if (currentDiderence < diference)
                    {
                        diference = currentDiderence;
                        solution = vectorOfSolution;
                        if (diference == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                minDistance++;
            }
            diferenceValue = diference;
            return solution.ToArray();
        }

        static int CountDiference(List<int[]> matrixOfDistances, ref List<int> vectorOfSolution)
        {
            var towerConnections = new int[vectorOfSolution.Count];
            var tmpMatrixOfDistance = new List<int[]>(matrixOfDistances);

            for (int i = 0; i < matrixOfDistances.Count(); i++)
            {
                int minIndex = 0;
                int minValue = int.MaxValue;
                for (int j = 0; j < vectorOfSolution.Count(); j++)
                {
                    if (vectorOfSolution[j] != 0 && matrixOfDistances[i][j] < minValue)
                    {
                         minValue = matrixOfDistances[i][j];
                         minIndex = j;
                    }
                }
                towerConnections[minIndex]++;
            }

            for (int i = 0; i < vectorOfSolution.Count; i++)
            {
                if (towerConnections[i] == 0 && vectorOfSolution[i] == 1)
                {
                    vectorOfSolution[i] = 0;
                }
            }

            return towerConnections.Where(x => x != 0).Max() - towerConnections.Where(x => x != 0).Min();
        }

        static int[] TowersWithLessDistance(List<int[]> matrixOfDistances, int minDistance)
        {
            int countOfClients = matrixOfDistances.Count();
            int countOfTowers = matrixOfDistances[0].Length;
            int[] lessThenMinNumberForEachTower = new int[countOfTowers];

            for (int i = 0; i < countOfClients; i++)
            {
                for (int j = 0; j < countOfTowers; j++)
                {
                    if (matrixOfDistances[i][j] <= minDistance)
                    {
                        lessThenMinNumberForEachTower[j]++;
                    }
                }
            }

            return lessThenMinNumberForEachTower;
        }
    }
}
