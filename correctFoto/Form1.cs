using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace correctFoto
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        public Form1()
        {
            InitializeComponent();
        }
        IntPtr thisWindow;
        Point LocationOfRenctagle;
        Point newLoc;
        Point newLoc2;
        Bitmap currentImage;
        Color mainColor = Color.Red;
        Pen pn = new Pen(HsvToRgb(0, 1f, 1f), 2);
        Bitmap cutImage;
        PictureBox pb;
        FileInfo[] images;
        int positionOfImage = 0;
        int h = 0;
        bool isDrawingNow = false;
        bool startDrawRenctagle = false;
        private void button1_Click(object sender, EventArgs e)
        {
            
            
            
            string FullName = "";
            try
            {
                DirectoryInfo df = new DirectoryInfo(textBox1.Text);

                // images = df.GetFiles().Where(p=>(p.Extension=="jpg"||p.Extension=="png"||p.Extension=="jpeg"||p.Extension=="tiff"||p.Extension=="gif"));

                images = df.GetFiles().Where(p => (p.Extension == ".jpg" || p.Extension == ".png" || p.Extension == ".jpeg" || p.Extension == ".tiff" || p.Extension == ".gif" || p.Extension == ".bmp" || p.Extension == ".eps" || p.Extension == ".pcx" || p.Extension == ".raw" || p.Extension == ".ico")).ToArray();

                positionOfImage = 0;
                progressBar1.Maximum = images.Count();
                progressBar1.Value = positionOfImage+1;
                label9.Text = (positionOfImage + 1) + @"/" + progressBar1.Maximum;

                FullName = images[positionOfImage].FullName;

                



                if (File.Exists(FullName))
                {
                    pb = new PictureBox();
                    pb.Load(FullName);
                    pictureBox1.Image = new Bitmap(pb.Image);
                    AllButtonsEnabled(true);

                }
               
                label5.Text = images[positionOfImage].Name;
                currentImage = (Bitmap)pictureBox1.Image.Clone();
                cutImage = (Bitmap)pictureBox1.Image.Clone();

                textBox2.Text = pictureBox1.Image.Width.ToString();          //////////ОТКРЫВАЕМ ПРИ ЗАГРУЗКЕ
                textBox3.Text = pictureBox1.Image.Height.ToString();
                label10.Text = "Загружено!";
            }
            catch(Exception ee)
            {
                label9.Text = "";
                MessageBox.Show("Папка пуста", "Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
                label10.Text = "Готово к работе!";
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                timer1.Stop();
                label10.Text = "Готово к работе!";
                //FullName = images[positionOfImage].FullName;
                Image imag;
                isDrawingNow = false;
                pictureBox1_MouseMove(sender, e);

                imag = (Image)cutImage.Clone();

                pictureBox1.Image = (Image)imag.Clone();///
                textBox2.Text = pictureBox1.Image.Width.ToString();         
                textBox3.Text = pictureBox1.Image.Height.ToString();
                imag.Dispose();
                pictureBox1.Invalidate();

            }
            catch (Exception ee)
            {
                MessageBox.Show("Ошибка при отпускании мыши" + ee.Message, "Oшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
               thisWindow = FindWindow(null, "Form1");
                RegisterHotKey(thisWindow, 1, (uint)0x0000, (uint)Keys.A);
                RegisterHotKey(thisWindow, 2, (uint)0x0000, (uint)Keys.D);
                RegisterHotKey(thisWindow, 3, (uint)0x0000, (uint)Keys.S);
                RegisterHotKey(thisWindow, 4, (uint)0x0000, (uint)Keys.Space);

                currentImage = (Bitmap)pictureBox1.Image.Clone();
            }
            catch (Exception)
            {

            }
            this.MaximumSize = new Size(this.Width, this.Height);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string FullName = "";

            try
            {
                positionOfImage--;
                if (positionOfImage == (-1))   positionOfImage = images.Length - 1;
                FullName = images[positionOfImage].FullName;

                if (File.Exists(FullName))
                {
                    pb = new PictureBox();
                    pb.Load(FullName);
                    pictureBox1.Image = new Bitmap(pb.Image);
                }

                label5.Text = images[positionOfImage].Name;
                textBox2.Text = pictureBox1.Image.Width.ToString();
                textBox3.Text = pictureBox1.Image.Height.ToString();
                currentImage = (Bitmap)pictureBox1.Image.Clone();
                cutImage = (Bitmap)pictureBox1.Image.Clone();
                pictureBox1.Invalidate();
                progressBar1.Value = positionOfImage + 1;
                label9.Text = (positionOfImage + 1) + @"/" + progressBar1.Maximum;
                label10.Text = "Загружено!";
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось загрузить новый файл" + ee.Message + " Имя файла: " + FullName, "Oшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label10.Text = "Готово к работе!";
            }


          



        }

        private void button3_Click(object sender, EventArgs e)
        {
            //using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            //{
                
            //    g.Clear(Color.FromArgb(0, 255, 255, 255));
            //    pictureBox1.Invalidate();
            //}
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (checkBox1.Checked)
            {
                h++;
                if (h > 360) h = 0;

                pn = new Pen(HsvToRgb(h, 1f, 1f), 2);
            }
            

            //label6.Text = "Mouse: x= " + MousePosition.X.ToString() + "  y= " + MousePosition.Y.ToString();
            //label7.Text = "PB.loc: x= " + pictureBox1.Location.X + "  " + "y= " + pictureBox1.Location.Y+"     PBsize: x="+pictureBox1.Width+" y= "+pictureBox1.Height; //"Cursor: x= "+Cursor.Position.X+" y= "+Cursor.Position.Y;
            //label8.Text = "E.x=" + e.Location.X + " e.y= " + e.Location.Y ;

            try {

                if (isDrawingNow)
                {
                    int x = e.Location.X;
                    int y = e.Location.Y;
                    if (e.Location.X > pictureBox1.Image.Width)
                    {
                        x = pictureBox1.Image.Width;
                        Cursor.Position = new Point(x + pictureBox1.Location.X, Cursor.Position.Y);
                    }
                    else if (e.Location.X < 0)
                    {
                        x = 8;
                        Cursor.Position = new Point(x + pictureBox1.Location.X, Cursor.Position.Y);
                    }

                    if (e.Location.Y > pictureBox1.Image.Height)
                    {
                        
                         y = pictureBox1.Image.Height;
                        Cursor.Position = new Point(Cursor.Position.X, y + pictureBox1.Location.Y);
                    }
                    else if (e.Location.Y < 0)
                    {
                        y = 31;
                      //  MessageBox.Show("x= "+ (x + pictureBox1.Location.X)+" y= "+ (y + pictureBox1.Location.Y));
                        Cursor.Position = new Point(Cursor.Position.X, y + pictureBox1.Location.Y);
                    }
                  
                    
                    newLoc = new Point(x, y);
                    //    newLoc = e.Location;
                    startDrawRenctagle = true;
                        
                }
                else if (!isDrawingNow)
                {
                    if (startDrawRenctagle)
                    {
                        pictureBox1.Image = (Image)currentImage.Clone();

                        if (((e.Location.X - LocationOfRenctagle.X) < 0) && ((e.Location.Y - LocationOfRenctagle.Y) < 0))
                        {
                            using (Bitmap bmp = new Bitmap(pictureBox1.Image))
                            {
                                cutImage = bmp.Clone(new Rectangle { X = e.Location.X, Y = e.Location.Y, Width = LocationOfRenctagle.X - e.Location.X, Height = LocationOfRenctagle.Y - e.Location.Y }, bmp.PixelFormat);

                            }


                        }
                        else if ((e.Location.Y - LocationOfRenctagle.Y) < 0)
                        {
                            using (Bitmap bmp = new Bitmap(pictureBox1.Image))
                            {
                                cutImage = bmp.Clone(new Rectangle { X = e.Location.X + (LocationOfRenctagle.X - e.Location.X), Y = e.Location.Y, Width = e.Location.X - LocationOfRenctagle.X, Height = LocationOfRenctagle.Y - e.Location.Y }, bmp.PixelFormat);

                            }

                        }
                        else if ((e.Location.X - LocationOfRenctagle.X) < 0)
                        {
                            using (Bitmap bmp = new Bitmap(pictureBox1.Image))
                            {
                                cutImage = bmp.Clone(new Rectangle { X = e.Location.X, Y = e.Location.Y + (LocationOfRenctagle.Y - e.Location.Y), Width = LocationOfRenctagle.X - e.Location.X, Height = e.Location.Y - LocationOfRenctagle.Y }, bmp.PixelFormat);

                            }


                        }
                        else
                        {
                            using (Bitmap bmp = new Bitmap(pictureBox1.Image))
                            {
                                cutImage = bmp.Clone(new Rectangle { X = LocationOfRenctagle.X, Y = LocationOfRenctagle.Y, Width = e.Location.X - LocationOfRenctagle.X, Height = e.Location.Y - LocationOfRenctagle.Y }, bmp.PixelFormat);

                            }
                            //g.DrawRectangle(pn, LocationOfRenctagle.X, LocationOfRenctagle.Y, e.Location.X - LocationOfRenctagle.X, e.Location.Y - LocationOfRenctagle.Y);
                        }
                        currentImage = (Bitmap)cutImage.Clone();
                        pictureBox1.Image =(Image) cutImage.Clone();
                        startDrawRenctagle = false;
                    }
                    LocationOfRenctagle = new Point(e.Location.X, e.Location.Y);
                    pictureBox1.Invalidate();
                }
            }
            catch (Exception ee)
            {

            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawingNow = true;
            timer1.Start();
            label10.Text = "Выделение области...";
            newLoc = e.Location;
            newLoc2 =e.Location; 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fd.SelectedPath;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (newLoc != newLoc2)
            {
                pictureBox1.Image = (Image)currentImage.Clone();
                newLoc2 = newLoc;
            }
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                if (((newLoc.X - LocationOfRenctagle.X) < 0) && ((newLoc.Y - LocationOfRenctagle.Y) < 0))
                {
                    
                    g.DrawRectangle(pn, newLoc.X, newLoc.Y, LocationOfRenctagle.X - newLoc.X, LocationOfRenctagle.Y - newLoc.Y);


                }
                else if ((newLoc.Y - LocationOfRenctagle.Y) < 0)
                {
                    g.DrawRectangle(pn, newLoc.X + (LocationOfRenctagle.X - newLoc.X), newLoc.Y, newLoc.X - LocationOfRenctagle.X, LocationOfRenctagle.Y - newLoc.Y);


                }
                else if ((newLoc.X - LocationOfRenctagle.X) < 0)
                {
               
                    g.DrawRectangle(pn, newLoc.X, newLoc.Y + (LocationOfRenctagle.Y - newLoc.Y), LocationOfRenctagle.X - newLoc.X, newLoc.Y - LocationOfRenctagle.Y);


                }
                else
                {
                
                    g.DrawRectangle(pn, LocationOfRenctagle.X, LocationOfRenctagle.Y, newLoc.X - LocationOfRenctagle.X, newLoc.Y - LocationOfRenctagle.Y);

                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1) Нажмите кнопку Путь... и выберите папку, из которой будут браться картинки.\n2)Нажмите кнопку загрузить и появится первое фото\n3)Что бы пропустить картинку, нажмите вперед\n4)Кликните на определенном месте и не отпуская кнопку выделите область. Картинка обрежется.\n5)Нажмите кнопку сохранить и картинка перезапишется и сразу откроется следующая.\n6)Нажмите кнопку назад что бы переделать предыдущие картинки\n7)Перезагрузить картинку - значит отменить все изменения до сохранения  \n\n\nКнопки быстрого доступа:\nA - 'Предыдущая картинка'\nD - 'Следующая картинка'\nS - 'Сохранить картинку'\nПробел - 'Перезагрузить картинку'", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void button5_Click(object sender, EventArgs e)   /////СОХРАНЕНИЕ
        {
            string FullName = "";
            label10.Text = "Сохраняется...";
            int numberOfTry = 0;
            try
            {
                FullName = images[positionOfImage].FullName;
                Image imag;

                isDrawingNow = false;
                    //    pictureBox1_MouseMove(sender, e);   /////СОХРАНЕНИЕ   /////СОХРАНЕНИЕ   /////СОХРАНЕНИЕ
                    imag = (Image)cutImage.Clone();

                pb.Dispose();

              //  pictureBox1.Image.Dispose();
                if (File.Exists(FullName))
                {
                    AllButtonsEnabled(false);
                    while (!WaitForFile(FullName, FileMode.Open))
                    {
                        numberOfTry++;
                        label10.Text = "Сохраняется... " + numberOfTry + " сек.";
                        await Task.Delay(1000);
                        if (numberOfTry > 5)
                        {
                            MessageBox.Show("Не удалось сохранить файл, попробуйте еще раз.", "Слишком быстрое обращение к файлу");
                            label10.Text = "Готово к работе!";
                            AllButtonsEnabled(true);
                            return;
                        }

                    }
                        File.Delete(FullName);   /////СОХРАНЕНИЕ   /////СОХРАНЕНИЕ   /////СОХРАНЕНИЕ   /////СОХРАНЕНИЕ   /////СОХРАНЕНИЕ
                        imag.Save(FullName);
                    

                    AllButtonsEnabled(true);
                }
                else imag.Save(FullName);
                positionOfImage++;
                if (positionOfImage >= images.Length) positionOfImage = 0;
                FullName = images[positionOfImage].FullName;
                if (File.Exists(FullName))
                {
                    pb = new PictureBox();
                    pb.Load(FullName);
                    pictureBox1.Image = new Bitmap(pb.Image);
                    currentImage = (Bitmap)pictureBox1.Image.Clone();
                    cutImage= (Bitmap)pictureBox1.Image.Clone();
                }

                pictureBox1.Invalidate();
                label5.Text = images[positionOfImage].Name;
                textBox2.Text = pictureBox1.Image.Width.ToString();          
                textBox3.Text = pictureBox1.Image.Height.ToString();
                progressBar1.Value = positionOfImage + 1;
                label9.Text = (positionOfImage + 1) + @"/" + progressBar1.Maximum;
                label10.Text = "Готово к работе!";
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось сохранить файл" + ee.Message + ". Имя файла: " + FullName, "Oшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label10.Text = "Готово к работе!";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string FullName = "";
            try
            {
                
                positionOfImage++;
                if (positionOfImage >= images.Length) positionOfImage = 0;
                FullName = images[positionOfImage].FullName;

                if (File.Exists(FullName))
                {
                    pb = new PictureBox();
                    pb.Load(FullName);
                    pictureBox1.Image = new Bitmap(pb.Image);
                }

                label5.Text = images[positionOfImage].Name;
                textBox2.Text = pictureBox1.Image.Width.ToString();
                textBox3.Text = pictureBox1.Image.Height.ToString();
                currentImage = (Bitmap)pictureBox1.Image.Clone();
                cutImage = (Bitmap)pictureBox1.Image.Clone();
                pictureBox1.Invalidate();
                progressBar1.Value = positionOfImage + 1;
                label9.Text = (positionOfImage + 1) + @"/" + progressBar1.Maximum;
                label10.Text = "Загружено!";
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось загрузить новый файл" + ee.Message + " Имя файла: " + FullName, "Oшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label10.Text = "Готово к работе!";
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string FullName = "";
            try
            {
                FullName = images[positionOfImage].FullName;

                if (File.Exists(FullName))
                {
                    pb = new PictureBox();
                    pb.Load(FullName);
                    pictureBox1.Image = new Bitmap(pb.Image);
                }
                currentImage = (Bitmap)pictureBox1.Image.Clone();
                cutImage = (Bitmap)pictureBox1.Image.Clone();
                pictureBox1.Invalidate();

            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось загрузить новый файл" + ee.Message + " Имя файла: " + FullName, "Oшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnregisterHotKey(thisWindow, 1);
            UnregisterHotKey(thisWindow, 2);
            UnregisterHotKey(thisWindow, 3);
            UnregisterHotKey(thisWindow, 4);
        }
        protected override void WndProc(ref Message keyPressed)
        {

            if (keyPressed.Msg == 0x0312)
            {
                switch (keyPressed.WParam.ToInt32())
                {
                    case 1:
                        button2.PerformClick();
                        break;
                    case 2:
                        button6.PerformClick();
                        break;
                    case 3:
                        button5.PerformClick();
                        break;
                    case 4:
                        button3.PerformClick();
                        break;
                }
             //   label5.Text = keyPressed.;
            }

            base.WndProc(ref keyPressed);
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            string FullName = "";
            int numberOfTry = 0;
            label10.Text = "Удаление...";
            try
            {
                pb.Dispose();


              FullName = images[positionOfImage].FullName;
              
                
                if (File.Exists(FullName))
                {
                    AllButtonsEnabled(false);
                    while (!WaitForFile(FullName, FileMode.Open))
                    {
                        
                        numberOfTry++;
                        label10.Text = "Удаляется... " + numberOfTry + " сек.";
                        await Task.Delay(1000);
                        if (numberOfTry > 5)
                        {
                            MessageBox.Show("Не удалось удалить файл, попробуйте еще раз.", "Слишком быстрое обращение к файлу");
                            label10.Text = "Готово к работе!";
                            AllButtonsEnabled(true);
                            return;
                        }
                    }
                    File.Delete(FullName);
                    AllButtonsEnabled(true);
                }
                DirectoryInfo df = new DirectoryInfo(textBox1.Text);

                images = df.GetFiles().Where(p => (p.Extension == ".jpg" || p.Extension == ".png" || p.Extension == ".jpeg" || p.Extension == ".tiff" || p.Extension == ".gif" || p.Extension == ".bmp" || p.Extension == ".eps" || p.Extension == ".pcx" || p.Extension == ".raw" || p.Extension == ".ico")).ToArray();

                progressBar1.Value = 0;
                progressBar1.Maximum = images.Count();
                if (positionOfImage >= images.Length) positionOfImage = 0;
                if (images.Length != 0)
                {
                    FullName = images[positionOfImage].FullName;
                    if (File.Exists(FullName))
                     {
                         pb = new PictureBox();
                           pb.Load(FullName);
                        pictureBox1.Image = new Bitmap(pb.Image);
                         currentImage = (Bitmap)pb.Image.Clone();
                        cutImage= (Bitmap)pb.Image.Clone();
                     }


                
                
                    label5.Text = images[positionOfImage].Name;
                    textBox2.Text = pictureBox1.Image.Width.ToString();         
                    textBox3.Text = pictureBox1.Image.Height.ToString();
                    progressBar1.Value = positionOfImage + 1;
                    label9.Text = (positionOfImage + 1) + @"/" + progressBar1.Maximum;
                }
                else
                {
                    AllButtonsEnabled(false);
                    pictureBox1.Image.Dispose();
                }
                
                pictureBox1.Invalidate();
                label10.Text = "Готово к работе!";
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось удалить файл" + ee.Message + ". Имя файла: " + FullName, "Oшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label10.Text = "Готово к работе!";
            }
        }


        static Color HsvToRgb(float h, float s, float v)
        {
            int i;
            float f, p, q, t;

            

            h /= 60;
            i = (int)Math.Floor(h);
            f = h - i;
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            float r, g, b;
            switch (i)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                default: r = v; g = p; b = q; break;
            }

            return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        private void button9_Click(object sender, EventArgs e)
        {
           ColorDialog cd = new ColorDialog();
            cd.FullOpen = true;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                
                pn = new Pen(cd.Color, 2);
            }
        }

        private void AllButtonsEnabled(bool mode)
        {
            button2.Enabled = mode;
            button6.Enabled = mode;
            button3.Enabled = mode;
            button1.Enabled = mode;
            button5.Enabled = mode;
            button4.Enabled = mode;
            button8.Enabled = mode;
            button7.Enabled = mode;
        }
        bool WaitForFile(string fullPath, FileMode mode)
        {
                    bool getIt = false;

                    FileStream fs = null;
                    try
                    {
                        using (fs = new FileStream(fullPath, mode))
                        {
                            getIt = true;
                            
                        }
                    }
                    catch (Exception)
                    {
                        if (fs != null)
                        {
                            fs.Dispose();
                        }
                        getIt = false;
                        
                    }
                
            return getIt;
        }
        //async void LetsFreeFile(string fullPath)
        //{
        //    for (int numTries = 0; numTries < 15; numTries++)
        //    {
        //        FileStream fs = null;
        //        try
        //        {
        //            using (fs = new FileStream(fullPath,FileMode.Open)) return; 
        //        }
        //        catch (Exception)
        //        {
        //            if (fs != null)
        //            {
        //                fs.Dispose();
        //            }
                    
        //            await Task.Delay(700);

        //        }
        //    }
        //}
        //private bool WaitForFile2(string filename)
        //{
        //    bool isFree = false;

        //    while (!isFree)
        //    {
        //        try
        //        {
        //            File.Move(filename, filename);
        //            isFree = false;
        //        }
        //        catch
        //        {
        //            Thread.Sleep(100);
        //        }
        //    }
        //    return isFree;
        //}
    }
}
