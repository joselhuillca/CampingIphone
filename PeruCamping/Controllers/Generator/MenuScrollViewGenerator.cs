using System;
using CoreGraphics;
using Foundation;

namespace PeruCamping
{
	public class MenuScrollViewGenerator:AbstractViewGenerator
	{
		public MenuScrollViewGenerator ()
		{
		}
		 
		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic, AppManagerController manager)
		{
			base.generateFromDictionary(dic, naviDic, manager);
			MenuScrollView menuView = new MenuScrollView();
			menuView.managerController = manager;
			this.fillStyleAttributesForView(menuView);
			Double actualP = Double.Parse((NSString)(dic.ValueForKey(new NSString("actualPage"))));
			menuView.actualPage = (nuint)(actualP);

			menuView.actualPage_static = (nuint)(actualP);

			NSArray subviews = (NSArray)(dic.ValueForKey(new NSString("subviews")));
			//menuView.numSubviews = (nint)subviews.Count;
			for (nuint i = 0; i < subviews.Count; i++)
			{
				NSDictionary subviewDic = subviews.GetItem<NSDictionary>((nuint)i);
				var condic = subviewDic.ValueForKey(new NSString("title"));
				if (condic != null)
				{
					NSDictionary dicmix;
					if (navigabilityDictionary != null)
					{
						NSObject[] arrayObj_ = { subviewDic, navigabilityDictionary, null };
						NSObject[] keysObj_ = { new NSString("dic"), new NSString("nav"), null };
						dicmix = NSDictionary.FromObjectsAndKeys(arrayObj_, keysObj_);
					}
					else
					{
						dicmix = NSDictionary.FromObjectAndKey(subviewDic, new NSString("dic"));
					}
					var title = subviewDic.ValueForKey(new NSString("title"));
					var pos = new CGRect(i*94,0,94,73);//Agregado para peru camping
					menuView.addSubview(dicmix, (NSString)(title)/*manager.getTextForDisplay((NSString)(title))*/,pos);
				}
				else
				{
					String typeClass = String.Format("ayarlaleyendaXamariniOS.{0}Generator", subviewDic.ValueForKey(new NSString("class")).ToString());
					//Console.WriteLine (typeClass);
					Type t = Type.GetType(typeClass);
					AbstractViewGenerator classGenerator = Activator.CreateInstance(t) as AbstractViewGenerator;
					UIBaseView bv = classGenerator.generateFromDictionary(subviewDic, navigabilityDictionary, manager);
					//bv.Frame = 
					menuView.addSubViewScroll(bv);
				}
			}

			return menuView;
		}
	}
}

