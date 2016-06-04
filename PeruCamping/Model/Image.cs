using System;
using CoreGraphics;

namespace PeruCamping
{
	public class Image:Resource
	{
		private CGPoint principalPointImage;
		public Resource resource;

		public Image ()
		{
			resource = new Resource ();
		}

		public String description(){
			return String.Format("Image: \n {0} \nprincipalPointImage:{1}",
				base.description(),
				principalPointImage.ToString());
		}
	}
}

