// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
using GLib;
using Gtk;
using NDesk.DBus;
using org.freedesktop.DBus;

//NOTE: this was made to work by making GLib.Object inherit from System.MarshalByRefObject
//this is easily done with monodis and ilasm
//it also needs signal parameter marshaling to be disabled as EventArgs parameters confuse the marshaler right now
public class TestGLib
{
	public static void OnClick (object o, EventArgs args)
	{
		Console.WriteLine (rbtn.Label);
		rbtn.Label += ".";
	}

	static Bus bus;

	static Button btn = null;
	static Button rbtn = null;

	public static void Main ()
	{
		DApplication.Init ();

		Application.Init ();

		btn = new Button ("Click me");
		btn.Clicked += OnClick;

		VBox vb = new VBox (false, 2);
		vb.PackStart (btn, false, true, 0);

		Window win = new Window ("DBus#");
		win.SetDefaultSize (640, 480);
		win.Add (vb);
		win.Destroyed += delegate {Application.Quit ();};
		win.ShowAll ();


		ObjectPath opath = new ObjectPath ("/org/freedesktop/DBus");
		string name = "org.freedesktop.DBus";

		bus = DApplication.Connection.GetObject<Bus> (name, opath);

		bus.NameAcquired += delegate (string acquired_name) {
			Console.Error.WriteLine ("NameAcquired: " + acquired_name);
		};

		string myName = bus.Hello ();
		Console.Error.WriteLine ("myName: " + myName);

		ObjectPath myOpath = new ObjectPath ("/org/ndesk/test");
		string myNameReq = "org.ndesk.gtest";

		if (bus.NameHasOwner (myNameReq)) {
			rbtn = DApplication.Connection.GetObject<Button> (myNameReq, new ObjectPath ("/org/ndesk/btn"));
		} else {
			NameReply nameReply = bus.RequestName (myNameReq, NameFlag.None);

			Console.WriteLine ("nameReply: " + nameReply);

			Connection.tmpConn = DApplication.Connection;
			DApplication.Connection.Marshal (btn, myNameReq, new ObjectPath ("/org/ndesk/btn"));
			rbtn = btn;
		}

		Application.Run ();
	}
}
