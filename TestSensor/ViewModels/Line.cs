using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;

namespace TestSensor.ViewModels;

public class Line
{
    public ISeries[] Series { get; set; } = [
        new LineSeries<int>(4, 2, 6),
    ];
}