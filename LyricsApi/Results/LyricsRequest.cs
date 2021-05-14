using System.Linq;

namespace LyricsApi.Results
{
	public class LyricsRequest
	{
		public string Artist { get; set; }

		public string Name { get; set; }

		public bool IsEmpty() =>
			string.IsNullOrEmpty(Artist)
			|| string.IsNullOrEmpty(Name);

		public string Url =>
			$"{char.ToUpper(Artist[0])}{Format(Artist)[1..].ToLower()}-{Format(Name)}";

		private static string Format(string value) =>
			value.ToCharArray().Aggregate(string.Empty,
				(s, c) => s + c switch
				{
					_ when char.IsLetter(c) => char.ToLower(c),
					_ when char.IsWhiteSpace(c) => '-',
					_ => null
				});
	}
}