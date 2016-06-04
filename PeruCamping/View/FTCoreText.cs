using System;
using UIKit;
using Foundation;
using CoreGraphics;
 
namespace PeruCamping
{
	public class FTCoreText : UIBaseView
	{

		UITextView fancyView;

		public string text;

		public FTCoreText(string text)
		{
			 
			 


 
			CGRect textrect = this.Bounds; 

			fancyView = new UITextView(textrect );

			fancyView.AttributedText = GetAttributedStringFromHtml(text);

			this.text = text;


 			fancyView.BackgroundColor = UIColor.Clear;
			fancyView.Editable = false;
 
			fancyView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleBottomMargin;
			this.AddSubview(fancyView);

		}

		public static NSAttributedString GetAttributedStringFromHtml(string html)
		{
			NSError error = null;

			var secondAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.Black,
				Font = UIFont.FromName("HelveticaNeue", 14f)
			};
			var firstAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.FromRGB(121, 12, 30),
				Font = UIFont.FromName("HelveticaNeue-Bold", 14f)
			};

 			var htmlString = new NSAttributedString(html,
				new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML },
				ref error);
			 
 
			return htmlString;
		}
		public static NSAttributedString GetAttributedStringFromHtml(NSUrl html)
		{
			NSAttributedStringDocumentAttributes importParams = new NSAttributedStringDocumentAttributes();
			importParams.DocumentType = NSDocumentType.HTML;
			NSError error = null;
			//NSDictionary dict = new NSDictionary ();
			var attrString = new NSAttributedString(html, ref error);
			return attrString;
		}
		public static NSString GetHTML(NSAttributedString attributedString)
		{
			var exportParams = new NSAttributedStringDocumentAttributes();
			exportParams.DocumentType = NSDocumentType.HTML;
			NSError err = null;
			NSData htmlData = attributedString.GetDataFromRange(new NSRange(0, attributedString.Length),
				exportParams, ref err);
			var retString = new NSString(htmlData, NSStringEncoding.UTF8);
			return retString;
		}


		public override void reDraw()
		{
			base.reDraw();
			fancyView.Frame = this.Bounds;
			////NSLog(@"%@ redrow",NSStringFromClass(self.class));
 			nuint position = (nuint)(this.attributePositionToRedraw());
		 
 		}

		public  override void inAnimation()
		{
			base.inAnimation();
			base.inAnimationUpDownRightLeft(new CGRect(1, 0, 1, 1), Constantes.transitionTime, 0, this);
		}

		public override void outAnimation()
		{
			base.outAnimation();
			base.outAnimationUpDownRightLeft(new CGRect(-1, 0, 1, 1), Constantes.transitionTime, 0, this);
		}

		public override void prepareRemove()
		{
			base.prepareRemove();
			fancyView = null;
			text = null;
		}

	}


}

