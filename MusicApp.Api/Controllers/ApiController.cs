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
        public IActionResult GetAudioFile(string id)
        {
            // Check if the id exists
            Track track = _tracks.Find(track => track.Id == id);
            if (track == null)
            {
                return NotFound("No track found for the specified id");
            }

            // Resolve the file path
            // Should resolve to {ApplicationPath}/audio/{genre}/{audio.mp3}
            string relativePath = Path.Combine("audio", track.Genre.ToLower(), track.FileName);
            string fullPath = PathUtils.GetFilePathFromApplicationPath(relativePath);

            // Return the file stream
            FileStream stream = System.IO.File.OpenRead(fullPath);
            return File(stream, "audio/mp3", true);
        }

        [HttpGet, Route("image/{id}")]
        public IActionResult GetAudioImage(string id)
        {
            // Check if the id exists
            Track track = _tracks.Find(track => track.Id == id);
            if (track == null)
            {
                return NotFound("No track found for the specified id");
            }

            // Resolve the file path
            // Should resolve to {ApplicationPath}/images/{genre}/{audio.jpeg}
            string fileName = Path.GetFileNameWithoutExtension(track.FileName) + ".jpeg";
            string relativePath = Path.Combine("images", track.Genre.ToLower(), fileName);
            string fullPath = PathUtils.GetFilePathFromApplicationPath(relativePath);

            // Return the file stream
            string mimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
            FileStream stream = System.IO.File.OpenRead(fullPath);
            return File(stream, mimeType, true);
        }

        [HttpGet, Route("tracks")]
        public IActionResult GetAllTracks()
        {
            return Ok(_tracks.ToArray());
        }
    }
}
