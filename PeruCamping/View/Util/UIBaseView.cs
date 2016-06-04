using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;
using Foundation;
using ObjCRuntime;
using System.Text.RegularExpressions;

namespace PeruCamping
{
	public partial class UIBaseView: UIView, InOutAnimationProtocol
	{
		
		protected NSMutableDictionary attributes;
		private int numberOfChildrenViews;
		private float transitionTime =0.4f;

		public AppManagerController managerController;

		public string StyleName {
			get;
			set;
		} 
		/*public UIBaseView ()
		{
			Console.WriteLine ("Constructor sin parmetro UINASEVIEW");
		}*/

		//This constructor initializes the hash table attributes
		public UIBaseView (NSMutableDictionary attr=null)
		{
			Console.WriteLine ("Constructor con parmetro UIBASEVIEW");
			if (attr == null)  attributes = new NSMutableDictionary();
			else attributes = attr;     
			numberOfChildrenViews = 0;
		}
 
		 
		public CGRect CGRectFromString(String frame)
		{

			CGRect rect= new CGRect();
			frame = frame.Replace (" ", "");

			frame = frame.Replace ("{{", "");
			frame = frame.Replace ("}}", "");
			frame = frame.Replace ("},{", " ");
			frame = frame.Replace (",", " ");

			var myList = new List<string>(frame.Split(' '));
			rect.X = (int)Double.Parse(myList[0]); 
			rect.Y = (int)Double.Parse(myList[1]); 
			rect.Width = (int)Double.Parse(myList[2]); 
			rect.Height = (int)Double.Parse(myList[3]); 

			return rect;
		}

		//update at correct frame of FrameAttribute depend of device and orientation.
		[Export("reDraw")]
		public virtual void reDraw()
		{
			Console.WriteLine(String.Format("{0} was redraw...",this.Class.Name));
			try {
				//this.Frame = CGRectFromString([[attributes objectForKey:@"frame"] objectAtIndex:[self attributePositionToRedraw]]);
				NSArray collection =(attributes.ValueForKey(new NSString("frame")))  as NSArray;
				 
				CGRect rect = CGRectFromString( collection.GetItem<NSString>(this.attributePositionToRedraw()).ToString() );
				//Console.WriteLine(rect);  
				this.Frame = rect;

			}
			catch (Exception exception) {
				Console.WriteLine(String.Format("Non Frame Found Exception redraw1: {0}",exception));
			}
			try {

				if (this.getImageBackgroundColorAttribute().Count!=0) {
					NSArray  collection = this.getImageBackgroundColorAttribute() as NSArray ;
					this.BackgroundColor = UIColor.Clear;
					if (collection != null)
					{
						UIImage img = UtilManagment.imageFromDocumentDirectory(collection.GetItem<NSString>((nuint)this.attributePositionToRedraw()));

						this.BackgroundColor = UIColor.FromPatternImage(img);

					}
					Console.WriteLine(this.getBackgroundColorAttribute());
					NSArray collection_ = this.getBackgroundColorAttribute() as NSArray;
					this.BackgroundColor = UIColor.FromPatternImage(UtilManagment.imageFromDocumentDirectory(collection_.GetItem<NSString>( (nuint)this.attributePositionToRedraw() )));

				} else {
					NSArray collection = this.getBackgroundColorAttribute() as NSArray;
					CGRect rect;
					CGRect.TryParse( collection.GetItem<NSDictionary>( (nuint)this.attributePositionToRedraw() ) ,out rect);
					this.BackgroundColor = UtilManagment.getColorFromRect(rect);
				}      

			}
			catch (Exception exception) {
				Console.WriteLine(String.Format("Non Frame Found Exception redraw2: {0}",exception));
			}
			this.reDrawSubView();
		}

		public virtual void reDrawSubView()
		{
			foreach (Object view in this.Subviews) {
				//if([view isKindOfClass:[UIBaseView class]]) [view reDraw];
				if (((UIView)view).RespondsToSelector(new Selector("reDraw"))) 
				{
					((UIBaseView)view).reDraw();
				}
			}
		}

		public void setGeneralAttributes()
		{
			try {
				NSArray collection = this.attributes.ValueForKey(new NSString("frame")) as NSArray;
				CGRect rect;
				CGRect.TryParse( collection.GetItem<NSDictionary>( (nuint)this.attributePositionToRedraw() ) ,out rect);
				this.Frame = rect;

			}
			catch (Exception exception)  {
				//NSLog(@"Non Frame Found Exception: %@",exception);
			} 
			try {

				if (this.getImageBackgroundColorAttribute().Count!=0) {
					NSArray collection = this.getBackgroundColorAttribute() as NSArray;
					this.BackgroundColor =  UIColor.FromPatternImage(UtilManagment.imageFromDocumentDirectory(collection.GetItem<NSString>( (nuint)this.attributePositionToRedraw() )));
				} else {
					NSArray collection = this.getBackgroundColorAttribute() as NSArray;
					CGRect rect;
					CGRect.TryParse( collection.GetItem<NSDictionary>( (nuint)this.attributePositionToRedraw() ) ,out rect);
					this.BackgroundColor = UtilManagment.getColorFromRect(rect);
				}      

			}
			catch (Exception exception)  {
				//NSLog(@"Non ImageBackground Found Exception: %@",exception);
			} 
		}

		//return the correct position into the array attribute depend of device and orientation. 
		public uint attributePositionToRedraw()
		{
			int index = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? 0 : 2;
			index += UtilManagment.orientation();
			return  (uint)index  ;
		}

		//set an array that contains four frames depend of the device and orientation.
		public void setFrameAttribute(NSArray frames)
		{
			attributes.SetValueForKey (frames.Copy (), new NSString("frame"));
		}

		//return an array that contains four frames depend of the device and orientation.
		public NSArray getFrameAttribute()
		{
			return (NSArray)attributes.ValueForKey(new NSString("frame"));
		}

		//set an array that contains four image's name depend of the device and orientation.
		public void setImageBackgroundColorAttribute(NSArray imageBackgroundColors)
		{
			attributes.SetValueForKey(imageBackgroundColors.Copy(),new NSString("imageBackgroundColor"));
		}

		//return an array that contains four image's name depend of the device and orientation
		public NSArray getImageBackgroundColorAttribute()
		{
			return (NSArray)attributes.ValueForKey(new NSString("imageBackgroundColor"));
		} 

		//set an array that contains four font's name depend of the device and orientation.
		public void setFontTitleAttribute(NSArray fonts)
		{
			attributes.SetValueForKey(fonts.Copy(),new NSString("fontTitle"));
		}

		//return an array that contains four font's name depend of the device and orientation.
		public NSArray getFontTitleAttribute()
		{
			return (NSArray)attributes.ValueForKey(new NSString("fontTitle"));
		}

		//set an array that contains four font sizes depend of the device and orientation.
		public void setFontSizeTitleAttribute(NSArray fontSizes)
		{
			attributes.SetValueForKey(fontSizes.Copy(),new NSString("fontSizeTitle"));
		}

		//return an array that contains four font sizes depend of the device and orientation.
		public NSArray getFontSizeTitleAttribute()
		{
			return (NSArray)attributes.ValueForKey(new NSString("fontSizeTitle"));
		}

		//set an array that contains four colors represented into CGRect in RGB that depend of the device and orientation
		public void setFontColorTitleAttribute(NSArray fontColors)
		{
			attributes.SetValueForKey(fontColors.Copy(),new NSString("fontColorTitle"));
		}

		//return an array that contains four colors represented into CGRect in RGB that depend of the device and orientation.
		public NSArray getFontColorTitleAttribute()
		{
			return (NSArray)attributes.ValueForKey(new NSString("fontColorTitle"));
		}

		public void setBackgroundColorAttribute(NSArray backgroundColors)
		{
			attributes.SetValueForKey(backgroundColors.Copy(),new NSString("backgroundColorAttribute"));
		}

		public NSArray getBackgroundColorAttribute()
		{
			return attributes.ValueForKey(new NSString("backgroundColorAttribute")) as NSArray;
		}


		public virtual void prepareRemove()
		{
			if (this.Subviews.Length>0) {
				foreach (UIView view in this.Subviews) {
					if (view.RespondsToSelector(new Selector("prepareRemove"))) {
						((UIBaseView)view).prepareRemove();
					}
					view.RemoveFromSuperview();
				}
			}
			this.RemoveFromSuperview ();
			attributes.Clear();
			attributes = null;
			managerController = null;
		}

		public void outAnimationUpDownRightLeft(CGRect state,float aimationDuration,float aimationDelay,UIView view)
		{
			float v1,v2,v3,v4;
			v1 = (float)state.Location.X;
			v2 = (float)state.Location.Y;
			v3 = (float)state.Size.Width;
			v4 = (float)state.Size.Height;

			UIView.BeginAnimations(null,new IntPtr());
			UIView.SetAnimationDuration(aimationDuration);
			UIView.SetAnimationDelay(aimationDelay);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);

			view.Frame = new CGRect(view.Center.X+(v1 * view.Frame.Size.Width), view.Center.Y+(v2 * view.Frame.Size.Height),v3*view.Frame.Size.Width,v4*view.Frame.Size.Height);
			view.Alpha = 0.0f;
			UIView.CommitAnimations();
		}

		public void inAnimationUpDownRightLeft(CGRect state,float aimationDuration,float aimationDelay,UIView view)
		{
			float v1,v2,v3,v4;
			v1 = (float)state.Location.X;
			v2 = (float)state.Location.Y;
			v3 = (float)state.Size.Width;
			v4 = (float)state.Size.Height;

			view.Alpha = 0.0f;
			CGRect frame = view.Frame;
			view.Frame = new CGRect(view.Center.X+(v1 * view.Frame.Size.Width), view.Center.Y+(v2 * view.Frame.Size.Height),v3*view.Frame.Size.Width,v4*view.Frame.Size.Height);

			UIView.BeginAnimations(null,new IntPtr());
			UIView.SetAnimationDuration(aimationDuration);
			UIView.SetAnimationDelay(aimationDelay);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);

			view.Alpha = 1.0f;
			view.Frame = frame;
			UIView.CommitAnimations();
		}



		//Functions Animation Protocol
		/*[Export("inAnimation")]
		public virtual void inAnimation(){
			if (this.Subviews.Length>0) {
				foreach (UIView view in this.Subviews) {
					if (view.RespondsToSelector(new Selector("inAnimation"))) {
						((UIBaseView)view).inAnimation();
						numberOfChildrenViews++;
					}
				}
			}
			this.Alpha = 0.0f;
			UIView.BeginAnimations(null,new IntPtr());
			UIView.SetAnimationDuration(transitionTime);
			UIView.SetAnimationDelay(0);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);     
			this.Alpha = 1.0f;
			UIView.CommitAnimations(); 
		}

		[Export("outAnimation")]
		public virtual void outAnimation(){
			if (this.Subviews.Length>0) {
				foreach (UIView view in this.Subviews) {
					if (view.RespondsToSelector(new Selector("outAnimation"))) {
						((UIBaseView)view).outAnimation();
						numberOfChildrenViews--;
					}
				}
			} 
			UIView.BeginAnimations(null,new IntPtr());
			UIView.SetAnimationDuration(transitionTime);
			UIView.SetAnimationDelay(0);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);     
			this.Alpha = 0.0f;   
			UIView.CommitAnimations(); 
		}*/

		[Export("inAnimation")]
		public virtual void inAnimation(){
			if (this.Subviews.Length>0) {
				foreach (UIView view in this.Subviews) {
					if (view.RespondsToSelector(new Selector("inAnimation"))) {
						((UIBaseView)view).inAnimation();
						numberOfChildrenViews++;
					}
					if (view is PanelLeftButton) {
						this.Alpha = 1.0f; 
					} else {
						this.Alpha = 1.0f; 
					}
				}
			}
			UIView.BeginAnimations(null,new IntPtr());
			UIView.SetAnimationDuration(transitionTime);
			UIView.SetAnimationDelay(0);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);
			UIView.CommitAnimations(); 
			 
		}

		[Export("outAnimation")]
		public virtual void outAnimation(){
			if (this.Subviews.Length>0) {
				foreach (UIView view in this.Subviews) {
					if (view.RespondsToSelector(new Selector("outAnimation"))) {
						((UIBaseView)view).outAnimation();
						numberOfChildrenViews--;
					}
					if (view is PanelLeftButton) {
						this.Alpha = 1.0f; 
					} else {
						this.Alpha = 0.0f; 
					}
				}
			} 
			UIView.BeginAnimations(null,new IntPtr());
			UIView.SetAnimationDuration(transitionTime);
			UIView.SetAnimationDelay(0);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);
			UIView.CommitAnimations();
		}
	}
}

