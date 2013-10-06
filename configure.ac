m4_define([api_version], [2.0])
m4_define([lib_version], [0.6.0])

AC_PREREQ(2.13)

AC_INIT([dbus-sharp-glib], lib_version)
AC_CONFIG_SRCDIR([src/GLib.cs])
AM_INIT_AUTOMAKE

AM_MAINTAINER_MODE

AC_SUBST([API_VERSION], [api_version])
AC_SUBST([VERSION], [lib_version])

AC_PROG_INSTALL

AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
	AC_MSG_ERROR([You need to install pkg-config])
fi
AC_SUBST(PKG_CONFIG)

MONO_REQ_VERSION=1.1.13
PKG_CHECK_MODULES(MONO, mono >= $MONO_REQ_VERSION)

AC_PATH_PROG(GMCS, mcs, no)
if test "x$GMCS" = "xno"; then
	AC_MSG_ERROR([You need to install mcs])
fi
AC_SUBST(GMCS)

AC_PATH_PROG(GACUTIL, gacutil, no)
if test "x$GACUTIL" = "xno"; then
	AC_MSG_ERROR([You need to install gacutil])
fi
AC_SUBST(GACUTIL)

DBUS_SHARP_REQUIRED_VERSION=0.7
PKG_CHECK_MODULES(DBUS_SHARP, dbus-sharp-2.0 >= $DBUS_SHARP_REQUIRED_VERSION)
AC_SUBST(DBUS_SHARP_LIBS)

#GLIB_REQUIRED_VERSION=2.0
#PKG_CHECK_MODULES(GLIB, gobject-2.0 >= $GTK_REQUIRED_VERSION)
#AC_SUBST(GLIB_CFLAGS)
#AC_SUBST(GLIB_LIBS)

AC_OUTPUT([
Makefile
dbus-sharp-glib-2.0.pc
src/AssemblyInfo.cs
src/Makefile
examples/Makefile
])

