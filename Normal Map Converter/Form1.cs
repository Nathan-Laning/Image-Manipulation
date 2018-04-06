using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Normal_Map_Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
            RESOLUTION_Slider.Enabled = false;
            button1.Enabled = false;
            Generate.Enabled = false;
        }
        Bitmap loadedImage;
        Bitmap newImage;
        MAP NORMAL;
        int red,green,blue;
        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.gif;*.png*;*.tiff*)";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (openFileDialog1.OpenFile() != null)
                    {
                        loadedImage = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                        pictureBox2.Image = loadedImage;
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                        NORMAL = new MAP(loadedImage);
                        NORMAL.GetBMP();
                        newImage = NORMAL.GetBMP();
                        pictureBox1.Image = newImage;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        RESOLUTION_Slider.Enabled = true;
                        button1.Enabled = true;
                        Generate.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Quit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void SaveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.gif;*.png*;*.tiff*)";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                newImage.Save(dialog.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NORMAL.AVE = Color.FromArgb(red, green, blue);
            NORMAL.GetBMP();
            newImage = NORMAL.GetBMP();
            pictureBox1.Image = newImage;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //byte[] b = new byte[] {NORMAL.AVE.A, NORMAL.AVE.R, NORMAL.AVE.G, NORMAL.AVE.B };
            //object a = NORMAL.AVE.R;
            //object b = NORMAL.AVE.G;
            //object c = NORMAL.AVE.B;
            Color c = Color.FromArgb(122, 123, 124);
            object[] o = new object[] { NORMAL.AVE.R, NORMAL.AVE.G, NORMAL.AVE.B };
            AVERAGE.Text = String.Format("{0},{1},{2}", o);
        }

     

        private void label2_Click(object sender, EventArgs e)
        {

        }
        /**
        * color sliders for Average
        **/
        private void red_Scroll(object sender, EventArgs e)
        {
            RED_TextBox.Text = RED_Slider.Value.ToString();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int temp = validateInput(RED_TextBox.Text);
            if (temp != -1)
            {
                RED_Slider.Value = temp;
                red = temp;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int temp = validateInput(BLUE_TextBox.Text);
            if (temp != -1)
            {
                BLUE_Slider.Value = temp;
                blue = temp;
            }
        }

        private int validateInput(string s)
        {
            try { 
            int val = int.Parse(s);
            if (val >= 0 && val <= 255)
            {
                return val;
                }else
                {
                    return -1;
                }
            }
            catch(Exception e) {
                return -1;
            }
        }

        private void GREEN_TextBox_TextChanged(object sender, EventArgs e)
        {
            
            int temp = validateInput(GREEN_TextBox.Text);
            if(temp != -1)
            {
                GREEN_Slider.Value =temp;
                green = temp;
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            NORMAL.findAverage();
            RED_TextBox.Text = NORMAL.AVE.R.ToString();
            GREEN_TextBox.Text = NORMAL.AVE.G.ToString();
            BLUE_TextBox.Text = NORMAL.AVE.B.ToString();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void RESOLUTION_Slider_Scroll(object sender, EventArgs e)
        {
            NORMAL.RESOLUTION = 10*RESOLUTION_Slider.Value;
        }

        private void BLUE_Slider_Scroll(object sender, EventArgs e)
        {
            BLUE_TextBox.Text = BLUE_Slider.Value.ToString();
        }

        private void GREEN_Slider_Scroll(object sender, EventArgs e)
        {
            GREEN_TextBox.Text = GREEN_Slider.Value.ToString();
        }
    }
    public class MAP
        {
            public int RESOLUTION=10;
            public Color AVE;
        public int RANGE = 3;
            private int height, width;
        private Color[,] Pixels;
        protected Color[,] Original_Pixels;
        protected Bitmap newImage, orignalImage;
            public MAP(Bitmap originalImage)
            {
                height = originalImage.Height;
                width = originalImage.Width;
                orignalImage = originalImage;
                Pixels = new Color[width, height];
                Original_Pixels = new Color[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Pixels[i, j] = originalImage.GetPixel(i, j);
                        Original_Pixels[i, j] = Pixels[i, j];
                }
                }
                newImage = new Bitmap(width, height);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        newImage.SetPixel(i, j, Color.FromArgb(128, 128, 255));
                    }
                }
                findAverage();

            }

            public void findAverage()
            {
                int BLUE = 0, RED = 0, GREEN = 0, ALPHA = 0;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        ALPHA += Original_Pixels[i, j].A;
                        BLUE += Original_Pixels[i, j].B;
                        RED += Original_Pixels[i, j].R;
                        GREEN += Original_Pixels[i, j].G;
                    }
                }
                int TOTAL = height * width;
                AVE = Color.FromArgb(ALPHA / TOTAL, RED / TOTAL, GREEN / TOTAL, BLUE / TOTAL);
            }

            /**
             * Generates Normal Map with all variables, variables are changeable in
             * other methods  
             * 
             *  X: -1 to +1 :  Red: 0 to 255
             *  Y: -1 to +1 :  Green: 0 to 255
             *  Z: 0 to -1 :  Blue: 128 to 255
             *  
             **/
            private void generate()
            {
                    for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                    Color[,] A = new Color[RANGE, RANGE];
                        for(int w = 0; w < RANGE; w++)
                        {
                            for( int l = 0; l < RANGE; l++)
                            {
                            try
                            {
                                A[w, l] = Original_Pixels[i - ((RANGE - 1) / 2) + w, j - ((RANGE - 1) / 2) + l];
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                A[w, l] = Color.FromArgb(128, 128, 255);
                            }
                        }
                        }
                        Pixels[i, j] = alter(A);
                }
                }
            }

        private Color cloneColor(Color c)
        {
            return Color.FromArgb(c.ToArgb());
        }
        /**
         * Alter
         * Alters the point based on points around it to indicate the direction of light 
         * then determines the intensity 
         * */
        private Color alter(Color[,] c)
        {
            int[,] intensity = new int[RANGE, RANGE];
            int max = 0;
            int[] xy = new int[]{ 0, 0 };
            for (int i = 0; i < RANGE; i++)
            {
                for(int j = 0; j < RANGE; j++)
                {
                    intensity[i, j] = compare(c[i, j], AVE);
                    if (i != ((RANGE - 1) / 2) && j!=((RANGE - 1) / 2))
                    {
                        int space = Math.Abs(intensity[i, j]);
                        if(max < space)
                        {
                            max = space; xy = new int[]{ i,j};
                        }
                    }
                }
            }

            if (max <= RESOLUTION) { return Color.FromArgb(128,128, 255);}
            if (intensity[xy[0], xy[1]] < 0) {
                for (int i = 0; i < 2; i++)
                {
                    if (xy[i] == 0) { xy[i] = 2; }
                    if (xy[i] == 2) { xy[i] = 0; }
                }
            }
            int red = 0, green = 0;
            for (int k = 0; k < (intensity[xy[0], xy[1]])/3; k += RESOLUTION)
            {
                for (int i = 0; i < xy[0]; i++)
                {
                    red += RESOLUTION;
                }
                for (int i = 0; i < xy[1]; i++)
                {
                    green += RESOLUTION;
                }
            }
            return Color.FromArgb(red, green,128); ;
            }


            private int compare(Color a, Color b)
            {
                return (a.B + a.R + a.G) - (b.B + b.R + b.G);
            }
            /**
             * GetBMP - Get Bitmap Image to display on screen
             * converts color array to bitmap, then returns it
             * 
             **/
            public Bitmap GetBMP()
            {
                generate();

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        newImage.SetPixel(i, j, Pixels[i, j]);
                    }
                }
                return newImage;
            }
        }

      
    }