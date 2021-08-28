using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using MusicApp.Api.Repositories;

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

        [HttpGet, Route("track/{id}")]
        public IActionResult GetAudioFile(string id)
        {
            Stream stream = _repo.GetAudio(id);
            if (stream is null) return NotFound();
            return File(stream, "audio/mp3", true);
        }

        [HttpGet, Route("image/{id}")]
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
