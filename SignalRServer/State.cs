using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignalRServer
{
    public class State
    {
        static State _e;
        public static State e { get { if (_e == null) _e = new State(); return _e; } }
        

        /// <summary>
        /// connection id, player
        /// </summary>
        public ConcurrentDictionary<string, Guid> connectedParticipants = new ConcurrentDictionary<string, Guid>(); //todo не убираю нигде неактивных
        public ConcurrentBag<string> connectedGameRunners = new ConcurrentBag<string>();
        public ConcurrentDictionary<Guid, List<Guid>> RunningGames = new ConcurrentDictionary<Guid, List<Guid>>();
        public Guid CurrentState = Guid.NewGuid();

        public DB CreateDb() { return new DB(Application.StartupPath+Path.DirectorySeparatorChar+"db.sdf"); }
    }
}
