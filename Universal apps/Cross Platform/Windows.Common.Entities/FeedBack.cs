using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if ANDROID
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
#endif
namespace OnlineVideos.Entities
{
	public class FeedBack
	{
		public string Id
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}

		public string ImageName
		{
			get;
			set;
		}
	}
}
