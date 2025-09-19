namespace YoTuViLo2.Extensions;

public static partial class ClassExtensions
{
    public static T Also<T>(this T obj, Action<T> action)
    {
        action(obj);
        return obj;
    }
}
