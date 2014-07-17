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
using AForge.Imaging.Filters;

namespace VideoFun
{
    class MachFilter
    {

        public static ComplexImage[] TransformImageArrayToFourierDomain(Bitmap[] trainingExamples, bool eraseLowFrequency)
        {
            // Load the images and transform them into fourier domain
            if (trainingExamples.Length <= 0)
                throw new Exception("Empty training examples have been provided!");
            int width = trainingExamples[0].Width;
            int height = trainingExamples[0].Height;
            int centerX = width / 2;
            int centerY = height / 2;
            int[] centerXs = new int[4] { centerX, centerX + 1, centerX, centerX+1 };
            int[] centerYs = new int[4] { centerY, centerY, centerY + 1, centerY + 1 };
            if (width <= 0 || height <= 0)
                throw new Exception("The side of the image must be positives!");
            ComplexImage[] complexImages = new ComplexImage[trainingExamples.Length];
            for (int i = 0; i < trainingExamples.Length; i++)
            {
                int w = trainingExamples[0].Width;
                int h = trainingExamples[0].Height;
                if (w != width || h != height)
                    throw new Exception("The training examples are different in sizes!");
                complexImages[i] = ComplexImage.FromBitmap(trainingExamples[i]);
                complexImages[i].ForwardFourierTransform();
                //complexImages[i].Data[0, 0] = new Complex(0, 0);    // Set the 0 frequency to 0 (edge detection)
                if (eraseLowFrequency)
                    for (int k = 0; k < 4; k++)
                        complexImages[i].Data[centerXs[k], centerYs[k]] = new Complex(0, 0);
            }
            return complexImages;
        }

        public static Complex GetComplexConjugate(Complex complex)
        {
            return new Complex(complex.Re, - complex.Im);
        }

        public static ComplexImage PrepareBlankComplexImageForFourierTransform(int w, int h)
        {
            ComplexImage cImg = ComplexImage.FromBitmap( new Bitmap(w,h, PixelFormat.Format8bppIndexed) );
            cImg.ForwardFourierTransform();
            return cImg;
        }

        public static ComplexImage PrepareComplexImageToFourierTransform(ComplexImage cImg) {
            int w = cImg.Width;
            int h = cImg.Height;
            ComplexImage result = PrepareBlankComplexImageForFourierTransform(w,h);
            for(int i=0;i<w;i++){
                for(int j=0;j<h;j++) {
                    result.Data[i, j] = cImg.Data[i,j];
                }
            }
            return result;
        }

        public static void CalculateThreeFourierMatrices(ComplexImage[] complexImages,
                                   out ComplexImage meanMatrix, out ComplexImage correlationMatrix, out ComplexImage similiarityMatrix)
        {
            int width = complexImages[0].Width;
            int height = complexImages[0].Height;
            int numImages = complexImages.Length;

            // Calculate average correlation height (ACH)
            //           average correlation energy (ACE)
            //           average silimiarity measure(ASM):
            //      ACH: mean of the fourier images
            //      ACE: mean of auto (self) correlation of fourier images
            //      ASM: mean of auto (self) correlation of the zero centered fourier images.
            meanMatrix = ComplexImage.FromBitmap(new Bitmap(width, height, PixelFormat.Format8bppIndexed));
            correlationMatrix = ComplexImage.FromBitmap(new Bitmap(width, height, PixelFormat.Format8bppIndexed));
            similiarityMatrix = ComplexImage.FromBitmap(new Bitmap(width, height, PixelFormat.Format8bppIndexed));

            for (int k = 0; k < numImages; k++)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Complex c = complexImages[k].Data[i, j];
                        meanMatrix.Data[i, j] += c / numImages;

                        Complex conjugate = GetComplexConjugate(c);
                        correlationMatrix.Data[i, j] = conjugate * c / numImages;
                    }
                }
            }

            for (int k = 0; k < numImages; k++)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Complex c = complexImages[k].Data[i, j];
                        Complex mean = meanMatrix.Data[i, j];
                        Complex zeroMeaned = (c - mean);
                        Complex zeroMeanedConjugate = GetComplexConjugate(zeroMeaned);
                        similiarityMatrix.Data[i, j] = zeroMeanedConjugate * zeroMeaned / numImages;
                    }
                }
            }

        }


        public static Bitmap TrainMachFilter(Bitmap[] trainingExamples, double alpha, double beta, double gamma, bool eraseLowFreqency)
        {
            ComplexImage[] complexImages = TransformImageArrayToFourierDomain(trainingExamples, eraseLowFreqency);

            ComplexImage meanMatrix;
            ComplexImage correlationMatrix;
            ComplexImage similiarityMatrix;

            CalculateThreeFourierMatrices(complexImages, out meanMatrix, out correlationMatrix, out similiarityMatrix);

            ComplexImage c1 = PrepareComplexImageToFourierTransform(meanMatrix);
            ComplexImage c2 = PrepareComplexImageToFourierTransform(correlationMatrix);
            ComplexImage c3 = PrepareComplexImageToFourierTransform(similiarityMatrix);
            c1.BackwardFourierTransform();
            c2.BackwardFourierTransform();
            c3.BackwardFourierTransform();

            c1.ToBitmap().Save("-c1.png");
            c2.ToBitmap().Save("-c2.png");
            c3.ToBitmap().Save("-c3.png");


            int width = complexImages[0].Width;
            int height = complexImages[0].Height;
            int numImages = complexImages.Length;

            ComplexImage filter = ComplexImage.FromBitmap(new Bitmap(width, height, PixelFormat.Format8bppIndexed));
            filter.ForwardFourierTransform();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Complex a = meanMatrix.Data[i,j];
                    Complex b = correlationMatrix.Data[i, j];
                    Complex c = similiarityMatrix.Data[i, j];
                    Complex den = a * alpha + b * beta + c * gamma;
                    //filter.Data[i, j] = meanMatrix.Data[i, j] / den;
                    filter.Data[i, j] = den;
                }
            }
            filter.BackwardFourierTransform();
            /*
            // Dymanic range stretch the images.
            double mean = 0, std = 0;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    mean += filter.Data[i,j].Re;
            mean /= width * height;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    double diff = filter.Data[i, j].Re - mean;
                    std += diff * diff;
                }
            std = Math.Sqrt(std / (width * height));
            double negativeStd = - std;

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    double v = filter.Data[i, j].Re - mean;
                    if (v <= negativeStd)
                        v = 0;
                    else if (v >= std)
                        v = 255;
                    else
                        v = (double) v / std * 255;
                    filter.Data[i, j].Re = v;
                }
            */
            return filter.ToBitmap();
        }


    }
}
