public static class DateTimeUtils
{
    public static DateTime? ConvertToUnspecified(DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Unspecified);
        }
        return null;
    }
}
