using System;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;

namespace PeruCamping
{
	public class AudioController: NSObject
	{
		private AVAudioPlayer audioPlayer;

		public AudioController ()
		{
		}

		public void playAudioWithUrl(NSUrl audioUrl)
		{
			

			Task.Factory.StartNew(() => { 
			
				audioPlayer = AVAudioPlayer.FromUrl(audioUrl);
				audioPlayer.PrepareToPlay();
				audioPlayer.Play();
			});

			 
		}
		public void stopAudio()
		{
			audioPlayer.Stop();
		}
	}
}

