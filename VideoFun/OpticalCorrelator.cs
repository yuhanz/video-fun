/**
 * Yuhan Zhang, Cal Poly Pomona, June 2010.
 *  
 *  
 * email: yuhanz@gmail.com
 *  
 * Optical Correlator:
 *      Simulation of Optical Correlation. Used for doing auto correlation in Fourier domain.
 *      (Given an image, find the best matching point in another image.)
 *      
  **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using AForge.Math;
using AForge.Imaging;

namespace VideoFun
{
    class OpticalCorrelator
    {
        private ComplexImage m_filterComplexImage;
        private int m_width;
        private int m_height;

        private int m_halfWidth;
        private int m_halfHeight;

        public OpticalCorrelator(Bitmap filter) {
            m_filterComplexImage = ComplexImage.FromBitmap(filter);
            m_filterComplexImage.ForwardFourierTransform();
            m_width = filter.Width;
            m_height = filter.Height;
            m_halfWidth = m_width/2;
            m_halfHeight = m_height/2;
        }


        public void Correlate(Bitmap scene, out int x, out int y)
        {
            if(scene.Width != m_width || scene.Height != m_height)
                throw new Exception("The size of the scene doesn't meet filter");
            ComplexImage sceneComplexImage = ComplexImage.FromBitmap(scene);
            sceneComplexImage.ForwardFourierTransform();

            // Multiply the conjugate
            for(int i=0;i<m_width;i++) {
                for(int j=0;j<m_height;j++) {
                    Complex c1 = sceneComplexImage.Data[i, j];
                    Complex c2 = m_filterComplexImage.Data[i, j];
                    Complex conjugate = new Complex(c2.Re, -c2.Im);
                    Complex product = conjugate * c1;
                    if (product.Magnitude!=0)
                        product = product / product.Magnitude;
                    sceneComplexImage.Data[i, j] = product;
                }
            }

            
            // Analysis the correlated image by finding the maximum point
            sceneComplexImage.BackwardFourierTransform();

            double max = sceneComplexImage.Data[0,0].Re;
            double current;
            x=0;
            y=0;
            for (int i = 0; i < m_width; i++)
            {
                for (int j = 0; j < m_height; j++)
                {
                    if((current=sceneComplexImage.Data[j,i].Re)>max) {
                        max = current;
                        x = i;
                        y = j;
                    }
                }
            }
            // round it between the size
            x -= m_halfWidth;
            y -= m_halfHeight;
            if (x < 0)
                x += m_width;
            if (y < 0)
                y += m_height;

        }



    }
}
