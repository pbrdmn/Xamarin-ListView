using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ListView
{
	public interface IParseStorage
	{
		/* Attraction API */
		Task<List<Attraction>> AttractionsGetAllAsync ();
		Task<List<Attraction>> AttractionsSearchAsync (string search);
		Task<List<Attraction>> AttractionsGetBucketedAsync (string user_id);
		Task<List<Attraction>> AttractionsGetAchievedAsync (string user_id);
		Task<Attraction> AttractionGetAsync (string id);
		Task AttractionSaveAsync (Attraction a);
		Task AttractionDeleteAsync (Attraction a);

		/* Relation API */
		Task<List<Relation>> RelationsGetAllAsync(string userId);
		Task<Relation> RelationGetAsync (string id);
		Task<Relation> RelationForAttractionAsync (string activity_id, string user_id);
		Task RelationSaveAsync (Relation r);
		Task RelationDeleteAsync (Relation r);

		/* User API */
		Task<bool> UserLoginAsync (string email, string password);
		Task<bool> UserRegisterAsync (string name, string email, string phone, string password);
		Task<bool> UserExistsAsync (string email);
		Task UserSaveAsync (User u);
		void UserGetCurrentUser();
		void UserLogOutAsync();

		/* Save File */
		Task<string> FileSaveAsync (string filename, byte[] data);
	}
}

