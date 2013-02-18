using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Feng.Windows.Utils
{
    public static class ImageHelper
    {
        public enum ImageConcatType
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        /// 在图像上描绘背景, 并改变图像大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image DrawImageBackgound(Image image, Color backgroundColor, int width, int height)
        {
            Image ret = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(ret);
            g.FillRectangle(new SolidBrush(backgroundColor), 0, 0, width, height);
            g.DrawImage(image, 0, 0, width, height);
            return ret;
        }

        /// <summary>
        /// 在图像上描绘背景
        /// </summary>
        /// <param name="image"></param>
        /// <param name="backgroundColor"></param>
        /// <returns></returns>
        public static Image DrawImageBackgound(Image image, Color backgroundColor)
        {
            Image imageBk = image.Clone() as Image;
            Graphics g = Graphics.FromImage(imageBk);
            g.FillRectangle(new SolidBrush(backgroundColor), 0, 0, imageBk.Width, imageBk.Height);
            return imageBk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static Image ConcatImage(Image image1, Image image2, ImageConcatType alignment)
        {
            Bitmap imageBk = null;
            switch(alignment)
            {
                case ImageConcatType.Horizontal:
                    imageBk = new Bitmap(image1.Width + image2.Width, Math.Max(image1.Height, image2.Height));
                    break;
                case ImageConcatType.Vertical:
                    imageBk = new Bitmap(Math.Max(image1.Width, image2.Width), image1.Height + image2.Height);
                    break;
            }

            Graphics g = Graphics.FromImage(imageBk);
            switch (alignment)
            {
                case ImageConcatType.Horizontal:
                    g.DrawImage(image1, 0, 0);
                    g.DrawImage(image2, image1.Width, 0);
                    break;
                case ImageConcatType.Vertical:
                    g.DrawImage(image1, 0, 0);
                    g.DrawImage(image2, 0, image1.Height);
                    break;
            }
            return imageBk;
        }

        /// <summary>
        /// 合并图像
        /// </summary>
        /// <param name="imageBackground"></param>
        /// <param name="imageForeground"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static Image CombineImage(Image imageBackground, Image imageForeground, ContentAlignment alignment)
        {
            Image imageBk = imageBackground.Clone() as Image;
            Graphics g = Graphics.FromImage(imageBk);
            Point point;
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    point = new Point(0, 0);
                    break;
                case ContentAlignment.TopRight:
                    point = new Point(imageBackground.Width - imageForeground.Width, 0);
                    break;
                case ContentAlignment.BottomLeft:
                    point = new Point(0, imageBackground.Height - imageForeground.Height);
                    break;
                case ContentAlignment.BottomRight:
                    point = new Point(imageBackground.Width - imageForeground.Width, imageBackground.Height - imageForeground.Height);
                    break;
                case ContentAlignment.TopCenter:
                    point = new Point(imageBackground.Width / 2 - imageForeground.Width / 2, 0);
                    break;
                case ContentAlignment.BottomCenter:
                    point = new Point(imageBackground.Width / 2 - imageForeground.Width / 2, imageBackground.Height - imageForeground.Height);
                    break;
                default:
                    throw new NotSupportedException("Not supported alignment");
            }

            g.DrawImage(imageForeground, point);

            return imageBk;
        }

    }
}
