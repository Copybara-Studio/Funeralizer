using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Funeralizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*
        [DllImport("FuneralizerASM.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void funeralizeAsm(int* rgb, int rgbSize);
        */

        [DllImport("FuneralizerCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void funeralize_cpp(int* rgb, int rgbSize);

        //helper to make code nicer :)
        unsafe public delegate void funeralizerDelegate(int* rgb, int rgb_size);
        unsafe public double RunFuneralizer(funeralizerDelegate funeralizingFunction, int[] rgb)
        {
            var rgbLength = rgb.Length;
            int[][] rgbArray = new int[10][];
            for (int i = 0; i < 10; ++i)
            {
                rgbArray[i] = (int[])rgb.Clone();

            }

            var execTime = Stopwatch.StartNew();
            for (int i = 0; i < 10; ++i)
            {
                fixed (int* rgbPtr = &rgbArray[i][0])
                {
                    funeralizingFunction(rgbPtr, rgbLength);
                }
            }
            execTime.Stop();
            return execTime.ElapsedMilliseconds;
        }

        //path to the image file
        private string filePath;
        //bitmap to store the original image and get access to its height and width
        Bitmap bmp;
        public MainWindow()
        {
            InitializeComponent();
            filePath = "";
            bmp = new Bitmap(1, 1);
        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.png;*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|";
            if (op.ShowDialog() == true)
            {
                filePath = op.FileName;
                imgPhoto.Source = new BitmapImage(new Uri(filePath));
                bmp = new Bitmap(filePath);
            }
        }
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (filePath == "")
            {
                MessageBox.Show("Please select a picture first.");
                return;
            }
            Bitmap asmBitmap;
            Bitmap cppBitmap;
            double asmAvgTime;
            double cppAvgTime;
            unsafe
            {
                int[] rgb = BitmapIntArray(filePath);
                int rgbSize = bmp.Height * bmp.Width * 3;

                //raczej tymczasowe 
                int[] rgbCpp = (int[])rgb.Clone();
                int[] rgbAsm = (int[])rgb.Clone();

                fixed (int* p = rgbCpp)
                {
                    funeralize_cpp(p, rgbSize);
                }
                /*
                fixed (int* p = rgbAsm)
                {
                    funeralizeAsm(p, rgbSize);
                }
                */
                /*double asmTotalTime = RunFuneralizer(funeralizeAsm, rgb);*/
                double cppTotalTime = RunFuneralizer(funeralize_cpp, rgb);

                asmBitmap = IntArrayBitmap(rgb/*Asm*/, bmp.Width, bmp.Height); //asmRgb in place of rgb
                asmAvgTime = 0/*asmTotalTime / 10*/;

                cppBitmap = IntArrayBitmap(rgbCpp, bmp.Width, bmp.Height); //cppRgb in place of rgb
                cppAvgTime = cppTotalTime / 10;

            }


            MessageBox.Show("Average time of 10 executions:\n" +
                            "ASM: " + asmAvgTime + "ms\n" +
                            "CPP: " + cppAvgTime + "ms");
            OpenFolderDialog save = new OpenFolderDialog();
            save.Title = "Save the Image Files";
            if (save.ShowDialog() == true)
            {
                asmBitmap.Save(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_asm.png");
                cppBitmap.Save(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_cpp.png");
                imgPhotoGreyscaleAsm.Source = new BitmapImage(new Uri(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_asm.png"));
                imgPhotoGreyscaleCpp.Source = new BitmapImage(new Uri(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_cpp.png"));
            }


            //SaveFileDialog asm = new SaveFileDialog();
            //asm.Title = "Save an Image File from ASM";
            //asm.Filter = "Portable Network Graphic (*.png)|*.png" +
            //             "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|";
            //asm.AddExtension = true;
            //asm.DefaultExt = "png";
            //if (asm.ShowDialog() == true)
            //{
            //    asmBitmap.Save(asm.FileName);
            //    imgPhotoGreyscaleAsm.Source = new BitmapImage(new Uri(asm.FileName));
            //}

            //SaveFileDialog cpp = new SaveFileDialog();
            //cpp.Title = "Save an Image File from CPP";
            //cpp.Filter = "Portable Network Graphic (*.png)|*.png" +
            //             "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|";
            //cpp.AddExtension = true;
            //cpp.DefaultExt = "png";
            //if (cpp.ShowDialog() == true)
            //{
            //    asmBitmap.Save(cpp.FileName);
            //    imgPhotoGreyscaleCpp.Source = new BitmapImage(new Uri(cpp.FileName));
            //}
        }

        public int[] BitmapIntArray(string filePath)
        {
            int[] rgb = new int[bmp.Width * bmp.Height * 3];
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixelColor = bmp.GetPixel(j, i);
                    rgb[(i * bmp.Width + j) * 3] = pixelColor.R;
                    rgb[(i * bmp.Width + j) * 3 + 1] = pixelColor.G;
                    rgb[(i * bmp.Width + j) * 3 + 2] = pixelColor.B;
                }
            }
            return rgb;
        }

        public Bitmap IntArrayBitmap(int[] rgb, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            int index = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bmp.SetPixel(j, i, Color.FromArgb(rgb[index], rgb[index + 1], rgb[index + 2]));
                    index += 3;
                }
            }
            return bmp;
        }
    }

}
