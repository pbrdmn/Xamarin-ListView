using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ListView
{
	public partial class UserPage : ContentPage
	{
		public UserPage ()
		{
			InitializeComponent ();

			this.BindingContext = App.Data.CurrentUser;

			//UpdateBtn.Clicked += OnUpdateBtnClicked;

			var logoutBtn = new ToolbarItem {
				Text = "Logout"
			};
			logoutBtn.Clicked += OnLoginBtnClicked;

			this.ToolbarItems.Add (logoutBtn);
		}

		/*
		public async void OnUpdateBtnClicked (object sender, EventArgs e) {
			try {
				await App.Data.storage.UserSaveAsync (App.Data.CurrentUser);
				await Navigation.PopAsync();
			} catch (Exception ex) {
				await DisplayAlert ("Update", "Request failed, please try again", "OK");
			}
		}
		*/

		public async void OnLoginBtnClicked (object sender, EventArgs e) {
			App.Data.storage.UserLogOutAsync ();
			App.Data.CurrentUser = null;
			await Navigation.PopAsync();
		}
	}
}

