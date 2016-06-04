using System;
using Foundation;

namespace PeruCamping
{
	public class LabelViewBarraTituloGenerator  : AbstractViewGenerator
	{
		public LabelViewBarraTituloGenerator ()
		{
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic,AppManagerController manager){
			//////NSLog(@"navigability: %@",naviDic);
			base.generateFromDictionary(dic,naviDic,manager);

			NSString textKey = new NSString ("text"); 
			LabelViewBarraTitulo label = new LabelViewBarraTitulo(manager.getTextForDisplay((NSString)(dic.ValueForKey(textKey))));
			this.fillStyleAttributesForView(label);  
			//[label reDraw];
			return label;
		}
	}
}

