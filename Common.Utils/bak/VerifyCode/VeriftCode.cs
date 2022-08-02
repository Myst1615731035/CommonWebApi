using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Common.Utils
{
	public class ImgVerifyCode
	{
		private const string _codes = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz";

		public static string GetCode(int count)
		{
			Random random = new Random(DateTime.Now.Millisecond);
			string text = string.Empty;
			for (int i = 0; i < count; i++)
			{
				text += _codes[random.Next(0, _codes.Length)].ToString();
			}
			return text;
		}

		public static byte[] DrawImage(string code, int width, int height, int fontSize)
		{
			if (code == null || code.Trim().Length == 0)
			{
				return new byte[0];
			}
			if (width < 1)
			{
				width = code.Length * 30;
			}
			if (height < 1)
			{
				height = 40;
			}
			if (fontSize < 1)
			{
				fontSize = 18;
			}
			int num = (int)Math.Floor((double)width * 1.0 / (double)code.Length);
			Bitmap bitmap = new Bitmap(code.Length * num, height + 4);
			Graphics graphics = Graphics.FromImage(bitmap);
			try
			{
				Random random = new Random();
				graphics.Clear(Color.White);
				for (int i = 0; i < 20; i++)
				{
					graphics.DrawLine(new Pen(Color.Silver), random.Next(bitmap.Width), random.Next(bitmap.Height), random.Next(bitmap.Width), random.Next(bitmap.Height));
				}
				for (int j = 0; j < 2; j++)
				{
					graphics.DrawLine(new Pen(Color.SlateGray, 1f), 0, random.Next(bitmap.Height), bitmap.Width, random.Next(bitmap.Height));
				}
				Color[] arrayColor = new Color[10]
				{
				Color.Black,
				Color.Red,
				Color.DarkRed,
				Color.DarkBlue,
				Color.Green,
				Color.DarkOrange,
				Color.Brown,
				Color.DarkCyan,
				Color.Purple,
				Color.Indigo
				};
				string[] arrayFont = new string[7]
				{
				"Verdana",
				"Microsoft Sans Serif",
				"Comic Sans MS",
				"Arial",
				"宋体",
				"楷体",
				"黑体"
				};
				for (int k = 0; k < code.Length; k++)
				{
					Font font = new Font(arrayFont[random.Next(0, arrayFont.Length)], fontSize, FontStyle.Bold | FontStyle.Italic);
					Rectangle rectangle = new Rectangle(num * k, 1, num, height);
					LinearGradientBrush brush = new LinearGradientBrush(rectangle, arrayColor[random.Next(0, arrayColor.Length)], arrayColor[random.Next(0, arrayColor.Length)], 1.5f, isAngleScaleable: true);
					StringFormat stringFormat = new StringFormat();
					stringFormat.Alignment = StringAlignment.Center;
					graphics.DrawString(code[k].ToString(), font, brush, rectangle, stringFormat);
				}
				for (int l = 0; l < 100; l++)
				{
					bitmap.SetPixel(random.Next(bitmap.Width), random.Next(bitmap.Height), Color.FromArgb(random.Next()));
				}
				graphics.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
					return memoryStream.ToArray();
				}
			}
			finally
			{
				graphics.Dispose();
				bitmap.Dispose();
			}
		}
	}
}
