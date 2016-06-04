using System;
using System.Drawing;

using CoreGraphics;
using Foundation;
using UIKit;


namespace PeruCamping
{
	public partial class TwitterView :  UIBaseView
    {

		UIWebView webView;
        /*private UIView topView;
		public UITableViewController table_test;
		UITableView test;*/

		public TwitterView()
        {
			/*table_test = new UITableViewController ();
            //Title = "UITableView Demo";
			table_test.Title = "UITableView Demo";

			table_test.EdgesForExtendedLayout = UIRectEdge.None;
			table_test.AutomaticallyAdjustsScrollViewInsets = false;

			test = new UITableView (this.Bounds);
			test.InputViewController = table_test;
			*/

			Ini ();
        }

		/*public TwitterView(UIView topView)
            : this()
        {
            this.topView = topView;
        }*/

        /*protected override void Dispose(bool disposing)
        {
			table_test.TableView.RemoveTwitterCover();

            base.Dispose(disposing);
        }*/

		public void Ini()
		{
			/*nfloat CoverViewHeight = 200;
			table_test.TableView.AddTwitterCover(UIImage.FromBundle("Archive.wdgt/edutic/story/slice3.png"), topView, CoverViewHeight);
			nfloat topViewHeight = 0;
			if (topView != null)
			{
				topViewHeight = topView.Bounds.Size.Height;
			}
			table_test.TableView.TableHeaderView = new UIView(new CGRect(0, 0, 320, CoverViewHeight + topViewHeight));

			this.Add (table_test);*/
			webView = new UIWebView (new CGRect(0,0,888,490));
			Add(webView);

			var url = "https://twitter.com/BarackObama/status/266031293945503744/photo/1?ref_src=twsrc%5Etfw"; // NOTE: https secure request
			webView.LoadRequest(new NSUrlRequest(new NSUrl(url)));

			webView.ScalesPageToFit = true;
		}

        /*public nint rowsInSection(UITableView tableview, nint section)
        {
            return 20;
        }

        public UITableViewCell getCell(UITableView tableView, NSIndexPath indexPath)
        {
			UITableViewCell cell = table_test.TableView.DequeueReusableCell("Cell");
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, "Cell");
            }
            cell.TextLabel.Text = "Cell " + (indexPath.Row + 1);
            return cell;
        }*/
    }
}