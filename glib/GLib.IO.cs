// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace NDesk.GLib
{
	public delegate bool IOFunc (IOChannel source, IOCondition condition, IntPtr data);

	//this is actually somewhat like Stream, but we don't use it that way
	[StructLayout (LayoutKind.Sequential)]
	public struct IOChannel
	{
		const string GLIB = "glib-2.0";

		public IntPtr Handle;

		[DllImport(GLIB)]
			static extern IntPtr g_io_channel_unix_new (int fd);

		[DllImport(GLIB)]
			//static extern int g_io_channel_unix_get_fd (IntPtr channel);
			static extern int g_io_channel_unix_get_fd (IOChannel channel);

		public IOChannel (int fd)
		{
			Handle = g_io_channel_unix_new (fd);
		}

		public int UnixFd
		{
			get {
				//return g_io_channel_unix_get_fd (Handle);
				return g_io_channel_unix_get_fd (this);
			}
		}

		[DllImport(GLIB)]
			static extern IOFlags g_io_channel_get_flags (IOChannel channel);

		[DllImport(GLIB)]
			static extern short g_io_channel_set_flags (IOChannel channel, IOFlags flags, IntPtr error);

		public IOFlags Flags
		{
			get {
				return g_io_channel_get_flags (this);
			} set {
				//TODO: fix return and error
				g_io_channel_set_flags (this, value, IntPtr.Zero);
			}
		}
	}

	public class IO
	{
		const string GLIB = "glib-2.0";

		[DllImport(GLIB)]
			protected static extern uint g_io_add_watch (IntPtr channel, IOCondition condition, IOFunc func, IntPtr user_data);

		[DllImport(GLIB)]
			protected static extern uint g_io_add_watch (IOChannel channel, IOCondition condition, IOFunc func, IntPtr user_data);

		[DllImport(GLIB)]
			protected static extern uint g_io_add_watch_full (IOChannel channel, int priority, IOCondition condition, IOFunc func, IntPtr user_data, IntPtr notify);


		public static ArrayList objs = new ArrayList ();

		public static void AddWatch (int fd, IOFunc func)
		{
			objs.Add (func);

			IOChannel channel = new IOChannel (fd);
			uint watch = g_io_add_watch (channel, IOCondition.In, func, IntPtr.Zero);
		}

	}
	//From Mono.Unix and poll(2)
	[Flags]
	enum PollEvents : short {
		POLLIN      = 0x0001, // There is data to read
		POLLPRI     = 0x0002, // There is urgent data to read
		POLLOUT     = 0x0004, // Writing now will not block
		POLLERR     = 0x0008, // Error condition
		POLLHUP     = 0x0010, // Hung up
		POLLNVAL    = 0x0020, // Invalid request; fd not open
		// XPG4.2 definitions (via _XOPEN_SOURCE)
		POLLRDNORM  = 0x0040, // Normal data may be read
		POLLRDBAND  = 0x0080, // Priority data may be read
		POLLWRNORM  = 0x0100, // Writing now will not block
		POLLWRBAND  = 0x0200, // Priority data may be written
	}

	public enum IOCondition : short
	{
		In = PollEvents.POLLIN,
		Out = PollEvents.POLLOUT,
		Pri = PollEvents.POLLPRI,
		Err = PollEvents.POLLERR,
		Hup = PollEvents.POLLHUP,
		Nval = PollEvents.POLLNVAL,
	}

	[Flags]
	public enum IOFlags : short
	{
		Append = 1 << 0,
		Nonblock = 1 << 1,
		//Read only flag
		IsReadable = 1 << 2,
		//Read only flag
		isWriteable = 1 << 3,
		//Read only flag
		IsSeekable = 1 << 4,
		//?
		Mask = (1 << 5) - 1,
		GetMask = Mask,
		SetMask = Append | Nonblock,
	}
}
