using System;
using Foundation;

namespace PeruCamping
{
	public class UIChangeButtonGenerator:AbstractViewGenerator
	{
		public UIChangeButtonGenerator ()
		{
			
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic,AppManagerController manager){
			
			base.generateFromDictionary(dic, naviDic, manager);

			var number_ = dic.ValueForKey (new NSString ("number"));
			Double number = -1;
			if(number_!=null){
				number = Double.Parse ((NSString)(number_));
			}

			NSString titulo = manager.getTextForDisplay ((NSString)(dic.ValueForKey (new NSString ("title"))));
			NSString imagen = (NSString)(dic.ValueForKey (new NSString ("image")));
			UIChangeButton button = new UIChangeButton(titulo,imagen,(int)number);    
			button.managerController = manager;
			//id linkObj = [navigabilityDictionary objectForKey:@"link"];
			NSObject linkObj = navigabilityDictionary.ValueForKey(new NSString("link"));
			if (linkObj!=null) {
				if ( linkObj is NSArray) {
					button.linkArray = (NSArray)(navigabilityDictionary.ValueForKey(new NSString("link")));
				}else{
					//button.linkArray = [NSArray arrayWithObject:linkObj];
					//button.linkArray = (NSArray)(NSArray.FromObject(linkObj));
					var linkO =  NSArray.FromStrings(linkObj.ToString());
					button.linkArray = linkO;
				}
			}


			//button.urlToOpen = [[dic objectForKey:@"urlToOpen"] substringFromIndex:1];
			//button.urlToOpen = (NSString)(dic.ValueForKey(new NSString("urlToOpen")));
			//button.timeInterval = [[dic objectForKey:@"timeInterval"] floatValue];
			//Double interval = Double.Parse((NSString)(dic.ValueForKey(new NSString("timeInterval"))));
			//button.timeInterval = (float)interval;

			button.urlToOpen = (NSString)(dic.ValueForKey(new NSString("urlToOpen")));
			//button.timeInterval =(NSString)(dic.ValueForKey(new NSString("timeInterval")));


			this.fillStyleAttributesForView(button);  


			NSArray subviews = (NSArray)(dic.ValueForKey(new NSString("subviews")));

			if (subviews != null) {
				for (nuint i = 0; i < subviews.Count; i++) {
					NSDictionary subviewDic = subviews.GetItem<NSDictionary> (i);
					String typeClass = String.Format ("PeruCamping.{0}Generator", subviewDic.ValueForKey (new NSString ("class")).ToString ());
					Type t = Type.GetType (typeClass);
					AbstractViewGenerator classGenerator = Activator.CreateInstance (t) as AbstractViewGenerator;

					UIBaseView bv = classGenerator.generateFromDictionary (subviewDic, navigabilityDictionary, manager);
					bv.UserInteractionEnabled = false;
					button.AddSubview (bv);
				}
			}

			return button;
		}
	}
}

