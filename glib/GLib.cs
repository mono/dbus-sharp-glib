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
		static bool Dispatch (IOChannel source, IOCondition condition, IntPtr data)
		{
			//Console.Error.WriteLine ("Dispatch " + source.UnixFd + " " + condition);
			connection.Iterate ();
			//Console.Error.WriteLine ("Dispatch done");

			return true;
		}

		static Connection connection = null;
		public static Connection Connection
		{
			get {
				if (connection == null)
					Init ();

				return connection;
			}
		}

		[Obsolete]
		public static void Init ()
		{
			connection = new Connection ();

			IOChannel channel = new IOChannel ((int)connection.sock.Handle);
			IO.AddWatch (channel, IOCondition.In, Dispatch);
		}
	}
}
