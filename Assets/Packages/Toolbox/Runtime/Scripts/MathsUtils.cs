public static class MathsUtils
{
    public static bool IsBetween(this float value, float a, float b)
    {
        
        
        if (a < b)
        {
            (b, a) = (a, b);
        }

        return a <= value && value <= b ;
    }
}