using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    //todo как бы это вынести на клиент
    //вынесены для краткости кода
    //Доступные спрайты. end - чтобы можно было легко пробежать по всем
    public enum ESprite { test, background, shell1, smallBack, tank, stoneTest,
                          background_field, background_cell,
                          green_hull, red_hull,
                          green_turret, red_turret,
                          green_armored, red_armored,
                          green_combine, red_combine,
                          green_cannon, red_cannon,
                          green_mine, red_mine,

                          tankBox, tankGun, cannonBox, cannonGun, armoBox, armoGun, combine, car,
                          tankBox2, tankGun2, cannonBox2, cannonGun2, armoBox2, armoGun2, combine2, car2,
                        gridPoint,gridPoint2,gridPoint1,
                          explosion, explosionBig, explosionSmall, explosionOut,explosionNuclear, stone,active1, active2,
                          end }
    //public enum EFont { blue, blueSmall, orange, fiolBright, fiol, gold, greenBright, green, greenSmall, red, end }
    
    //действия, которые поддерживает клавиатура. Должны быть привязаны конкретные кнопки в конструкторе
    public enum EKeyboardAction { Fire, Esc, Enter, left, up, right, down,I,O, Unit1, Unit2, Unit3, Unit4, Unit5, end };

    public class Config
    {
        #region nested sprite config class
        public class SpriteConfig
        {
            public readonly string file;
            public readonly int horFrames, vertFrames;
            public readonly int depth;
            /// <summary>
            /// имя относительно екзешника, количество кадров по горизонтали и вертикали в файле
            /// Чем больше глубина, тем выше спрайт рисуется
            /// </summary>
            public SpriteConfig(string file, int horFrames, int vertFrames, int depth=0)
            {
                this.file = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, file);
                this.horFrames = horFrames;
                this.vertFrames = vertFrames;
                this.depth = depth;
            }
            public SpriteConfig(string file) : this(file, 1, 1) { }
        }
        #endregion

        //Сюда кидаем и спрайты, и фонты. Ключ должен быть EFont.ToString() или ESprite.ToString()
        static public readonly Dictionary<string, SpriteConfig> Sprites = new Dictionary<string, SpriteConfig>();
        //сопоставили действия клавиатуры с конкретными клавишами
        static public readonly Dictionary<EKeyboardAction, byte> Keys= new Dictionary<EKeyboardAction,byte>();
        public static int GetSpriteFramesCount(ESprite sprite)
        {
            return Sprites[sprite.ToString()].horFrames * Sprites[sprite.ToString()].vertFrames;
        }

        public static string WindowName = "ContestAI"; 
        public const double ScreenWidth = 400;
        public const double ScreenHeight = 300;
        public const int TimePerFrame = 20; //в миллисекундах

        static Config()
        {
            //Sprites.Add(ESprite.test.ToString(), new SpriteConfig("textures//test.png", 4, 4));
            //Sprites.Add(ESprite.background.ToString(), new SpriteConfig("textures//back6.png", 1, 1,-1000));
            //Sprites.Add(ESprite.smallBack.ToString(), new SpriteConfig("textures//smallback.png", 1, 1));
            //Sprites.Add(ESprite.shell1.ToString(), new SpriteConfig("textures//shell1.png", 1, 1, -1));
            //Sprites.Add(ESprite.explosion.ToString(), new SpriteConfig("textures//explosion.png", 9, 9,20));

            //Sprites.Add(ESprite.background_field.ToString(), new SpriteConfig("textures//background_field.png", 1, 1,-100));
            //Sprites.Add(ESprite.background_cell.ToString(), new SpriteConfig("textures//background_cell.png", 1, 1,-100));
            //Sprites.Add(ESprite.green_hull.ToString(), new SpriteConfig("textures//hull_green.png", 1, 1));
            //Sprites.Add(ESprite.red_hull.ToString(), new SpriteConfig("textures//hull_red.png", 1, 1));
            //Sprites.Add(ESprite.green_turret.ToString(), new SpriteConfig("textures//turret_green.png", 1, 1));
            //Sprites.Add(ESprite.red_turret.ToString(), new SpriteConfig("textures//turret_red.png", 1, 1));
            //Sprites.Add(ESprite.green_armored.ToString(), new SpriteConfig("textures//armored_green.png", 1, 1));
            //Sprites.Add(ESprite.red_armored.ToString(), new SpriteConfig("textures//armored_red.png", 1, 1));
            //Sprites.Add(ESprite.green_combine.ToString(), new SpriteConfig("textures//combine_green.png", 1, 1));
            //Sprites.Add(ESprite.red_combine.ToString(), new SpriteConfig("textures//combine_red.png", 1, 1));
            //Sprites.Add(ESprite.green_mine.ToString(), new SpriteConfig("textures//mine_green.png", 1, 1));
            //Sprites.Add(ESprite.red_mine.ToString(), new SpriteConfig("textures//mine_red.png", 1, 1));
            //Sprites.Add(ESprite.green_cannon.ToString(), new SpriteConfig("textures//cannon_green.png", 1, 1));
            //Sprites.Add(ESprite.red_cannon.ToString(), new SpriteConfig("textures//cannon_red.png", 1, 1));

            ////misha

            //Sprites.Add(ESprite.gridPoint.ToString(), new SpriteConfig("textures//gridPoint.png", 1, 1,-100));
            //Sprites.Add(ESprite.gridPoint2.ToString(), new SpriteConfig("textures//gridPoint1.png", 1, 1,-100));
            //Sprites.Add(ESprite.gridPoint1.ToString(), new SpriteConfig("textures//gridPoint2.png", 1, 1,-100));

            //Sprites.Add(ESprite.stone.ToString(), new SpriteConfig("textures//main//effect3.png", 8, 4,-1));
            //Sprites.Add(ESprite.explosionBig.ToString(), new SpriteConfig("textures//main//explosion6-7.png", 7, 6,20));
            //Sprites.Add(ESprite.explosionNuclear.ToString(), new SpriteConfig("textures//main//beautiful3.png", 6, 5,25));
            //Sprites.Add(ESprite.explosionOut.ToString(), new SpriteConfig("textures//main//explosion3_5.png", 5, 3));
            //Sprites.Add(ESprite.explosionSmall.ToString(), new SpriteConfig("textures//main//explosion6_8.png", 8, 6));
            //Sprites.Add(ESprite.active1.ToString(), new SpriteConfig("textures//main//player1.png", 8, 4,-120));
            //Sprites.Add(ESprite.active2.ToString(), new SpriteConfig("textures//main//player2.png", 8, 4,-120));

            //Sprites.Add(ESprite.tankBox.ToString(), new SpriteConfig("textures//main//units//tank-box.png", 1, 1));
            //Sprites.Add(ESprite.tankGun.ToString(), new SpriteConfig("textures//main//units//tank-gun-2-3.png", 1, 1,10));
            //Sprites.Add(ESprite.cannonBox.ToString(), new SpriteConfig("textures//main//units//cannon-box.png", 1, 1));
            //Sprites.Add(ESprite.cannonGun.ToString(), new SpriteConfig("textures//main//units//cannon-gun.png", 1, 1,10));
            //Sprites.Add(ESprite.armoBox.ToString(), new SpriteConfig("textures//main//units//armo-box.png", 1, 1));
            //Sprites.Add(ESprite.armoGun.ToString(), new SpriteConfig("textures//main//units//armo-gun.png", 1, 1,1));
            //Sprites.Add(ESprite.combine.ToString(), new SpriteConfig("textures//main//units//combine.png", 1, 1));
            //Sprites.Add(ESprite.car.ToString(), new SpriteConfig("textures//main//units//car.png", 1, 1));

            ////второй игрок
            //Sprites.Add(ESprite.tankBox2.ToString(), new SpriteConfig("textures//main//units//tank-box2.png", 1, 1));
            //Sprites.Add(ESprite.tankGun2.ToString(), new SpriteConfig("textures//main//units//tank-gun-2-3-2.png", 1, 1, 10));
            //Sprites.Add(ESprite.cannonBox2.ToString(), new SpriteConfig("textures//main//units//cannon-box2.png", 1, 1));
            //Sprites.Add(ESprite.cannonGun2.ToString(), new SpriteConfig("textures//main//units//cannon-gun2.png", 1, 1, 10));
            //Sprites.Add(ESprite.armoBox2.ToString(), new SpriteConfig("textures//main//units//armo-box2.png", 1, 1));
            //Sprites.Add(ESprite.armoGun2.ToString(), new SpriteConfig("textures//main//units//armo-gun2.png", 1, 1, 1));
            //Sprites.Add(ESprite.combine2.ToString(), new SpriteConfig("textures//main//units//combine2.png", 1, 1));
            //Sprites.Add(ESprite.car2.ToString(), new SpriteConfig("textures//main//units//car2.png", 1, 1));

            ////----

            //Sprites.Add(ESprite.stoneTest.ToString(), new SpriteConfig("textures//stone.png", 1, 1));

            //Sprites.Add(ESprite.tank.ToString(), new SpriteConfig("textures//tank.png", 2, 1));

            //Sprites.Add(EFont.blue.ToString(), new SpriteConfig("fonts/blue.png", 16, 10));
            //Sprites.Add(EFont.blueSmall.ToString(), new SpriteConfig("fonts/blueSmall.png", 16, 10));
            //Sprites.Add(EFont.fiol.ToString(), new SpriteConfig("fonts/fiol.png", 16, 10));
            //Sprites.Add(EFont.fiolBright.ToString(), new SpriteConfig("fonts/fiolBright.png", 16, 10));
            //Sprites.Add(EFont.gold.ToString(), new SpriteConfig("fonts/gold.png", 16, 10));

            //Sprites.Add(EFont.green.ToString(), new SpriteConfig("fonts/green.png", 16, 10));
            //Sprites.Add(EFont.greenBright.ToString(), new SpriteConfig("fonts/greenBright.png", 16, 10));
            //Sprites.Add(EFont.greenSmall.ToString(), new SpriteConfig("fonts/greenSmall.png", 16, 10));
            //Sprites.Add(EFont.orange.ToString(), new SpriteConfig("fonts/orange.png", 16, 10));
            //Sprites.Add(EFont.red.ToString(), new SpriteConfig("fonts/red.png", 16, 10));

            //Keys.Add(EKeyboardAction.Fire, 32);
            //Keys.Add(EKeyboardAction.Unit1, 49);
            //Keys.Add(EKeyboardAction.Unit2, 50);
            //Keys.Add(EKeyboardAction.Unit3, 51);
            //Keys.Add(EKeyboardAction.Unit4, 52);
            //Keys.Add(EKeyboardAction.Unit5, 53);
            //Keys.Add(EKeyboardAction.Esc, 27);
            //Keys.Add(EKeyboardAction.Enter, 13);
            //Keys.Add(EKeyboardAction.left, 37);
            //Keys.Add(EKeyboardAction.up, 38);
            //Keys.Add(EKeyboardAction.right, 39);
            //Keys.Add(EKeyboardAction.down, 40);
            //Keys.Add(EKeyboardAction.I, (int)'i');
            //Keys.Add(EKeyboardAction.O, (int)'o');
        }

        public const string FontLetters = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя!@#$%^&*()_+=,./?<>[]\{}|1234567890~`‘“№→-";



    }
}
