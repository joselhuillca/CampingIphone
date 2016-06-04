using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace PeruCamping
{
	public class LabelView : UIBaseView
	{
		private UILabel textLabel;

		public NSString text;

		public LabelView ()
		{
		}

		public LabelView (NSString _text)
		{
			text = _text;
			textLabel = new UILabel();        
			textLabel.BackgroundColor = UIColor.Clear;
			this.AddSubview(textLabel);
		}

		public override void reDraw(){
			base.reDraw();
			////NSLog(@"%@ redrow",NSStringFromClass(self.class));
			textLabel.Text = text;
			nuint position = (nuint)(this.attributePositionToRedraw());
			//textLabel.textColor = [UtilManagment getColorFromRect:CGRectFromString([[self getFontColorTitleAttribute] objectAtIndex:[self attributePositionToRedraw]])]; 
			textLabel.TextColor = UtilManagment.getColorFromRect(CGRectFromString(this.getFontColorTitleAttribute().GetItem<NSString>(position)));
			//textLabel.font = [UIFont fontWithName:[[self getFontTitleAttribute] objectAtIndex:[self attributePositionToRedraw]] size:[[[self getFontSizeTitleAttribute] objectAtIndex:[self attributePositionToRedraw]] intValue]];
			textLabel.Font = UIFont.FromName((this.getFontTitleAttribute().GetItem<NSString>(position)),(nfloat)(Double.Parse(this.getFontSizeTitleAttribute().GetItem<NSString>(position))));
			//textLabel.frame = self.bounds; 
			textLabel.Frame = this.Bounds;
		}

		public override void inAnimation(){
			base.inAnimation();    
			base.inAnimationUpDownRightLeft(new CGRect(1, 0, 1, 1),Constantes.transitionTime,0,this);    
		}

		public override void outAnimation(){
			base.outAnimation();    
			base.outAnimationUpDownRightLeft(new CGRect(-1, 0, 1, 1),Constantes.transitionTime,0,this);    
		}

		public override void prepareRemove(){
			base.prepareRemove();
			textLabel = null;
			text = null;
		}
	}
}

