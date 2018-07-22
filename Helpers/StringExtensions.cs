namespace last_fm_not_mine_alert_web
{
    public static class StringExtensions
    {
        public static bool IsNotNullAndEquals(this string str1, string str2)
        {
            return !string.IsNullOrEmpty(str1) && str1.Equals(str2);
        }
    }
}
