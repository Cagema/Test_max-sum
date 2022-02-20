using System;

namespace Test_max_sum
{
    class Program
    {
        static void Main(string[] args)
        {
            string input =
                   @"2
                    8 4
                   6 5 3
                  1 2 7 9
                 3 4 5 6 7
                8 9 0 1 2 9";

            var levelArray = input.Split('\n');
            int[,] matrixNum = new int[levelArray.Length, levelArray.Length];
            for (int levelIndex = 0; levelIndex < levelArray.Length; levelIndex++)
            {
                if (string.IsNullOrWhiteSpace(levelArray[levelIndex]))
                {
                    throw new ArgumentException("Incorrect input numbers string", input);
                }

                var numbers = levelArray[levelIndex].Trim().Split(' ');
                if (numbers.Length != levelIndex + 1)
                {
                    throw new ArgumentException("Incorrect input numbers string", input);
                }

                for (int numberIndex = 0; numberIndex < numbers.Length; numberIndex++)
                {
                    if (string.IsNullOrWhiteSpace(numbers[numberIndex]))
                    {
                        throw new ArgumentException("Incorrect input numbers string", input);
                    }

                    int num = 0;
                    try
                    {
                        num = int.Parse(numbers[numberIndex]);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Wrong format number " + (numberIndex + 1) + " level " + (levelIndex + 1));
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("Number is too big");
                    }

                    
                    matrixNum[levelIndex, numberIndex] = num;
                }
            }

            Console.WriteLine("Input numbers:");
            for (int i = 0; i < levelArray.Length; i++)
            {
                for (int j = 0; j < levelArray.Length; j++)
                {
                    Console.Write(matrixNum[i, j].ToString() + ' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            int maxSum = SearchMaxSumUpToDown(levelArray.Length, matrixNum);
            Console.WriteLine();
            Console.WriteLine("The maximum sum of the path from top to bottom = " + maxSum);

            maxSum = MaxSumSearch(0, 0, matrixNum, levelArray.Length);
            Console.WriteLine();
            Console.WriteLine("Max path sum in recursive way = " + maxSum);


        }

        /// <summary>
        /// The method of enumerating all values from top to bottom with entering the sum in a separate matrix
        /// </summary>
        /// <param name="level">The size of the pyramid of numbers</param>
        /// <param name="matrixNum">Input matrix</param>
        /// <returns>Max path sum</returns>
        private static int SearchMaxSumUpToDown(int level, int[,] matrixNum)
        {
            int maxSum = int.MinValue;
            int[,] matrixSum = new int[level, level];

            for (int i = 0; i < level; i++)
            {
                for (int j = 0; j < level; j++)
                {
                    matrixSum[i, j] = matrixNum[i, j];
                }
            }

            for (int i = 1; i < level; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (j == 0)
                    {
                        if (matrixSum[i, j] + matrixSum[i - 1, j] > matrixSum[i, j])
                        {
                            matrixSum[i, j] += matrixSum[i - 1, j];
                        }
                    }
                    else if (j == i)
                    {
                        if (matrixSum[i, j] + matrixSum[i - 1, j - 1] > matrixSum[i, j])
                        {
                            matrixSum[i, j] += matrixSum[i - 1, j - 1];
                        }
                    }
                    else
                    {
                        int leftParent = matrixSum[i, j] + matrixSum[i - 1, j - 1];
                        int rightParent = matrixSum[i, j] + matrixSum[i - 1, j];
                        if (leftParent > rightParent)
                        {
                            matrixSum[i, j] += matrixSum[i - 1, j - 1];
                        }
                        else
                        {
                            matrixSum[i, j] += matrixSum[i - 1, j];
                        }
                    }

                    if (maxSum < matrixSum[i, j])
                    {
                        maxSum = matrixSum[i, j];
                    }
                }
            }

            Console.WriteLine("New matrix with path sum:");
            for (int i = 0; i < level; i++)
            {
                for (int j = 0; j < level; j++)
                {
                    Console.Write(matrixSum[i, j].ToString() + ' ');
                }
                Console.WriteLine();
            }

            return maxSum;
        }

        /// <summary>
        /// Recursive method for finding the maximum sum
        /// </summary>
        /// <param name="currentLevelIndex">Index to designate the level</param>
        /// <param name="currentNumberIndex">Index to designate the value</param>
        /// <param name="matrix">Input matrix</param>
        /// <param name="maxLevel">Matrix size</param>
        /// <returns>Max path sum</returns>
        private static int MaxSumSearch(int currentLevelIndex, int currentNumberIndex, int[,] matrix, int maxLevel)
        {
            if (currentLevelIndex >= maxLevel || currentNumberIndex >= maxLevel)
            {
                return 0;
            }

            if (currentLevelIndex == maxLevel - 1)
            {
                return matrix[currentLevelIndex, currentNumberIndex];
            }

            return matrix[currentLevelIndex, currentNumberIndex] + Math.Max(MaxSumSearch(currentLevelIndex + 1, currentNumberIndex, matrix, maxLevel),
                                                                            MaxSumSearch(currentLevelIndex + 1, currentNumberIndex + 1, matrix, maxLevel));
        }
    }
}
