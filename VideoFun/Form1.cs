using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;

namespace VideoFun
{
    public partial class MainForm : Form
    {

        private VideoProcessor m_videoProcessor = new VideoProcessor();
        private FrameManager m_frameManager = new FrameManager();
        private Rectangle m_clip = new Rectangle(0, 0, 640, 480);

        private bool m_mouseDown = false;

        private int m_clipStartIndex = 0;
        private int m_clipEndIndex = 0;

        public MainForm()
        {
            InitializeComponent();
            m_videoProcessor.displayPictureBox = displayPictureBox;
            m_videoProcessor.statusTextBox = statusTextBox;
            m_videoProcessor.frameManager = m_frameManager;
        }

        public void Callback(int numFrames)
        {
            m_videoProcessor.endOfVideoCallback -= Callback;
            timeTrackBar.Maximum = numFrames - 1;
        }

        private void openVideoButton_Click(object sender, EventArgs e)
        {
            string filename = m_frameManager.OpenVideoFile();
            if (filename == null || filename.Length == 0)
                return;
            Bitmap firstFrame = m_videoProcessor.LoadFile(filename);
            m_clip = new Rectangle( new Point(0, 0), firstFrame.Size);
            displayPictureBox.Image = firstFrame;
            m_videoProcessor.endOfVideoCallback += Callback;

            //m_imageInDisplay = firstFrame;

            Callback((int) m_videoProcessor.m_num_frames);
        }

        private void SaveVideoButton_Click(object sender, EventArgs e)
        {
            m_videoProcessor.SaveAvi("DIVX_newVideo.avi", m_clipStartIndex, 
                                    m_clipEndIndex, m_clip, (int)m_videoProcessor.m_fps,
                                    CvInvoke.CV_FOURCC('D', 'I', 'V', 'X'));

            //m_frameManager.SaveAvi("newVideo.avi", m_clipStartIndex, m_clipEndIndex, m_clip, (int)m_videoProcessor.m_fps);
        }


        private void timeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            /*
            //timeTrackBar.
            int value = timeTrackBar.Value;
            Bitmap bmp = m_videoProcessor.frameManager.GetFrame(value);
            displayPictureBox.Image = bmp;
            //m_imageInDisplay = bmp;
             * */

            int value = timeTrackBar.Value;
            displayPictureBox.Image = m_videoProcessor.GetFrame(value);
            int seconds = m_videoProcessor.GetTimePosition(value);
            displayLabel.Text = TimeSpan.FromSeconds(seconds).ToString();
            

            //m_imageInDisplay = bmp;

        }

        private void displayPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_clip.X = e.X;
                m_clip.Y = e.Y;
                m_mouseDown = true;
            }

        }

        private void displayPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_clip.Width = e.X - m_clip.X;
                m_clip.Height = e.Y - m_clip.Y;
                m_mouseDown = false;
            }
        }

        private void displayPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_mouseDown)
            {
                m_clip.Width = e.X - m_clip.X;
                m_clip.Height = e.Y - m_clip.Y;
                Graphics g = displayPictureBox.CreateGraphics();
                g.DrawRectangle( new Pen(Color.Red), m_clip);
                displayLabel.Text = "(" + m_clip.Width + "," + m_clip.Height + ")";
            }
        }

        private void startTimeButton_Click(object sender, EventArgs e)
        {
            m_clipStartIndex = timeTrackBar.Value;
        }

        private void endTimeButton_Click(object sender, EventArgs e)
        {
            m_clipEndIndex = timeTrackBar.Value;
        }

        private void debugButton_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap[] bmps = new Bitmap[3] {
                                        new Bitmap("image1.jpg"),
                                        new Bitmap("image2.jpg"),
                                        new Bitmap("image3.jpg")};
                for (int i = 0; i < bmps.Length; i++)
                {
                    Image<Gray, byte> image = (new Image<Gray, byte>(bmps[i]));
//                    Gray averageIntensity = image.GetAverage();
//                    image = image.Canny(averageIntensity, averageIntensity);
                    bmps[i] = image.ToBitmap();
//                    image.Save("1.png");
                }

                double a = double.Parse(textBox1.Text);
                double b = double.Parse(textBox2.Text);
                double c = double.Parse(textBox3.Text);

                Bitmap bmp = MachFilter.TrainMachFilter(bmps, a, b, c, true);
                displayPictureBox.Image = bmp;
                bmp.Save("filter.bmp");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void trackButton_Click(object sender, EventArgs e)
        {
            int centerX = m_clip.X + m_clip.Width / 2;
            int centerY = m_clip.Y + m_clip.Height / 2;

            int matchSize = 128;
            int halfMatchSize = matchSize / 2;
            
            int i = m_clipStartIndex;

            Image<Gray, byte> sourceImage = new Image<Gray, byte>(m_frameManager.GetFrame(i));
            //sourceImage = sourceImage.Canny(new Gray(128), new Gray(128));

            Image<Gray, byte> destImage;

            Image<Gray, byte> sourceClip = sourceImage.GetSubRect(_GetSquareByCenter(centerX, centerY, matchSize, halfMatchSize));
            OpticalCorrelator oc = new OpticalCorrelator(sourceClip.ToBitmap());


            Graphics g = displayPictureBox.CreateGraphics();
            while(i<m_clipEndIndex)
            {
                //Image<Gray, byte> sourceClip = sourceImage.GetSubRect(_GetSquareByCenter(centerX, centerY, matchSize, halfMatchSize));
                //OpticalCorrelator oc = new OpticalCorrelator(sourceClip.ToBitmap());
                ++i;
                destImage = new Image<Gray, byte>(m_frameManager.GetFrame(i));
                //destImage = destImage.Canny(new Gray(128), new Gray(128));
                Image<Gray, byte> destClip = destImage.GetSubRect(_GetSquareByCenter(centerX, centerY, matchSize, halfMatchSize));
                int x, y;
                oc.Correlate(destClip.ToBitmap(), out x, out y);
                int dx = x - halfMatchSize;
                int dy = y - halfMatchSize;
                centerX += dx;
                centerY += dy;
                sourceImage = destImage;

                displayLabel.Text = dx + "," + dy;
                //g.DrawString(, Font.
                g.DrawImage(destImage.ToBitmap(), 0, 0);
                g.DrawRectangle(new Pen(Color.Pink), _GetSquareByCenter(centerX, centerY, matchSize, halfMatchSize));
            }
            g.Dispose();
        }

        private Rectangle _GetSquareByCenter(int centerX, int centerY, int matchSize, int halfMatchSize)
        {
            return new Rectangle(centerX - halfMatchSize, centerY - halfMatchSize, matchSize, matchSize);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void saveFramesButton_Click(object sender, EventArgs e)
        {
            m_videoProcessor.SaveFrames(m_clipStartIndex, m_clipEndIndex, m_clip, 5);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            MessageBox.Show("Drag!");
        }

        private void displayPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            MessageBox.Show("Drag!");
        }

        private void displayPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            MessageBox.Show("Drag Entered!");
        }

        private void statusTextBox_DragDrop(object sender, DragEventArgs e)
        {
            statusTextBox.Text = "Dropped!";
        }

        private void statusTextBox_DragEnter(object sender, DragEventArgs e)
        {
            statusTextBox.Text = "Drag Entered!";
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            // make sure they're actually dropping files (not text or anything else)
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                // allow them to continue
                // (without this, the cursor stays a "NO" symbol
                e.Effect = DragDropEffects.All;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            // transfer the filenames to a string array
            // (yes, everything to the left of the "=" can be put in the
            // foreach loop in place of "files", but this is easier to understand.)
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            SortFilenamesByDate(files);
            m_videoProcessor.SaveAvi("Test.avi", files, 30, CvInvoke.CV_FOURCC('X', 'V', 'I', 'D'));
            
        }

        private string[] SortFilenamesByDate(string[] files)
        {
            DateTime[] timeArray = new DateTime[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                timeArray[i] = new FileInfo(files[i]).CreationTime;
            }

            Array.Sort(timeArray, files);
            return files;
        }


    }
}
