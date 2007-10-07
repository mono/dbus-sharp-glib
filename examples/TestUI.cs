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
		BusG.Init ();
		Application.Init ();

		btn = new Button ("Click me");
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
		ObjectPath path = new ObjectPath ("/org/ndesk/btn");

		if (bus.RequestName (bus_name) == RequestNameReply.PrimaryOwner) {
			bus.Register (path, btn);
			rbtn = btn;
		} else {
			rbtn = bus.GetObject<Button> (bus_name, path);
		}

		//run the main loop
		Application.Run ();
	}
}
