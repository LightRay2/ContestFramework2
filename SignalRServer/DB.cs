using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer
{
    //это для миграций
    //class ProjectInitializer : MigrateDatabaseToLatestVersion<DB, GIS_proto.Migrations.DB.Configuration>
    //{
    //    public ProjectInitializer()
    //        : base(true)
    //    {

    //    }

    //}
    public class DB:DbContext
    {
        [Obsolete("Используйте State.e.CreateDb()")]
        public DB():base("DBConnectionString")
        {

        }

        [Obsolete("Используйте State.e.CreateDb()")]
        public DB(string connectionString):base(connectionString)
        {
            Database.SetInitializer<DB>(null);

            //это для миграций
            //Database.SetInitializer(new ProjectInitializer());
            //Database.Initialize(true);
        }

        public DbSet<Player> Player { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<GamePlayer> GamePlayer { get; set; }
    }

    public class Player
    {
        public Player()
        {
            Id = Guid.NewGuid();
            SolutionSubmitDateTime = DateTime.Now; //это ничего не значит
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [MaxLength]
        public byte[] Solution { get; set; }
        public string SolutionExtension { get; set; }
        public DateTime SolutionSubmitDateTime { get; set; }

        public virtual HashSet<GamePlayer> GamePlayer { get; set; }
    }

    public class Game
    {
        public Game()
        {
            Id = Guid.NewGuid();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        [MaxLength]
        public string Content { get; set; }


        public virtual HashSet<GamePlayer> GamePlayer {get;set;}
    }

    public class GamePlayer
    {
        public int Id { get; set; }
        public string Result { get; set; }


        public virtual Game Game { get; set; }
        public virtual Player Player { get; set; }
    }
}
