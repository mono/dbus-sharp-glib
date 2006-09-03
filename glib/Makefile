DBUS_SHARP_PREFIX=../../dbus-sharp

# See How Not to Use VPATH: http://make.paulandlesley.org/vpath.html
VPATH=$(DBUS_SHARP_PREFIX)/src

#Note that this makefile isn't healthy, but it works for me
#Makefile bug: doesn't seem to work with make -B

#.PHONY: all

all: NDesk.DBus.GLib.dll test


NDesk.DBus.GLib.dll: PKGS = glib-sharp-2.0

NDesk.DBus.GLib.dll: NDesk.DBus.dll GIO.cs GLib.cs

test.exe: PKGS = gtk-sharp-2.0

test.exe: NDesk.DBus.dll NDesk.DBus.GLib.dll Test.cs

#%: %.exe

test: test.exe
	echo '#!/bin/sh' > $@
	echo 'MONO_PATH=$(DBUS_SHARP_PREFIX)/src:$$MONO_PATH' >> $@
	echo '$(RUNTIME) $^ $$@' >> $@
	chmod +x $@

include $(DBUS_SHARP_PREFIX)/include.mk
