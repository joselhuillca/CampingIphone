using System;

namespace PeruCamping
{
	public class Resource
	{
		public int idResource;
		public String link;
		public String srcResource;
		public String titleResource;
		public String srcBackgroundTitle;

		public Resource ()
		{
		}

		public String description()
		{
			return String.Format ("id:{0} \nlink:{1} \nsrc:{2} \ntitle:{3} srcBackgroundTitle:{4}",
				idResource,
				link,
				srcResource,
				titleResource,
				srcBackgroundTitle);
		}
	}
}

