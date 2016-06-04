using System;
using UIKit;
using CoreGraphics;
using ObjCRuntime;
//using ayarlaleyendaXamariniOS;
using Foundation;
using System.Collections.Generic;

namespace PeruCamping
{
	public class StarRatingView : UIBaseView
	{
		JSFavStarControl rating;

		/*public StarRatingView (float x,float y)
		{
			UIImage dot, star;
			dot = UIImage.FromFile ("starRating/starblack.png"); 
			star = UIImage.FromFile ("starRating/starwhite.png"); 
			//rating = [[JSFavStarControl alloc] initWithLocation:CGPointMake(110, 220) dotImage:dot starImage:star];
			rating = new JSFavStarControl(new CGPoint(110,220),dot,star);
			//[rating addTarget:self action:@selector(updateRating:) forControlEvents:UIControlEventValueChanged];
			rating.AddTarget(this,new Selector("updateRating:"),UIControlEvent.ValueChanged);


			this.AddSubview(rating);
		}

		[Export ("updateRating:")]
		public void updateRating(NSObject sender)
		{
			//NSLog(@"Rating: %d", [(JSFavStarControl *)sender rating]);
			//[label setText:[NSString stringWithFormat:@"%d", [(JSFavStarControl *)sender rating]]];

			int value = ((JSFavStarControl)sender).rating;

		}


		public override void reDraw(){
			base.reDraw();
			////NSLog(@"%@ redrow",NSStringFromClass(self.class));

			rating.Frame =  this.Bounds;
		}

		public void prepareRemove(){
			base.prepareRemove();
			rating = null;
		}*/
		public StarRatingView (float x,float y)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			_x = x;
			_y = y;

			Frame = new CGRect (_x, _y, _size * 5, _size);

			initControls ();
		}

		//settings
		nfloat _size =  18 ;
		nfloat _x = 0 ;
		nfloat _y = 0 ;


		List<RateStarView> rateList  = new List<RateStarView>();
		void initControls()
		{
			for (int i = 0; i < 5; i++) {
				RateStarView star = new RateStarView (i*_size , 0, i); 
				Add (star);
				rateList.Add (star);

				star.UserInteractionEnabled = true;

				UITapGestureRecognizer gesture = new UITapGestureRecognizer(()=>{
					setRate (star.index);
				});
				AddGestureRecognizer (gesture);
				gesture.NumberOfTapsRequired = 1 ; 
				star.AddGestureRecognizer (gesture);
			}
			setRate (2);
		}


		void setRate(int index)
		{
			for (int i = 0; i < 5; i++) {
				if(i<=index)
					rateList [i].startimg.Image = UIImage.FromFile ("starRating/starblack.png");
				else
					rateList [i].startimg.Image = UIImage.FromFile ("starRating/starwhite.png");
			} 
		}

	}

	 
	public class RateStarView : UIView
	{
		public RateStarView (nfloat x, nfloat y, int i)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			_x = x;
			_y = y;
			index = i;

			Frame = new CGRect (_x, _y, _size, _size);

			initControls (); 
		}


		//settings
		nfloat _size =  18 ;
		nfloat _x = 0 ;
		nfloat _y = 0 ;
		public int index = 0  ;

		public UIImageView startimg;

		void initControls()
		{
			startimg =  new UIImageView(new CGRect(0,0,_size,_size));
			startimg.Image = UIImage.FromFile ("starRating/starblack.png"); 
			Add (startimg);
		}

	}
}

