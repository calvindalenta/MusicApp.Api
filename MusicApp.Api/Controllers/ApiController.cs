using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using MusicApp.Api.Repositories;
using MusicApp.Api.Utilities;

namespace MusicApp.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    public class ApiController : ControllerBase
    {
        private readonly IRepository _repo;

        public ApiController(IRepository repo)
        {
            _repo = repo;
        }

#if DEBUG
        [HttpGet, Route("test/track")]
        public IActionResult GetTestAudioFile(string id)
        {
            string fullPath = PathUtils.GetFilePathFromApplicationPath("c.mp3");

            if (!System.IO.File.Exists(fullPath)) return null;

            // Return the file stream
            var stream = System.IO.File.OpenRead(fullPath);
            if (stream is null) return NotFound();
            return File(stream, "audio/mp3", true);
        }
#endif

        [HttpGet, Route("track/{id}")]
        public IActionResult GetAudioFile(string id)
        {
            Stream stream = _repo.GetAudio(id);
            if (stream is null) return NotFound();
            //HttpContext.Response.Headers.Add("cache-control", new[] { "public,max-age=31536000" });
            //HttpContext.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") });
            return File(stream, "audio/mp3", true);
        }

        [HttpGet, Route("image/{id}")]
        [ResponseCache(Duration = 3600 * 24 * 30)] // 30 days
        public IActionResult GetAudioImage(string id)
        {
            Stream stream = _repo.GetImage(id);
            if (stream is null) return NotFound();
            string mimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
            return File(stream, mimeType, true);
        }

        [HttpGet, Route("tracks")]
        public IActionResult GetAllTracks()
        {
            return Ok(_repo.GetAllTracks());
        }
    }
}
