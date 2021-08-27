using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApp.Api.Repositories
{
    public interface IRepository
    {
        public Stream GetImage(string id);
        public Stream GetAudio(string id);
        public IEnumerable<Track> GetAllTracks();
    }
}
