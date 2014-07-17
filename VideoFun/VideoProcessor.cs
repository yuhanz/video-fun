/**
 * Yuhan Zhang, Cal Poly Pomona, June 2010.
 *  
 *  
 * email: yuhanz@gmail.com
 *  
 * VideoProcessor:
 *      Read video information and strip out the frames
 *      
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;


using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
//using Tao.FFmpeg;

namespace VideoFun
{
    class VideoProcessor
    {
        private Capture m_capture;
        public FrameManager frameManager;

        public double m_fps;
        public double m_num_frames;
        public double m_seconds;

        public PictureBox displayPictureBox;
        public TextBox statusTextBox;
        public TrackBar timeTrackBar;

        public delegate void EndOfVideoCallback(int numFrames);
        public EndOfVideoCallback endOfVideoCallback;

        public void ProcessFrame(object sender, EventArgs arg)
        {
            try
            {
                Image<Bgr, Byte> frame = m_capture.QueryFrame();
                if (frame == null)
                {
                    endOfVideoCallback(frameManager.GetNumOfFrames());
                    DisposeCapture();
                    return;
                }

                double framePos = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_FRAMES);   // this is miliseconds.
                double avi_ratio = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_AVI_RATIO);
                double milisec = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_MSEC);
                m_fps = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_FPS);
                m_num_frames = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_COUNT);


                statusTextBox.Text = "ratio: " + avi_ratio
                                        + "\r\nframePos: " + framePos
                                        + "\r\nmilisec: " + milisec
                                        + "\r\nfps: " + m_fps
                                        + "\r\nnum_frames: " + m_num_frames;
                Bitmap frameBitmap = (Bitmap)frame.ToBitmap();
                displayPictureBox.Image = (Image)frameBitmap;

                frameManager.SaveFrame(frameBitmap, framePos);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                DisposeCapture();
            }
            //System.Threading.Thread.Sleep( (int) (1000/fps));

        }

        public Bitmap LoadFile(string filename)
        {
            DisposeCapture();
            m_capture = new Capture(filename);
            Image<Bgr, Byte> frame = m_capture.QueryFrame();
            //Application.Idle += ProcessFrame;

            double framePos = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_FRAMES);   // this is miliseconds.
            double avi_ratio = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_AVI_RATIO);
            double seconds = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_MSEC);
            double msec = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_MSEC);
            m_fps = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_FPS);
            m_num_frames = m_capture.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_COUNT);

            if (m_num_frames == 0)
            {
                if (seconds > 0)
                    m_num_frames = seconds * m_fps;
                else
                    m_num_frames = framePos / 1000 * m_fps;
            }
            else if(seconds > 0)
            {
                m_fps = m_num_frames / seconds;
            }

            return frame.ToBitmap();
        }


        public Bitmap GetFrame(int framePos)
        {
            if(m_capture == null)
                throw new Exception("Capture is not initialized.");
            m_capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_FRAMES, framePos);
            Image<Bgr, byte> frame = m_capture.QueryFrame();
            return frame == null ? null : frame.ToBitmap();
        }

        // in seconds
        public int GetTimePosition(int framePos)
        {
            return (int) (framePos / m_fps);
        }


        private void DisposeCapture()
        {
            if (m_capture != null)
            {
                Application.Idle -= ProcessFrame;
                m_capture.Dispose();
                m_capture = null;
            }
        }


        public void SaveAvi(string aviFilename, int startIndex, int endIndex, Rectangle clip, int fps, int fourCC)
        {
            VideoWriter videoWriter = new VideoWriter(aviFilename, fourCC, fps, clip.Width, clip.Height, true);

            int residue = clip.Width % 12;
            if (residue > 0)
            {
                clip.Width += (12 - residue);     // correcting the bug of requiring the width to be multiple of 12.
                //clip.Width -= residue;
            }

            double startTime = this.GetTimePosition(startIndex);
            double endTime  = this.GetTimePosition(endIndex);

            statusTextBox.Text = "startFrame: " + startIndex + " "
                                 + "endFrame: " + endIndex + "\r\n"
                                + "# frame: " + (endIndex - startIndex) + "\r\n"
                                 + "startTime: " + startTime + "\r\n"
                                 + "endTime: " + endTime + "\r\n"
                                 + "duration: " + (endTime - startTime) + "\r\n";

            m_capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_FRAMES, startIndex);
            for (int i = startIndex; i < endIndex; i++)
            {
                //Image<Bgr, byte> img = new Image<Bgr, byte>(m_filenameList[i]);
                Image<Bgr, byte> img = m_capture.QueryFrame();
                if (img == null)
                    break;

                img = img.GetSubRect(clip);
                //img.Save("1.png");
                //img = img.Resize(scale, INTER.CV_INTER_LINEAR);

                //videoWriter.WriteFrame(img);      // Directly writing SubRect Image is causing a bug. (video is shifted)
                videoWriter.WriteFrame(new Image<Bgr, byte>(img.ToBitmap()));
            }

            videoWriter.Dispose();
        }

        public void SaveAvi(string aviFilename, string[] files, int fps, int fourCC)
        {
            if (files.Length <= 0)
                throw new Exception("Empty file list received");

            Bitmap bmp = (Bitmap) Bitmap.FromFile(files[0]);
            int width = bmp.Width;
            int height = bmp.Height;
            VideoWriter videoWriter = new VideoWriter(aviFilename, fourCC, fps, width, height, true);
            
            int residue = width % 12;
            if (residue > 0)
            {
                width += (12 - residue);     // correcting the bug of requiring the width to be multiple of 12.
            }

            for (int i = 0; i < files.Length; i++)
            {
                videoWriter.WriteFrame(new Image<Bgr, byte>(files[i]));
            }

            videoWriter.Dispose();
        }

        public void SaveFrames(int startIndex, int endIndex, Rectangle clip, int fps)
        {
            m_capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_FRAMES, startIndex);
            double samplingGap = m_fps / fps;

            int sampled = 0;

            for (int i = 0; i < endIndex - startIndex; i++)
            {
                Image<Bgr, byte> img = m_capture.QueryFrame();
                if (img == null)
                    break;

                if (i / samplingGap <= sampled)
                    continue;
                ++sampled;

                //img = img.GetSubRect(clip);
                string filename = String.Format("{0:000000}.png", sampled);
                img.ToBitmap().Save(filename, ImageFormat.Png);
            }
        }
    }
}
