using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace Main.Commands
{
    class Edit
    {
        internal Command command;
        private Bitmap bitmap;
        private byte r, g, b;
        private int[] pixelPos;
        private byte[,,] pixelColors;
        private bool pixLoaded;

        public Edit(Command c)
        {
            command = c;
            pixelPos = new int[2];
        }
        public Bitmap Bitmap
        {
            get => bitmap;
            set => bitmap = value;
        }

        public void Init()
        {
            try
            {
                Bitmap = new Bitmap(command.IPath);
                Pixload();
                TempSave();
            }
            catch
            {
                Console.WriteLine("error 404: The given path is incorrect");
            }
        }
        public void Show()
        {
            try
            {
                System.Diagnostics.Process.Start("temp.jpg");
            }
            catch
            {
                Console.WriteLine("Bitmap was not created");
            }
        }

        public void Blur(string inten)
        {
            if (ThrowExeption(() => !pixLoaded, "Pixels not loaded see --help for help")) return;
            int intensity = int.Parse(inten);
            int sum, rsum = 0, gsum = 0, bsum = 0;
            float[,] matrix = new float[,] { { 1, 4, 7, 4, 1 }, { 4, 16, 26, 16, 4 }, { 7, 26, 41, 26, 7 }, { 4, 16, 26, 16, 4 }, { 1, 4, 7, 4, 1 } };

            foreach (int[] n in Looper())
            {
                for (int i = 0; i < 3 + 2 * (intensity - 1); i++)
                {
                    for (int j = 0; j < 3 + 2 * (intensity - 1); j++)
                    {

                        int x = n[0] - intensity - 1;
                        int y = n[1] - intensity - 1;

                        if (y + i > 0 && x + j > 0 && y + i < bitmap.Height - 1 && x + j < bitmap.Width - 1)
                        {
                            byte r = pixelColors[y + i, x + j, 0];
                            byte g = pixelColors[y + i, x + j, 1];
                            byte b = pixelColors[y + i, x + j, 2];

                            rsum += r;
                            gsum += g;
                            bsum += b;
                        }
                    }

                }
                sum = 3 + 2 * (intensity - 1);
                sum *= sum;

                Bitmap.SetPixel(n[0], n[1], Color.FromArgb(rsum / sum, gsum / sum, bsum / sum));
                //Cleaning up
                rsum = 0;
                gsum = 0;
                bsum = 0;
                pixLoaded = false;
            }


            TempSave();

        }
        public void Pixload()
        {
            pixelColors = new byte[bitmap.Height, bitmap.Width, 3];
            for (int y = 0; y < bitmap.Height; y++)
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    //R
                    pixelColors[y, x, 0] = c.R;
                    //G
                    pixelColors[y, x, 1] = c.G;
                    //B
                    pixelColors[y, x, 2] = c.B;
                }
            pixLoaded = true;
        }

        public void Gscal()
        {
            if (ThrowExeption(() => bitmap == null && !pixLoaded, "Bitmap was not created see documentation for help")) return;
            foreach (int[] n in Looper())
            {
                int sum = (r + b + g) / 3;
                Bitmap.SetPixel(n[0], n[1], Color.FromArgb(sum, sum, sum));
            }
            TempSave();
        }
        public void Negim(string value)
        {
            if (ThrowExeption(() => bitmap == null && !pixLoaded, "Bitmap was not created see documentation for help")) return;
            float f = float.Parse(value, CultureInfo.InvariantCulture);
            f = f > 1 || f < 0 ? 1 : f;
            foreach (int[] n in Looper())
            {
                int sum = (int)(f * 255);
                Bitmap.SetPixel(n[0], n[1], Color.FromArgb(Math.Abs(sum - r), Math.Abs(sum - g), Math.Abs(sum - b)));
            }
            TempSave();
        }
        private IEnumerable<int[]> Looper()
        {

            for (int y = 0; y < Bitmap.Height; ++y)
                for (int x = 0; x < Bitmap.Width; ++x)
                {

                    //R
                    r = pixelColors[y, x, 0];
                    //G
                    g = pixelColors[y, x, 1];
                    //B
                    b = pixelColors[y, x, 2];

                    pixelPos[0] = x;
                    pixelPos[1] = y;

                    yield return pixelPos;
                }

            r = 0;
            g = 0;
            b = 0;

        }
        public void Bwhite()
        {

            if (ThrowExeption(() => bitmap == null && !pixLoaded, "Bitmap was not created see documentation for help")) return;
            foreach (int[] n in Looper())
            {
                byte sum = 0;
                if (765 - (r + g + b) < r + g + b) sum = 255;
                Bitmap.SetPixel(n[0], n[1], Color.FromArgb(sum, sum, sum));
            }
            TempSave();
        }

        private void TempSave()
        {
            bitmap.Save("temp.jpg");
        }

        delegate bool checkExeption();

        private bool ThrowExeption(checkExeption cex, string extxt)
        {
            Console.WriteLine(cex());
            if (cex())
            {
                Console.WriteLine(extxt);
                return true;
            }
            return false;
        }

    }

}
