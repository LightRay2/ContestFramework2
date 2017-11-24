using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace GifTransformer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog { Filter = "GIF|*.gif" };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            Image gif = Image.FromFile(fileDialog.FileName);
            FrameDimension dim = new FrameDimension(gif.FrameDimensionsList[0]);
            int frames = gif.GetFrameCount(dim);
            int framesInRow = Enumerable.Range(1, frames).First(x => x * x >= frames);
            Bitmap resultingImage = new Bitmap(gif.Width * 6, gif.Height*5);

            for (int i = 0; i < frames; i++)
            {
                gif.SelectActiveFrame(dim, i);
                
                Rectangle destRegion = new Rectangle(gif.Width * (i% framesInRow), gif.Height*(i/ framesInRow), gif.Width, gif.Height);
                Rectangle srcRegion = new Rectangle(0, 0, gif.Width, gif.Height);

                using (Graphics grD = Graphics.FromImage(resultingImage))
                {
                    grD.CompositingMode = CompositingMode.SourceCopy;
                    grD.CompositingQuality = CompositingQuality.HighQuality;
                    grD.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    grD.SmoothingMode = SmoothingMode.HighQuality;
                    grD.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    var bmp = new Bitmap(gif);
                    for (int row = 0; row < gif.Height; row++)
                    {
                        for (int col = 0; col < gif.Width; col++)
                        {
                            var pixel = bmp.GetPixel(col, row);
                            bmp.SetPixel(col,row, TranslatePixel(pixel, col, row, gif.Width, gif.Height));
                        }
                    }
                    grD.DrawImage(bmp, destRegion, srcRegion, GraphicsUnit.Pixel);

                   // var g = Graphics.FromImage(gif);
                   // g.DrawImage(bmp, new Point());
                }

            }



            pictureBox1.Image = resultingImage;
            resultingImage = ResizeImage(resultingImage, int.Parse(comboBox1.Text), int.Parse(comboBox1.Text));


            resultingImage.Save( fileDialog.FileName.Substring(0,fileDialog.FileName.Length-3)+"png", ImageFormat.Png);
            MessageBox.Show("Сохранено в той же папке. Кадров по горизонтали: " + framesInRow + ", по вертикали: " + Math.Ceiling(frames * 1.0 / framesInRow)+". Исходный размер: "+gif.Width+" x "+gif.Height);
        }

        Color TranslatePixel(Color pixel, int x, int y, int width, int height)
        {
            double close = 0.1;

            double hor = (double)x/width, vert = (double)y/height;

            if (x > width / 2) hor = 1 - hor;
            if (y > height / 2) vert = 1 - vert;

            var realClose = Math.Min(hor, vert);
            if (realClose < 0) realClose = 0;

            double mult = realClose < close ? (realClose / close ) : 0.9999999;

            if(pixel.B + pixel.R + pixel.G > 200)
                return Color.FromArgb((int)(254*(mult+0.0000001)), pixel);
            double a = (pixel.B + pixel.R + pixel.G) / 200.0;
            if (a < 0) a = 0;
            int alpha = (int)(a* mult* 254);
            
            return Color.FromArgb(alpha, pixel);

        }

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

      //  public static Image gif = Image.FromFile("image.gif"); 
        private void Form1_Load(object sender, EventArgs e)
        {
         //   pictureBox1.Image = gif; 
            //pictureBox1.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var res = gif.Clone();
            
        }
    }
}
