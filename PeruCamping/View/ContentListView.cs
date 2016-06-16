using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace PeruCamping
{
	public class ContentListView: UIBaseView
	{
		
		UITableView table;

		public ContentListView()
		{
			
		}



		public void Ini()
		{
			table = new UITableView(this.Bounds); // defaults to Plain style
			List<CampamentoIndex> indices = new List<CampamentoIndex>();
			llenar_indices(indices);
			table.Source = new TableSource(indices, this);
			this.AddSubview(table);

			table.RowHeight = UITableView.AutomaticDimension;
			/*//SEparacion entre celdas
				table.SeparatorColor = UIColor.Blue;
				table.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
				// blur effect
				table.SeparatorEffect =
					UIBlurEffect.FromStyle(UIBlurEffectStyle.Dark);

				//vibrancy effect
				var effect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Light);
				table.SeparatorEffect = UIVibrancyEffect.FromBlurEffect(effect);

				table.SeparatorInset.InsetRect(new CGRect(4, 4, 150, 2));*/
		}

		public override void reDraw()
		{
			base.reDraw();
			Ini();
		}

		public void llenar_indices(List<CampamentoIndex> indices)
		{
			for (int i = 0; i < 10;i++)
			{
				string head = "Titulo" + String.Format("_campamento_{0}", i);
				string descrip = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, ";
				string img = "Archive.wdgt/Hitec/info.png";
				indices.Add(new CampamentoIndex(head,descrip,img));
			}
		}

		public class TableSource : UITableViewSource
		{

			CampamentoIndex[] TableItems;
			string CellIdentifier = "TableCell";
			ContentListView owner;

			public TableSource(List<CampamentoIndex> indices,ContentListView ow)
			{
				TableItems = indices.ToArray();
				this.owner = ow;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return (nint)TableItems.Length;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell(CellIdentifier) as CustomIndicesCell;
				//---- if there are no cells to reuse, create a new one
				if (cell == null)
				{ cell = new CustomIndicesCell(new NSString(CellIdentifier)); }
				//cell.Accessory = UITableViewCellAccessory.Checkmark;
				//cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier);

				cell.UpdateCell(TableItems[indexPath.Row].titulo
								, TableItems[indexPath.Row].descripcion
				                , UIImage.FromFile(TableItems[indexPath.Row].imgen_info));

				/*cell.TextLabel.Text = TableItems[indexPath.Row].titulo;
				cell.DetailTextLabel.Text = TableItems[indexPath.Row].descripcion;
				cell.ImageView.Image = UIImage.FromFile(TableItems[indexPath.Row].imgen_info);*/


				return cell;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				UIAlertController okAlertController = UIAlertController.Create("IR A la informacion de los campamentos", TableItems[indexPath.Row].titulo, UIAlertControllerStyle.Alert);
				okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

				owner.managerController.vc.PresentViewController(okAlertController, true, null);

    			tableView.DeselectRow(indexPath, true);
			}

			public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
			{
				UIAlertController okAlertController = UIAlertController.Create("IR A Mis mejores Campamentos Touched", TableItems[indexPath.Row].titulo, UIAlertControllerStyle.Alert);
				okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
				owner.managerController.vc.PresentViewController(okAlertController, true, null);

				tableView.DeselectRow(indexPath, true);
			}
		}

		public class CustomIndicesCell : UITableViewCell
		{
			UILabel headingLabel, subheadingLabel;
			UIImageView imageView;
			public CustomIndicesCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
			{
				SelectionStyle = UITableViewCellSelectionStyle.Gray;
				//ContentView.BackgroundColor = UIColor.FromRGB(218, 255, 127);
				imageView = new UIImageView();
				headingLabel = new UILabel()
				{
					//Font = UIFont.FromName("Cochin-BoldItalic", 22f),
					TextColor = UIColor.FromRGB(127, 51, 0),
					BackgroundColor = UIColor.Clear
				};
				subheadingLabel = new UILabel()
				{
					//Font = UIFont.FromName("AmericanTypewriter", 12f),
					//TextColor = UIColor.FromRGB(38, 127, 0),
					TextAlignment = UITextAlignment.Center,
					BackgroundColor = UIColor.Clear
				};

				ContentView.Frame = new CGRect(0,0,375,200);
				ContentView.Bounds = new CGRect(0, 0, 375, 200);

				ContentView.AddSubviews(new UIView[] { headingLabel, subheadingLabel, imageView });



			}
			public void UpdateCell(string caption, string subtitle, UIImage image)
			{
				imageView.Image = image;
				headingLabel.Text = caption;
				subheadingLabel.Text = subtitle;
			}
			public override void LayoutSubviews()
			{
				base.LayoutSubviews();
				imageView.Frame = new CGRect(ContentView.Bounds.Width - 63, 10, 33, 33);
				headingLabel.Frame = new CGRect(10, 5, ContentView.Bounds.Width - 63, 20);
				subheadingLabel.Frame = new CGRect(10, 20,ContentView.Bounds.Width - 63, 20);
			}
		}
	}
}

