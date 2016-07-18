using System;
using System.ComponentModel;

namespace ListView {
	public class Attraction {
		public Attraction (string id, string name, string description, string location, string image) {
			this.ID = id;
			this.Name = name;
			this.Description = description;
			this.Location = location;
			this.Image = image;
		}

		public Attraction () {
			this.ID = null;
			this.Name = null;
			this.Description = null;
			this.Location = null;
			this.Image = null;
		}

		public string ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public string Image { get; set; }

		public override string ToString() {
			return Name;
		}

		public async void AddToBucket () {
			Relation newRelation = new Relation(this.ID);
			await App.Data.storage.RelationSaveAsync (newRelation);
		}
	}
}

