package md54c7c7b53b00edbd13dcfeee4023055b9;


public class Welcome
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("FinalPrice.Welcome, FinalPrice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Welcome.class, __md_methods);
	}


	public Welcome () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Welcome.class)
			mono.android.TypeManager.Activate ("FinalPrice.Welcome, FinalPrice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
