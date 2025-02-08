namespace TestSensor;
using Microsoft.Maui.Devices.Sensors;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

public partial class MainPage : ContentPage
{
    // 低通滤波参数
    private double alpha = 0.8;
    private double gravityX, gravityY, gravityZ;
    

    public MainPage()
    {
        InitializeComponent();
                // 初始化折线系列
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        StartSensor();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        StopSensor();
    }
    private void StartSensor()
    {
        // 以加速度计为例，其他传感器替换类名（如 Gyroscope）
        if (Accelerometer.IsSupported)
        {
            Accelerometer.ReadingChanged += OnReadingChanged;
            Accelerometer.Start(SensorSpeed.UI); // 传感器速度模式
        }
        else
        {
            DisplayAlert("错误", "设备不支持此传感器", "确定");
        }
    }

    private void StopSensor()
    {
        if (Accelerometer.IsSupported)
        {
            Accelerometer.Stop();
            Accelerometer.ReadingChanged -= OnReadingChanged;
        }
    }

    private void OnReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        var reading = e.Reading;

        // 更新重力分量
        UpdateGravity(reading.Acceleration.X, reading.Acceleration.Y, reading.Acceleration.Z);

        // 计算线性加速度（去除重力）
        double linearX = reading.Acceleration.X - gravityX;
        double linearY = reading.Acceleration.Y - gravityY;
        double linearZ = reading.Acceleration.Z - gravityZ;
        double acceleration = Math.Sqrt(linearX * linearX + linearY * linearY + linearZ * linearZ);
        // 更新 UI
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            // LabelX.Text = $"X轴运动: {linearX:F6} m/s²";
            // LabelY.Text = $"Y轴运动: {linearY:F6} m/s²";
            // LabelZ.Text = $"Z轴运动: {linearZ:F6} m/s²";
            //AddDataPoint(acceleration);
        });
    }

    private void UpdateGravity(double x, double y, double z)
    {
        gravityX = alpha * gravityX + (1 - alpha) * x;
        gravityY = alpha * gravityY + (1 - alpha) * y;
        gravityZ = alpha * gravityZ + (1 - alpha) * z;
    }
}

