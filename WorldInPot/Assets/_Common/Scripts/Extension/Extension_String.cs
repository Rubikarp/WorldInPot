using System.Linq;

public static class Extension_String
{
	public static string RandomString(int length) => RandomString(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
	public static string RandomString(int length, string chars)
	{
		return new string(Enumerable.Repeat(chars, length).Select(s => s[UnityEngine.Random.Range(0, chars.Length)]).ToArray());
	}

	public static string RankingLetters(int rank)
	{
		switch (rank)
		{
			case 1:
				return "st";
			case 2:
				return "nd";
			case 3:
				return "rd";
			default:
				return "th";
		}
	}
}
