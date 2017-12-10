using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer
{
    public class Manager
    {
        static Manager _e;
        public static Manager e { get { if (_e == null) _e = new Manager(); return _e; } }
        /// <summary>
        /// connection id, player
        /// </summary>
        public ConcurrentDictionary<string, ServerPlayer> connectedParticipants = new ConcurrentDictionary<string, ServerPlayer>();
        public ConcurrentBag<string> connectedGameRunners = new ConcurrentBag<string>();
        public ConcurrentDictionary<Guid, UploadingFileInfo> uploadingFiles = new ConcurrentDictionary<Guid, UploadingFileInfo>();
    }
    public class GameRunner
    {

    }


    public class UploadingFileInfo
    {
        static int currentId = 0;

        public string fileName = null;
        public int id;

        public int partCount;
        public byte[][] bytes;
        public UploadingFileInfo(int partCount)
        {
            id = currentId++;
            this.partCount = partCount;
            bytes = new byte[partCount][];
        }

        public bool AddPartAndCheckFinish(byte[] part, int number)
        {
            bytes[number] = part;
            bool finished = bytes.Count(x => x == null) == 0;
            return finished;
        }

    }
}
