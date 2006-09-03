DBUS_SHARP_PREFIX=../../dbus-sharp

# See How Not to Use VPATH: http://make.paulandlesley.org/vpath.html
VPATH=$(DBUS_SHARP_PREFIX)/src

#Note that this makefile isn't healthy, but it works for me

.PHONY: all

all: test-glib

test-glib.exe: PKGS = gtk-sharp-2.0

test-glib.exe: NDesk.DBus.dll GIO.cs TestGlib.cs

%: %.exe
	echo 'MONO_PATH=$(DBUS_SHARP_PREFIX)/src:$$MONO_PATH' > $@
	echo 'mono --debug $^' >> $@
	chmod +x $@

include $(DBUS_SHARP_PREFIX)/include.mk
