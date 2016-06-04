using System;
using Foundation;

namespace PeruCamping
{
	public class XboxBackViewGenerator:AbstractViewGenerator
	{
		public XboxBackViewGenerator ()
		{
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic,AppManagerController manager){
			base.generateFromDictionary(dic, naviDic, manager);
			XboxBackView backView; 
			if (navigabilityDictionary!=null) {
				//backView = [[XboxBackView alloc] initWithBackViewName:[navigabilityDictionary objectForKey:@"back"] Title:[manager getTextForDisplay:[dic objectForKey:@"title"]] Flecha:[dic objectForKey:@"flecha"]];

				NSString back = (Foundation.NSString)navigabilityDictionary.ValueForKey(new NSString("back"));
				NSString titulo = manager.getTextForDisplay((NSString)(dic.ValueForKey(new NSString("title"))));
				NSString flecha = (NSString)(dic.ValueForKey(new NSString("flecha")));
				backView = new XboxBackView(back,titulo,flecha);
			}else{
				//backView = [[XboxBackView alloc] initWithBackViewName:nil Title:[manager getTextForDisplay:[dic objectForKey:@"title"]] Flecha:[NSURL URLWithString:[dic objectForKey:@"flecha"]]];
				NSString titulo = manager.getTextForDisplay((NSString)(dic.ValueForKey(new NSString("title"))));
				NSUrl flech = NSUrl.FromString (dic.ValueForKey (new NSString ("flecha")).ToString());
				NSString flecha = new NSString (flech.ToString ());
				backView = new XboxBackView(new NSString(),titulo,flecha);
			}    
			backView.managerController = manager;

			NSArray subviews = (NSArray)(dic.ValueForKey(new NSString("subviews")));

			for (nuint i = 0; i < subviews.Count; i++) {
				NSDictionary subviewDic = subviews.GetItem<NSDictionary> (i);
				String typeClass = String.Format ("PeruCamping.{0}Generator", subviewDic.ValueForKey (new NSString ("class")).ToString ());
				Type t = Type.GetType (typeClass);
				AbstractViewGenerator classGenerator = Activator.CreateInstance (t) as AbstractViewGenerator;

				UIBaseView bv = classGenerator.generateFromDictionary(subviewDic, navigabilityDictionary, manager);
				backView.AddSubview (bv);
			}

			this.fillStyleAttributesForView(backView); 
			return backView;
		}
	}
}

