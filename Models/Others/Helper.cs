namespace Tourist_Place.Models.Others
{
    public static class Helper
    {
        public static string NoTilde(this string str)
        {
            return str.Replace("~", "");
        }
    }
}
