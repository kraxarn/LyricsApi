namespace LyricsApi.Results
{
	public class LyricsResult
	{
		public string Lyrics { get; set; }

		public bool IsValid => !string.IsNullOrEmpty(Lyrics);
	}
}