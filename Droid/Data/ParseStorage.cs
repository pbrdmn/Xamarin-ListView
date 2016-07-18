using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;

using System.Collections.Generic;
using System.Text;
using System.IO;

using Android.Graphics;

namespace ListView.Droid
{
	public class ParseStorage : IParseStorage
	{
		static ParseStorage todoServiceInstance = new ParseStorage();
		public static ParseStorage Default { get { return todoServiceInstance; } }

		protected ParseStorage () {
			ParseClient.Initialize (Constants.ApplicationId, Constants.Key);
		}

		/* Attraction API */
		public async Task<List<Attraction>> AttractionsSearchAsync (string search) {
			// Simplistic search, should search more fields, order by best match
			var query = ParseObject.GetQuery ("Attraction").WhereContains("Description", search).OrderBy ("Name");
			var rs = await query.FindAsync ();
			var list = new List<Attraction> ();
			foreach (var r in rs) {
				list.Add (AttractionFromParseObject (r));
			}
			return list;
		}
		public async Task<List<Attraction>> AttractionsGetAllAsync () {
			var query = ParseObject.GetQuery ("Attraction").OrderBy ("Name");
			var rs = await query.FindAsync ();
			var list = new List<Attraction> ();
			foreach (var r in rs) {
				list.Add (AttractionFromParseObject (r));
			}
			return list;
		}
		public async Task<List<Attraction>> AttractionsGetBucketedAsync (string user_id) {
			var bucketedRelations = ParseObject.GetQuery ("Relation").WhereEqualTo ("User", user_id).WhereEqualTo("Achieved", null);
			var bucketedAttractions = from attraction in ParseObject.GetQuery ("Attraction")
				join relation in bucketedRelations on attraction ["objectId"] equals relation ["Attraction"]
				select attraction;
			var rs = await bucketedAttractions.FindAsync ();
			var list = new List<Attraction> ();
			foreach (var r in rs) {
				list.Add (AttractionFromParseObject (r));
			}
			return list;
		}
		public async Task<List<Attraction>> AttractionsGetAchievedAsync (string user_id) {
			var bucketedRelations = ParseObject.GetQuery ("Relation").WhereEqualTo ("User", user_id).WhereNotEqualTo("Achieved", null);
			var bucketedAttractions = from attraction in ParseObject.GetQuery ("Attraction")
				join relation in bucketedRelations on attraction ["objectId"] equals relation ["Attraction"]
				select attraction;
			var rs = await bucketedAttractions.FindAsync ();
			var list = new List<Attraction> ();
			foreach (var r in rs) {
				list.Add (AttractionFromParseObject (r));
			}
			return list;
		}
		public async Task<Attraction> AttractionGetAsync (string id) {
			var query = ParseObject.GetQuery ("Attraction").WhereEqualTo ("objectId", id);
			var r = await query.FirstAsync();
			return AttractionFromParseObject(r);
		}
		public async Task AttractionSaveAsync (Attraction a) {
			await AttractionToParseObject (a).SaveAsync ();
		}
		public async Task AttractionDeleteAsync (Attraction a) {
			await AttractionToParseObject (a).DeleteAsync ();
		}
		static Attraction AttractionFromParseObject(ParseObject o) {
			return new Attraction (
				o.ObjectId,
				o ["Name"].ToString(),
				o ["Description"].ToString(),
				o ["Location"].ToString(),
				(string) (o ["Image"] ?? "wollongong.jpg") // Default image if none is set
			);
		}
		static ParseObject AttractionToParseObject(Attraction a) {
			var o = new ParseObject ("Attraction");
			if (a.ID != null) {
				o.ObjectId = a.ID;
			}
			o ["Name"] = a.Name;
			o ["Description"] = a.Description;
			o ["Location"] = a.Location;
			o ["Image"] = a.Image;
			return o;
		}

		/* Relation API */
		public async Task<List<Relation>> RelationsGetAllAsync(string userId) {
			var list = new List<Relation> ();
			try {
				var query = ParseObject.GetQuery ("Attraction").WhereEqualTo ("User", userId);
				var rs = await query.FindAsync ();
				foreach (var r in rs) {
					list.Add (RelationFromParseObject (r));
				}
			} catch (Exception e) {
			}
			return list;
		}
		public async Task<Relation> RelationGetAsync (string id) {
			try {
				var query = ParseObject.GetQuery ("Relation").WhereEqualTo ("objectId", id);
				var r = await query.FirstAsync();
				return RelationFromParseObject(r);
			} catch (Exception e) {
				return null;
			}
		}
		public async Task RelationSaveAsync (Relation r) {
			await RelationToParseObject (r).SaveAsync ();
		}
		public async Task RelationDeleteAsync (Relation r) {
			await RelationToParseObject (r).DeleteAsync ();
		}
		public async Task<Relation> RelationForAttractionAsync (string attraction_id, string user_id) {
			try {
				var query = ParseObject.GetQuery ("Relation").WhereEqualTo ("Attraction", attraction_id).WhereEqualTo("User", user_id);
				var r = await query.FirstAsync();
				return RelationFromParseObject(r);
			} catch (Exception e) {
				return null;
			}
		}
		static Relation RelationFromParseObject(ParseObject o) {
			return new Relation (
				o.ObjectId,
				o ["Attraction"].ToString(),
				o ["User"].ToString(),
				o ["Notes"].ToString(),
				(DateTime?)o ["Bucketed"],
				(DateTime?)o ["Achieved"]
			);
		}
		static ParseObject RelationToParseObject(Relation a) {
			var o = new ParseObject ("Relation");
			if (a.ID != null) {
				o.ObjectId = a.ID;
			}
			o ["Attraction"] = a.Attraction;
			o ["User"] = a.User;
			o ["Notes"] = a.Notes;
			o ["Bucketed"] = a.Bucketed;
			o ["Achieved"] = a.Achieved;
			return o;
		}

		/* User API */
		public async Task<bool> UserLoginAsync (string email, string password) {
			try
			{
				await ParseUser.LogInAsync(email, password);
				App.Data.CurrentUser = UserFromParseObject(ParseUser.CurrentUser);
				return true;
			} catch (Exception e) {
				// The login failed. Check the error to see why.
				return false;
			}
		}
		public async Task<bool> UserRegisterAsync (string name, string email, string phone, string password) {
			try {
				var user = new ParseUser () {
					Username = email,
					Password = password,
					Email = email
				};
				user ["Name"] = name;
				user ["Phone"] = phone;

				await user.SignUpAsync();
				return true;
			} catch (Exception e) {
				return false;
			}
		}
		public async Task<bool> UserExistsAsync (string email) {
			try {
				await (from user in ParseUser.Query
					where user.Get<string>("email") == email
					select user).FirstAsync();
				return true;
			} catch (Exception e) {
				return false;
			}
		}
		public async Task UserSaveAsync (User u) {
			await UserToParseObject (u).SaveAsync ();
		}
		public void UserGetCurrentUser () {
			try {
				App.Data.CurrentUser = UserFromParseObject(ParseUser.CurrentUser);
			} catch (Exception e) {
				App.Data.CurrentUser = null;
			}
		}
		public void UserLogOutAsync () {
			ParseUser.LogOut();
		}
		static User UserFromParseObject(ParseObject o) {
			return new User (
				o.ObjectId,
				o ["Name"].ToString (),
				o ["email"].ToString (),
				o ["Phone"].ToString ()
			);
		}
		static ParseObject UserToParseObject(User u) {
			var o = new ParseObject ("User");
			if (u.ID != null) {
				o.ObjectId = u.ID;
			}
			o ["Name"] = u.Name;
			o ["email"] = u.Email;
			o ["Phone"] = u.Phone;
			return o;
		}

		/* Save File */
		public async Task<string> FileSaveAsync (string filename, byte[] data) {
			try {
				byte[] thumbData = ResizeImage(data, 640, 360);
				ParseFile file = new ParseFile (filename, thumbData);
				await file.SaveAsync ();
				Uri uri = file.Url;
				return file.Url.ToString();
			} catch (Exception e) {
				var i = e;
				return "wollongong.jpg";
			}
		}
		public static byte[] ResizeImage(byte[] imageData, float width, float height) {
			// Load the bitmap 
			Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

			float TargetHeight = 0;
			float TargetWidth = 0;

			var Height = originalImage.Height;
			var Width = originalImage.Width;

			if (Height > Width) // Tall Images
			{
				TargetHeight = height;
				float ratio = Height / height;
				TargetWidth = Width / ratio;
			}
			else // Wide Images
			{
				TargetWidth = width;
				float ratio = Width / width;
				TargetHeight = Height / ratio;
			}

			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)TargetWidth, (int)TargetHeight, false);

			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
				return ms.ToArray();
			}
		}
	}
}