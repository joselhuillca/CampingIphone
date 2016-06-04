using System;
 using Foundation;



namespace PeruCamping
{
	public class FTCoreTextGenerator : AbstractViewGenerator
	{
		public FTCoreTextGenerator()
		{
		}


		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic, AppManagerController manager)
		{
			//////NSLog(@"navigability: %@",naviDic);
			base.generateFromDictionary(dic, naviDic, manager);

			NSString textKey = new NSString("text");

 
			 
			NSString filePath = dic.ValueForKey(textKey) as NSString;
			string text = UtilManagment.htmlStringFromDocumentDirectory(filePath);

			FTCoreText label = new FTCoreText(text);
			label.managerController = manager;

			this.fillStyleAttributesForView(label);
			//[label reDraw];
			return label;
		} 

	}
}

