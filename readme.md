# `VRPSolve` 使用说明

`VRPSolve` 是VRP求解核心算法的C#封装类，该类调用 `VRP-Kernel.dll` 中的核心算法导出函数，并对数据类型进行封装，以方便C#中的调用。



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
      int[][] path,    //所有车的路径，path[i]为第i辆车的路径(不包含原点)
      double[] load,   //所有车的载重，load[i]为第i辆车的载重
      double[] mileage //所有车的里程，mileage[i]为第i辆车的历程
  );
  ```



## 使用示例

```c#
using System;

class Program
{
    // 当求解完成时，此方法将会被调用
    static private void OnFinish(int[][] path, double[] load, double[] mileage)
    {
        Console.WriteLine("求解完成！\n");

        // 输出每辆车的配送路径，以及相应的载重、里程
        for (int i = 0; i < path.Length; ++i)
        {
            Console.Write("Car " + i + ":\t");
            for (int j = 0; j < path[i].Length; ++j)
            {
                Console.Write(path[i][j] + " ");
            }

            Console.WriteLine();
            Console.Write("load: " + load[i] + " ");
            Console.Write("mileage: " + mileage[i]);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
    
    static void Main()
    {
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

参见 `Program.cs`