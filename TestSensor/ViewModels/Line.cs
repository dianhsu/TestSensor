using LiveChartsCore;
using LiveChartsCore.Defaults; // 包含 ObservablePoint
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestSensor.ViewModels;

public class Line : INotifyPropertyChanged
{
    private ObservableCollection<ObservablePoint> _dataPoints = new();
    public ObservableCollection<ObservablePoint> DataPoints
    {
        get => _dataPoints;
        set
        {
            _dataPoints = value;
            OnPropertyChanged();
        }
    }

    public ISeries[] Series { get; set; }

    public Line()
    {
        // 初始化折线图系列
        Series = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Values = DataPoints, // 设置数据点集合
                Name = "实时数据", // 设置系列名称
                GeometrySize = 0, // 设置数据点大小
                Fill = null, // 设置填充色
                Stroke = new SolidColorPaint(SKColors.LightBlue) { StrokeThickness = 2 }, // 设置线条颜色和粗细
            }
        };
    }
    // 添加新数据点（示例方法）
    public void AddDataPoint(double x, double y)
    {
        DataPoints.Add(new ObservablePoint(x, y));

        // 限制数据点数量（保留最近50个点）
        if (DataPoints.Count > 200)
            DataPoints.RemoveAt(0);
    }
    public Axis[] XAxes { get; set; } = new Axis[]
    {
        new Axis
        {
            Name = "时间（秒）",
            IsVisible = false
        }
    };

    public Axis[] YAxes { get; set; } = new Axis[]
    {
        new Axis
        {
            Labeler = value => $"{value:F3}",
            Name = "加速度（m/s²）",
            MinLimit = 0,
        }
    };
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public double getAverage()
    {
        double sum = 0;
        if (DataPoints.Count == 0)
            return sum;
        foreach (var point in DataPoints)
        {
            sum += point.Y.GetValueOrDefault();
        }
        return sum / DataPoints.Count;
    }
    public double getMax(){
        double max = 0;
        if (DataPoints.Count == 0)
            return max;
        foreach (var point in DataPoints)
        {
            if (point.Y.GetValueOrDefault() > max)
                max = point.Y.GetValueOrDefault();
        }
        return max;
    }
}
