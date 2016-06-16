using System;
using Foundation;

namespace PeruCamping
{
	public class ImageViewGenerator:AbstractViewGenerator
	{
		public ImageViewGenerator ()
		{
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic,AppManagerController manager){
			
			base.generateFromDictionary (dic, naviDic, manager); 

			var isScaled = dic.ValueForKey(new NSString("isScaled"));
			if (isScaled == null)
			{
				isScaled = new NSString("false");
			}

			ImageView baseView = new ImageView((NSString)isScaled);
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

