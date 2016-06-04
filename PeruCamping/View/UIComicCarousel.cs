using System;
using UIKit;
using Foundation;
using CoreGraphics;
using System.Collections.Generic;
using System.Linq;
//using ayarlaleyendaXamariniOS;

namespace PeruCamping
{
	
	public class UIComicCarousel : UIBaseView
	{
		 
		private UILabel textLabel;

		public NSString text;

		UIScrollView carousel;


		nfloat h = 150.0f;
		nfloat w = 86.0f;
		nfloat padding = 5.0f;
 		List<Avatar> items;
		public UIComicCarousel(List<Avatar> objects, NSArray itemAttribute)
		{
			var itemSize = itemAttribute.GetItem<NSString>((nuint)(this.attributePositionToRedraw()));
			var itemRect = CGRectFromString(itemSize);
			this.w = itemRect.Width;
			this.h = itemRect.Height;


			this.items = objects;
			carousel = new UIScrollView{
				Frame = new CGRect(0, 0, this.Bounds.Width, h + 2 * padding),
				ContentSize = new CGSize((w + padding) * items.Count, h),
				BackgroundColor = UIColor.Clear,
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth

			};
 
			//carousel.DataSource = new CarouselDataSource(objects);

			carousel.AutoresizingMask =   UIViewAutoresizing.FlexibleHeight;


			for (int i = 0; i < items.Count; i++)
			{
				UIView view = ViewForAvatars(i);
				view.Frame = new CGRect(padding * (i + 1) + (i * w), padding, w, h);

				carousel.AddSubview( view );

				//buttons.Add(button);
			}
 
			// handle item selections
		
			 
			this.AddSubview(carousel);

		}

		 UIView ViewForAvatars(nint index )
		{
			

			var view = new UIView(new CGRect(0, 0, w, h + 30) );
			//view.Alignment = UIStackViewAlignment.Center;
			//view.Distribution = UIStackViewDistribution.FillEqually;
			//view.Axis = UILayoutConstraintAxis.Vertical;

			UITextView label = null;
			UIImageView imageView = null;
			view.AddGestureRecognizer (new UITapGestureRecognizer ((t) => {
				tap(t, items[(int)index].navigability);

			}));
		 
				// create new view if no view is available for recycling
			imageView = new UIImageView(new CGRect(0, 0, w, h));
			imageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			UIImage imgAvatar = UtilManagment.imageFromDocumentDirectory(new NSString(items[(int)index].imageAvatar));
			imageView.Image = imgAvatar;
			imageView.ContentMode = UIViewContentMode.Center;

			label = new UITextView(new CGRect(0, 114, w, 30));
			label.BackgroundColor = UIColor.Clear;
			label.Text = items[(int)index].nameAvatar;

 
 			label.TextColor = UIColor.White;

			label.TextAlignment = UITextAlignment.Center;
			//label.Font = label.Font.WithSize(12);


			view.Add(imageView);
			view.Add(label);
		  
			// set the values of the view
		
			return view;
		} 

		public override void reDraw()
		{
			base.reDraw();
		 
			nuint idxDraw = this.attributePositionToRedraw();
			var colorAtt = this.getFontColorTitleAttribute();



			carousel.Frame = this.Bounds;
		}

		public void tap(NSObject sender, string navUIView){       
			NSUrl fileURL =new NSUrl(NSBundle.MainBundle.PathForResource("sounds/select1","wav"));
			this.managerController.audioController.playAudioWithUrl(fileURL);
			 
			if (navUIView != null) {
 
				this.managerController.showViewForViewName( navUIView );
			}    
		}


		public override void inAnimation()
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
			textLabel = null;
			text = null;
		}
	}
}

