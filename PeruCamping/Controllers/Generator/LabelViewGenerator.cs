using System;
using Foundation;

namespace PeruCamping
{
	public class LabelViewGenerator : AbstractViewGenerator
	{
		public LabelViewGenerator ()
		{
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic,AppManagerController manager){
			//////NSLog(@"navigability: %@",naviDic);
			base.generateFromDictionary(dic,naviDic,manager);

			NSString textKey = new NSString ("text"); 
			LabelView label = new LabelView(manager.getTextForDisplay((NSString)(dic.ValueForKey(textKey))));
			this.fillStyleAttributesForView(label);  
			//[label reDraw];
			return label;
		}
	}
}

