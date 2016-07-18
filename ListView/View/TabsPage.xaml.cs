using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ListView
{
	public partial class TabsPage : TabbedPage
	{
		public ToolbarItem loginBtn;
		public ToolbarItem createBtn;

		public TabsPage ()
		{
			InitializeComponent ();

			discoverListView.ItemTapped += OnAttractionTapped;
			bucketedListView.ItemTapped += OnAttractionTapped;
			achievedListView.ItemTapped += OnAttractionTapped;

			bucketedListView.ItemsSource = null;
			achievedListView.ItemsSource = null;


			createBtn = new ToolbarItem {};
			createBtn.Clicked += OnCreateBtnClicked;
			this.ToolbarItems.Add (createBtn);

			loginBtn = new ToolbarItem {};
			loginBtn.Clicked += OnLoginBtnClicked;
			this.ToolbarItems.Add (loginBtn);
				
			this.Appearing += OnTabsPageAppearing;
			this.CurrentPageChanged += OnCurrentPageChanged;

			SearchTerm.TextChanged += OnSearchTermCompleted;
		}

		public async void OnTabsPageAppearing (object sender, EventArgs e) {
			TabsPage page = (TabsPage)sender;
			if (App.Data.CurrentUser != null) {
				int firstSpace = App.Data.CurrentUser.Name.IndexOf (' ');
				string firstName = App.Data.CurrentUser.Name.Substring (0, firstSpace);
				page.loginBtn.Text = firstName;
				page.createBtn.Text = "Create";
			} else {
				page.loginBtn.Text = "Login";
				page.createBtn.Text = "";
			}

			// Return to Discover tab after logout
			if ((App.Data.CurrentUser == null) && (this.CurrentPage != this.Children[0])) {
				this.CurrentPage = this.Children[0];
				return;
			}

			// Load data for the appropriate ListView
			if (this.CurrentPage == this.Children[0]) {
				discoverListView.ItemsSource = await App.Data.storage.AttractionsGetAllAsync ();
				return;
			}
			if (this.CurrentPage == this.Children [1]) {
				bucketedListView.ItemsSource = await App.Data.storage.AttractionsGetBucketedAsync (App.Data.CurrentUser.ID);
				return;
			}
			if (this.CurrentPage == this.Children [2]) {
				achievedListView.ItemsSource = await App.Data.storage.AttractionsGetAchievedAsync (App.Data.CurrentUser.ID);
				return;
			}
		}

		public async void OnLoginBtnClicked (object sender, EventArgs e) {
			if (App.Data.CurrentUser == null) {
				await Navigation.PushAsync (new LoginPage ());
			} else {
				await Navigation.PushAsync (new UserPage ());
			}
		}

		public async void OnCreateBtnClicked (object sender, EventArgs e) {
			await Navigation.PushAsync(new CreatePage());
		}

		public async void OnCurrentPageChanged (object sender, EventArgs e) {
			// This will display the Login page before showing "Bucketed" or "Achieved" tabs
			if ((App.Data.CurrentUser == null) && (this.CurrentPage != this.Children[0])) {
				this.CurrentPage = this.Children[0];
				await Navigation.PushAsync (new LoginPage ());
				return;
			}

			if (this.CurrentPage == this.Children[0]) {
				discoverListView.ItemsSource = await App.Data.storage.AttractionsGetAllAsync ();
				return;
			}

			if (this.CurrentPage == this.Children [1]) {
				bucketedListView.ItemsSource = await App.Data.storage.AttractionsGetBucketedAsync (App.Data.CurrentUser.ID);
				return;
			}

			if (this.CurrentPage == this.Children [2]) {
				achievedListView.ItemsSource = await App.Data.storage.AttractionsGetAchievedAsync (App.Data.CurrentUser.ID);
				return;
			}
		}

		public async void OnAttractionTapped (object sender, ItemTappedEventArgs e) {
			var selectedAttraction = (Attraction)e.Item;
			var detailsPage = new DetailsPage(selectedAttraction);
			await Navigation.PushAsync(detailsPage);
		}

		public async void OnSearchTermCompleted (object sender, EventArgs e) {
			if (string.IsNullOrEmpty (SearchTerm.Text)) {
				// Reset the list to default
				discoverListView.ItemsSource = await App.Data.storage.AttractionsGetAllAsync ();
			} else {
				// Search for the term
				discoverListView.ItemsSource = await App.Data.storage.AttractionsSearchAsync (SearchTerm.Text);
			}

		}
	}
}

