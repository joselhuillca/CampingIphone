using System;
using Foundation;

namespace PeruCamping
{
	public class ContentListViewGenerator: AbstractViewGenerator
	{
		public ContentListViewGenerator()
		{
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic, AppManagerController manager)
		{
			//////NSLog(@"navigability: %@",naviDic);
			base.generateFromDictionary(dic, naviDic, manager);

			ContentListView listView = new ContentListView();
			listView.managerController = manager;
			this.fillStyleAttributesForView(listView);
			//[label reDraw];
			return listView;
		}
	}
}

