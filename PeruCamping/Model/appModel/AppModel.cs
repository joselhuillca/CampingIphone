using System;
using Foundation;

namespace PeruCamping
{
	public class AppModel
	{
		public String issueID;
		public NSDate releaseDate;

		public NSDictionary model;
		public NSDictionary modelNavigability;
		public NSDictionary modelStyle;
		public NSDictionary modelGridTemplates;
		public NSDictionary modelBooks;
		public NSDictionary modelProductRequest;
		public NSDictionary textLanguages;

		public AppModel ()
		{
		}

		/* contentURL returns the effective URL where we'll store the magazine content and data;
 			if Newsstand is supported, we'll return the NKIssue URL, if not will provide a sub-directory
 			of the Caches directory whose name is the issue ID
 		*/
		public NSUrl contentURL() {
			NSUrl theURL;

			//isOS5
			theURL = NSUrl.FromFilename(issueID);

			Console.WriteLine(String.Format("Content URL: {0}",theURL));

			// returns the url
			return theURL;
		}

		public String description() {
			return String.Format("{0} : ID={1} Released={2} model={3}",
				this.description(),
				issueID,
				releaseDate,
				model
			);
		}

		public void modifyDictionaryName(String name, String key ,String obj) {
			if (!name.Equals("")) {
				NSMutableDictionary dic = NSMutableDictionary.FromDictionary(model);
				var tmp = this.modifyDictionary(dic,name,key,obj);
				if (((NSMutableDictionary)tmp).Count!=0) {
					this.model = tmp as NSDictionary;
				}
			}    
		}

		public NSObject modifyDictionary(NSObject dic,String name,String key,String obj)
		{
			if (dic is NSMutableArray) {
				NSMutableArray dicTmp = (NSMutableArray)(dic);
				for (int i = 0; i < (int)dicTmp.Count; i++) {
					NSObject tmp = dicTmp.GetItem<NSObject>((nuint)i);            
					if (!(tmp is String)) {
						NSObject obj_ = this.modifyDictionary(dicTmp.GetItem<NSObject>((nuint)i),name,key,obj);
						if (obj_!=null) {
							dicTmp.ReplaceObject(i,obj_);
						}
						obj_ = null;
					}
				}
				return dicTmp;
			}else if(dic is NSMutableDictionary){
				NSMutableDictionary dicTmp = NSMutableDictionary.FromDictionary ((NSDictionary)dic);

				if (dicTmp.ValueForKey(new NSString(key))!=null && dicTmp.ValueForKey(new NSString("name")).ToString().Equals(name)) {
					//dicTmp.Remove(key);
					//dicTmp.SetValueForKey(obj,key); 
					return dicTmp;
				}

				/*foreach(NSString aKey in dic){
					NSObject tmp = dicTmp.ValueForKey(aKey);
					if (!(tmp is NSString)) {
						NSObject obj_ = this.modifyDictionary(((NSMutableArray)dicTmp).GetItem<NSObject>((nuint)aKey),name,key,obj);
						if (obj_!=null) {                    
							dicTmp.SetValueForKey (obj_, aKey);
						}
						obj = null;
					}            
				} */
				return dicTmp;
			}else 
				return null; 
		}
	}
}

