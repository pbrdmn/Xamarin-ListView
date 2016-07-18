package parse;


public class ParsePushService
	extends mono.android.app.IntentService
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onHandleIntent:(Landroid/content/Intent;)V:GetOnHandleIntent_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Parse.ParsePushService, Parse.Android, Version=1.7.0.0, Culture=neutral, PublicKeyToken=null", ParsePushService.class, __md_methods);
	}


	public ParsePushService (java.lang.String p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == ParsePushService.class)
			mono.android.TypeManager.Activate ("Parse.ParsePushService, Parse.Android, Version=1.7.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public ParsePushService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ParsePushService.class)
			mono.android.TypeManager.Activate ("Parse.ParsePushService, Parse.Android, Version=1.7.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onHandleIntent (android.content.Intent p0)
	{
		n_onHandleIntent (p0);
	}

	private native void n_onHandleIntent (android.content.Intent p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
