// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
using GLib;
using Gtk;
using NDesk.DBus;
using org.freedesktop.DBus;

public class TestGlib
{
	public static bool Dispatch (IOChannel source, IOCondition condition, IntPtr data)
	{
		Console.WriteLine ("Dispatch " + source.UnixFd + " " + condition);
		conn.Iterate ();
		Console.WriteLine ("Dispatch done");

		return true;
	}

	public static void OnClick (object o, EventArgs args)
	{
		Console.WriteLine ("click");

		Console.WriteLine ();
		foreach (string n in bus.ListNames ())
			//Console.WriteLine (n);
			tv.Buffer.Text += n+'\n';
	}

	static TextView tv;

	static Connection conn;
	static Bus bus;

	public static void Main ()
	{
		Application.Init ();

		conn = new Connection ();

		tv = new TextView ();
		ScrolledWindow sw = new ScrolledWindow ();
		sw.Add (tv);

		Button btn = new Button ("Click me");
		btn.Clicked += OnClick;

		VBox vb = new VBox (false, 2);
		vb.PackStart (sw, true, true, 0);
		vb.PackStart (btn, false, true, 0);

		Window win = new Window ("DBus#");
		win.SetDefaultSize (640, 480);
		win.Add (vb);
		win.Destroyed += delegate {Application.Quit ();};
		win.ShowAll ();


		ObjectPath opath = new ObjectPath ("/org/freedesktop/DBus");
		string name = "org.freedesktop.DBus";

		bus = conn.GetObject<Bus> (name, opath);

		bus.NameAcquired += delegate (string acquired_name) {
			Console.WriteLine ("NameAcquired: " + acquired_name);
		};

		string myName = bus.Hello ();
		Console.WriteLine ("myName: " + myName);

		IO.AddWatch ((int)conn.sock.Handle, Dispatch);

		Application.Run ();
	}
}
