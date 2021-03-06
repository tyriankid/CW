using Hidistro.Core;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
namespace Hidistro.UI.Web
{
	public class VerifyCodeImage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				base.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
				string str = Globals.CreateVerifyCode(4);
				int maxValue = 45;
				int num2 = str.Length * 20;
				Bitmap image = new Bitmap(num2 - 3, 27);
				Graphics graphics = Graphics.FromImage(image);
				graphics.Clear(Color.AliceBlue);
				graphics.DrawRectangle(new Pen(Color.Black, 0f), 0, 0, image.Width - 1, image.Height - 3);
				System.Random random = new System.Random();
				Pen pen = new Pen(Color.LightGray, 0f);
				for (int i = 0; i < 50; i++)
				{
					int x = random.Next(0, image.Width);
					int y = random.Next(0, image.Height);
					graphics.DrawRectangle(pen, x, y, 1, 1);
				}
				char[] chArray = str.ToCharArray();
				StringFormat format = new StringFormat(StringFormatFlags.NoClip)
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center
				};
				Color[] colorArray = new Color[]
				{
					Color.Black,
					Color.Red,
					Color.DarkBlue,
					Color.Green,
					Color.Brown,
					Color.DarkCyan,
					Color.Purple,
					Color.DarkGreen
				};
				for (int j = 0; j < chArray.Length; j++)
				{
					int index = random.Next(7);
					random.Next(4);
					Font font = new Font("Microsoft Sans Serif", 17f, FontStyle.Bold);
					Brush brush = new SolidBrush(colorArray[index]);
					Point point = new Point(14, 11);
					float angle = (float)random.Next(-maxValue, maxValue);
					graphics.TranslateTransform((float)point.X, (float)point.Y);
					graphics.RotateTransform(angle);
					graphics.DrawString(chArray[j].ToString(), font, brush, 1f, 1f, format);
					graphics.RotateTransform(-angle);
					graphics.TranslateTransform(2f, (float)(-(float)point.Y));
				}
				System.IO.MemoryStream stream = new System.IO.MemoryStream();
				image.Save(stream, ImageFormat.Gif);
				base.Response.ClearContent();
				base.Response.ContentType = "image/gif";
				base.Response.BinaryWrite(stream.ToArray());
				graphics.Dispose();
				image.Dispose();
			}
			catch
			{
			}
		}
	}
}
