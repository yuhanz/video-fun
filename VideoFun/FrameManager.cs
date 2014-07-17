/**
 * Yuhan Zhang
 * 06/22/2010
 *      Frame Manager helps stripping the movie files into frames,
 *      and reconstruct the cliped region back into a movie.
 *      
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Web;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
//using Tao.FFmpeg;
namespace VideoFun
{
    class FrameManager
    {
        private string m_videoDir;
        private List<string> m_filenameList = new List<string>();
        private int frameWidth;
        private int frameHeight;

        private bool pathExisted = false;

        public void SaveFrame(Bitmap frame, double framePos)
        {
            string frameFilename = m_videoDir + "\\" + framePos + ".png";
            if (!pathExisted)
                frame.Save(frameFilename);
            if (m_filenameList.Count <= 0)
            {
                frameWidth = frame.Width;
                frameHeight = frame.Height;
            }
            m_filenameList.Add(frameFilename);
        }

        public int GetNumOfFrames()
        {
            return m_filenameList.Count;
        }

        public Bitmap GetFrame(int index)
        {
            return new Bitmap(m_filenameList[index]);
        }

        public void ClearFrames()
        {
            m_filenameList.Clear();
        }

        public string OpenVideoFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "image files (*.jpg)|*.jpg|(*.bmp)|*.bmp|(*.png)|*.png|(*.gif)|*.gif|All files (*.*)|*.*";
            dialog.Title = "Open a video";
            string filename = (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;
            m_videoDir = HttpUtility.UrlEncode(filename);
            if (m_videoDir == null)
                return "";
            
            //if(!(pathExisted = Directory.Exists(m_videoDir)))
                //Directory.CreateDirectory(m_videoDir);
            return filename;
        }


        /*
        public void SaveAvi(string frameDirectory, string aviFilename)
        {
            VideoWriter videoWriter = new VideoWriter(aviFilename, 33, 640, 360, true);

            DirectoryInfo videoFrameDir = new DirectoryInfo(frameDirectory);

            FileInfo[] files = videoFrameDir.GetFiles("*.png");
            Console.WriteLine("files:");
            foreach (FileInfo file in files)
            {
                videoWriter.WriteFrame(new Image<Bgr, byte>(file.FullName));
            }

            CvInvoke.CV_FOURCC('F', 'L', 'V', '1');

            videoWriter.Dispose();
        }*/

        public void SaveAvi(string aviFilename, int startIndex, int endIndex, Rectangle clip, int fps)
        {
            VideoWriter videoWriter = new VideoWriter(aviFilename, CvInvoke.CV_FOURCC('I', 'Y', 'U', 'V'), fps, clip.Width, clip.Height, true);

            int residue = clip.Width % 12;
            if (residue > 0)
            {
                clip.Width += (12 - residue);     // correcting the bug of requiring the width to be multiple of 12.
                //clip.Width -= residue;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                Image<Bgr, byte> img = new Image<Bgr, byte>(m_filenameList[i]);
                img = img.GetSubRect(clip);
                //img.Save("1.png");
                //img = img.Resize(scale, INTER.CV_INTER_LINEAR);

                //videoWriter.WriteFrame(img);      // Directly writing SubRect Image is causing a bug. (video is shifted)
                videoWriter.WriteFrame(new Image<Bgr, byte>(img.ToBitmap()));
            }

            videoWriter.Dispose();
        }
        /*
        public void SaveAvi(string aviFilename, int startIndex, int endIndex, Rectangle clip, int fps)
        {
            VideoWriter videoWriter = new VideoWriter(aviFilename, CvInvoke.CV_FOURCC('I', 'Y', 'U', 'V'), fps, clip.Width, clip.Height, true);

            int residue = clip.Width % 12;
            if (residue > 0)
            {
                clip.Width += (12 - residue);     // correcting the bug of requiring the width to be multiple of 12.
                //clip.Width -= residue;
            }

            for(int i=startIndex;i<endIndex;i++)
            {
                Image<Bgr, byte> img = new Image<Bgr, byte>(m_filenameList[i]);
                img = img.GetSubRect(clip);
                //img.Save("1.png");
                //img = img.Resize(scale, INTER.CV_INTER_LINEAR);

                //videoWriter.WriteFrame(img);      // Directly writing SubRect Image is causing a bug. (video is shifted)
                videoWriter.WriteFrame(new Image<Bgr, byte>(img.ToBitmap()));
            }

            videoWriter.Dispose();
        }
         * */

    }
}
