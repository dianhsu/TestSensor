namespace TestSensor;
using Microsoft.Maui.Devices.Sensors;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
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

    private void OnReadingChanged(object sender, AccelerometerChangedEventArgs e)
    {
        // 读取数据（示例：加速度计）
        var acceleration = e.Reading.Acceleration;
        
        // 更新 UI（必须切换到主线程）
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            LabelX.Text = $"X: {acceleration.X:F2}";
            LabelY.Text = $"Y: {acceleration.Y:F2}";
            LabelZ.Text = $"Z: {acceleration.Z:F2}";
        });
    }

}

