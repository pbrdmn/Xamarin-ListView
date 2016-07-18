using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ListView
{
	public class AppData : INotifyPropertyChanged
	{
		//the view will register to this event when the DataContext is set
		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged(string propName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		public AppData (IParseStorage storage) 
		{
			this.storage = storage;
			CurrentUser = null;
		}

		public IParseStorage storage;
		public List<User> UserList;

		private User _currentUser;
		public User CurrentUser {
			get { return _currentUser; }
			set {
				if (value == _currentUser)
					return;
				_currentUser = value;
				RaisePropertyChanged ("CurrentUser");
			}
		}
	}
}

