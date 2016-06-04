using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Foundation;


namespace PeruCamping
{

	public class Avatar  {
		public nint idAvatar { get; set;}
		public string nameAvatar{ get; set; }
		public string imageAvatar{ get; set; }
		public string urlAvatar{ get; set; }
		public string navigability{ get; set; }
	}


	public class UIComicCarouselGenerator : AbstractViewGenerator
	{
		public UIComicCarouselGenerator()
		{
			
		}


		public override UIBaseView generateFromDictionary(NSDictionary dic, NSDictionary naviDic, AppManagerController manager)
		{
			Contract.Ensures(Contract.Result<UIBaseView>() != null);

			base.generateFromDictionary(dic, naviDic, manager);

			List<Avatar> avatarArray = new List<Avatar>();

			NSArray list = dic.ValueForKey(new NSString("avatars")) as NSArray;
			for (nuint  i = 0; i < list.Count; i++)
			{
				NSDictionary avalistOfObjs = list.GetItem<NSDictionary>(i);
				Avatar avatar = new Avatar();

				avatar.imageAvatar = avalistOfObjs.ValueForKey(new NSString("ImageAvatar")).ToString();
				avatar.nameAvatar = avalistOfObjs.ValueForKey(new NSString("NameAvatar")).ToString();
				avatar.urlAvatar = avalistOfObjs.ValueForKey(new NSString("UrlAvatar")).ToString();

			 
				NSString str = avalistOfObjs.ValueForKey (new NSString ("navigability")) as NSString;
				if (str != null)
					avatar.navigability = str.ToString();
				 
				avatarArray.Add(avatar);
			}

		 
			NSArray itemAttribute = styleDictionary.ValueForKey(new NSString("FrameItemAttribute")) as NSArray;
		
			UIComicCarousel carousel = new UIComicCarousel(avatarArray, itemAttribute);
			this.fillStyleAttributesForView(carousel);
			carousel.managerController = manager;
			return carousel;
		}
	}
}

