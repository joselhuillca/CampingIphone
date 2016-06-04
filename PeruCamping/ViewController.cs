using System;

using UIKit;
using Foundation;
using CoreGraphics;

namespace PeruCamping
{
	public partial class ViewController : UIViewController
	{

		public enum StoreStatusType
		{
			StoreStatusNotInizialized,
			StoreStatusDownloading,
			StoreStatusReady,
			StoreStatusError
		};

		//ASINetworkQueue *networkQueue;
		private bool failed;

		private NSMutableDictionary _booksDic;

		private NSMutableDictionary urisDownloaded;

		private bool willNotDonwloadSomething;

		public AppModel model;
		private AppManagerController managerController;
		private UILabel activityIndicatorLabel;
		private UIImageView backgroundView;
		private bool firstTime = true;
		private UIActivityIndicatorView activityIndicator;

		//PPPController *ppp;
		public AppModelManager modelManager;


		private StoreStatusType _status;
		UIView mainSubView;

		UIView mainHome;
		UIChangeButton mainHome_;

		//nfloat h = 750, w = 888, x = 82, y = 20;
		nfloat h = 750, w = 888, x = 0, y = 0;

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			this.View.BackgroundColor = UIColor.LightGray;

			/*NSString[] frameStr =  new NSString[] { new NSString("{ { 81,78},{ 888,194} }"), new NSString("{ { 81,78},{ 888,194} }"), new NSString("{ { 81,78},{ 888,194} }"), new NSString("{ { 81,78},{ 888,194} }") };
		
			NSArray attr = NSArray.FromNSObjects( frameStr );
			NSMutableDictionary attrDic = new NSMutableDictionary();
			attrDic.Add(new NSString("frame"), attr);
			attrDic.Add(new NSString("imageBackgroundColor"), attr);*/

			//mainSubView = new UIBaseView(attrDic );//{ BackgroundColor = UIColor.White};
			mainSubView = new UIView( new CGRect (x, y, w, h));
			this.View.BackgroundColor = UIColor.LightGray;

			UIImageView background = new UIImageView(this.View.Bounds);
			background.Image = UIImage.FromFile("Archive.wdgt/edutic/backgroundmuro.png");



			/*------------------------------*/
			/*NSString[] frameHome =  new NSString[] { new NSString(" "), new NSString("  "), new NSString("  "), new NSString("{ { 0,0},{ 22,19} }") };
			attr = NSArray.FromNSObjects( frameStr );
			attrDic = new NSMutableDictionary();
			attrDic.Add(new NSString("frame"), attr);
			attrDic.Add(new NSString("imageBackgroundColor"), attr);*/
			/*mainHome = new UIView( new CGRect (-82, -20,81, 81));
			mainHome.BackgroundColor = UIColor.Black;

			mainHome_ = new UIChangeButton (new NSString (""), new NSString ("/edutic/icons/home.png"), 0);

			mainSubView.Add (mainHome);*/
			/*------------------------------*/

			this.View.Add(background);
			this.View.Add(mainSubView);

			activityIndicatorLabel = new UILabel();
			modelManager = new AppModelManager();


			//// app model class
			NSMutableArray _list = null;
			NSMutableArray _listStyle = null;
			NSMutableArray _listNavegability = null;
			NSMutableDictionary _listInformation = null;
			NSDictionary _dateDic = null;

			var cachestore = this.fileURLOfCachedStoreFile();
			_list = NSMutableArray.FromUrl(cachestore);
			_listStyle = NSMutableArray.FromUrl(this.fileURLOfCachedStoreStyleFile());
			_listNavegability = NSMutableArray.FromUrl(this.fileURLOfCachedStoreNavigabilityFile());
			_listInformation = NSMutableDictionary.FromUrl(this.fileURLOfCachedStoreInformationFile());
			_dateDic = NSDictionary.FromUrl(this.fileURLOfCachedStoreDateFile());

			_booksDic = NSMutableDictionary.FromUrl(AppModelManager.fileURLOfCachedBooksFile());


			if (!(_list != null && _listStyle != null && _listNavegability != null))
			{
				Console.WriteLine("Store download failed.");
				model = null;

			}
			else
			{
				NSDictionary issueDictionary = (NSDictionary)_list.GetItem<NSObject>(_list.Count - 1);
				AppModel anIssue = new AppModel();
				anIssue.issueID = issueDictionary.ValueForKey(new NSString("ID")).ToString();
				anIssue.releaseDate = (NSDate)_dateDic.ValueForKey(new NSString("Release date"));
				anIssue.model = (NSDictionary)issueDictionary.ValueForKey(new NSString("model"));
				anIssue.modelStyle = (NSDictionary)_listStyle.GetItem<NSObject>(_listStyle.Count - 1).ValueForKey(new NSString("style"));
				anIssue.modelGridTemplates = (NSDictionary)_listStyle.GetItem<NSObject>(_listStyle.Count - 1).ValueForKey(new NSString("GridTemplates"));
				anIssue.modelNavigability = (NSDictionary)_listNavegability.GetItem<NSObject>(_listNavegability.Count - 1).ValueForKey(new NSString("navigability"));
				anIssue.textLanguages = (NSDictionary)_listInformation.ValueForKey(new NSString("texts"));
				anIssue.modelBooks = _booksDic;

				model = anIssue;

			}
			modelManager.model = model;
			this.showShelf();


		}

		public void showShelf()
		{
			// TODO : isbuyyed 
			if (firstTime  )
			{

				firstTime = false;


				backgroundView = null;
				// activityIndicator = null;

				//foreach (UIView vw in this.View.Subviews)
				//{
				//	vw.RemoveFromSuperview();
				//}

				activityIndicatorLabel.Alpha = 0;

				managerController = new AppManagerController();
				managerController.model = modelManager.model;
				managerController.view = this.mainSubView;

				managerController.showView();
				//initBadrWithView(this.mainSubView);

				Console.WriteLine("Entro a showShelf ");
			}
			else
			{
				Console.WriteLine("NO Entro a showShelf ");
			}
		}

		void initBadrWithView(UIView view)
		{
			UIBaseViewGenerator mainViewGenerator = new UIBaseViewGenerator();

			NSDictionary dic = (NSDictionary)(model.model.ValueForKey(new NSString("mainMenu")));

			mainViewGenerator.managerController = managerController;

			UIBaseView  mainView = mainViewGenerator.generateFromDictionary(dic, model.modelNavigability, managerController );





			var subviews = mainView.Subviews;
			for (int i = 0; i < subviews.Length; i++) {
				var sb = subviews[i] as UIBaseView;

				if (sb.StyleName.Equals("floatView"))
				{
					this.mainSubView.Add(sb);

				}
				sb.reDraw();

			}


		}

		private static NSUrl fileURLOfCachedBooksFile()
		{
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuBooks", "plist"), false, null);
		}

		public NSUrl fileURLOfCachedStoreFile()
		{
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperu", "plist"), false, null);
		}

		public NSUrl fileURLOfCachedStoreStyleFile()
		{
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuStyle", "plist"), false, null);
		}

		public NSUrl fileURLOfCachedStoreNavigabilityFile()
		{
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuNavegavitity", "plist"), false, null);
		}

		public NSUrl fileURLOfCachedStoreDateFile()
		{
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuDate", "plist"), false, null);
		}

		public NSUrl fileURLOfCachedStoreInformationFile()
		{
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuInformation", "plist"), false, null);
		}

		public static void saveBook(NSDictionary bookDic)
		{
			bookDic.WriteToUrl(fileURLOfCachedBooksFile(), true);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

