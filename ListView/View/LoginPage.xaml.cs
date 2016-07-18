using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListView
{
	public partial class LoginPage : ContentPage
	{
		public bool newUser;
		ToolbarItem LoginBtn;

		public LoginPage ()
		{
			InitializeComponent ();

			Email.Completed += OnEmailCompleted;

			Password.Completed += OnPasswordCompleted;

			LoginBtn = new ToolbarItem ();
			LoginBtn.Clicked += OnPasswordCompleted;
			ToolbarItems.Add (LoginBtn);

			Appearing += (object sender, EventArgs e) => {
				Email.Focus ();
			};
		}

		public async void OnEmailCompleted (object sender, EventArgs e) {
			NameHolder.IsVisible = false;
			PhoneHolder.IsVisible = false;
			PasswordHolder.IsVisible = false;

			if (string.IsNullOrEmpty(Email.Text) || (Email.Text.Trim().Length < 1)) {
				await DisplayAlert ("Login", "Please complete your email", "OK");
				Email.Focus();
				return;
			}

			if (await App.Data.storage.UserExistsAsync (Email.Text.Trim ())) {
				newUser = false;
				PasswordHolder.IsVisible = true;
				LoginBtn.Text = "Login";
				Password.Focus();
			} else {
				newUser = true;
				NameHolder.IsVisible = true;
				PhoneHolder.IsVisible = true;
				PasswordHolder.IsVisible = true;
				LoginBtn.Text = "Register";
				Name.Focus();
			}
		}

		public async void OnPasswordCompleted (object sender, EventArgs e) {
			if (newUser) {
				await CheckRegister();
			} else {
				await CheckLogin ();
			}
		}

		public async Task CheckLogin () {
			if (string.IsNullOrEmpty(Email.Text) || (Email.Text.Trim().Length < 1)) {
				await DisplayAlert ("Login", "Please complete your email", "OK");
				Email.Focus();
				return;
			}
			if (string.IsNullOrEmpty(Password.Text) || (Password.Text.Trim().Length < 6)) {
				await DisplayAlert ("Login", "Please check your password, minimum 6 characters", "OK");
				Password.Focus();
				return;
			}

			if (await App.Data.storage.UserLoginAsync (Email.Text.Trim (), Password.Text.Trim ())) {
				await Navigation.PopAsync ();
			} else {
				await DisplayAlert("Invalid Login", "Please check your details", "OK");
			}
		}

		public async Task CheckRegister () {
			if (string.IsNullOrEmpty(Email.Text) || (Email.Text.Trim().Length < 1)) {
				await DisplayAlert ("Register", "Please complete your email", "OK");
				Email.Focus();
				return;
			}
			if (string.IsNullOrEmpty(Name.Text) || (Name.Text.Trim().Length < 2)) {
				await DisplayAlert ("Register", "Please complete your name", "OK");
				Name.Focus();
				return;
			}
			if (string.IsNullOrEmpty(Phone.Text) || (Phone.Text.Trim().Length < 8)) {
				await DisplayAlert ("Register", "Please complete your phone", "OK");
				Phone.Focus();
				return;
			}
			if (string.IsNullOrEmpty(Password.Text) || (Password.Text.Trim().Length < 6)) {
				await DisplayAlert ("Register", "Please check your password, minimum 6 characters", "OK");
				Password.Focus();
				return;
			}

			if (await App.Data.storage.UserRegisterAsync(
				Name.Text.Trim(),
				Email.Text.Trim(),
				Phone.Text.Trim(),
				Password.Text.Trim())) {
				await DisplayAlert ("Registration Success", "Please Login", "Continue");

				newUser = false;
				NameHolder.IsVisible = false;
				PhoneHolder.IsVisible = false;
				PasswordHolder.IsVisible = true;
				Password.Focus();
			} else {
				await DisplayAlert ("Registration Error", "Please try again", "OK");
			}
		}
	}
}

