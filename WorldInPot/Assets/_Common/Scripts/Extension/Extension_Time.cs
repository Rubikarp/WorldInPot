using System;
using UnityEngine;
public static class Extension_Time
{
	public static string ToFrenchString(this DateTime date) => date.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR"));
	public static string ToBritishString(this DateTime date) => date.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-UK"));

	public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek)
	{
		int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
		return date.AddDays(-1 * diff).Date;
	}
	public static int FirstWeekDayOfThisMonth(DayOfWeek aimDayOfWeek)
	{
		DayOfWeek dayOfWeekOnTheFirst = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).DayOfWeek;
		int firstAimDayOfTheMonth = (7 + (dayOfWeekOnTheFirst - aimDayOfWeek)) % 7;

		return firstAimDayOfTheMonth;
	}
	public static int X_WeekDayOfThisMonth(int X, DayOfWeek aimDayOfWeek)
	{
		X = Mathf.Clamp(X, 1, 4);
		int result = FirstWeekDayOfThisMonth(aimDayOfWeek);

		for (int i = 1; i < X; i++)
		{
			result += 7;
		}
		return result;
	}
	public static int LastWeekDayOfThisMonth(DayOfWeek aimDayOfWeek)
	{
		int dayInThisMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

		int result = FirstWeekDayOfThisMonth(aimDayOfWeek);
		while (result + 7 <= dayInThisMonth)
		{
			result += 7;
		}
		return result;
	}
}
