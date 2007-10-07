DBUS_SHARP_PREFIX=../../dbus-sharp
DBUS_SHARP_GLIB_PREFIX=..

# See How Not to Use VPATH: http://make.paulandlesley.org/vpath.html
VPATH=$(DBUS_SHARP_PREFIX)/src $(DBUS_SHARP_GLIB_PREFIX)/src

#Note that this makefile isn't healthy, but it works for me
#Makefile bug: doesn't seem to work with make -B

all: run test.exe test-export.exe test-system-bus.exe

test.exe: PKGS = gtk-sharp-2.0

test.exe: NDesk.DBus.dll NDesk.DBus.GLib.dll Test.cs

test-export.exe: PKGS = gtk-sharp-2.0

test-export.exe: NDesk.DBus.dll NDesk.DBus.GLib.dll TestExport.cs

test-system-bus.exe: PKGS = gtk-sharp-2.0

test-system-bus.exe: NDesk.DBus.dll NDesk.DBus.GLib.dll TestSystemBus.cs

test-ui.exe: PKGS = gtk-sharp-2.0

test-ui.exe: NDesk.DBus.dll NDesk.DBus.GLib.dll TestUI.cs

test-threads.exe: PKGS = gtk-sharp-2.0

test-threads.exe: NDesk.DBus.dll NDesk.DBus.GLib.dll TestThreads.cs


run:
	echo '#!/bin/sh' > $@
	echo 'export MONO_PATH=$(DBUS_SHARP_GLIB_PREFIX)/src:$(DBUS_SHARP_PREFIX)/src:$$MONO_PATH' >> $@
	echo '$(RUNTIME) $$@' >> $@
	chmod +x $@


include $(DBUS_SHARP_PREFIX)/include.mk
