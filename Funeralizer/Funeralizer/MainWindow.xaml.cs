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
        [DllImport("FuneralizerASM.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void funeralize_asm(int* rgb, int rgbSize);
       

        [DllImport("FuneralizerCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void funeralize_cpp(int* rgb, int rgbSize);

        //String array with times of execution
        string[] times = new string[10];

        //helper to make code nicer :)
        unsafe public delegate void funeralizerDelegate(int* rgb, int rgb_size);
        unsafe public double RunFuneralizer(funeralizerDelegate funeralizingFunction, int[] rgb)
        {
            double totalTime = 0;
            var rgbLength = rgb.Length;
            int[][] rgbArray = new int[10][];
            for (int i = 0; i < 10; ++i)
            {
                rgbArray[i] = (int[])rgb.Clone();

            }

            var execTime = Stopwatch.StartNew();
            for (int i = 0; i < 10; ++i)
            {
                execTime.Restart();
                fixed (int* rgbPtr = &rgbArray[i][0])
                {
                    funeralizingFunction(rgbPtr, rgbLength);
                }
                execTime.Stop();
                totalTime += execTime.Elapsed.TotalMilliseconds;
                times[i] = execTime.Elapsed.TotalMilliseconds.ToString();
            }
            return totalTime;
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
            if (!cbxAsm.IsChecked.Value && !cbxCpp.IsChecked.Value)
            {
                MessageBox.Show("Please select at least one option.");
                return;
            }
            string message = "Average time of 10 executions:\n";
            double averageTime = 0;
            if (cbxAsm.IsChecked.Value)
            {
                Bitmap asmBitmap;
                unsafe
                {
                    int[] rgb = BitmapIntArray(filePath);
                    int rgbSize = bmp.Height * bmp.Width * 3;

                    int[] rgbAsm = (int[])rgb.Clone();

                    fixed (int* p = rgbAsm)
                    {
                        funeralize_asm(p, rgbSize);
                    }
                    double asmTotalTime = RunFuneralizer(funeralize_asm, rgb);
                    averageTime = asmTotalTime / 10;
                    asmBitmap = IntArrayBitmap(rgbAsm, bmp.Width, bmp.Height);
                    message += "ASM: " + averageTime + " ms\n";                    
                    OpenFolderDialog save = new OpenFolderDialog();
                    save.Title = "Save the Image File (ASM)";
                    if (save.ShowDialog() == true)
                    {
                        for (int i = 0; i < 10; ++i)
                            File.AppendAllText(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_asm.txt", times[i] + " ms\n");
                        asmBitmap.Save(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_asm.png");
                        asmBitmap.Dispose();
                        imgPhotoGreyscaleAsm.Source = new BitmapImage(new Uri(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_asm.png"));
                    }
                }
            }
            if (cbxCpp.IsChecked.Value)
            {
                Bitmap cppBitmap;
                unsafe
                {
                    int[] rgb = BitmapIntArray(filePath);
                    int rgbSize = bmp.Height * bmp.Width * 3;

                    int[] rgbCpp = (int[])rgb.Clone();

                    fixed (int* p = rgbCpp)
                    {
                        funeralize_cpp(p, rgbSize);
                    }
                    double cppTotalTime = RunFuneralizer(funeralize_cpp, rgb);
                    averageTime = cppTotalTime / 10;
                    cppBitmap = IntArrayBitmap(rgbCpp, bmp.Width, bmp.Height);
                    message += "CPP: " + averageTime + " ms\n";
                    OpenFolderDialog save = new OpenFolderDialog();
                    save.Title = "Save the Image File (C++)";
                    if (save.ShowDialog() == true)
                    {
                        for (int i = 0; i < 10; ++i)
                            File.AppendAllText(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_cpp.txt", times[i] + " ms\n");
                        cppBitmap.Save(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_cpp.png");
                        cppBitmap.Dispose();
                        imgPhotoGreyscaleCpp.Source = new BitmapImage(new Uri(save.FolderName + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_cpp.png"));
                    }
                }
            }

            MessageBox.Show(message);
            
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
