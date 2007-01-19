DBUS_SHARP_PREFIX=../../dbus-sharp

# See How Not to Use VPATH: http://make.paulandlesley.org/vpath.html
VPATH=$(DBUS_SHARP_PREFIX)/src

#Note that this makefile isn't healthy, but it works for me
#Makefile bug: doesn't seem to work with make -B

#.PHONY: all

all: NDesk.DBus.GLib.dll

NDesk.DBus.GLib.dll: CSFLAGS=-keyfile:../ndesk.snk

NDesk.DBus.GLib.dll: NDesk.DBus.dll AssemblyInfo.cs GLib.IO.cs GLib.cs ../ndesk.snk

.PHONY:
install: NDesk.DBus.GLib.dll
	$(GACUTIL) $(GACUTIL_FLAGS) -i NDesk.DBus.GLib.dll -f -package ndesk-dbus-glib-1.0

include $(DBUS_SHARP_PREFIX)/include.mk
