using System;
using Foundation;

namespace PeruCamping
{
	public class MapViewGenerator : AbstractViewGenerator
	{
		public MapViewGenerator ()
		{
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic,AppManagerController manager){
			base.generateFromDictionary(dic,naviDic,manager);

			MapView label = new MapView();
			this.fillStyleAttributesForView(label); 
			return label;
		}
	}
}

