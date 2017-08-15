using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Hishop.Weixin.MP.Util
{
    public static class PdfToImage
    {
        /// <summary>
        /// 转换的图片清晰度，1最不清醒，10最清晰
        /// </summary>
        public enum Definition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }

        /// <summary>
        /// 将PDF文档转换为图片的方法
        /// </summary>
        /// <param name="pdfInputPath">PDF文件路径</param>
        /// <param name="imageOutputPath">图片输出路径</param>
        /// <param name="imageName">生成图片的名字</param>
        /// <param name="startPageNum">从PDF文档的第几页开始转换</param>
        /// <param name="endPageNum">从PDF文档的第几页开始停止转换</param>
        /// <param name="imageFormat">设置所需图片格式</param>
        /// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
        public static void ConvertPDFToImage(string pdfInputPath, string imageOutputPath, string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition)
        {
            PDFFile pdfFile = PDFFile.Open(pdfInputPath);
            if (!Directory.Exists(imageOutputPath))
            {
                Directory.CreateDirectory(imageOutputPath);
            }
            // validate pageNum
            if (startPageNum <= 0)
            {
                startPageNum = 1;
            }
            if (endPageNum > pdfFile.PageCount)
            {
                endPageNum = pdfFile.PageCount;
            }
            if (startPageNum > endPageNum)
            {
                int tempPageNum = startPageNum;
                startPageNum = endPageNum;
                endPageNum = startPageNum;
            }
            // start to convert each page
            for (int i = startPageNum; i <= endPageNum; i++)
            {
                Bitmap pageImage = pdfFile.GetPageImage(i - 1, 56 * (int)definition);
                int canKao = pageImage.Width > pageImage.Height ? pageImage.Height : pageImage.Width;
                int newHeight = canKao > 1080 ? pageImage.Height / 2 : pageImage.Height;
                int newWidth = canKao > 1080 ? pageImage.Width / 2 : pageImage.Width;
                Bitmap newPageImage = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(newPageImage);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //重新画图的时候Y轴减去130，高度也减去130  这样水印就看不到了
                g.DrawImage(pageImage, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 40, pageImage.Width, pageImage.Height - 40), GraphicsUnit.Pixel);
                //newPageImage.Save(imageOutputPath + imageName + i.ToString() + "." + imageFormat.ToString(), imageFormat);
                newPageImage.Save(imageOutputPath + imageName + "." + imageFormat.ToString(), imageFormat);
                g.Dispose();
                newPageImage.Dispose();
                pageImage.Dispose();
            }
            pdfFile.Dispose();
        }


        /// <summary>
        /// 将1个PDF文档所有页全部转换为图片
        /// </summary>
        /// <param name="pdfInputPath">PDF文件路径</param>
        /// <param name="imageOutputPath">图片输出路径</param>
        /// <param name="imageName">生成图片的名字</param>
        /// <param name="imageFormat">设置所需图片格式</param>
        /// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
        public static void ConvertAllPDF2Images(string pdfInputPath, string imageOutputPath, string imageName, ImageFormat imageFormat, Definition definition)
        {
            PDFFile pdfFile = PDFFile.Open(pdfInputPath);
            if (!Directory.Exists(imageOutputPath))
            {
                Directory.CreateDirectory(imageOutputPath);
            }
            int startPageNum = 1;
            int endPageNum = pdfFile.PageCount;
            //  var bitMap = new Bitmap[endPageNum];
            for (int i = startPageNum; i <= endPageNum; i++)
            {
                try
                {
                    Bitmap pageImage = pdfFile.GetPageImage(i - 1, 56 * (int)definition);
                    int canKao = pageImage.Width > pageImage.Height ? pageImage.Height : pageImage.Width;
                    int newHeight = canKao > 1080 ? pageImage.Height / 2 : pageImage.Height;
                    int newWidth = canKao > 1080 ? pageImage.Width / 2 : pageImage.Width;
                    Bitmap newPageImage = new Bitmap(newWidth, newHeight);
                    Graphics g = Graphics.FromImage(newPageImage);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //重新画图的时候Y轴减去40，高度也减去40  这样水印就看不到了
                    g.DrawImage(pageImage, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 40, pageImage.Width, pageImage.Height - 40), GraphicsUnit.Pixel);
                    newPageImage.Save(imageOutputPath + imageName + i.ToString() + "." + imageFormat.ToString(), imageFormat);
                    g.Dispose();
                    newPageImage.Dispose();
                    pageImage.Dispose();
                }
                catch (Exception ex)
                {

                }
            }
            //合并图片
            //  var mergerImg = MergerImg(bitMap);
            //保存图片
            //  mergerImg.Save(imageOutputPath, imageFormat);
            pdfFile.Dispose();
        }

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="maps"></param>
        /// <returns></returns>
        private static Bitmap MergerImg(params Bitmap[] maps)
        {
            int i = maps.Length;
            if (i == 0)
                throw new Exception("图片数不能够为0");
            else if (i == 1)
                return maps[0];
            //创建要显示的图片对象,根据参数的个数设置宽度
            Bitmap backgroudImg = new Bitmap(maps[0].Width, i * maps[0].Height);
            Graphics g = Graphics.FromImage(backgroudImg);
            //清除画布,背景设置为白色
            g.Clear(System.Drawing.Color.White);
            for (int j = 0; j < i; j++)
            {
                g.DrawImage(maps[j], 0, j * maps[j].Height, maps[j].Width, maps[j].Height);
            }
            g.Dispose();
            return backgroudImg;
        }
    }
}
