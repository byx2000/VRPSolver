using System;
using System.Runtime.InteropServices;
class VRPSolver
{
    // 原始回调方法
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Callback(int numCar, IntPtr path, IntPtr pathLen, IntPtr load, IntPtr mileage);

    // 原始核心算法导出函数
    [DllImport("VRP-Kernel.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern void Solve(
        int numNode, IntPtr x, IntPtr y, IntPtr demand, // 配送点信息
        int numCar, IntPtr capacity, IntPtr disLimit, // 车辆信息
        double k1, double k2, double k3, // 权重
        Callback callback); // 回调方法

    // 用C#数据类型包装后的回调方法
    public delegate void OnFinish(int[][] path, double[] load, double[] mileage);

    // 用C#数据类型包装后的核心算法
    public static void Solve(
        double[] x, double[] y, double[] demand,  // 配送点信息
        double[] capacity, double[] disLimit, // 车辆信息
        double k1, double k2, double k3, // 权重
        OnFinish onFinish) // 回调方法
    {
        // 复制配送点数据

        int numNode = x.Length;

        IntPtr px = Marshal.AllocHGlobal(numNode * sizeof(double));
        Marshal.Copy(x, 0, px, numNode);

        IntPtr py = Marshal.AllocHGlobal(numNode * sizeof(double));
        Marshal.Copy(y, 0, py, numNode);

        IntPtr pDemand = Marshal.AllocHGlobal(numNode * sizeof(double));
        Marshal.Copy(demand, 0, pDemand, numNode);

        // 复制车辆数据

        int numCar = capacity.Length;

        IntPtr pCapacity = Marshal.AllocHGlobal(numCar * sizeof(double));
        Marshal.Copy(capacity, 0, pCapacity, numCar);

        IntPtr pDisLimit = Marshal.AllocHGlobal(numCar * sizeof(double));
        Marshal.Copy(disLimit, 0, pDisLimit, numCar);

        // 调用原始核心算法代码

        Solve(numNode, px, py, pDemand, numCar, pCapacity, pDisLimit, k1, k2, k3, 
            // 包装原始回调方法
            delegate (int t_numCar, IntPtr path, IntPtr pathLen, IntPtr load, IntPtr mileage)
            {
                int[][] pathArr = new int[numCar][];

                for (int i = 0; i < numCar; ++i)
                {
                    int cPath = Marshal.ReadInt32(pathLen + i * sizeof(int));
                    pathArr[i] = new int[cPath];
                    Marshal.Copy(Marshal.ReadIntPtr(path + i * 4), pathArr[i], 0, cPath);
                }

                double[] loadArr = new double[numCar];
                double[] mileageArr = new double[numCar];

                Marshal.Copy(load, loadArr, 0, numCar);
                Marshal.Copy(mileage, mileageArr, 0, numCar);

                onFinish(pathArr, loadArr, mileageArr);
            });
    }
}
