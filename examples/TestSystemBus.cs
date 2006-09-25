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
	static Bus sysBus;

	static DemoObject demo;

	public static void Main ()
	{
		Application.Init ();

		Window win = new Window ("D-Bus#");
		win.SetDefaultSize (640, 480);
		win.Destroyed += delegate {Application.Quit ();};
		win.ShowAll ();

		bus = DApplication.SessionBus;
		sysBus = DApplication.SystemBus;

		string myNameReq = "org.ndesk.gtest";
		ObjectPath myPath = new ObjectPath ("/org/ndesk/test");

		if (bus.NameHasOwner (myNameReq)) {
			demo = DApplication.SessionConnection.GetObject<DemoObject> (myNameReq, myPath);
		} else {
			NameReply nameReply = bus.RequestName (myNameReq, NameFlag.None);

			Console.WriteLine ("nameReply: " + nameReply);

			demo = new DemoObject ();
			sysBus.NameOwnerChanged += demo.FireChange;
			DApplication.SessionConnection.Marshal (demo, myNameReq, myPath);
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
		NameOwnerChangedOnSystemBus += delegate {};
		DApplication.SystemBus.NameOwnerChanged += NameOwnerChangedOnSystemBus;
	}
	*/

	//is it possible to do this without needing the following method?
	public void FireChange (string name, string old_owner, string new_owner)
	{
		Console.WriteLine ("Asked to fire off signal NameOwnerChangedOnSystemBus");
		if (NameOwnerChangedOnSystemBus != null)
			NameOwnerChangedOnSystemBus (name, old_owner, new_owner);
	}

	public event NameOwnerChangedHandler NameOwnerChangedOnSystemBus;
}
