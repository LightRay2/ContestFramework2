using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework
{
    public enum TimelinePositions { left, top,right,bottom } //todo now only right is implemented
    public static class FrameworkSettings
    {
        /// <summary>
        /// внутренние настройки, влияющие на способ работы фреймворка (вряд ли придется их менять)
        /// </summary>
        public static InnerSettingsClass ForInnerUse = new InnerSettingsClass();
        public class InnerSettingsClass
        {
           // public int FileUploadBufferSize = 4096;
            public string RoamingPathWithSlash { get
                {
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + FrameworkSettings.GameNameEnglish + "\\";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    return path;
                }
            }

            public bool GetAllUserTurnsImmediately { get; set; }

            /// <summary>
            /// воспринимает кратные 16
            /// </summary>
            public int TimerInterval = 16;

            
        }

        /// <summary>
        /// 0 значит 
        /// </summary>
        public static int PlayersPerGameMax = 2;
        public static int PlayersPerGameMin = 2;
        public static int FramesPerTurn=50;

        public static List<Tuple<string, bool>> DefaultProgramAddresses = new List<Tuple<string, bool>>();

        /// <summary>
        /// для дебага игры подходит
        /// </summary>
        public static bool RunGameImmediately = false;
        public static bool AllowFastGameInBackgroundThread { get; set; }
        public static double ExecutionTimeLimitSeconds = 2;

        /// <summary>
        /// будет написано в заголовке формы, настройки в роаминге будут с таким именем (чтобы не путать с другими играми, старайтесь дать уникальное имя)
        /// </summary>
        public static string GameNameEnglish = "ContestAI";
        /// <summary>
        /// когда нажимают кнопку, сначала одинаковый для всех программ хелп, затем этот с новой строки
        /// </summary>
        public static string AdditionalHelpOnGameForm = "";
        public static TimelineSettings Timeline = new TimelineSettings();
        public  class TimelineSettings
        {
            public bool Enabled = true;
            public double TileWidth = 30;
            public double TileLength = 30;
            public TimelinePositions Position = TimelinePositions.right;
          //  public Enum FontNormalTurn;
           // public Enum FontErrorTurn;

            public int TurnScrollSpeedByMouseOrArrow = 4;
            public int TurnScrollSpeedByPageUpDown = 20;

            public double ScrollAnimationTimeMs = 400;
            public double FollowAnimationTimeMs = 800;
        }
    }
}
