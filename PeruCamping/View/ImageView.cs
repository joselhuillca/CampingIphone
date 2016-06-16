using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.Drawing;

namespace PeruCamping
{
	public class ImageView : UIBaseView
	{
		public NSString isScaled; 

		public ImageView(NSString isScaled_)
		{
			this.isScaled = isScaled_;
		}

		public ImageView ()
		{
		}

		[Export ("reDraw")]
		public override void reDraw(){
			base.reDraw();
			NSArray collection = this.getImageBackgroundColorAttribute() as NSArray;
			if (collection != null && isScaled.Equals(new NSString("true")))
			{
				try
				{

					UIImage img = UtilManagment.imageFromDocumentDirectory(collection.GetItem<NSString>((nuint)this.attributePositionToRedraw()));
					img = Constantes.ScaleToSize(img, (int)this.Bounds.Width, (int)this.Bounds.Height);//HUILLCA -- escalar la imagen
					this.BackgroundColor = UIColor.FromPatternImage(img);
				}
				catch (Exception e)
				{
					//Error al cargar la imagen
				}

			}
		}

		public void stopAnimations(){//Para el Grid view
			////NSLog(@"grid view stop animations");
			//UITransitionBaseView transitionView;    
			//for (int i = 0 ; i < resourceViewArray.count ; i++) {                
				//transitionView = [resourceViewArray objectAtIndex:i];
				//[transitionView stop];        

			//}
		}

		public override void inAnimation(){
			base.inAnimation();
			this.Alpha = 0;
			this.Transform = CGAffineTransform.Scale(this.Transform, 1.0f, 0.1f);
  
			UIView.BeginAnimations(null,new IntPtr());
			UIView.SetAnimationDuration(Constantes.transitionTime);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);

			this.Transform = CGAffineTransform.MakeIdentity();
			this.Alpha = 1;

			UIView.CommitAnimations();
		}

		public override void outAnimation(){    
			this.stopAnimations();//solo para Gridview
			base.outAnimation();

			UIView.BeginAnimations(null,new IntPtr());
			UIView.SetAnimationDuration(Constantes.transitionTime);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);

			this.Transform = CGAffineTransform.Scale(this.Transform, 1.0f, 0.1f);
			this.Alpha = 0;

			UIView.CommitAnimations();
			this.Transform = CGAffineTransform.MakeIdentity ();
		}


	}
}

