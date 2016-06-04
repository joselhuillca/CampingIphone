using System;
using UIKit;
using Foundation;
using CoreGraphics;
using ObjCRuntime;
using System.IO;

namespace PeruCamping
{
	public class UtilManagment
	{
		public UtilManagment ()
		{
		}

		//This static method convert a cgrect that has the RGB representation into UIColor
		public static UIColor getColorFromRect(CGRect rectColor)
		{
			//return [UIColor colorWithRed:rectColor.origin.x/255 green:rectColor.origin.y/255 blue:rectColor.size.width/255 alpha:rectColor.size.height];
			return new UIColor(rectColor.Location.X/255,rectColor.Location.Y/255,rectColor.Size.Width/255,rectColor.Size.Height);
		}

		//return 0 if the device is in vertical orientation and 1 where it's in horizontal orientation
		public static int orientation()
		{
			var currentOrientation = UIApplication.SharedApplication.StatusBarOrientation;
			bool isPortrait = currentOrientation == UIInterfaceOrientation.Portrait 
				|| currentOrientation == UIInterfaceOrientation.PortraitUpsideDown;

			return isPortrait ? 0: 1;
		}

		public static UIImage imageFromDocumentDirectory(NSString imageName)
		{
			String imagePathInner = String.Format("{0}{1}", "Archive.wdgt", imageName);
			UIImage theImage =  UIImage.FromFile(imagePathInner);

			return theImage;
		}

		public static string htmlStringFromDocumentDirectory(NSString imageName)
		{
			String imagePathInner = String.Format("{0}{1}", "Archive.wdgt", imageName);
			var urlPath = NSUrl.FromFilename(imagePathInner);
			string fileContents = "";

			using (FileStream fileStream = File.Open(urlPath.Path, FileMode.Open, FileAccess.Read, FileShare.None)) {

				using (StreamReader reader = new StreamReader(fileStream))
				{
					fileContents = reader.ReadToEnd();
				}

			}
			return fileContents;
		}
 

		public static UIImage imageByScalingAndCroppingForSize(CGSize targetSize,UIImage image,CGPoint point)
		{
			UIImage sourceImage = image;
			UIImage newImage = null;        
			CGSize imageSize = sourceImage.Size;
			float width = (float)imageSize.Width;
			float height = (float)imageSize.Height;
			float targetWidth = (float)targetSize.Width;
			float targetHeight = (float)targetSize.Height;
			float scaleFactor = 0.0f;
			float scaledWidth = targetWidth;
			float scaledHeight = targetHeight;
			CGPoint thumbnailPoint = new CGPoint(0,0);
			CGPoint principalPoint = new CGPoint(point.X/width, point.Y/width);

			if (CGSize.Equals(imageSize, targetSize) == false) 
			{
				float widthFactor = targetWidth / width;
				float heightFactor = targetHeight / height;

				if (widthFactor > heightFactor) 
					scaleFactor = widthFactor; // scale to fit height
				else
					scaleFactor = heightFactor; // scale to fit width
				scaledWidth  = width * scaleFactor;
				scaledHeight = height * scaleFactor;

				// center the image
				if (widthFactor > heightFactor)
				{
					thumbnailPoint.Y = (targetHeight - scaledHeight) * principalPoint.Y; 
				}
				else 
					if (widthFactor < heightFactor)
					{
						thumbnailPoint.X = (targetWidth - scaledWidth) * principalPoint.X;
					}
			} 

			UIGraphics.BeginImageContext(targetSize); // this will crop

			CGRect thumbnailRect = new CGRect();
			thumbnailRect.Location = thumbnailPoint;
			thumbnailRect.Size = new CGSize (scaledWidth,scaledHeight);

			sourceImage.DrawAsPatternInRect(thumbnailRect);

			newImage = UIGraphics.GetImageFromCurrentImageContext();
			if (newImage == null) {
				//pop the context to get back to the default
				UIGraphics.EndImageContext();
			}
			return newImage;

		}
	}
}

