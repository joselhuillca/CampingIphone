using System;
using Foundation;
using UIKit;
using ObjCRuntime;
using CoreGraphics;

namespace PeruCamping
{
	//swipe
	public partial class MenuScrollView: UIBaseView,AnimationProtocol
	{
		// scroll that contains a UIView for each title 
		private UIScrollView scroll;
		//scroll that contains all title label 
		private UIScrollView titleScroll;
		// array that contains a UILabel of each title. 
		private NSMutableArray titleArray;    
		// represent a page in the scroll and the position of the title en the titleArray 

		private NSMutableArray subviews;
		private NSMutableArray subviewsFrames;
		private bool firstTime;

		private NSMutableArray backgroundView;

		private bool firstTimeScroll;

		public NSMutableArray subviewCache;

		public nuint actualPage; 
		public string name;

		public MenuScrollView ()
		{
			scroll = new UIScrollView();
			scroll.PagingEnabled = true;
			//scroll.Delegate = this;
			scroll.ShowsHorizontalScrollIndicator = false;
			scroll.ShowsVerticalScrollIndicator = false;
			this.AddSubview(scroll);

			titleScroll = new UIScrollView ();
			titleScroll.ShowsHorizontalScrollIndicator = false;
			titleScroll.ShowsVerticalScrollIndicator = false;
			this.AddSubview (titleScroll);

			titleArray = new NSMutableArray();
			actualPage = 0;  

			subviews = new NSMutableArray();
			subviewsFrames = new NSMutableArray();
			firstTime = true;

			backgroundView = new NSMutableArray();
			firstTimeScroll = true;

			subviewCache = new NSMutableArray();

		}
		NSString NSStringFromCGRect (CGRect f) {
			return new NSString ( f.ToString () );
		}

		public void addSubview(NSDictionary view,NSString title)
		{
			UILabel titleLabel = new UILabel();
			titleLabel.BackgroundColor = UIColor.Clear;
			titleLabel.Text = title;
			titleLabel.TextColor = UIColor.Gray;

			titleLabel.UserInteractionEnabled = true;  	

			UITapGestureRecognizer tapGesture =  new UITapGestureRecognizer(  (t) => {
				handleTapGesture(t);
			}); 
			titleLabel.AddGestureRecognizer(tapGesture);       
			//titleLabel.Delegate
			titleScroll.AddSubview( titleLabel ) ;    
			titleArray.Add(titleLabel );


			NSObject[] frames_array = NSMutableArray.FromArray<NSObject> (this.getFrameAttribute ()); 
			NSMutableArray frames = new  NSMutableArray (  );
			frames.AddObjects (frames_array); 
			for (nuint k =0 ; k<frames.Count ; k++) {
				CGRect f =   CGRectFromString(frames.GetItem<NSString>(k));
				f.X = f.Size.Width * (titleArray.Count -1);
				f.Y = 0;
				frames.ReplaceObject( (nint)k,  NSStringFromCGRect(f) );
			}    
			subviewsFrames.Add ( frames );
			subviews.Add(view );

			this.createViewAtIndex(subviews.Count-1);
		}



		public  void  createViewAtIndex(nuint  k ){

			NSDictionary dic =  this.subviews.GetItem<NSDictionary> ( k  );

			String typeClass = String.Format ("PeruCamping.{0}Generator", dic.ValueForKey (new NSString ("dic")).ValueForKey (new NSString ("class")));
			Console.WriteLine (typeClass);
			Type t = Type.GetType (typeClass);
			AbstractViewGenerator classGenerator = Activator.CreateInstance (t) as AbstractViewGenerator;

			var dicccc = dic.ValueForKey (new NSString ("dic")) as NSDictionary;
			var naviDic =  dic.ValueForKey(new NSString ("nav")) as NSDictionary; 
			UIBaseView actualView = classGenerator.generateFromDictionary(dicccc, naviDic,  this.managerController );

			actualView.setFrameAttribute( subviewsFrames.GetItem<NSArray>(k) );
			actualView.reDraw () ;

			for (nuint i = 0; i < backgroundView.Count; i++) {
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
			backgroundView.Add (view);
		}

		//when a MenuScroll is created, this method must called, at the end to refresh the menu selection
		public void refresh()
		{
			//[self scrollViewDidEndDecelerating:scroll];
			this.scrollViewDidEndDecelerating (scroll);
		}

		public void scrollViewDidEndDecelerating(UIScrollView scrollView ) {
			if (firstTime) {        
				firstTime = false;
			}else{
 				NSUrl fileURL = NSUrl.FromFilename(NSBundle.MainBundle.PathForResource("sounds/tab_Switch1","wav"));
 
				this.managerController.audioController.playAudioWithUrl ( fileURL);
			}

			UILabel  titleLabel = titleArray.GetItem<UILabel> ( actualPage) ;
			CGRect rect = CGRectFromString( this.getTitleUnselectedColorAttribute ().GetItem <NSString>(this.attributePositionToRedraw()) ); 
			titleLabel.TextColor = UtilManagment.getColorFromRect (rect) ;

			this.stopAnimations();

			actualPage = ( nuint )( scroll.ContentOffset.X/scroll.Frame.Size.Width  );
			actualPage = actualPage<0||actualPage>=titleArray.Count?0:actualPage;
			titleLabel = titleArray.GetItem<UILabel> (actualPage );

			CGRect rectc = CGRectFromString( this.getTitleSelectedColorAttribute ().GetItem <NSString>( this.attributePositionToRedraw () )  ); 
			titleLabel.TextColor = UtilManagment.getColorFromRect (rectc) ;   

			this.startAnimations(); 


			this.refreshTitleScrollFromLabelSelected(titleLabel);   

		}


		//set an array that contains four colors's of the title when it is selected,that depend of the device and orientation.
		public void setTitleSelectedColorAttribute(NSArray selectedColors)
		{
			attributes.SetValueForKey(selectedColors.Copy(),new NSString("selectedColors"));
		}

		//return an array that contains four colors's of the title when it is selected,that depend of the device and orientation. 
		public NSArray getTitleSelectedColorAttribute()
		{
			return (NSArray)(attributes.ValueForKey(new NSString("selectedColors")));
		}

		//set an array that contains four colors's of the title when it is not selected,that depend of the device and orientation.
		public void setTitleUnselectedColorAttribute(NSArray unselectedColors)
		{
			attributes.SetValueForKey (unselectedColors.Copy (), new NSString ("unselectedColors"));
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
			attributes.SetValueForKey(titleHeights.Copy(),new NSString("titleHeights"));
		}

		public NSArray getTitleHeightAttribute()
		{
			return (NSArray)(attributes.ValueForKey(new NSString("titleHeights")));
		}

		public void  ScrollViewWillBeginDragging(UIScrollView scrollView){
			if (firstTimeScroll) {
				//[backgroundView makeObjectsPerformSelector:@selector(setAlpha:) withObject:[NSNumber numberWithDouble:1.0]];
				for (nuint i = 0; i < backgroundView.Count; i++) {
					UIView vw = backgroundView.GetItem<UIView>(i);
					vw.Alpha = 0;
				}firstTimeScroll = false;
			}    
		}

		/* ----------------------------------GESTURERECONIGZER */
		// works when a tap gesture was apply to a title, moving the content of the scroll at correct position
		//*/
		public void handleTapGesture(UITapGestureRecognizer gestureRecognizer)
		{
			for (nuint i = 0; i < backgroundView.Count; i++) {
				UIView vw = backgroundView.GetItem<UIView>(i);
				vw.Alpha = 1;
			}

			NSUrl fileURL = NSUrl.FromFilename(NSBundle.MainBundle.PathForResource("sounds/tab_Switch1","wav"));
			//self.urlAudioFile = fileURL;
			this.managerController.audioController.playAudioWithUrl(fileURL);
			scroll.ScrollRectToVisible(new CGRect(scroll.Frame.Size.Width*(titleArray.IndexOf(gestureRecognizer.View)),scroll.Frame.Location.Y,scroll.Frame.Size.Width,scroll.Frame.Size.Height),true);

			UILabel titleLabel = titleArray.GetItem<UILabel> (actualPage);
			NSString textColAttr_ = this.getTitleUnselectedColorAttribute().GetItem<NSString>((nuint)(this.attributePositionToRedraw()));
			titleLabel.TextColor = UtilManagment.getColorFromRect (CGRectFromString(textColAttr_));

			this.stopAnimations();

			actualPage = titleArray.IndexOf (gestureRecognizer.View);


			this.startAnimations();

			titleLabel = titleArray.GetItem<UILabel>((nuint)actualPage);
			NSString textColAttr = this.getTitleSelectedColorAttribute().GetItem<NSString>((nuint)(this.attributePositionToRedraw()));
			titleLabel.TextColor = UtilManagment.getColorFromRect(CGRectFromString(textColAttr.ToString()));

			this.refreshTitleScrollFromLabelSelected(titleLabel); 
		}

		public void refreshTitleScrollFromLabelSelected(UILabel selectedLabel)
		{
			
			if ( selectedLabel.Frame.Location.X > titleScroll.Frame.Size.Width/2 ) {
				titleScroll.ScrollRectToVisible(new CGRect(selectedLabel.Frame.Location.X + titleScroll.Frame.Size.Width/2-selectedLabel.Frame.Size.Width/2, 0,selectedLabel.Frame.Size.Width,selectedLabel.Frame.Size.Height),true);
			}    

			if (selectedLabel.Frame.Location.X < titleScroll.ContentSize.Width - titleScroll.Frame.Size.Width/2) {
				titleScroll.ScrollRectToVisible(new CGRect(selectedLabel.Frame.Location.X - titleScroll.Frame.Size.Width/2+selectedLabel.Frame.Size.Width/2, 0,selectedLabel.Frame.Size.Width,selectedLabel.Frame.Size.Height),true);
			}
		}

		public void startAnimations()
		{
			var bv = (UIView)(subviewCache.GetItem<NSObject>((nuint)actualPage));
			var bv_ = bv as AnimationProtocol;
			if (bv_!=null) {
				bv_.startAnimations ();
			} 
			foreach(NSObject sv in bv.Subviews)
			{
				var sv_ = sv as AnimationProtocol;
				if (sv_ != null) {
					sv_.startAnimations ();
				}
			}
		}

		public void stopAnimations(){
			for(nuint i = 0; i< subviewCache.Count;i++) {
				UIView bv = (UIView)(subviewCache.GetItem<NSObject>(i));
				var bv_ = bv as AnimationProtocol;
				if(bv_!=null){
					bv_.stopAnimations ();
				}
				foreach (NSObject sv in bv.Subviews) {
					var sv_ = sv as AnimationProtocol;
					if (sv_ != null) {
						sv_.stopAnimations ();
					}
				}
			}
		}

		public override void  inAnimation(){
			
			base.inAnimation();    
			//[[subviewCache objectAtIndex:actualPage] inAnimation];
			subviewCache.GetItem<UIBaseView>(actualPage).inAnimation();
		}

		public override void outAnimation(){
			base.outAnimation ();
			//[[subviewCache objectAtIndex:actualPage] outAnimation];
			this.stopAnimations();
			if (scroll.Subviews.Length > 0) {
				foreach (NSObject view in scroll.Subviews) {
					/*if ([view respondsToSelector:@selector(stopMediaGrid)]) {
						[view performSelector:@selector(stopMediaGrid)];                               
					}
					for (id sv in [view subviews]) {
						if ([sv respondsToSelector:@selector(stopMediaGrid)]) {
							[sv performSelector:@selector(stopMediaGrid)];                               
						}

					}*/
				}
			}

		}

		public override void prepareRemove(){
			
			//[super prepareRemove];
			base.prepareRemove();
			scroll = null;
			titleArray.RemoveAllObjects ();
			titleArray = null;
			titleScroll = null;
			subviews.RemoveAllObjects ();
			subviewsFrames.RemoveAllObjects ();
			backgroundView.RemoveAllObjects ();
			name = null;
			
		}

		[Export("reDraw")]
		public override void reDraw(){  

			base.reDraw(); 
			scroll.Frame = this.Bounds;

			scroll.ContentSize = new CGSize(scroll.Frame.Size.Width*titleArray.Count,scroll.Frame.Size.Height);
			scroll.ScrollRectToVisible(new CGRect(scroll.Frame.Size.Width*actualPage,scroll.Frame.Location.Y,scroll.Frame.Size.Width,scroll.Frame.Size.Height),false);
			UILabel label;

 			
			NSString str = this.getTitleHeightAttribute().GetItem<NSString>(this.attributePositionToRedraw());

			titleScroll.Frame = CGRectFromString(str);
			nfloat titleSpace = 0; 


			for (nuint k = 0; k<titleArray.Count; k++) {
				label =  titleArray.GetItem<UILabel> (k);
				var fontTitle = this.getFontTitleAttribute ().GetItem<NSString> (this.attributePositionToRedraw ());
				var fontSize =  this.getFontSizeTitleAttribute ().GetItem<NSNumber> (this.attributePositionToRedraw ());

				float asFloat = (float) fontSize;

				label.Font =  UIFont.FromName(fontTitle,  (nfloat)asFloat );

				label.SizeToFit();
				label.Frame = new CGRect(titleSpace, 0, label.Frame.Size.Width, label.Frame.Size.Height);        
				titleSpace += label.Frame.Size.Width + titleScroll.Frame.Size.Height;        
			}

			label = titleArray.GetItem<UILabel> ( titleArray.Count - 1);
			titleScroll.ContentSize = new CGSize(titleSpace - titleScroll.Frame.Size.Height, label.Frame.Size.Height);
			titleScroll.Frame = new CGRect(titleScroll.Frame.X, titleScroll.Frame.Y, titleScroll.Frame.Size.Width, label.Frame.Size.Height);
			label = null;
		 
 			if (firstTime) {
				this.refresh ()  ;
				//firstTime = NO;
				for (nuint  i = 0; i <   backgroundView.Count; i++) {
				
					UIView vw = backgroundView.GetItem<UIView>(i);
					vw.Alpha = 0;
				}
			}
		}

		[Export("reDrawSubView")]
		public override void reDrawSubView(){
			this.stopAnimations();
			foreach (NSObject view in scroll.Subviews) {        
				if (view.RespondsToSelector(new Selector("reDraw"))) {
					((UIBaseView)(view)).reDraw();
					////NSLog(@"frame: %@",NSStringFromCGRect([(UIView *)view frame]));
				}              
			}  
		}

		public void appearAllViews(){
			foreach (UIView vw in scroll.Subviews) {
				vw.Alpha = 1.0f;
			}
		}

		public void reDrawView(NSObject bv){
			/*if (bv.RespondsToSelector(new Selector("SetAlpha:"))) {
				((UIView)(bv)).SetAlpha(0.f);
			}
			if (bv.RespondsToSelector(new Selector("reDraw"))) {        
				bv.reDraw();
			} */

			/*UIView.beginAnimations (null, null);
			[UIView setAnimationDuration:1.];
			[UIView setAnimationDelay:1.];
			[UIView setAnimationCurve:UIViewAnimationCurveEaseOut];     
			(UIView)bv.SetAlpha(1.0f); 
			UIView.commitAnimations(); */

		}



	}
}