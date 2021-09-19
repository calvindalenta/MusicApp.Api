using Microsoft.Extensions.Configuration;
using MusicApp.Api.Models;
using MusicApp.Api.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicApp.Api.Repositories
{
    public class AudioRepository : IRepository
    {
        private readonly List<Track> _tracks;

        public AudioRepository(IConfiguration config)
        {
            string jsonPath = PathUtils.GetFilePathFromApplicationPath(
                 config.GetValue<string>("TracksFileName")
                 );
            var jsonText = File.ReadAllText(jsonPath);
            _tracks = JsonSerializer.Deserialize<List<Track>>(jsonText);

            foreach (var track in _tracks)
            {
                track.ImageUrl = $"/api/image/{track.Id}";
                track.TrackUrl = $"/api/track/{track.Id}";
            }
        }

        public IEnumerable<Track> GetAllTracks()
        {
            return _tracks;
        }

        public Stream GetAudio(string id)
        {
            // Check if the id exists
            Track track = _tracks.Find(track => track.Id == id);
            if (track == null) return null;

            // Resolve the file path
            // Should resolve to {ApplicationPath}/audio/{genre}/{audio.mp3}
            string fullPath = ResolvePath(track.FileName, "audio", track.Genre.ToLower());

            if (!File.Exists(fullPath)) return null;

            // Return the file stream
            return File.OpenRead(fullPath);
        }

        private Stream GetDefaultImage()
        {
            string relativePath = Path.Combine("assets", "images", "default.jpeg");
            string fullPath = PathUtils.GetFilePathFromApplicationPath(relativePath);
            return File.OpenRead(fullPath);
        }

        public Stream GetImage(string id)
        {
            if (id == "default")
            {
                return GetDefaultImage();
            }

            // Check if the id exists
            Track track = _tracks.Find(track => track.Id == id);
            if (track == null) return null;

            // Resolve the file path
            // Should resolve to {ApplicationPath}/images/{genre}/{audio.jpeg}
            string fileName = Path.GetFileNameWithoutExtension(track.FileName) + ".jpeg";
            string fullPath = ResolvePath(fileName, "images", track.Genre.ToLower());

            if (!File.Exists(fullPath)) return null;

            // Return the file stream
            return File.OpenRead(fullPath);
        }

        private string ResolvePath(string fileName, string fileType, string genre)
        {
            string relativePath = Path.Combine("assets", fileType, genre, fileName);
            string fullPath = PathUtils.GetFilePathFromApplicationPath(relativePath);
            return fullPath;
        }

    }
}
