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
	//FIXME: this API needs review and de-unixification. It is horrid, but gets the job done.
	public static class DApplication
	{
		static bool SystemDispatch (IOChannel source, IOCondition condition, IntPtr data)
		{
			systemConnection.Iterate ();
			return true;
		}

		static bool SessionDispatch (IOChannel source, IOCondition condition, IntPtr data)
		{
			sessionConnection.Iterate ();
			return true;
		}

		static void HandleNameAcquired (string name)
		{
			//do nothing
		}

		static Connection systemConnection = null;
		public static Connection SystemConnection
		{
			get {
				if (systemConnection == null)
					systemConnection = Init (Address.SystemBus, SystemDispatch);

				return systemConnection;
			}
		}

		static Connection sessionConnection = null;
		public static Connection SessionConnection
		{
			get {
				if (sessionConnection == null)
					sessionConnection = Init (Address.SessionBus, SessionDispatch);

				return sessionConnection;
			}
		}

		//this will need to change later, but is needed for now
		static Bus systemBus = null;
		public static Bus SystemBus
		{
			get {
				if (systemBus == null) {
					systemBus = SystemConnection.GetObject<Bus>("org.freedesktop.DBus", new ObjectPath("/org/freedesktop/DBus"));
					systemBus.Hello ();
					systemBus.NameAcquired += HandleNameAcquired;
				}

				return systemBus;
			}
		}

		//this will need to change later, but is needed for now
		static Bus sessionBus = null;
		public static Bus SessionBus
		{
			get {
				if (sessionBus == null) {
					sessionBus = SessionConnection.GetObject<Bus>("org.freedesktop.DBus", new ObjectPath("/org/freedesktop/DBus"));
					sessionBus.Hello ();
					sessionBus.NameAcquired += HandleNameAcquired;
				}

				return sessionBus;
			}
		}

		//this is just temporary
		private static Connection Init (string address, IOFunc dispatchHandler)
		{
			Connection conn = Connection.Open (address);

			IOChannel channel = new IOChannel ((int)conn.SocketHandle);
			IO.AddWatch (channel, IOCondition.In, dispatchHandler);

			return conn;
		}
	}
}
