using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LyricsApi.Results;
using Microsoft.AspNetCore.Mvc;

namespace LyricsApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LyricsController : ControllerBase
	{
		private static Dictionary<string, LyricsResult> cache = new();
		private static HtmlWeb web = new();

#if DEBUG
		[Route("")]
		public async Task<LyricsResult> Get(string title, string artist) =>
			await Post(new LyricsRequest
			{
				Artist = artist,
				Name = title
			});
#endif

		[HttpPost]
		[Route("")]
		public async Task<LyricsResult> Post([FromBody] LyricsRequest request)
		{
			if (request.IsEmpty())
			{
				return new LyricsResult();
			}

			if (cache.ContainsKey(request.Url))
			{
				return cache[request.Url];
			}

			var doc = await web.LoadFromWebAsync($"https://genius.com/{request.Url}-lyrics");
			var results = new LyricsResult
			{
				Lyrics = doc.DocumentNode
					.Descendants()
					.FirstOrDefault(n => n.GetClasses()
						.Any(c => c == "lyrics" || c.Contains("Lyrics__Root")))?
					.InnerText.Trim()
			};

			cache[request.Url] = results;
			return results;
		}
	}
}