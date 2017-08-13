using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class Log
    {
        public static void CheckIfDebug(params bool[] condition){
            if(Debugger.IsAttached && condition.Any(x=>x==false))
            {
                throw new Exception();
            }
        }

        public static void LogOrThrow(Exception ex)
        {
            if (Debugger.IsAttached)
            {
                throw ex;
            }
            else
            {
                int k = 20;
                while (ex != null && k-- > 0)
                {
                    AppendLog(ex.Message + Environment.NewLine + ex.StackTrace);
                    ex = ex.InnerException;
                } 
            }
        }

        private static string LogPath =  Directory.GetCurrentDirectory() + "\\log.txt";

        public static void AppendLog(string message)
        {
           File.AppendAllText(LogPath, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + Environment.NewLine + message + Environment.NewLine + "---------------------------------------" + Environment.NewLine);
        }
       
    }
}
