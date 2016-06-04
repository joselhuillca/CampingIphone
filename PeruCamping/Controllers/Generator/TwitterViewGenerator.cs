using System;
using Foundation;

namespace PeruCamping
{
	public class TwitterViewGenerator : AbstractViewGenerator
	{
		public TwitterViewGenerator ()
		{
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic,NSDictionary naviDic,AppManagerController manager){
			////NSLog(@"navigability: %@",naviDic);
			//[super generateFromDictionary:dic NavigabilityDic:naviDic ManagerView:manager];
			base.generateFromDictionary(dic,naviDic,manager);

			//NSString value = (NSString)(dic.ValueForKey(new NSString("rating")));
			//Double val = Double.Parse (value);

			//StarRatingView *label = [[StarRatingView alloc] initWithRating:[value floatValue]];
			TwitterView label = new TwitterView();//esta ubucacion no cuenta, toma la del itourStyle
			//[self fillStyleAttributesForView:label];  
			this.fillStyleAttributesForView(label);

			return label;
		}
	}
}

