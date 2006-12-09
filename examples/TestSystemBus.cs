// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
using GLib;
using Gtk;
using NDesk.DBus;
using org.freedesktop.DBus;

public class TestGLib
{
	static Bus bus;
	static IBus sysBus;

	static DemoObject demo;

	public static void Main ()
	{
		BusG.Init ();
		Application.Init ();

		Window win = new Window ("D-Bus#");
		win.SetDefaultSize (640, 480);
		win.Destroyed += delegate {Application.Quit ();};
		win.ShowAll ();

		bus = Bus.Session;
		sysBus = Bus.System.GetObject<IBus> ("org.freedesktop.DBus", new ObjectPath ("/org/freedesktop/DBus"));

		string myNameReq = "org.ndesk.gtest";
		ObjectPath myPath = new ObjectPath ("/org/ndesk/test");

		if (bus.NameHasOwner (myNameReq)) {
			demo = bus.GetObject<DemoObject> (myNameReq, myPath);
		} else {
			RequestNameReply nameReply = bus.RequestName (myNameReq, NameFlag.None);

			Console.WriteLine ("nameReply: " + nameReply);

			demo = new DemoObject ();
			sysBus.NameOwnerChanged += demo.FireChange;
			bus.Register (myNameReq, myPath, demo);
		}

		Application.Run ();
	}
}

[Interface ("org.ndesk.gtest")]
public class DemoObject : MarshalByRefObject
{
	/*
	public DemoObject ()
	{
		NameOwnerChangedOnSystem += delegate {};
		DApplication.System.NameOwnerChanged += NameOwnerChangedOnSystem;
	}
	*/

	//is it possible to do this without needing the following method?
	public void FireChange (string name, string old_owner, string new_owner)
	{
		Console.WriteLine ("Asked to fire off signal NameOwnerChangedOnSystem");
		if (NameOwnerChangedOnSystem != null)
			NameOwnerChangedOnSystem (name, old_owner, new_owner);
	}

	public event NameOwnerChangedHandler NameOwnerChangedOnSystem;
}
