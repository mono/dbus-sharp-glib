// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
//using GLib;
//using Gtk;
using NDesk.DBus;
using NDesk.GLib;
using org.freedesktop.DBus;

namespace NDesk.DBus
{
	//FIXME: this API needs review and de-unixification
	public static class DApplication
	{
		public static bool Dispatch (IOChannel source, IOCondition condition, IntPtr data)
		{
			//Console.Error.WriteLine ("Dispatch " + source.UnixFd + " " + condition);
			connection.Iterate ();
			//Console.Error.WriteLine ("Dispatch done");

			return true;
		}

		public static Connection Connection
		{
			get {
				return connection;
			}
		}

		static Connection connection;
		static Bus bus;

		public static void Init ()
		{
			connection = new Connection ();

			//ObjectPath opath = new ObjectPath ("/org/freedesktop/DBus");
			//string name = "org.freedesktop.DBus";

			/*
			bus = connection.GetObject<Bus> (name, opath);

			bus.NameAcquired += delegate (string acquired_name) {
				Console.Error.WriteLine ("NameAcquired: " + acquired_name);
			};

			string myName = bus.Hello ();
			Console.Error.WriteLine ("myName: " + myName);
			*/

			IO.AddWatch ((int)connection.sock.Handle, IOCondition.In, Dispatch);
		}
	}
}
