using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            #region simple game bots
            //var typeName = SimpleGameEasy.Run();
            // var typeName = SimpleGameNormal.Run();
            #endregion

            //var typeName = SolverA.Run();
            var typeName = SolverB.Run();
            //var typeName = Misha1.Run();
           // var typeName = Misha2.Run();

            

            //код ниже переименовывает и переносит exe в папку output, например, он будет называться /_OUTPUT_USE_SHOW_ALL_FILES/SimpleGameEasy.exe
            if (Debugger.IsAttached)
            {
                var exePath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                var exeDirectory = Path.GetDirectoryName(exePath);
                var debugOrReleaseDirectory = Directory.GetParent(exeDirectory).FullName;
                var projectDirectory = Directory.GetParent(debugOrReleaseDirectory).FullName;
                var outputDirectory = projectDirectory + Path.DirectorySeparatorChar + "_OUTPUT_USE_SHOW_ALL_FILES";
                File.Copy(exePath, outputDirectory + Path.DirectorySeparatorChar + typeName + ".exe", overwrite: true);
            }
        }
    }
}
