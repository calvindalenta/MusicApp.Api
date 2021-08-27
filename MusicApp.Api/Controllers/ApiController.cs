using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace MusicApp.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    public class ApiController : ControllerBase
    {

        private readonly ILogger<ApiController> _logger;
        private readonly List<Track> _tracks;

        public ApiController(ILogger<ApiController> logger, List<Track> tracks)
        {
            _logger = logger;
            _tracks = tracks;
        }

        [HttpGet, Route("track/{id}")]
        public IActionResult Music(string id)
        {
            // Check if the id exists
            Track track = _tracks.Find(track => track.Id == id);
            if (track == null)
            {
                return NotFound("No track found for the specified id");
            }

            // Resolve the file path
            // Should resolve to {ApplicationPath}/{genre}/{audio.mp3}
            string relativePath = Path.Combine(track.Genre.ToLower(), track.FileName);
            string fullPath = PathUtils.GetFilePathFromApplicationPath(relativePath);

            // Return the file stream
            FileStream stream = System.IO.File.OpenRead(fullPath);
            return File(stream, "audio/mp3", true);
        }

        [HttpGet, Route("tracks")]
        public IActionResult Tracks()
        {
            return Ok(_tracks.ToArray());
        }
    }
}
