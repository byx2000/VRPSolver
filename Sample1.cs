﻿using System;

class Sample1
{
    // 当求解完成时，此方法将会被调用
    static private void OnFinish(int[][] path, double[] load, double[] mileage)
    {
        Console.WriteLine("Sample1求解完成！\n");

        double maxMileage = 0.0;
        for (int i = 0; i < path.Length; ++i)
        {
            if (path[i].Length == 0) continue;

            Console.Write("Car " + i + ": 0->");
            for (int j = 0; j < path[i].Length; ++j)
            {
                Console.Write(path[i][j] + "->");
            }
            Console.WriteLine("0");

            Console.WriteLine("载重：" + load[i] + " 里程：" + mileage[i]);
            Console.WriteLine();

            maxMileage = Math.Max(maxMileage, mileage[i]);
        }

        Console.WriteLine("时间：" + maxMileage);
        Console.WriteLine();
    }

    static public void Run()
    {
        // 10个配送点

        // VRP问题相关数据
        double[] x = { 12.32, 5.96, 7.73, 10.57, 18.27, 17.55, 18.95, 19.39, 12.75, 11.7, 8.45 };
        double[] y = { 8.35, 9.39, 7.07, 3.55, 3.97, 6.72, 11.01, 14.33, 14.73, 10.6, 13.48 };
        double[] demand = { 0.0, 1.4, 1.5, 0.3, 0.5, 1.7, 0.1, 1.3, 1.1, 2.5, 2.2 };
        double[] capacity = { 5.0, 5.0, 5.0, 5.0, 5.0 };
        double[] disLimit = { 20.0, 20.0, 20.0, 20.0, 20.0 };
        double k1 = 100.0;
        double k2 = 1.0;
        double k3 = 1.0;

        // 求解
        VRPSolver.Solve(x, y, demand, capacity, disLimit, k1, k2, k3, OnFinish);
    }
}
