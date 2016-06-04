using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace PeruCamping
{
	public class XboxBackView : UIBaseView
	{
		private UIButton closeButton;
		private UIBaseView backView;
		private UILabel titleLabel; 
		private UIImage flechaImage;
		private NSString flechaPath;

		public NSString backViewName;

		public XboxBackView (UIBaseView view ,NSString title,NSString flecha)
		{
			backView = view;

			//closeButton = [UIButton buttonWithType:UIButtonTypeCustom] ;
			closeButton = UIButton.FromType(UIButtonType.Custom);
			closeButton.SizeToFit();
			//[closeButton addTarget:self action:@selector(changeScreenView:) forControlEvents:UIControlEventTouchUpInside];
			//closeButton.AddTarget(this.GetTargetForAction(new Selector("changeScreenView:"),null),null,UIControlEvent.TouchUpInside);
			closeButton.AddTarget(null,new Selector("changeScreenView:"),UIControlEvent.TouchUpInside);
			//[self addSubview:closeButton];
			this.AddSubview(closeButton);

			titleLabel = new UILabel ();
			titleLabel.Text = title;
			titleLabel.BackgroundColor = UIColor.Clear;
			this.AddSubview(titleLabel);

			//flechaImage = [UIImage imageNamed:flecha];
			flechaImage = UIImage.FromFile(flecha);
		}


		public XboxBackView (NSString viewName ,NSString title, NSString flecha)
		{
			backViewName = viewName;

			//closeButton = [UIButton buttonWithType:UIButtonTypeCustom] ;
			closeButton = UIButton.FromType(UIButtonType.Custom);
			//[closeButton sizeToFit];
			closeButton.SizeToFit();
			//[closeButton addTarget:self action:@selector(changeScreenView:) forControlEvents:UIControlEventTouchUpInside];
			//closeButton.AddTarget(this.GetTargetForAction(new Selector("changeScreenView:"),null),null,UIControlEvent.TouchUpInside);
			closeButton.AddTarget(null,new Selector("changeScreenView:"),UIControlEvent.TouchUpInside);
			this.AddSubview(closeButton);

			titleLabel = new UILabel();
			titleLabel.Text = title;
			titleLabel.BackgroundColor = UIColor.Clear;
			this.AddSubview(titleLabel);

			flechaPath = flecha;
			//flechaImage = [UtilManagment imageFromDocumentDirectory:flecha];  
		}

		[Export("reDraw")]
		public override void reDraw(){
			base.reDraw();

			if (flechaImage==null) {
				//flechaImage = [UtilManagment imageFromDocumentDirectory:flechaPath]; 
				flechaImage = UtilManagment.imageFromDocumentDirectory(flechaPath);
			} 

			if (UtilManagment.orientation()==0) {
				closeButton.Frame = new CGRect (12, 0, 88, 111); 
			}else{
				closeButton.Frame = new CGRect (45,35, 88, 88);
			} 

			if (UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Phone) {
				//[closeButton setImage:flechaImage forState:UIControlStateNormal]; 
				closeButton.SetImage(flechaImage,UIControlState.Normal);
			}else{
				//[closeButton setImage:flechaImage forState:UIControlStateNormal]; 
				closeButton.SetImage(flechaImage,UIControlState.Normal);
			}
			nuint position = (nuint)(this.attributePositionToRedraw());
			//titleLabel.textColor = [UtilManagment getColorFromRect:CGRectFromString([[self getFontColorTitleAttribute] objectAtIndex:[self attributePositionToRedraw]])]; 
			titleLabel.TextColor = UtilManagment.getColorFromRect(CGRectFromString(this.getFontColorTitleAttribute().GetItem<NSString>(position)));
			//titleLabel.font = [UIFont fontWithName:[[self getFontTitleAttribute] objectAtIndex:[self attributePositionToRedraw]] size:[[[self getFontSizeTitleAttribute] objectAtIndex:[self attributePositionToRedraw]] intValue]];
			titleLabel.Font = UIFont.FromName(this.getFontTitleAttribute().GetItem<NSString>(position),(nfloat)(Double.Parse(this.getFontSizeTitleAttribute().GetItem<NSString>(position))));
			//titleLabel.frame = CGRectMake(closeButton.frame.origin.x+closeButton.frame.size.width+10, closeButton.frame.origin.y+33, self.frame.size.width-(closeButton.frame.origin.x+closeButton.frame.size.width),self.frame.size.height/20);  
			titleLabel.Frame = new CGRect(closeButton.Frame.X+closeButton.Frame.Size.Width+10,closeButton.Frame.Y+33,this.Frame.Width-(closeButton.Frame.X+closeButton.Frame.Size.Width),this.Frame.Size.Height/20);
		}

		[Export ("changeScreenView:")]
		public void changeScreenView(NSObject sender) { 
			NSUrl fileURL =new NSUrl(NSBundle.MainBundle.PathForResource("sounds/snd_buttonback1","wav"));
			this.managerController.audioController.playAudioWithUrl(fileURL);
			if (backView!=null) {
				//[UIView beginAnimations:@"CLOSEFULLSCREEN" context:NULL];
				UIView.BeginAnimations("CLOSEFULLSCREEN");
				UIView.SetAnimationDuration(0.30f);

				this.Alpha = 0;
				backView.Alpha = 1;

				UIView.SetAnimationDelegate(this);    
				UIView.CommitAnimations ();
			}else{
				this.managerController.showViewForViewName(backViewName);
			}
		}

		public NSArray getContentBounds()
		{
			//NSMutableArray *contentFrames = [NSMutableArray arrayWithArray:[self getFrameAttribute]] ;
			var contentFrames = NSMutableArray.FromArrayOfArray(this.getFrameAttribute());
			for (int k =0 ; k<contentFrames.Length ; k++) {
				CGRect f = CGRectFromString(contentFrames.GetValue(k).ToString());
				f.X = 0;
				f.Y = f.Size.Height*2/20;
				//f.Size.Height = f.Size.Height*19/20;
				var f_width = f.Size.Width;
				f.Size = new CGSize (f_width, f.Size.Height * 19 / 20);
				//[contentFrames replaceObjectAtIndex:k withObject:NSStringFromCGRect(f)];
				contentFrames.SetValue(this.NSStringFromCGRect(f),k);
			}
			//[contentView setFrameAttribute:contentFrames];

			var contentBounds = NSMutableArray.FromArrayOfArray (this.getFrameAttribute ());
			for (int k =0 ; k<contentBounds.Length ; k++) {
				CGRect f = CGRectFromString(contentBounds.GetValue(k).ToString());
				f.X = 0;
				f.Y = 0;        
				contentBounds.SetValue(this.NSStringFromCGRect(f),k);
			}
			return NSArray.From (contentBounds);

		}

		NSString NSStringFromCGRect (CGRect f) {
			return new NSString ( f.ToString () );
		}

		public override void inAnimation(){
			base.inAnimation();    
			this.inAnimationUpDownRightLeft(new CGRect(1, 0, 1, 1),Constantes.transitionTime, 0,titleLabel);    
		}

		public override void outAnimation(){
			base.outAnimation();

			this.outAnimationUpDownRightLeft(new CGRect(-1, 0, 1, 1),Constantes.transitionTime, 0,titleLabel);  

		}

		public override void prepareRemove(){
			base.prepareRemove();
			closeButton = null;
			backView = null;
			titleLabel = null;
			flechaImage = null;
			flechaPath = null;
			backViewName = null;
		}

	}
}

