using System;
using Foundation;
using System.Collections.Generic;

namespace PeruCamping
{
	public class UIBaseViewGenerator:AbstractViewGenerator
	{
		public UIBaseViewGenerator ()
		{
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic,AppManagerController manager){
			Console.WriteLine ("Entro a generateFromDictionary de UIBASEGENERATOR");

			base.generateFromDictionary(dic,naviDic,manager);  

			UIBaseView baseView = new UIBaseView ();
			baseView.StyleName = (dic.ValueForKey(new NSString("style")) as NSString).ToString();
			this.fillStyleAttributesForView(baseView); 

			NSArray subviews = (NSArray)(dic.ValueForKey(new NSString("subviews")));

			if (subviews == null)
				return baseView;//Aumentado

			for (nuint i = 0; i < subviews.Count; i++) {
				NSDictionary subviewDic = subviews.GetItem<NSDictionary> (i);
				String typeClass = String.Format ("PeruCamping.{0}Generator", subviewDic.ValueForKey (new NSString ("class")).ToString ());
				Type t = Type.GetType (typeClass);
				AbstractViewGenerator classGenerator = Activator.CreateInstance (t) as AbstractViewGenerator;
		 
				UIBaseView bv = classGenerator.generateFromDictionary(subviewDic, navigabilityDictionary, manager);
				baseView.AddSubview (bv);
			}
			return baseView;
		}
	}
}

