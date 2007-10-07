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

		string bus_name = "org.ndesk.gtest";
		ObjectPath path = new ObjectPath ("/org/ndesk/test");

		if (bus.RequestName (bus_name) == RequestNameReply.PrimaryOwner) {
			//create a new instance of the object to be exported
			demo = new DemoObject ();
			bus.Register (path, demo);
		} else {
			//import a remote to a local proxy
			demo = bus.GetObject<DemoObject> (bus_name, path);
		}

		//run the main loop
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
