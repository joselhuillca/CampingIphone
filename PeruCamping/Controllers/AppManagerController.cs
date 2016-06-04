using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using ObjCRuntime;
//using MLearning.Badr;
using System.Threading.Tasks;

namespace PeruCamping
{
	public class AppManagerController: UIBaseView
	{
		protected NSMutableDictionary visualObjectsDictionary;

		private NSMutableArray books;
		private UIBaseView transitionView;
		private NSString lastViewName;   
		public UIBaseView bView;

		public AppModel model;
		public UIView view;
		public AudioController audioController;
		public bool canRotate;

		public AppManagerController ()
		{
			visualObjectsDictionary = new NSMutableDictionary ();
			//[self performSelectorInBackground:@selector(initializeSound) withObject:nil];

			 
			this.initializeSound();

			canRotate = true;
		}

 		public void initializeSound(){
			audioController = new AudioController();  
			NSUrl fileURL = NSUrl.FromFilename(NSBundle.MainBundle.PathForResource("sounds/tab_Switch1","wav"));
			audioController.playAudioWithUrl (fileURL);
		}

		public void showView()
		{
			this.showViewForViewName("mainMenu");
		}

		public void transitionViewMethod() {
			this.view.BringSubviewToFront(bView);
			bView.Alpha = 1.0f;
			/*if (bView.RespondsToSelector(new Selector("inAnimationPanelLeft"))) {
				bView.inAnimationPanelLeft();
			}*/
			if (bView.RespondsToSelector(new Selector("inAnimation"))) {
				bView.inAnimation();
			}
			canRotate = true;  
		}

		public void outAnimationView(InOutAnimationProtocol lastView){

			/*if (RespondsToSelector(new Selector("outAnimationPanelLeft"))) {
				lastView.outAnimationPanelLeft();
			}*/
			if (/*lastView. */RespondsToSelector(new Selector("outAnimation"))) {
				lastView.outAnimation();
			}
		}

		public void showViewForViewName(String viewName)
		{
			canRotate = false;    
			bView = null;
			bView = (UIBaseView)(visualObjectsDictionary.ValueForKey(new NSString(viewName)));
			if (bView != null) {
				//bView.Alpha = 0;
				foreach (UIView view in bView.Subviews) {
					if (view is UIChangeButton) {
						this.Alpha = 1.0f; 
					} else {
						this.Alpha = 0.0f; 
					}
				}
				this.view.AddSubview (bView);
				this.view.BringSubviewToFront (bView);    
			} else {
				NSDictionary dic = (NSDictionary)(model.model.ValueForKey (new NSString (viewName)));

				String typeClass = String.Format ("PeruCamping.{0}Generator", dic.ValueForKey (new NSString ("class")).ToString ());
				Console.WriteLine (typeClass);

				Type t = Type.GetType (typeClass);
				AbstractViewGenerator classGenerator = Activator.CreateInstance (t) as AbstractViewGenerator;

				classGenerator.managerController = this;
				bView = classGenerator.generateFromDictionary (dic, model.modelNavigability, this);      


				visualObjectsDictionary.SetValueForKey (bView, new NSString (viewName));
				//bView.Alpha = 0;

				foreach (UIView view in bView.Subviews) {
					if (view is UIChangeButton) {
						this.Alpha = 1.0f; 
					} else {
						this.Alpha = 0.0f; 
					}
				}
 
				this.view.AddSubview (bView);        

				dic = null;
				classGenerator = null;
			}
			 

			if (lastViewName != null ) {
				UIBaseView lastView = (UIBaseView)(visualObjectsDictionary.ValueForKey(lastViewName));//lastView es null????VEr
				this.outAnimationView (lastView);

				AppDelegate delegate_ = (AppDelegate)((UIApplication.SharedApplication).Delegate);

				// ViewController vc = (ViewController)(delegate_.Window.RootViewController);
 
				lastView.RemoveFromSuperview ();
				lastView = null; 
			}

			if (bView.RespondsToSelector(new Selector("reDraw"))) {
				bView.reDraw();        
			}else{
				UIInterfaceOrientation orient;
				if ( UtilManagment.orientation() == 0) {
					orient = UIInterfaceOrientation.Portrait;
				}else{
					orient = UIInterfaceOrientation.LandscapeRight;
				}
 			}

			//[self transitionViewMethod];
			this.transitionViewMethod();

			lastViewName = new NSString(viewName); 

		}

		public NSString getTextForDisplay(NSString text)
		{
			var rpt =(this.model.textLanguages.ValueForKey (text));
			// TODO LocalizedString
			var idiom = NSBundle.MainBundle.LocalizedString ("language", "");
			var rptIdiom = rpt.ValueForKey (new NSString(idiom));
			Console.WriteLine (String.Format("**********************{0}----------{1}",rpt,rptIdiom));

			return (NSString)rptIdiom;
		}

		public void createBooksFromDictionary(NSDictionary dic){}

		UIBaseView fun_bView(){
			return bView;
		}
	}
}

