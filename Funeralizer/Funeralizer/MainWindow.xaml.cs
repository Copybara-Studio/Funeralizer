using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;


namespace Funeralizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath;
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
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
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
            int[] rgb = BitmapIntArray(filePath);
            Bitmap bmp1 = IntArrayBitmap(rgb, bmp.Width, bmp.Height);
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PNG Image|*.png";
            save.Title = "Save an Image File";
            if (save.ShowDialog() == true)
            {
                bmp1.Save(save.FileName);
            }
            imgPhotoGreyscale.Source = new BitmapImage(new Uri(save.FileName));
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

