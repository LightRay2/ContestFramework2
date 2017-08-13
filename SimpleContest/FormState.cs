using Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SimpleContest
{
    public class FormState : INotifyPropertyChanged, IParamsFromStartForm //todo вынести в хелпер
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static string saveLoadPath = FrameworkSettings.ForInnerUse.RoamingPathWithSlash + "Settings.xml";
        bool loading = true;
        public bool SaveToFile = true;

        static bool legalCreation = false; //чтобы не ошиблись и не вызвали конструктор напрямую
        /// <summary>
        /// не нужно вызывать, используйте static LoadOrCreate
        /// </summary>
        public FormState()
        {
            if (legalCreation == false)
                throw new Exception("Используйте для создания LoadOrCreate");
            legalCreation = false;
        }
        public static FormState LoadOrCreate()
        {
            legalCreation = true;
            var loadedSettings = Serialize.TryReadFromXmlFile<FormState>(saveLoadPath);

            
            if (loadedSettings == null)
            {
                loadedSettings = new FormState();
                for(int i = 0; i < FrameworkSettings.DefaultProgramAddresses.Count; i++)
                {
                    loadedSettings.ProgramAddressesAll.Add(FrameworkSettings.DefaultProgramAddresses[i].Item1);
                    if (FrameworkSettings.DefaultProgramAddresses[i].Item2)
                        loadedSettings.ProgramAddressesInMatch.Add(i);
                }
            }
            try
            {
                for (int i = 0; i < loadedSettings.ProgramAddressesAll.Count; i++)
                {
                    if (File.Exists(loadedSettings.ProgramAddressesAll[i]) == false)
                    {
                        loadedSettings.RemoveProgramAddress(i);
                        i--;
                    }
                }
            }
            catch
            {

            }
            

            loadedSettings.loading = false;

            return loadedSettings;
        }

        public FormState DeepClone()
        {
            return Serialize.TryDeepClone(this);
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
                bool success = Serialize.TryWriteToXmlFile<FormState>(saveLoadPath, this);
                Log.CheckIfDebug(success);
            }
        }

        // ================сами свойства===========================
       

        ObservableCollection<string> _programAddressesAll;
        public ObservableCollection<string> ProgramAddressesAll
        {
            get { 
                if (_programAddressesAll == null) 
                { 
                    _programAddressesAll = new ObservableCollection<string>();
                    _programAddressesAll.CollectionChanged += (s, e) => Notify("ProgramAddressesAll"); 
                } 
                return _programAddressesAll; }
        }

        ObservableCollection<int> _programAddressesInMatch;
        public ObservableCollection<int> ProgramAddressesInMatch
        {
            get
            {
                if (_programAddressesInMatch == null)
                {
                    _programAddressesInMatch = new ObservableCollection<int>();
                    _programAddressesInMatch.CollectionChanged += (s, e) => Notify("ProgramAddressesInMatch");
                }
                return _programAddressesInMatch;
            }
        }

        string javaPath;
        public string JavaPath
        {
            get { return javaPath; }
            set { javaPath = value; if (!loading)  Notify("JavaPath"); }
        }

        string pythonPath;
        public string PythonPath
        {
            get { return pythonPath; }
            set { pythonPath = value; if (!loading) Notify("PythonPath"); }
        }

        ObservableCollection<object> _gameParamsList;
        [XmlIgnore]
        public ObservableCollection<object> GameParamsList
        {
            get
            {
                if (_gameParamsList == null)
                {
                    _gameParamsList = new ObservableCollection<object>();
                    _gameParamsList.CollectionChanged += (s, e) => Notify("GameParamsList");
                }
                return _gameParamsList;
            }
        }
        int _randomSeed= new Random().Next();
        public int RandomSeed
        {
            get { return _randomSeed; }
            set { _randomSeed = value; if (!loading) Notify("RandomSeed"); }
        }

        internal void RemoveProgramAddress(int index)
        {
            ProgramAddressesInMatch.Remove(index);
            for (int i = 0; i < ProgramAddressesInMatch.Count; i++)
            {
                if (ProgramAddressesInMatch[i] > index)
                    ProgramAddressesInMatch[i]--;
            }
            ProgramAddressesAll.RemoveAt(index);
        }

        double _FramesPerTurnMultiplier = 1.0;
        /// <summary>
        /// когда менем скорость, меняется и он
        /// </summary>
        public double FramesPerTurnMultiplier
        {
            get { return _FramesPerTurnMultiplier; }
            set { _FramesPerTurnMultiplier = value; if (!loading) Notify("FramesPerTurnMultiplier"); }
        }



        bool _saveReplays = false;
        public bool SaveReplays
        {
            get { return _saveReplays; }
            set { _saveReplays = value; if (!loading) Notify("SaveReplays"); }
        }

        string _replayFolder = FrameworkSettings.ForInnerUse.RoamingPathWithSlash + "Replays";
        public string ReplayFolder
        {
            get { return _replayFolder; }
            set { _replayFolder = value; if (!loading) Notify("ReplayFolder"); }
        }

        public string ReplayFileName
        {
            get
            {
                return string.Format("{0}__{1}__{2}.rpl",
                    DateTime.Now.ToString("HHmmss_ddMMyyyy"),
                    Path.GetFileNameWithoutExtension(this.ProgramAddressesAll[this.ProgramAddressesInMatch[0]]),
                    Path.GetFileNameWithoutExtension(this.ProgramAddressesAll[this.ProgramAddressesInMatch[1]]));
            }
        }
    }
}
