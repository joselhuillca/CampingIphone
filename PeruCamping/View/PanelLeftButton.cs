using System;
using UIKit;
using Foundation;
using CoreGraphics;
using ObjCRuntime;

namespace PeruCamping
{
	public class PanelLeftButton : UIBaseView
	{
		//image view that will be show at view
		private UIImageView imageView;
		//title that will be show at view
		private UILabel titleLabel;
		// number that will be show at view
		private UILabel numberLabel;
		private UIButton button;

		private int posLinkArray;
		private NSTimer timer;

		public float timeInterval;
		public NSArray linkArray;
		public NSString urlToOpen;

		bool image_exist = false;

		public PanelLeftButton (NSString title,NSString imageName,int number)
		{
			//imageView = [[UIImageView alloc] initWithImage:[UtilManagment imageFromDocumentDirectory:imageName]];

			if(imageName!=null){
				imageView = new UIImageView(UtilManagment.imageFromDocumentDirectory(imageName));
				//imageView.backgroundColor = [UIColor clearColor];
				imageView.BackgroundColor = UIColor.Clear;
				//[self addSubview:imageView];
				this.AddSubview(imageView);
				image_exist = true;
			}

			titleLabel = new UILabel();        
			titleLabel.Text = title;          
			titleLabel.BackgroundColor = UIColor.Clear;
			this.AddSubview(titleLabel);

			numberLabel = new UILabel ();
			if (number >= 0)
				numberLabel.Text = String.Format ("{0}", number);
			numberLabel.BackgroundColor = UIColor.Clear;
			this.AddSubview(numberLabel);

			//button = [UIButton buttonWithType:UIButtonTypeCustom];
			button = UIButton.FromType(UIButtonType.Custom);
			//[button addTarget:self action:@selector(tap:) forControlEvents:UIControlEventTouchUpInside];        
			button.AddTarget(null,new Selector("tap:"),UIControlEvent.TouchUpInside);
			this.AddSubview(button);

			posLinkArray = 0;

			timeInterval = 10.0f;
		}

		public void changeNumberValue(int number)
		{
			numberLabel.Text = String.Format ("{0}", number);
			this.reDraw();
		}

		/**
 			here the image number and title are relocated
 		*/
		[Export ("reDraw")]
		public override void reDraw(){
			base.reDraw();
			if (image_exist == true) {
				imageView.Frame = new CGRect (this.Frame.Size.Width / 2 - 25, this.Frame.Size.Height - 52, 25, 22);
			}
			nuint idxDraw = this.attributePositionToRedraw ();
			var colorAtt = this.getFontColorTitleAttribute ();

			if (colorAtt != null) {
				NSString str = colorAtt.GetItem<NSString> (idxDraw);

				CGRect rect = CGRectFromString (str);

				titleLabel.TextColor = UtilManagment.getColorFromRect (rect); 
				titleLabel.Frame = new CGRect (10, this.Bounds.Size.Height - 20, this.Bounds.Size.Width - 10, 20);

				//numberLabel.textColor = [UtilManagment getColorFromRect:CGRectFromString([[self getFontColorTitleAttribute] objectAtIndex:[self attributePositionToRedraw]])]; 
				numberLabel.TextColor = UtilManagment.getColorFromRect (CGRectFromString (this.getFontColorTitleAttribute ().GetItem<NSString> ((nuint)(this.attributePositionToRedraw ()))));
				//numberLabel.font = [UIFont fontWithName:[[self getFontTitleAttribute] objectAtIndex:[self attributePositionToRedraw]] size:[[[self getFontSizeTitleAttribute] objectAtIndex:[self attributePositionToRedraw]] intValue]];
				Double fontsize = Double.Parse (this.getFontSizeTitleAttribute ().GetItem<NSString> ((nuint)(this.attributePositionToRedraw ())));
				numberLabel.Font = UIFont.FromName (this.getFontTitleAttribute ().GetItem<NSString> (this.attributePositionToRedraw ()), (nfloat)(fontsize));
				//numberLabel.frame = CGRectMake(self.bounds.size.width/2+5, self.bounds.size.height-52, self.bounds.size.width/2, 22);
				numberLabel.Frame = new CGRect (this.Bounds.Size.Width / 2 + 5, this.Bounds.Height - 52, this.Bounds.Size.Width / 2, 22);
			}
			button.Frame = this.Bounds;

		}

		[Export ("tap:")]
		public void tap(NSObject sender){       
			NSUrl fileURL =new NSUrl(NSBundle.MainBundle.PathForResource("sounds/select1","wav"));
			this.managerController.audioController.playAudioWithUrl(fileURL);
			if (urlToOpen!=null) {
				//[[UIApplication sharedApplication] openURL: [NSURL URLWithString:urlToOpen]];
				UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(urlToOpen));
				return;
			}

			else if (linkArray.GetItem<NSObject>((nuint)posLinkArray)!=null) {
				//[self.managerController showViewForViewName:[linkArray objectAtIndex:posLinkArray]];
				this.managerController.showViewForViewName(linkArray.GetItem<NSString>((nuint)posLinkArray));
			}    
		}

		public void stopAnimations(){
			timer.Invalidate ();
			timer = null;
		}

		public void startAnimations(){
			//timer = [NSTimer scheduledTimerWithTimeInterval:timeInterval target:self selector:@selector(changePos:) userInfo:nil repeats:YES];
			timer = NSTimer.CreateScheduledTimer(timeInterval,this,new Selector("changePos:"),null,true);
		}

		[Export ("changePos:")]
		public void changePos( NSObject sender){
			if (posLinkArray == (int)(linkArray.Count)) {
				posLinkArray = 0;
			}else posLinkArray ++;

		}

		public void setTimeInterval(float timeInterval1){
			if (timeInterval1>0) {
				this.timeInterval = timeInterval1;
			}
		}

		public override void outAnimation(){  
			base.outAnimation ();

			//this.outAnimationUpDownRightLeft (new CGRect (0, 1, 1, 1), Constantes.transitionTime, 0, this);

		}

		public override void inAnimation(){
			base.inAnimation ();  

			//this.inAnimationUpDownRightLeft(new CGRect(0, 1, 1, 1),Constantes.transitionTime,0,this);

		}

		public override void prepareRemove(){
			base.prepareRemove();
			imageView = null;
			titleLabel = null;
			numberLabel = null;
			button = null;
			timer = null;
			linkArray = null;
			urlToOpen = null;
		}
	}
}

