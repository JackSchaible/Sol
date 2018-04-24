namespace Assets.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static T GetFromCast<T>(this object o)
        {
            return (T) o;
        }
    }
}
