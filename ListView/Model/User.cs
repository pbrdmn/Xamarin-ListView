using System;

namespace ListView {
	public class User {
		public User (string id, string name, string email, string phone, string password) {
			this.ID = id;
			this.Name = name;
			this.Email = email;
			this.Phone = phone;
			this.Password = password;
		}

		public User (string id, string name, string email, string phone) {
			this.ID = id;
			this.Name = name;
			this.Email = email;
			this.Phone = phone;
		}

		public string ID { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Password { get; set; }

		public override string ToString () {
			return string.Format ("[User: ID={0}, Name={1}, Email={2}, Phone={3}, Password={4}]", ID, Name, Email, Phone, Password);
		}
	}
}

