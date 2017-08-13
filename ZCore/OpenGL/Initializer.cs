using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace Framework
{
    class Initializer
    {

        #region public static methods


        public static void SetupViewport(GLControl control)
        {
            //viewport
            double w = Config.ScreenWidth;
            double h = Config.ScreenHeight;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0,
                 control.Width, control.Height); // Use all of the glControl painting area
        }

        public static void LoadTextures(Dictionary<Enum, SpriteList> dict)
        {
            var availableFormats = new string[] { "BMP", "GIF", "EXIG", "JPG", "PNG", "TIFF" };
            var pathList = new List<string>();
            foreach (var format in availableFormats)
            {
                pathList.AddRange(System.IO.Directory.GetFiles(Application.StartupPath, "*." + format.ToLower(), System.IO.SearchOption.AllDirectories));
            }
            //todo потестить большими и маленькими буквами разширение
           
            foreach (var item in dict.ToList()) //todo реализовать поддержку гифок
            {
                var path = pathList
                    .FirstOrDefault(p=> Path.GetFileNameWithoutExtension(p) ==item.Key.ToString());
                if(path == null)
                    throw new Exception(string.Format("Файл с именем {0} не найден в директории приложения и поддиректориях. Разрешенные форматы изображений: BMP, GIF, EXIG, JPG, PNG and TIFF", path));
                
                Vector2d realSize, loadedSize;
                int texNumber = LoadTexture(path, out realSize, out loadedSize);
                item.Value.InitialSize = realSize;
                item.Value.ScaleToPowerOf2 = loadedSize.DivEach(realSize);
                item.Value.OpenglTexture = texNumber;
                dict.Remove(item.Key);
                dict.Add(item.Key, item.Value);
            }
        }

        public static Dictionary<string, int> LoadTexturesOld()
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach (var tex in Config.Sprites)
            {
                Vector2d realSize, loadedSize; //todo что нибудь с этим сделать
                int code = LoadTexture(Config.Sprites[tex.Key].file, out realSize,out loadedSize);
                if (code != -1) res.Add(tex.Key, code);
            }
            return res;
        }
        #endregion

        #region private texture load and make
        public static int LoadTexture(string filename, out Vector2d realSize, out Vector2d loadedSize)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            var powersOf2 = new List<int>{2};
            for(int i =0; i  < 20 ; i++) powersOf2.Add(powersOf2.Last() * 2);

            Bitmap unstretchedBmp = LoadAsArgb(filename);
            realSize = new Vector2d(unstretchedBmp.Width, unstretchedBmp.Height);
            var bmp = ResizeImage(unstretchedBmp,
                powersOf2.Min(x => x < unstretchedBmp.Width ? int.MaxValue : x),
                powersOf2.Min(x => x < unstretchedBmp.Height ? int.MaxValue : x));
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            loadedSize = new Vector2d(bmp.Width, bmp.Height);

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            return id;
        }

        static Bitmap LoadAsArgb(string file)
        {
            //todo cast to 1024
            Bitmap orig = new Bitmap(file);
            Bitmap clone = new Bitmap(orig.Width, orig.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (Graphics gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(orig, new Rectangle(0, 0, clone.Width, clone.Height));
            }
            orig.Dispose();

            return clone;
        }


        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        #endregion
    }
}
