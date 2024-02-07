using System;
using System.Collections.Generic;
using System.Globalization;

public static class TimeZoneExtensions
{
    private static readonly Dictionary<string, string> OlsonWindowsTimeZones = new Dictionary<string, string>()
    {
        {"Africa/Bangui", "W. Central Africa Standard Time"},
        {"Europe/Istanbul", "Turkey Standard Time"},
        {"America/New_York", "Eastern Standard Time"},
        {"America/Chicago", "Central Standard Time"},
        {"America/Denver", "Mountain Standard Time"},
        {"America/Anchorage", "Alaskan Standard Time"},
        {"America/Honolulu", "Hawaiian Standard Time"},
        {"America/Puerto_Rico", "Atlantic Standard Time"},
        {"America/Tijuana", "Pacific Standard Time"},
        {"America/Indiana/Indianapolis", "US Eastern Standard Time"},
        {"America/Indiana/Knox", "Central Standard Time"},
        {"America/Indiana/Marengo", "US Eastern Standard Time"},
        {"America/Indiana/Petersburg", "Eastern Standard Time"},
        {"America/Indiana/Tell_City", "Central Standard Time"},
        {"America/Indiana/Vevay", "US Eastern Standard Time"},
        {"America/Indiana/Vincennes", "Eastern Standard Time"},
        {"America/Indiana/Winamac", "Eastern Standard Time"},
        {"America/Detroit", "Eastern Standard Time"},
        {"America/Adak", "Aleutian Standard Time"},
        {"America/Nome", "Alaskan Standard Time"},
        {"America/Yakutat", "Alaskan Standard Time"},
        {"America/Sitka", "Alaskan Standard Time"},
        {"America/Boise", "Mountain Standard Time"},
        {"America/Inuvik", "Mountain Standard Time"},
        {"America/Iqaluit", "Eastern Standard Time"},
        {"America/Jamaica", "SA Pacific Standard Time"},
        {"America/Juneau", "Alaskan Standard Time"},
        {"America/Kentucky/Louisville", "Eastern Standard Time"},
        {"America/Kentucky/Monticello", "Eastern Standard Time"},
        {"America/Kralendijk", "SA Western Standard Time"},
        {"America/La_Paz", "SA Western Standard Time"},
        {"America/Lima", "SA Pacific Standard Time"},
        {"America/Los_Angeles", "Pacific Standard Time"},
        {"America/Louisville", "Eastern Standard Time"},
        {"America/Lower_Princes", "SA Western Standard Time"},
        {"America/Maceio", "SA Eastern Standard Time"},
        {"America/Managua", "Central America Standard Time"},
        {"America/Manaus", "SA Western Standard Time"},
        {"America/Marigot", "SA Western Standard Time"},
        {"America/Martinique", "SA Western Standard Time"},
        {"America/Matamoros", "Central Standard Time"},
        {"America/Mazatlan", "Mountain Standard Time"},
        {"America/Menominee", "Central Standard Time"},
        {"America/Merida", "Central Standard Time"},
        {"America/Metlakatla", "Pacific Standard Time"},
        {"America/Mexico_City", "Central Standard Time"},
        {"America/Miquelon", "Saint Pierre Standard Time"},
        {"America/Moncton", "Atlantic Standard Time"},
        {"America/Monterrey", "Central Standard Time"},
        {"America/Montevideo", "Montevideo Standard Time"},
        {"America/Montreal", "Eastern Standard Time"},
        {"America/Montserrat", "SA Western Standard Time"},
        {"America/Nassau", "Eastern Standard Time"},
        {"America/Nipigon", "Eastern Standard Time"},
        {"America/Noronha", "UTC-02"},
        {"America/North_Dakota/Beulah", "Central Standard Time"},
        {"America/North_Dakota/Center", "Central Standard Time"},
        {"America/North_Dakota/New_Salem", "Central Standard Time"},
        {"America/Ojinaga", "Mountain Standard Time"},
        {"America/Panama", "SA Pacific Standard Time"},
        {"America/Pangnirtung", "Eastern Standard Time"},
        {"America/Paramaribo", "SA Eastern Standard Time"},
        {"America/Phoenix", "US Mountain Standard Time"},
        {"America/Port-au-Prince", "Eastern Standard Time"},
        {"America/Port_of_Spain", "SA Western Standard Time"},
        {"America/Porto_Velho", "SA Western Standard Time"},
        {"America/Punta_Arenas", "Magallanes Standard Time"},
        {"America/Rainy_River", "Central Standard Time"},
        {"America/Rankin_Inlet", "Central Standard Time"},
        {"America/Recife", "SA Eastern Standard Time"},
        {"America/Regina", "Canada Central Standard Time"},
        {"America/Resolute", "Central Standard Time"},
        {"America/Rio_Branco", "SA Pacific Standard Time"},
        {"America/Santa_Isabel", "Pacific Standard Time"},
        {"America/Santarem", "SA Eastern Standard Time"},
        {"America/Santiago", "Pacific SA Standard Time"},
        {"America/Santo_Domingo", "SA Western Standard Time"},
        {"America/Sao_Paulo", "E. South America Standard Time"},
        {"America/Scoresbysund", "Azores Standard Time"},
        {"America/St_Barthelemy", "SA Western Standard Time"},
        {"America/St_Johns", "Newfoundland Standard Time"},
        {"America/St_Kitts", "SA Western Standard Time"},
        {"America/St_Lucia", "SA Western Standard Time"},
        {"America/St_Thomas", "SA Western Standard Time"},
        {"America/St_Vincent", "SA Western Standard Time"},
        {"America/Swift_Current", "Canada Central Standard Time"},
        {"America/Tegucigalpa", "Central America Standard Time"},
        {"America/Thule", "Atlantic Standard Time"},
        {"America/Thunder_Bay", "Eastern Standard Time"},
        {"America/Toronto", "Eastern Standard Time"},
        {"America/Tortola", "SA Western Standard Time"},
        {"America/Vancouver", "Pacific Standard Time"},
        {"America/Whitehorse", "Yukon Standard Time"},
        {"America/Winnipeg", "Central Standard Time"},
        {"America/Yellowknife", "Mountain Standard Time"}
    };


    public static TimeZoneInfo GetTimeZoneInfo(this string olsonTimeZoneId)
    {
        var windowsTimeZoneId = ConvertOlsonTimeZoneToWindowsTimeZone(olsonTimeZoneId);
        TimeZoneInfo timeZoneInfo;

        try
        {
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            timeZoneInfo = TimeZoneInfo.Utc;
        }

        return timeZoneInfo;
    }

    private static string ConvertOlsonTimeZoneToWindowsTimeZone(string olsonTimeZone)
    {
        if (OlsonWindowsTimeZones.TryGetValue(olsonTimeZone, out var windowsTimeZone))
        {
            return windowsTimeZone;
        }

        return olsonTimeZone; // Olson zaman dilimi Windows eşleştirmeleri içinde bulunamazsa
    }
}