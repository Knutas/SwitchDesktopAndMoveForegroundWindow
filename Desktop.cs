using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

﻿namespace SwitchDesktopAndMoveForegroundWindow;

/// <summary>
/// Provides access to Windows Virtual Desktop management functionality.
/// </summary>
public static partial class Desktop
{
    private const int LeftDirection = 3;
    private const int RightDirection = 4;

    private static readonly IVirtualDesktopManagerInternal VirtualDesktopManagerInternal;

    [LibraryImport("ole32.dll")]
    private static partial int CoCreateInstance(
        ref Guid rclsid,
        nint pUnkOuter,
        uint dwClsContext,
        ref Guid riid,
        out nint ppv);

    static Desktop()
    {
        const uint CLSCTX_LOCAL_SERVER = 0x4;
        var CLSID_ImmersiveShell = new Guid("C2F03A33-21F5-47FA-B4BB-156362A2F239");
        var IID_IServiceProvider = typeof(IServiceProvider).GUID;
        var CLSID_VirtualDesktopManagerInternal = new Guid("C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B");
        var CLSID_IVirtualDesktopManagerInternal = typeof(IVirtualDesktopManagerInternal).GUID;

        Marshal.ThrowExceptionForHR(CoCreateInstance(ref CLSID_ImmersiveShell, 0, CLSCTX_LOCAL_SERVER, ref IID_IServiceProvider, out var serviceProviderPointer));

        try
        {
            IServiceProvider serviceProvider;
            unsafe
            {
                serviceProvider = ComInterfaceMarshaller<IServiceProvider>.ConvertToManaged((void*)serviceProviderPointer)!;
            }
            VirtualDesktopManagerInternal = serviceProvider.QueryService(ref CLSID_VirtualDesktopManagerInternal, ref CLSID_IVirtualDesktopManagerInternal);
        }
        finally
        {
            if (serviceProviderPointer != 0)
            {
                Marshal.Release(serviceProviderPointer);
            }
        }
    }

    /// <summary>
    /// Moves the currently active window to an adjacent virtual desktop and switches to it.
    /// </summary>
    /// <param name="direction">The direction to move (Left or Right).</param>
    /// <remarks>
    /// Silently ignores the operation if there is no desktop in the specified direction
    /// (e.g., already at the leftmost/rightmost desktop).
    /// </remarks>
    public static void MoveActiveWindow(Direction direction)
    {
        var current = VirtualDesktopManagerInternal.GetCurrentDesktop();
        var dir = direction == Direction.Left ? LeftDirection : RightDirection;

        try
        {
            var targetDesktop = VirtualDesktopManagerInternal.GetAdjacentDesktop(current, dir);
            VirtualDesktopManagerInternal.SwitchDesktopAndMoveForegroundView(targetDesktop);
        }
        catch (COMException)
        {
            // Probably invalid target desktop (e.g. no desktop to the left/right)
        }
    }

    public enum Direction
    {
        Left,
        Right
    }
}
