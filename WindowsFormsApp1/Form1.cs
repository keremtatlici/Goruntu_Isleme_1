using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;

        OpenFileDialog fd = new OpenFileDialog();

        int[] HistogramRed = new int[256];
        int[] HistogramGreen = new int[256];
        int[] HistogramBlue = new int[256];
        int[] HistogramRedK = new int[256];
        int[] HistogramGreenK = new int[256];
        int[] HistogramBlueK = new int[256];
        int[] YüzdelikRed = new int[256];
        int[] YüzdelikGreen = new int[256];
        int[] YüzdelikBlue = new int[256];
        int Çözünürlük;

        String a = "";
        private Bitmap _current;
        

        public Form1()
        {
            InitializeComponent();
            button1.Text = "Bir Resim Seçiniz";
            button2.Text = "Siyah-Beyaz -->";
            button3.Text = "Negatif -->";
            button6.Text = "Sola Çevir -->";
            button7.Text = "Sağa Çevir -->";
            button8.Text = "Histogram Uygula -->";
            button9.Text = "Parlaklık Ayarla -->";
            button10.Text = "Ters Çevir -->";
            button11.Text = "Aynalama -->";
            button12.Text = "Resmi Sıfırla ";
            button13.Text = "Boyutlama -->";

            
        }

        private void button1_Click(object sender, EventArgs e)//resim seç
        {
            

            fd.Filter = " (*.jpg)|*.jpg|(*.png)|*.png";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = new Bitmap(fd.OpenFile());
            }
        }

        private void button2_Click(object sender, EventArgs e)//Siyah-Beyaz
        {
            SiyahBeyaz();
            this.pictureBox2.Image = new Bitmap(bmp);
        }

        private void button3_Click(object sender, EventArgs e)//Negatif
        {
            Negatif();
            this.pictureBox2.Image = new Bitmap(bmp);
        }
        private void button6_Click(object sender, EventArgs e)//Sola Çevir
        {
            this.bmp = new Bitmap(fd.OpenFile());
            Bitmap b = new Bitmap(bmp.Height, bmp.Width);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    b.SetPixel(j, b.Height - 1 - i, bmp.GetPixel(i, j));

                }
            }          
            this.pictureBox2.Image = new Bitmap(b);

        }

        private void button7_Click(object sender, EventArgs e)//Sağa Çevir
        {
            this.bmp = new Bitmap(fd.OpenFile());
            Bitmap b = new Bitmap(bmp.Height, bmp.Width);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    b.SetPixel(b.Width - j - 1, i, bmp.GetPixel(i, j));
                }
            }
            this.pictureBox2.Image = new Bitmap(b);
        }
        private void button8_Click(object sender, EventArgs e)//Histogram Uygula
        {
            this.bmp = new Bitmap(fd.OpenFile());
            Bitmap renderedImage = this.bmp;

            uint pixels = (uint)renderedImage.Height * (uint)renderedImage.Width;
            decimal Const = 255 / (decimal)pixels;

            int x, y, R, G, B;


            int[] HistogramRed2 = new int[256];
            int[] HistogramGreen2 = new int[256];
            int[] HistogramBlue2 = new int[256];


            for (var i = 0; i < renderedImage.Width; i++)
            {
                for (var j = 0; j < renderedImage.Height; j++)
                {
                    var piksel = renderedImage.GetPixel(i, j);

                    HistogramRed2[(int)piksel.R]++;
                    HistogramGreen2[(int)piksel.G]++;
                    HistogramBlue2[(int)piksel.B]++;

                }
            }

            int[] cdfR = HistogramRed2;
            int[] cdfG = HistogramGreen2;
            int[] cdfB = HistogramBlue2;

            for (int r = 1; r <= 255; r++)
            {
                cdfR[r] = cdfR[r] + cdfR[r - 1];
                cdfG[r] = cdfG[r] + cdfG[r - 1];
                cdfB[r] = cdfB[r] + cdfB[r - 1];
            }

            for (y = 0; y < renderedImage.Height; y++)
            {
                for (x = 0; x < renderedImage.Width; x++)
                {
                    Color pixelColor = renderedImage.GetPixel(x, y);

                    R = (int)((decimal)cdfR[pixelColor.R] * Const);
                    G = (int)((decimal)cdfG[pixelColor.G] * Const);
                    B = (int)((decimal)cdfB[pixelColor.B] * Const);

                    Color newColor = Color.FromArgb(R, G, B);
                    renderedImage.SetPixel(x, y, newColor);
                }
            }
            this.pictureBox2.Image = renderedImage;
        }

        private void button9_Click(object sender, EventArgs e)//Parlaklık Ayarla
        {
            this.bmp = new Bitmap(fd.OpenFile());
            int brightness = int.Parse(textBox1.Text);
            Bitmap temp = (Bitmap)_current;
            Bitmap bmap = this.bmp;
            if (brightness < -255) brightness = -255;
            if (brightness > 255) brightness = 255;
            Color col;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    col = bmap.GetPixel(i, j);
                    int colRed = col.R + brightness;
                    int colGreen = col.G + brightness;
                    int colBlue = col.B + brightness;

                    if (colRed < 0) colRed = 1;
                    if (colRed > 255) colRed = 255;

                    if (colGreen < 0) colGreen = 1;
                    if (colGreen > 255) colGreen = 255;

                    if (colBlue < 0) colBlue = 1;
                    if (colBlue > 255) colBlue = 255;

                    bmap.SetPixel(i, j, Color.FromArgb((byte)colRed, (byte)colGreen, (byte)colBlue));
                }
            }
            _current = (Bitmap)bmap.Clone();
            this.pictureBox2.Image = new Bitmap(_current);
        }

        private void button10_Click(object sender, EventArgs e)//Ters Çevir
        {
            this.bmp = new Bitmap(fd.OpenFile());
            Bitmap b = new Bitmap(bmp.Height, bmp.Width);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    b.SetPixel(j, b.Height - 1 - i, bmp.GetPixel(i, j));

                }
            }
            this.bmp = new Bitmap(b);
            Bitmap ba = new Bitmap(bmp.Height, bmp.Width);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    ba.SetPixel(j, ba.Height - 1 - i, bmp.GetPixel(i, j));

                }
            }

            this.pictureBox2.Image = new Bitmap(ba);

        }
        private void button11_Click(object sender, EventArgs e)//Aynalama
        {
            int OrtR, OrtG, OrtB;
            Bitmap A = new Bitmap(fd.OpenFile());
            Bitmap B = new Bitmap(fd.OpenFile());
            for (int j = 0; j < A.Height; j++)
                for (int k = 0; k < A.Width; k++)
                {
                    OrtR = A.GetPixel(k, j).R;
                    OrtG = A.GetPixel(k, j).G;
                    OrtB = A.GetPixel(k, j).B;
                    B.SetPixel(A.Width - k - 1, j, Color.FromArgb(OrtR, OrtG, OrtB));
                }
            pictureBox2.Image = B;

        }

        private void button12_Click(object sender, EventArgs e)//Resmi Sıfırla
        {
            this.bmp = new Bitmap(fd.OpenFile());
            this.pictureBox2.Image = new Bitmap(bmp);
        }

        private void button13_Click(object sender, EventArgs e)//Boyutlama
        {
            int genis = int.Parse(textBox2.Text);
            int a = genis;
            int b = a;
            this.bmp = new Bitmap(fd.OpenFile());
            this.bmp = new Bitmap(ResizeBitmap(this.bmp,a,b));
            this.pictureBox2.Image = new Bitmap(bmp);


        }
        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

        public Image SiyahBeyaz()
        {
            this.bmp = new Bitmap(fd.OpenFile());
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int hede = (bmp.GetPixel(i, j).R + bmp.GetPixel(i, j).G + bmp.GetPixel(i, j).B) / 3;

                    bmp.SetPixel(i, j, Color.FromArgb(hede, hede, hede));
                }
            }
            return bmp;
        }

        public Image Negatif()
        {
            this.bmp = new Bitmap(fd.OpenFile());
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmp.SetPixel(i, j, Color.FromArgb(255 - bmp.GetPixel(i, j).R, 255 - bmp.GetPixel(i, j).G, 255 - bmp.GetPixel(i, j).B));
                }
            }
            return bmp;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
