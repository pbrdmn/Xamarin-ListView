using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ListView
{
	public partial class DetailsPage : ContentPage
	{
		public ToolbarItem bucketBtn;
		public ToolbarItem completeBtn;

		public Relation relation;
		public Attraction attraction;

		public DetailsPage (Attraction selectedAttraction)
		{
			InitializeComponent ();

			attraction = selectedAttraction;
			BindingContext = selectedAttraction;
			relation = null;

			completeBtn = new ToolbarItem {};
			completeBtn.Clicked += OnCompleteBtnClicked;
			ToolbarItems.Add (completeBtn);

			bucketBtn = new ToolbarItem {};
			bucketBtn.Clicked += OnBucketBtnClicked;
			ToolbarItems.Add (bucketBtn);

			Appearing += OnPageAppearing;
		}

		public async void OnPageAppearing (object sender, EventArgs e) {
			if (App.Data.CurrentUser == null) {
				completeBtn.Text = "";
				bucketBtn.Text = "Login";
				return;
			}

			completeBtn.Text = "";
			bucketBtn.Text = "Bucket";
			
			string attraction_id = attraction.ID;
			string user_id = App.Data.CurrentUser.ID;
			relation = await App.Data.storage.RelationForAttractionAsync (attraction_id, user_id);
			if (relation != null) {
				// The attraction has been bucketed
				if (relation.Achieved == null) {
					completeBtn.Text = "Complete";
					bucketBtn.Text = "Unbucket";
				} else {
					completeBtn.Text = "Completed";
					bucketBtn.Text = "";
				}
			}	
		}

		public async void OnBucketBtnClicked (object sender, EventArgs e) {
			if (App.Data.CurrentUser == null) {
				await Navigation.PushAsync (new LoginPage ());
				return;
			}

			if (relation == null) {
				// The attraction has not been bucketed
				// Create relation
				Relation newRelation = new Relation(attraction.ID);
				await App.Data.storage.RelationSaveAsync(newRelation);
				relation = newRelation;
				bucketBtn.Text = "Unbucket";
				completeBtn.Text = "Complete";
			} else {
				// The attraction has been bucketed
				// Delete relation
				await App.Data.storage.RelationDeleteAsync(relation);
				relation = null;
				completeBtn.Text = "";
				bucketBtn.Text = "Bucket";
			}
		}

		public async void OnCompleteBtnClicked (object sender, EventArgs e) {
			if (relation == null) {
				return;
			}

			if (relation.Achieved == null) {
				// The attraction has not been bucketed
				// Create relation
				relation.Achieved = DateTime.Now;
				await App.Data.storage.RelationSaveAsync(relation);
				completeBtn.Text = "Completed";
			} else {
				// The attraction has been bucketed
				// Delete relation
				relation.Achieved = null;
				await App.Data.storage.RelationSaveAsync(relation);
				completeBtn.Text = "Complete";
			}
		}
	}
}

