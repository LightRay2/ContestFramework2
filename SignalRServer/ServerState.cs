using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SignalRServer
{
    public class ServerState : INotifyPropertyChanged
    {
        public const string GameRunnerPassword = "qazASD890";
        static ServerState _e;
        public static ServerState e
        {
            get
            {
                if (_e == null)
                {
                    Directory.CreateDirectory(FolderWithSlash);
                    Directory.CreateDirectory(FolderExecutablesWithSlash);
                    Directory.CreateDirectory(FolderReplaysWithSlash);
                    _e = ServerState.LoadOrCreate();
                    if (_e.Players.Count == 0)
                    {
                        _e.Players.Add(new ServerPlayer { uniqueName = "one", password = "one" });
                        _e.Players.Add(new ServerPlayer { uniqueName = "two", password = "two" });
                        _e.Players.Add(new ServerPlayer { uniqueName = "three", password = "three" });
                        _e.Players.Add(new ServerPlayer { uniqueName = "four", password = "four" });
                        _e.Players.Add(new ServerPlayer { uniqueName = "five", password = "five" });
                    }
                    
                }
                return _e;
            }
        }
        public static string FILENAME = FolderWithSlash + "state.txt";
        public static string FolderWithSlash = Application.StartupPath + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;
        public static string FolderExecutablesWithSlash = FolderWithSlash + "Executables" + Path.DirectorySeparatorChar;
        public static string FolderReplaysWithSlash = FolderWithSlash + "Replays" + Path.DirectorySeparatorChar;

        #region organisation


        public event PropertyChangedEventHandler PropertyChanged;
        public static string saveLoadPath = FolderWithSlash + "state.txt";
        bool loading = true;
        public bool SaveToFile = true;

        static bool legalCreation = false; //чтобы не ошиблись и не вызвали конструктор напрямую
        /// <summary>
        /// не нужно вызывать, используйте static LoadOrCreate
        /// </summary>
        public ServerState()
        {
            if (legalCreation == false)
                throw new Exception("Используйте для создания LoadOrCreate");
            legalCreation = false;
        }
        public static ServerState LoadOrCreate()
        {
            legalCreation = true;
            var loadedSettings = TryReadFromXmlFile<ServerState>(saveLoadPath);


            if (loadedSettings == null)
            {
                loadedSettings = new ServerState();

            }

            loadedSettings.loading = false;

            return loadedSettings;
        }

        public ServerState DeepClone()
        {
            return TryDeepClone(this);
        }

        public void SaveChanges()
        {
            bool success = TryWriteToXmlFile<ServerState>(saveLoadPath, this);
        }
        private void Notify(String info)
        {
            if (loading)
                return;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
            if (SaveToFile)
            {
                bool success = TryWriteToXmlFile<ServerState>(saveLoadPath, this);
                //Log.CheckIfDebug(success);
            }
        }
        public static bool TryWriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            bool ok = true;
            TextWriter writer = null;
            try
            {
                var serializer = XmlSerializer.FromTypes(new[] { typeof(T) })[0];
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            catch (Exception ex)
            {
                ok = false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
            return ok;
        }

        public static T TryReadFromXmlFile<T>(string filePath) where T : new()
        {
            bool ok = true;
            T returned = default(T);
            TextReader reader = null;
            try
            {
                var serializer = XmlSerializer.FromTypes(new[] { typeof(T) })[0];// new XmlSerializer(typeof(T)); кидает эксепшн
                reader = new StreamReader(filePath);
                returned = (T)serializer.Deserialize(reader);
            }
            catch
            {
                ok = false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            if (!ok)
                return default(T);
            else
                return returned;
        }

        public static T TryDeepClone<T>(T source) where T : new()
        {
            bool ok = true;
            T returned = default(T);
            try
            {
                var serializer = XmlSerializer.FromTypes(new[] { typeof(T) })[0];// new XmlSerializer(typeof(T)); кидает эксепшн
                var tempStream = new MemoryStream();
                serializer.Serialize(tempStream, source);
                returned = (T)serializer.Deserialize(tempStream);
            }
            catch
            {
                ok = false;
            }
            finally
            {
            }
            if (!ok)
                return default(T);
            else
                return returned;
        }
        #endregion

        // ================сами свойства===========================


        ObservableCollection<ServerPlayer> _players = new ObservableCollection<ServerPlayer>();
        public ObservableCollection<ServerPlayer> Players
        {
            get
            {
                if (_players == null)
                {
                    _players = new ObservableCollection<ServerPlayer>();
                    _players.CollectionChanged += (s, e) => Notify("Players");
                }
                return _players;
            }
        }
        
        ObservableCollection<ServerGame> _games;
        public ObservableCollection<ServerGame> Games
        {
            get
            {
                if (_games == null)
                {
                    _games = new ObservableCollection<ServerGame>();
                    _games.CollectionChanged += (s, e) => Notify("Games");
                }
                return _games;
            }
        }

        Guid _currentState = Guid.NewGuid();
        public Guid CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; if (!loading) Notify("CurrentState"); }
        }
        


    }

    public class ServerPlayer
    {
        public string uniqueName;
        public string password;
        public string lastProgramRelativePathOrNull;
        public DateTime programSent;
        [XmlIgnore]
        [JsonIgnore]
        public string AbsolutePath { get { return ServerState.FolderExecutablesWithSlash + lastProgramRelativePathOrNull; } }
    }
    
    public class ServerGame
    {
        const string DATE_FORMAT = "dd.MM.yyyy.HH.mm.ss";
        public Guid id;
        [Obsolete("Use another constructor")]
        public ServerGame() { }
        public ServerGame(List<Tuple<ServerPlayer, string>> playersAndScores, string jsonData)
        {
            id = Guid.NewGuid();
            //now !! delete all __ in players and programs  
            var date = DateTime.Now;
            RelativePath = $"{date.ToString(DATE_FORMAT)}__{playersAndScores.Count}__{string.Join("__", playersAndScores.Select(x => x.Item1.uniqueName + "__" + x.Item2))}__.txt";
            File.WriteAllText(ServerState.FolderReplaysWithSlash + RelativePath, jsonData);
        }

        public List<string> Split()
        {
            return RelativePath.Split(new string[] { "__" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }


        [XmlIgnore]
        public DateTime dateTime { get { return DateTime.ParseExact(Split()[0], DATE_FORMAT, CultureInfo.InvariantCulture); } }
        [XmlIgnore]
        [Obsolete("warning! can be null")]
        public List<Tuple<ServerPlayer, string>> playersAndScores
        {
            get
            {
                int cnt = int.Parse(Split()[1]);
                var list = new List<Tuple<ServerPlayer, string>>();
                for(int i = 0; i < cnt; i++)
                {
                    var playerName = Split()[2 + i * 2];
                    var playerScore = Split()[2 + i * 2 + 1];
                    var player = ServerState.e.Players.First(x=>x.uniqueName==playerName);
                    list.Add(Tuple.Create(player, playerScore));
                }

                return list;
            }
        }
        [XmlIgnore]
        [Obsolete("полезет в файл, может долго доставаться - иметь это в виду")]
        public string JsonData
        {
            get
            {
                return File.ReadAllText(ServerState.FolderReplaysWithSlash+ RelativePath);


            }
        }
        public string RelativePath;
        public bool FinishedAndSaved;

    }
}
