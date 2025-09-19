namespace YoTuViLo2.Extensions;

public static partial class ClassExtensions
{
    public static Double GetEmptySpace(this Border obj)
    {
        return obj.Margin.VerticalThickness + obj.Padding.VerticalThickness + (obj.StrokeThickness * 2);
    }
}
