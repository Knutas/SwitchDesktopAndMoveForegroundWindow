using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SwitchDesktopAndMoveForegroundWindow;

[GeneratedComInterface]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("3F07F4BE-B107-441A-AF0F-39D82529072C")]
internal partial interface IVirtualDesktop;

/// <summary>
/// Defines methods for managing and interacting with virtual desktops on Windows.
/// </summary>
/// <remarks>This specific GUID is for Windows 11 22H2 and up.</remarks>
[GeneratedComInterface]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("53F5CA0B-158F-4124-900C-057158060B27")]
internal partial interface IVirtualDesktopManagerInternal
{
    void Unused1();
    void Unused2();
    void Unused3();
    IVirtualDesktop GetCurrentDesktop();
    void Unused4();
    IVirtualDesktop GetAdjacentDesktop(IVirtualDesktop from, int direction);
    void Unused5();
    void SwitchDesktopAndMoveForegroundView(IVirtualDesktop desktop);
}

[GeneratedComInterface]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
internal partial interface IServiceProvider
{
    IVirtualDesktopManagerInternal QueryService(ref Guid service, ref Guid riid);
}
