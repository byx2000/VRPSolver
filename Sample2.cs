using System;
using System.IO;

class Sample2
{
    // 当求解完成时，此方法将会被调用
    static private void OnFinish(int[][] path, double[] load, double[] mileage)
    {
        Console.WriteLine("Sample2求解完成！\n");

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
    }

    static public void Run()
    {
        // 50个配送点   文件输入

        // 打开文件
        StreamReader reader = new StreamReader("in.txt");
        string txt = reader.ReadToEnd();
        char[] sep = { ' ', '\t', '\n' };
        string[] input = txt.Split(sep);

        int index = 0;

        // 解析数据
        int numNode = Convert.ToInt32(input[index++]);
        double[] x = new double[numNode];
        double[] y = new double[numNode];
        double[] demand = new double[numNode];
        for (int i = 0; i < numNode; ++i)
        {
            x[i] = Convert.ToDouble(input[index++]);
            y[i] = Convert.ToDouble(input[index++]);
            demand[i] = Convert.ToDouble(input[index++]);
        }

        int numCar = Convert.ToInt32(input[index++]);
        double[] capacity = new double[numCar];
        double[] disLimit = new double[numCar];
        for (int i = 0; i < numCar; ++i)
        {
            capacity[i] = Convert.ToDouble(input[index++]);
            disLimit[i] = Convert.ToDouble(input[index++]);
        }

        double k1 = Convert.ToDouble(input[index++]);
        double k2 = Convert.ToDouble(input[index++]);
        double k3 = Convert.ToDouble(input[index++]);

        // 求解
        VRPSolver.Solve(x, y, demand, capacity, disLimit, k1, k2, k3, OnFinish);
    }
}
