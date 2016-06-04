using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace PeruCamping
{
	public class LabelViewBarraTitulo  : UIBaseView
	{
		private UILabel textLabel;

		public NSString text;

		public LabelViewBarraTitulo ()
		{
		}

		public LabelViewBarraTitulo (NSString _text)
		{
			text = _text;
			textLabel = new UILabel();        
			textLabel.BackgroundColor = UIColor.Clear;
			this.AddSubview(textLabel);
		}

		public override void reDraw(){
			base.reDraw();
			textLabel.Text = text;
			nuint position = (nuint)(this.attributePositionToRedraw());
			textLabel.TextColor = UtilManagment.getColorFromRect(CGRectFromString(this.getFontColorTitleAttribute().GetItem<NSString>(position)));
			textLabel.Font = UIFont.FromName((this.getFontTitleAttribute().GetItem<NSString>(position)),(nfloat)(Double.Parse(this.getFontSizeTitleAttribute().GetItem<NSString>(position))));
			textLabel.Frame = this.Bounds;
		}

		public override void inAnimation(){
			base.inAnimation();       
		}

		public override void outAnimation(){
			base.outAnimation();       
		}

		public override void prepareRemove(){
			base.prepareRemove();
			textLabel = null;
			text = null;
		}
	}
}

