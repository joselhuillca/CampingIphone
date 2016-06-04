using System;
using UIKit;
using CoreGraphics;

namespace PeruCamping
{
	public class JSFavStarControl: UIControl
	{
		int RATING_MAX=5;
		
		private int _rating;
		private UIImage _dot, _star;

		public int rating {
			get{
				return _rating;
			}
			set{ 
				_rating = value;
			}
		}

		public JSFavStarControl (CGPoint location,UIImage dotImage ,UIImage starImage)
		{
			_rating = 0;
			this.BackgroundColor = UIColor.Clear;
			this.Opaque = false;

			//_dot = dotImage.Retain();
			//_star = starImage.retain();
		}

		public void drawRect(CGRect rect)
		{
			CGPoint currPoint = new CGPoint(0,0);

			for (int i = 0; i < _rating; i++)
			{
				if (_star!=null){
					_star.Draw(currPoint);
				}
				else{
					//[@"★" drawAtPoint:currPoint withFont:[UIFont boldSystemFontOfSize:22]];
					("★").DrawString(currPoint,UIFont.BoldSystemFontOfSize(22));
				}

				currPoint.X += 20;
			}

			int remaining = RATING_MAX - _rating;

			for (int i = 0; i < remaining; i++)
			{
				if (_dot!=null){
					_dot.Draw(currPoint);
				}
				else{
					//[@" •" drawAtPoint:currPoint withFont:[UIFont boldSystemFontOfSize:22]];
					(" •").DrawString(currPoint,UIFont.BoldSystemFontOfSize(22));
				}
				currPoint.X += 20;
			}
		}

		public void dealloc()
		{
			_dot.DangerousRelease();
			_star.DangerousRelease();

			_dot = null;
			_star = null;

			base.Delete (null);
		}

		public override bool BeginTracking(UITouch touch,UIEvent e)
		{
			nfloat width = this.Frame.Size.Width;
			CGRect section = new CGRect(0, 0, width / RATING_MAX, this.Frame.Size.Height);

			CGPoint touchLocation=touch.LocationInView(this);

			for (int i = 0; i < RATING_MAX; i++)
			{		
				if (touchLocation.X > section.Location.X && touchLocation.X < section.Location.X + section.Size.Width)
				{ // touch is inside section
					if (_rating != (i+1))
					{
						_rating = i+1;
						this.SendActionForControlEvents(UIControlEvent.ValueChanged);
					}

					break;
				}

				section.X += section.Size.Width;
			}

			this.SetNeedsDisplay();
			return true;
		}

		public override bool ContinueTracking(UITouch touch,UIEvent e)
		{
			nfloat width = this.Frame.Size.Width;
			CGRect section = new CGRect(0, 0, width / RATING_MAX, this.Frame.Size.Height);

			CGPoint touchLocation = touch.LocationInView(this);

			if (touchLocation.X < 0)
			{
				if (_rating != 0)
				{	
					_rating = 0;
					this.SendActionForControlEvents(UIControlEvent.ValueChanged);
				}
			}
			else if (touchLocation.X > width)
			{
				if (_rating != 5)
				{
					_rating = 5;
					this.SendActionForControlEvents(UIControlEvent.ValueChanged);
				}
			}
			else
			{
				for (int i = 0; i < RATING_MAX; i++)
				{
					if (touchLocation.X > section.Location.X && touchLocation.X < section.Location.X + section.Size.Width)
					{ // touch is inside section
						if (_rating != (i+1))
						{
							_rating = i+1;
							this.SendActionForControlEvents(UIControlEvent.ValueChanged);
						}
						break;
					}
					section.X += section.Size.Width;
				}
			}
			this.SetNeedsDisplay();
			return true;
		}

		public void EndTracking(UITouch touch,UIEvent e)
		{
			nfloat width = this.Frame.Size.Width;
			CGRect section = new CGRect(0, 0, width / RATING_MAX, this.Frame.Size.Height);

			CGPoint touchLocation = touch.LocationInView(this);

			if (touchLocation.X < 0)
			{
				if (_rating != 0)
				{	
					_rating = 0;
					this.SendActionForControlEvents(UIControlEvent.ValueChanged);
				}
			}
			else if (touchLocation.X > width)
			{
				if (_rating != 5)
				{
					_rating = 5;
					this.SendActionForControlEvents(UIControlEvent.ValueChanged);
				}

			}
			else
			{
				for (int i = 0; i < RATING_MAX; i++)
				{
					if (touchLocation.X > section.Location.X && touchLocation.X < section.Location.X + section.Size.Width)
					{
						if (_rating != (i+1))
						{
							_rating = i+1;
							this.SendActionForControlEvents(UIControlEvent.ValueChanged);
						}

						break;
					}

					section.X += section.Size.Width;
				}
			}

			this.SetNeedsDisplay();
		}

	}
}

