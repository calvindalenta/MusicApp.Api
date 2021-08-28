using MusicApp.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApp.Api.Repositories
{
    public interface IRepository
    {
        /// <summary>
        ///     Opens an image file stream for reading.
        /// </summary>
        /// <param name="id">The track's id.</param>
        /// <returns>A stream of the image file opened.</returns>
        public Stream GetImage(string id);

        /// <summary>
        ///     Opens an audio file stream for reading. 
        /// </summary>
        /// <param name="id">The track's id.</param>
        /// <returns>A stream of the audio file opened.</returns>
        public Stream GetAudio(string id);

        /// <summary>
        ///     Get a list of available tracks.
        /// </summary>
        /// <returns>An enumerable of tracks.</returns>
        public IEnumerable<Track> GetAllTracks();
    }
}
