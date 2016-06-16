using System;
using UIKit;
using Foundation;
using CoreGraphics;
using ObjCRuntime;
using System.Drawing;

namespace PeruCamping
{
	public class ScrollDelegatePageControl : UIScrollViewDelegate
	{
		ScrollPageControllView scrollPage;
		public ScrollDelegatePageControl(ScrollPageControllView refs)
		{
			this.scrollPage = refs;

		}

		public override void DecelerationEnded(UIScrollView scrollView)
		{
			this.scrollPage.scrollViewDidEndDecelerating(scrollView);
		}

		public override void DecelerationStarted(UIScrollView scrollView)
		{
			this.scrollPage.ScrollViewWillBeginDragging(scrollView);
		}
	}

	public class ScrollPageControllView : UIBaseView, AnimationProtocol
	{

		// scroll that contains a UIView for each title 
		private UIScrollView scroll;
		// array that contains a UILabel of each title. 
		private NSMutableArray titleArray;
		// represent a page in the scroll and the position of the title en the titleArray 

		private NSMutableArray subviews;
		private NSMutableArray subviewsFrames;
		private bool firstTime;

		UIImageView episode0;

		private NSMutableArray backgroundView;

		private bool firstTimeScroll;

		public NSMutableArray subviewCache;

		public nuint actualPage;
		public nuint actualPage_static;
		public string name;

		private UIButton closeButton;//Huillca
		private UIImage cerrarImage;//Huillca
		public NSString btnCerrar;
		public nint numSubviews;
		public string backView;

		private UIPageControl pageControl;

		public ScrollPageControllView(NSString btnC)
		{
			scroll = new UIScrollView();
			scroll.PagingEnabled = true;
			var del = new ScrollDelegatePageControl(this);

			scroll.Delegate = del;

			scroll.ShowsHorizontalScrollIndicator = false;
			scroll.ShowsVerticalScrollIndicator = false;
			this.AddSubview(scroll);

			titleArray = new NSMutableArray();
			actualPage = 0;
			actualPage_static = 0;
			btnCerrar = btnC;

			subviews = new NSMutableArray();
			subviewsFrames = new NSMutableArray();
			firstTime = true;

			backgroundView = new NSMutableArray();
			firstTimeScroll = true;

			subviewCache = new NSMutableArray();

			closeButton = UIButton.FromType(UIButtonType.Custom);
			closeButton.SizeToFit();
			closeButton.AddTarget(null, new Selector("changeScreenView_scroll:"), UIControlEvent.TouchUpInside);
			this.AddSubview(closeButton);

			episode0 = new UIImageView(UIImage.FromFile("episode0.png"));
			if (btnCerrar.Equals(new NSString("true")))
			{
				this.AddSubview(episode0);
			}



			cerrarImage = UIImage.FromFile("Archive.wdgt/edutic/wallpapers/close.png");
			//PAGECONTROL-----------------------------------------------

			this.pageControl = new UIPageControl();
			this.pageControl.HidesForSinglePage = true;
			this.pageControl.ValueChanged += HandlePageControlHeadValueChanged;
			//this.pageControl.BackgroundColor = UIColor.DarkGray;
			this.pageControl.PageIndicatorTintColor = UIColor.White;
			this.pageControl.CurrentPageIndicatorTintColor = UIColor.Red;
			this.AddSubview(this.pageControl);

		}

		NSString NSStringFromCGRect(CGRect f)
		{
			string rect_ = "{" + (f.X).ToString() + "," + (f.Y).ToString() + "},{" + (f.Width).ToString() + "," + (f.Height).ToString() + "}";
			return new NSString(rect_);
		}

		public void addSubview(NSDictionary view, NSString title)
		{
			UILabel titleLabel = new UILabel();

			UITapGestureRecognizer tapGesture = new UITapGestureRecognizer((t) =>
			{
				handleTapGesture(t);
			});
			titleLabel.AddGestureRecognizer(tapGesture);
			titleArray.Add(titleLabel);


			NSObject[] frames_array = NSMutableArray.FromArray<NSObject>(this.getFrameAttribute());
			NSMutableArray frames = new NSMutableArray();
			frames.AddObjects(frames_array);
			for (nuint k = 0; k < frames.Count; k++)
			{
				CGRect f = CGRectFromString(frames.GetItem<NSString>(k));
				f.X = f.Size.Width * (titleArray.Count - 1);
				f.Y = 0;
				frames.ReplaceObject((nint)k, NSStringFromCGRect(f));
			}
			subviewsFrames.Add(frames);
			subviews.Add(view);

			this.createViewAtIndex(subviews.Count - 1);

			this.pageControl.Pages = numSubviews;
		}

		public void createViewAtIndex(nuint k)
		{

			NSDictionary dic = this.subviews.GetItem<NSDictionary>(k);

			String typeClass = String.Format("ayarlaleyendaXamariniOS.{0}Generator", dic.ValueForKey(new NSString("dic")).ValueForKey(new NSString("class")));
			//Console.WriteLine (typeClass);
			Type t = Type.GetType(typeClass);
			AbstractViewGenerator classGenerator = Activator.CreateInstance(t) as AbstractViewGenerator;

			var dicccc = dic.ValueForKey(new NSString("dic")) as NSDictionary;
			var naviDic = dic.ValueForKey(new NSString("nav")) as NSDictionary;
			UIBaseView actualView = classGenerator.generateFromDictionary(dicccc, naviDic, this.managerController);

			actualView.setFrameAttribute(subviewsFrames.GetItem<NSArray>(k));
			actualView.reDraw();

			for (nuint i = 0; i < backgroundView.Count; i++)
			{
				UIView vw = backgroundView.GetItem<UIView>(i);
				vw.Alpha = 0;
			}
			firstTimeScroll = true;
			scroll.AddSubview(actualView);
			subviewCache.Add(actualView);
			actualView = null;

		}

		public void addSubViewScroll(UIBaseView view)
		{
			view.Alpha = 0;
			scroll.AddSubview(view);
			backgroundView.Add(view);
		}

		//when a MenuScroll is created, this method must called, at the end to refresh the menu selection
		public void refresh()
		{
			this.scrollViewDidEndDecelerating(scroll);
		}

		public void scrollViewDidEndDecelerating(UIScrollView scrollView)
		{
			if (firstTime)
			{
				firstTime = false;
			}
			else
			{
				NSUrl fileURL = NSUrl.FromFilename(NSBundle.MainBundle.PathForResource("sounds/tab_Switch1", "wav"));

				this.managerController.audioController.playAudioWithUrl(fileURL);
			}

			this.stopAnimations();

			actualPage = (nuint)(scroll.ContentOffset.X / scroll.Frame.Size.Width);
			actualPage = actualPage < 0 || actualPage >= titleArray.Count ? 0 : actualPage;

			pageControl.CurrentPage = (nint)actualPage;

			this.startAnimations();


		}


		//set an array that contains four colors's of the title when it is selected,that depend of the device and orientation.
		public void setTitleSelectedColorAttribute(NSArray selectedColors)
		{
			attributes.SetValueForKey(selectedColors.Copy(), new NSString("selectedColors"));
		}

		//return an array that contains four colors's of the title when it is selected,that depend of the device and orientation. 
		public NSArray getTitleSelectedColorAttribute()
		{
			return (NSArray)(attributes.ValueForKey(new NSString("selectedColors")));
		}

		//set an array that contains four colors's of the title when it is not selected,that depend of the device and orientation.
		public void setTitleUnselectedColorAttribute(NSArray unselectedColors)
		{
			attributes.SetValueForKey(unselectedColors.Copy(), new NSString("unselectedColors"));
		}

		//return an array that contains four colors's of the title when it is not selected,that depend of the device and orientation. 
		public NSArray getTitleUnselectedColorAttribute()
		{
			return (NSArray)(attributes.ValueForKey(new NSString("unselectedColors")));
		}

		/** set an array that contains four frames that is the area where the title list will be shown,that depend of the device and orientation. 
		 The configuration is the following:
		 {{x,y},{width,spaceBetweenTitle}} ==> 'x','y','width' are the position of the scroll of title on the frame, and 'spaceBetweenTitle' is the space that will be between each title
		 */
		public void setTitleHeightAttribute(NSArray titleHeights)
		{
			attributes.SetValueForKey(titleHeights.Copy(), new NSString("titleHeights"));
		}

		public NSArray getTitleHeightAttribute()
		{
			return (NSArray)(attributes.ValueForKey(new NSString("titleHeights")));
		}

		public NSArray getFrameAttribute()
		{
			return (NSArray)(attributes.ValueForKey(new NSString("frame")));
		}

		public void ScrollViewWillBeginDragging(UIScrollView scrollView)
		{
			if (firstTimeScroll)
			{
				for (nuint i = 0; i < backgroundView.Count; i++)
				{
					UIView vw = backgroundView.GetItem<UIView>(i);
					vw.Alpha = 0;
				}
				firstTimeScroll = false;
			}
		}

		/* ----------------------------------GESTURERECONIGZER */
		// works when a tap gesture was apply to a title, moving the content of the scroll at correct position
		//*/
		public void handleTapGesture(UITapGestureRecognizer gestureRecognizer)
		{
			for (nuint i = 0; i < backgroundView.Count; i++)
			{
				UIView vw = backgroundView.GetItem<UIView>(i);
				vw.Alpha = 1;
			}

			NSUrl fileURL = NSUrl.FromFilename(NSBundle.MainBundle.PathForResource("sounds/tab_Switch1", "wav"));
			this.managerController.audioController.playAudioWithUrl(fileURL);
			scroll.ScrollRectToVisible(new CGRect(scroll.Frame.Size.Width * (titleArray.IndexOf(gestureRecognizer.View)), scroll.Frame.Location.Y, scroll.Frame.Size.Width, scroll.Frame.Size.Height), true);

			this.stopAnimations();

			//actualPage = titleArray.IndexOf (gestureRecognizer.View);


			this.startAnimations();
		}

		public void startAnimations()
		{
			var bv = (UIView)(subviewCache.GetItem<NSObject>((nuint)actualPage));
			var bv_ = bv as AnimationProtocol;
			if (bv_ != null)
			{
				bv_.startAnimations();
			}
			foreach (NSObject sv in bv.Subviews)
			{
				var sv_ = sv as AnimationProtocol;
				if (sv_ != null)
				{
					sv_.startAnimations();
				}
			}
		}

		public void stopAnimations()
		{
			for (nuint i = 0; i < subviewCache.Count; i++)
			{
				UIView bv = (UIView)(subviewCache.GetItem<NSObject>(i));
				var bv_ = bv as AnimationProtocol;
				if (bv_ != null)
				{
					bv_.stopAnimations();
				}
				foreach (NSObject sv in bv.Subviews)
				{
					var sv_ = sv as AnimationProtocol;
					if (sv_ != null)
					{
						sv_.stopAnimations();
					}
				}
			}
		}

		public override void inAnimation()
		{

			base.inAnimation();
			subviewCache.GetItem<UIBaseView>(actualPage).inAnimation();
		}

		public override void outAnimation()
		{
			base.outAnimation();
			this.stopAnimations();
			if (scroll.Subviews.Length > 0)
			{
				//para los Grid
			}

		}

		public override void prepareRemove()
		{
			base.prepareRemove();
			scroll = null;
			titleArray.RemoveAllObjects();
			titleArray = null;
			subviews.RemoveAllObjects();
			subviewsFrames.RemoveAllObjects();
			backgroundView.RemoveAllObjects();
			name = null;
			closeButton = null;
		}

		[Export("reDraw")]
		public override void reDraw()
		{

			base.reDraw();

			//CGRect rectScroll = CGRectFromString(this.getFrameAttribute ().GetItem <NSString>(this.attributePositionToRedraw()));
			scroll.Frame = this.Bounds;

			actualPage = actualPage_static;//HUILLCA
			pageControl.CurrentPage = (nint)actualPage;
			CGRect rect = CGRectFromString(this.getTitleHeightAttribute().GetItem<NSString>(this.attributePositionToRedraw()));
			pageControl.Frame = new CGRect(0, rect.Y, rect.Width, rect.Height);

			scroll.ContentSize = new CGSize(scroll.Frame.Size.Width * titleArray.Count, scroll.Frame.Size.Height);
			scroll.ScrollRectToVisible(new CGRect(scroll.Frame.Size.Width * actualPage, scroll.Frame.Location.Y, scroll.Frame.Size.Width, scroll.Frame.Size.Height), false);

			if (firstTime)
			{
				this.refresh();
				for (nuint i = 0; i < backgroundView.Count; i++)
				{

					UIView vw = backgroundView.GetItem<UIView>(i);
					vw.Alpha = 0;
				}
			}
			for (nuint i = 0; i < this.subviewCache.Count; i++)
			{

				var subview = this.subviewCache.GetItem<UIBaseView>(i);

				if (btnCerrar.Equals(new NSString("true")))
					subview.Frame = new CGRect(0, subview.Frame.Y, scroll.Frame.Width, scroll.Frame.Height);
				else
					subview.Frame = new CGRect(i * scroll.Frame.Width, subview.Frame.Y, scroll.Frame.Width, scroll.Frame.Height);

			}
			if (btnCerrar.Equals(new NSString("true")))
			{
				if (UtilManagment.orientation() == 0)
				{//portrait
					closeButton.Frame = new CGRect(680, 45, 78, 78); //
					episode0.Frame = new CGRect(515, 700, 250, 80);
				}
				else
				{//landscape
					closeButton.Frame = new CGRect(905, 35, 78, 78);
					episode0.Frame = new CGRect(665, 650, 335, 107);
				}
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				{
					closeButton.SetImage(cerrarImage, UIControlState.Normal);
				}
				else
				{
					closeButton.SetImage(cerrarImage, UIControlState.Normal);
				}
			}


		}

		[Export("reDrawSubView")]
		public override void reDrawSubView()
		{
			this.stopAnimations();
			foreach (NSObject view in scroll.Subviews)
			{
				if (view.RespondsToSelector(new Selector("reDraw")))
				{
					((UIBaseView)(view)).reDraw();
				}
			}
		}

		public void appearAllViews()
		{
			foreach (UIView vw in scroll.Subviews)
			{
				vw.Alpha = 1.0f;
			}
		}

		public void reDrawView(NSObject bv)
		{
			if (bv.RespondsToSelector(new Selector("SetAlpha:")))
			{
				((UIBaseView)(bv)).Alpha = 0.0f;
			}
			if (bv.RespondsToSelector(new Selector("reDraw")))
			{
				((UIBaseView)bv).reDraw();
			}

			UIView.BeginAnimations(null, new IntPtr());
			UIView.SetAnimationDuration(1.0f);
			UIView.SetAnimationDelay(1.0f);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);
			((UIView)bv).Alpha = 1.0f;
			UIView.CommitAnimations();

		}

		[Export("changeScreenView_scroll:")]
		public void changeScreenView_scroll(NSObject sender)
		{
			NSUrl fileURL = new NSUrl(NSBundle.MainBundle.PathForResource("sounds/snd_buttonback1", "wav"));
			this.managerController.audioController.playAudioWithUrl(fileURL);

			this.managerController.showViewForViewName(backView);


		}

		private void HandlePageControlHeadValueChanged(object sender, EventArgs e)
		{
			this.stopAnimations();

			this.scroll.SetContentOffset(new PointF((float)(this.pageControl.CurrentPage * this.scroll.Frame.Width), 0.0f), true);
			NSUrl fileURL = NSUrl.FromFilename(NSBundle.MainBundle.PathForResource("sounds/tab_Switch1", "wav"));
			this.managerController.audioController.playAudioWithUrl(fileURL);

			this.startAnimations();
		}
	}
}

