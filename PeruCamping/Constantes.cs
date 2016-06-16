using System;
using System.Drawing;
using CoreGraphics;
using UIKit;

namespace PeruCamping
{
	public class Constantes
	{
		public Constantes()
		{
		}

		public static string STORE_CHANGED_STATUS_NOTIFICATION = "StoreChangedStatus";

		public static float transitionTime = 0.4f;

		// resize the image to be contained within a maximum width and height, keeping aspect ratio
		public static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
		{
			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1) return sourceImage;
			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;
			UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
			sourceImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage;
		}

		//Scale and Crop UIImage
		public static UIImage ScaleToSize(UIImage image, int width, int height)
		{
			UIGraphics.BeginImageContext(new SizeF(width, height));
			CGContext ctx = UIGraphics.GetCurrentContext();
			float ratio = (float)width / (float)height;

			ctx.AddRect(new RectangleF(0.0f, 0.0f, width, height));
			ctx.Clip();

			var cg = image.CGImage;
			float h = cg.Height;
			float w = cg.Width;
			float ar = w / h;

			if (ar != ratio)
			{
				// Image's aspect ratio is wrong so we'll need to crop
				float scaleY = height / h;
				float scaleX = width / w;
				PointF offset;
				SizeF crop;
				float size;

				if (scaleX >= scaleY)
				{
					size = h * (w / width);
					offset = new PointF(0.0f, h / 2.0f - size / 2.0f);
					crop = new SizeF(w, size);
				}
				else
				{
					size = w * (h / height);
					offset = new PointF(w / 2.0f - size / 2.0f, 0.0f);
					crop = new SizeF(size, h);
				}

				// Crop the image and flip it to the correct orientation (otherwise it will be upside down)
				ctx.ScaleCTM(1.0f, -1.0f);
				using (var copy = cg.WithImageInRect(new RectangleF(offset, crop)))
				{
					ctx.DrawImage(new RectangleF(0.0f, 0.0f, width, -height), copy);
				}
			}
			else
			{
				image.Draw(new RectangleF(0.0f, 0.0f, width, height));
			}

			UIImage scaled = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return scaled;
		}
	}
}

