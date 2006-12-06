// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
//using ST = System.Threading;
using System.Threading;
using Gtk;
using NDesk.DBus;
using org.freedesktop.DBus;

public class TestThreads
{
	public static void OnClick (object o, EventArgs args)
	{
		foreach (string n in bus.ListNames ()) {
			TextIter endIter = tv.Buffer.EndIter;
			tv.Buffer.Insert (ref endIter, n + '\n');
		}
	}

	public static void Task ()
	{
		Console.WriteLine (bus.ListNames ());
	}

	public static void OnClickQuit (object o, EventArgs args)
	{
		//Application.Quit ();
		Thread t = new Thread (Task);
		t.Start ();
	}

	static TextView tv;

	static IBus bus;

	public static void Main ()
	{
		BusG.Init ();
		Application.Init ();

		tv = new TextView ();
		ScrolledWindow sw = new ScrolledWindow ();
		sw.Add (tv);

		Button btn = new Button ("Click me");
		btn.Clicked += OnClick;

		Button btnq = new Button ("Click me (thread)");
		btnq.Clicked += OnClickQuit;

		VBox vb = new VBox (false, 2);
		vb.PackStart (sw, true, true, 0);
		vb.PackStart (btn, false, true, 0);
		vb.PackStart (btnq, false, true, 0);

		Window win = new Window ("D-Bus#");
		win.SetDefaultSize (640, 480);
		win.Add (vb);
		win.Destroyed += delegate {Application.Quit ();};
		win.ShowAll ();

		bus = Bus.Session.GetObject<IBus> ("org.freedesktop.DBus", new ObjectPath ("/org/freedesktop/DBus"));

		Application.Run ();
	}
}
