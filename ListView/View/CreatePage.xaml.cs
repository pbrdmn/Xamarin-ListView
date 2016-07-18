using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;

namespace ListView {
	public partial class CreatePage : ContentPage {

		string pictureUrl;
		ToolbarItem CreateBtn;

		public CreatePage ()
		{
			InitializeComponent ();

			CameraBtn.Clicked += OnCameraBtnClicked;

			CreateBtn = new ToolbarItem {
				Text = "Create"
			};
			CreateBtn.Clicked += OnCreateBtnClicked;
			ToolbarItems.Add (CreateBtn);
		}

		public async void OnCreateBtnClicked (object sender, EventArgs e) {
			if (string.IsNullOrEmpty(Name.Text.Trim())) {
				await DisplayAlert ("Create", "Please enter a Name", "OK");
				Name.Focus();
				return;
			}
			if (string.IsNullOrEmpty(Description.Text.Trim())) {
				await DisplayAlert ("Create", "Please enter a Description", "OK");
				Description.Focus();
				return;
			}
			if (string.IsNullOrEmpty(Location.Text.Trim())) {
				await DisplayAlert ("Create", "Please enter a Location", "OK");
				Location.Focus();
				return;
			}

			// Put the file into Parse and retrieve the Parse URL
			// HELP: The ParseFile.SaveAsync() method crashes.
			// TODO: Put this file somewhere on the internet so other devices can access the image
			//pictureUrl = await App.Data.storage.FileSaveAsync (pictureName, pictureData);

			Attraction createAttraction = new Attraction (
				null,
				Name.Text.Trim(),
				Description.Text.Trim(),
				Location.Text.Trim(),
				pictureUrl
			);

			try {
				await App.Data.storage.AttractionSaveAsync (createAttraction);
				await DisplayAlert ("Create", "Attraction Saved", "Continue");
				await Navigation.PopAsync();
			} catch (Exception ex) {
				await DisplayAlert ("Create", "There was a problem. Please try again", "OK");
			}
		}

		public async void OnCameraBtnClicked (object sender, EventArgs e) {
			await GetPictureAsync ();
		}

		private async Task GetPictureAsync() {
			try {
				// Attempt to use the camera or fall-back to picking from the camera roll
				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
					var mediaFile = await CrossMedia.Current.PickPhotoAsync();
					if (mediaFile == null) return;

					string pictureName = mediaFile.Path.Substring(mediaFile.Path.LastIndexOf('/') + 1);
					byte[] pictureData;
					using (BinaryReader br = new BinaryReader(mediaFile.GetStream())) {
						long fileSize = mediaFile.GetStream().Length;
						pictureData = br.ReadBytes((int)fileSize);
					}
					pictureUrl = await App.Data.storage.FileSaveAsync(pictureName, pictureData);
					Picture.Source = pictureUrl;
				} else {
					var mediaFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions());
					if (mediaFile == null) return;

					string pictureName = mediaFile.Path.Substring(mediaFile.Path.LastIndexOf('/') + 1);
					byte[] pictureData;
					using (BinaryReader br = new BinaryReader(mediaFile.GetStream())) {
						long fileSize = mediaFile.GetStream().Length;
						pictureData = br.ReadBytes((int)fileSize);
					}
					pictureUrl = await App.Data.storage.FileSaveAsync(pictureName, pictureData);
					Picture.Source = pictureUrl;
				}

			} catch (Exception e) {
				await DisplayAlert ("Create Attraction", string.Format ("Camera Error. {0}", e.Message), "OK");
			}
		}
	}
}

