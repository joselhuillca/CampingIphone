using System;
using Foundation;
using ObjCRuntime;
using System.Collections.Generic;

using System.Net;
using System.Collections;
 
using System.Linq;
using System.Text;
 
 

using System.IO;
using System.Xml;
using System.Reflection;

namespace PeruCamping
{
	public class AbstractViewGenerator
	{
		public NSDictionary styleDictionary;
 		public NSDictionary navigabilityDictionary;

		public AppManagerController managerController;

		public AbstractViewGenerator ()
		{
		}

	

		public virtual UIBaseView generateFromDictionary(NSDictionary dic ,NSDictionary  naviDic ,AppManagerController manager){
			Console.WriteLine ("Entro a generateFromDictionary de AbstractViewGenerator");
			try {
				styleDictionary = (NSDictionary)(manager.model.modelStyle.ValueForKey((NSString)(dic.ValueForKey(new NSString("style")))));
				 
			}
			catch (Exception exception) {
				Console.WriteLine("not exist style for this object");
				styleDictionary = null;
				throw   exception;
			}

			try {
				//NSDictionary navi = naviDic;
				navigabilityDictionary = (NSDictionary)(naviDic.ValueForKey((NSString)(dic.ValueForKey(new NSString("navigability")))));//[naviDic objectForKey:[dic objectForKey:@"navigability"]];
			}
			catch(Exception exception) {
				Console.WriteLine("not exist Navigability for this object");
				navigabilityDictionary = null;
				//throw exception;
			}
			return null;
		}



		public void fillStyleAttributesForView(UIBaseView view)
		{ 
			foreach(var aKey in styleDictionary){
				Console.WriteLine ((aKey.Key).ToString());
				NSString aKey_ = new NSString ((aKey.Key).ToString ());
				NSObject value = (NSObject)(styleDictionary.ValueForKey (aKey_));
				this.setAttibute(aKey_,value,view);
			} 
			/*for(NSString *aKey in styleDictionary){
			[self setAttibute:aKey Value:[styleDictionary valueForKey:aKey] View:view];
			} */
			/*for (nuint i = 0; i < styleDictionary.Count; i++) {
			NSString aKey = (NSString)(styleDictionary[i].ToString());
			this.setAttibute(aKey,styleDictionary.ValueForKey(aKey),view);
			}*/
		}

	public void setAttibute(NSString attribute,NSObject value,UIBaseView view)
		{
			// TODO estandarizar	
			if (attribute.Equals(new NSString("FrameItemAttribute"))) {
				return;
			}
			Console.WriteLine (string.Format("**set{0}:",attribute));
			Console.WriteLine (string.Format("\t\tset{0}:",value.ToString()));

			var selector =  string.Format("set{0}",attribute);
			/*if (view.RespondsToSelector (selector)) {
				view.PerformSelector (selector, value);
			} */
		
			MethodInfo methodInfo = view.GetType().GetMethod(selector);

			object[] parametersArray = new object[] { value };
			methodInfo.Invoke(view, parametersArray);

		}

	}
}

