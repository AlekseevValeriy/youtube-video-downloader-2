namespace YoTuViLo2.Extensions;

public static partial class ClassExtensions
{
    public static Boolean NotEquals(this Object obj, Object value)
    {
        if (value != null)
            return obj != value;
        return false;
    }
}
