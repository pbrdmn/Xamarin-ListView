using System;

using Xamarin.Forms;

namespace ListView
{
	public class App : Application
	{
		public App (AppData appData)
		{
			SetData (appData);

			appData.storage.UserGetCurrentUser ();
			
			// The root page of your application
			MainPage = new NavigationPage (new TabsPage()) {
				BarBackgroundColor = Color.FromHex("#008877"),
				BarTextColor = Color.White
			};
		}

		public static AppData data;
		public static AppData Data {
			get { return data; }
			set { data = value; }
		}
		public static void SetData (AppData appData) {
			Data = appData;
		}
			

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

