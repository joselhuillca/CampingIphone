using System;
using Foundation;
using System.Threading.Tasks;
using CoreFoundation;
//using MLearning.Badr;

namespace PeruCamping
{

	public class AppModelManager: AppDelegate
	{
		public enum StoreStatusType{
			StoreStatusNotInizialized,
			StoreStatusDownloading,
			StoreStatusReady,
			StoreStatusError
		}  ;

		//ASINetworkQueue *networkQueue;
		private bool failed;

		private NSOperationQueue downloadQueue;
		private NSMutableDictionary _booksDic;

		private NSMutableDictionary urisDownloaded;

		private bool willNotDonwloadSomething;

		public AppModel model;

		private StoreStatusType _status;

		/* takes note of the current store status; this can be used by the view controllers to update their UI according to the status */
		public StoreStatusType status {
			get {
				return _status;
			}
			set {
				if(_status==value) 
					return;
				_status=value;

				DispatchQueue.MainQueue.DispatchAsync (() => {
					int nwst = (int)_status;
					NSNotificationCenter.DefaultCenter.PostNotificationName(Constantes.STORE_CHANGED_STATUS_NOTIFICATION,NSNumber.FromNUInt((nuint)nwst));
				});
			}
		}

		public AppModelManager ()
		{
			status = StoreStatusType.StoreStatusNotInizialized;     
			willNotDonwloadSomething = true;

			//model = new AppModel ();
		}
		 

		/* startup
			 this begins the Store startup sequence, which must be run immediately after the Store has been initialized;
			 main purpose of startup is to connect with the publisher server to fetch the list of currently available magazines;
			 besides the store will load the current status of user issues
 		*/
		public void startup()
		{
			Console.WriteLine ("Entro a starup");
			this.downloadStoreIssues ();
		}

		/* returns YES if the store information is ready */
		public bool isStoreReady() {
			return (status==StoreStatusType.StoreStatusReady);
		}

		//download all issues info from the publisher server and builds the storeIssues status; at the end sends a notification */
		public void downloadStoreIssues()
		{
			this.status = StoreStatusType.StoreStatusDownloading;

			try
			{
				DispatchQueue.GetGlobalQueue(DispatchQueuePriority.High).DispatchAsync(() =>
				{
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
						this.status = StoreStatusType.StoreStatusError;
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

						if (willNotDonwloadSomething)
						{
							this.status = StoreStatusType.StoreStatusReady;
						}
						issueDictionary = null;
						anIssue = null;
					}

					_list = null;
					_listNavegability = null;
					_listStyle = null;
					_listInformation = null;
					_dateDic = null;
				});

			}
			catch (Exception e)
			{
				Console.WriteLine(e.StackTrace);

			}
		}

		public static NSUrl fileURLOfCachedBooksFile(){
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuBooks","plist"),false,null);
		}

		public NSUrl fileURLOfCachedStoreFile() {
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperu","plist"),false,null);
		}

		public NSUrl fileURLOfCachedStoreStyleFile(){
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuStyle","plist"),false,null);
		}

		public NSUrl fileURLOfCachedStoreNavigabilityFile(){
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuNavegavitity","plist"),false,null);
		}

		public NSUrl fileURLOfCachedStoreDateFile(){
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuDate","plist"),false,null);
		}

		public NSUrl fileURLOfCachedStoreInformationFile(){
			return NSUrl.CreateFileUrl(NSBundle.MainBundle.PathForResource("Archive.wdgt/itourperuInformation","plist"),false,null);
		}

		public static void saveBook(NSDictionary bookDic)
		{
			bookDic.WriteToUrl (fileURLOfCachedBooksFile(), true);
		}
	}
}

