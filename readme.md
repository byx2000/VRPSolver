# VRPSolver 使用说明

`VRPSolver` 是VRP求解核心算法的C#封装类，该类调用 `VRP-Kernel.dll` 中的核心算法导出函数，并对原始C++数据类型进行封装，以方便C#中的调用。



## 配置

为在项目中使用 `VRPSolver` ，需要如下配置：

1. 将C#项目的生成目标平台设置为 `x86` （默认为 `Any CPU` ），编译运行
2. 将 `VRPSolver.cs` 和 `VRP-Kernel.dll` 放到项目根目录，并添加到项目中
3. 找到项目生成的exe文件所在的目录（`项目名称\bin\x86\Debug\netcoreapp3.x`），将 `VRP-Kernel.dll` 复制到该目录下



## 接口说明

类 `VRPSolver` 的公共接口如下：

* 求解函数

  ```c#
  public static void Solve
  (
      double[] x, //x[i]为第i个配送点的横坐标，原点为x[0]
      double[] y, //y[i]为第i个配送点的纵坐标，原点为y[0]
      double[] demand,  //demand[i]为第i个配送点的货物需求量，demand[0]将被忽略
      double[] capacity, //capacity[i]为第i辆车的最大载重量
      double[] disLimit, //disLimit[i]为第i辆车的最大里程
      double k1, double k2, double k3, //k1、k2、k3分别为时间、里程、车辆数的权重
      OnFinish onFinish //求解完成后的回调方法(请看下面介绍)
  ); 
  ```

* 求解完成后的回调方法（委托）

  ```c#
  public delegate void OnFinish
  (
      int[][] path,    //path[i]为第i辆车的路径(不包含原点)
      double[] load,   //load[i]为第i辆车的载重
      double[] mileage //mileage[i]为第i辆车的里程
  );
  ```



## 使用示例

Sample1.cs

```c#
using System;

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
```

Sample2.cs

输入文件：in.txt

```c#
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
```

