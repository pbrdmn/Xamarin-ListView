using System;
using System.ComponentModel;

namespace ListView {
	public class Relation {
		public Relation (string id, string activity_id, string user_id, string notes, DateTime? bucketed, DateTime? achieved) {
			this.ID = id;
			this.Attraction = activity_id;
			this.User = user_id;
			this.Notes = notes;
			this.Bucketed = bucketed;
			this.Achieved = achieved;
		}

		public Relation (string id, string activity_id, string user_id, string notes) {
			this.ID = id;
			this.Attraction = activity_id;
			this.User = user_id;
			this.Notes = notes;
			this.Bucketed = DateTime.Now;
			this.Achieved = null;
		}

		public Relation (string activity_id) {
			this.ID = null;
			this.Attraction = activity_id;
			this.User = App.Data.CurrentUser.ID;
			this.Notes = "";
			this.Bucketed = DateTime.Now;
			this.Achieved = null;
		}

		public string ID { get; set; }
		public string Attraction { get; set; }
		public string User { get; set; }
		public string Notes { get; set; }
		public DateTime? Bucketed { get; set; }
		public DateTime? Achieved { get; set; }

		public void Complete () {
			this.Achieved = DateTime.Now;
		}
	}
}

