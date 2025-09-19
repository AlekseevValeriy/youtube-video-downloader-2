namespace YoTuViLo2.Extensions;

public static partial class ClassExtensions
{
    public static Boolean IsNull(this Object obj)
    {
        return obj is null;
    }
}
