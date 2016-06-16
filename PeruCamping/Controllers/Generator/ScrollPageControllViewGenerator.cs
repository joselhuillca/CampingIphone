using System;
using Foundation;

namespace PeruCamping
{
	public class ScrollPageControllViewGenerator: AbstractViewGenerator
	{
		public ScrollPageControllViewGenerator()
		{
		}

		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic, AppManagerController manager)
		{
			base.generateFromDictionary(dic, naviDic, manager);
			NSString btnCerrar = (NSString)(dic.ValueForKey(new NSString("btnCerrar")));
			ScrollPageControllView menuView = new ScrollPageControllView(btnCerrar);
			menuView.managerController = manager;
			this.fillStyleAttributesForView(menuView);
			menuView.btnCerrar = btnCerrar;
			Double actualP = Double.Parse((NSString)(dic.ValueForKey(new NSString("actualPage"))));
			var backV = dic.ValueForKey(new NSString("backView"));
			if (btnCerrar.Equals(new NSString("true")) && backV != null)
			{
				menuView.backView = backV.ToString();
			}
			else
			{
				menuView.backView = "mainMenu";
			}

			menuView.actualPage = (nuint)(actualP);
			menuView.actualPage_static = (nuint)(actualP);
			menuView.name = (dic.ValueForKey(new NSString("name"))).ToString();

			NSArray subviews = (NSArray)(dic.ValueForKey(new NSString("subviews")));
			menuView.numSubviews = (nint)subviews.Count;
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
					menuView.addSubview(dicmix, manager.getTextForDisplay((NSString)(title)));
				}
				else
				{
					String typeClass = String.Format("ayarlaleyendaXamariniOS.{0}Generator", subviewDic.ValueForKey(new NSString("class")).ToString());
					//Console.WriteLine (typeClass);
					Type t = Type.GetType(typeClass);
					AbstractViewGenerator classGenerator = Activator.CreateInstance(t) as AbstractViewGenerator;
					UIBaseView bv = classGenerator.generateFromDictionary(subviewDic, navigabilityDictionary, manager);
					menuView.AddSubview(bv);
				}
			}

			return menuView;
		}
	}
}

