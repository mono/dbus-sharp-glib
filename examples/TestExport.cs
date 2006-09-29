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
	public static void OnClick (object o, EventArgs args)
	{
		demo.Say ("Button clicked");
	}

	static Bus bus;

	static DemoObject demo;

	public static void Main ()
	{
		BusG.Init ();
		Application.Init ();

		Button btn = new Button ("Click me");
		btn.Clicked += OnClick;

		VBox vb = new VBox (false, 2);
		vb.PackStart (btn, false, true, 0);

		Window win = new Window ("D-Bus#");
		win.SetDefaultSize (640, 480);
		win.Add (vb);
		win.Destroyed += delegate {Application.Quit ();};
		win.ShowAll ();


		bus = Bus.Session;

		string myNameReq = "org.ndesk.gtest";
		ObjectPath myPath = new ObjectPath ("/org/ndesk/test");

		if (bus.NameHasOwner (myNameReq)) {
			demo = bus.GetObject<DemoObject> (myNameReq, myPath);
		} else {
			NameReply nameReply = bus.RequestName (myNameReq);

			Console.WriteLine ("nameReply: " + nameReply);

			demo = new DemoObject ();
			bus.Register (myNameReq, myPath, demo);
		}

		Application.Run ();
	}
}

[Interface ("org.ndesk.gtest")]
public class DemoObject : MarshalByRefObject
{
	public void Say (string text)
	{
		Console.WriteLine (text);
	}
}
