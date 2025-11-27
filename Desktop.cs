namespace SwitchDesktopAndMoveForegroundWindow;

/// <summary>
/// Provides access to Windows Virtual Desktop management functionality.
/// </summary>
public static class Desktop
{
    private const int LeftDirection = 3;
    private const int RightDirection = 4;

    private static readonly IVirtualDesktopManagerInternal VirtualDesktopManagerInternal;

    static Desktop()
    {
        var CLSID_ImmersiveShell = new Guid("C2F03A33-21F5-47FA-B4BB-156362A2F239");
        var CLSID_VirtualDesktopManagerInternal = new Guid("C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B");
        var CLSID_IVirtualDesktopManagerInternal = typeof(IVirtualDesktopManagerInternal).GUID;

        var shell = (IServiceProvider)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_ImmersiveShell)!)!;
        VirtualDesktopManagerInternal = shell.QueryService(ref CLSID_VirtualDesktopManagerInternal, ref CLSID_IVirtualDesktopManagerInternal);
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
        var targetDesktop = VirtualDesktopManagerInternal.GetAdjacentDesktop(current, dir);

        // If desktop is null it is probable an invalid target (e.g. no desktop to the left/right)
        if (targetDesktop != null)
        {
            VirtualDesktopManagerInternal.SwitchDesktopAndMoveForegroundView(targetDesktop);
        }
    }

    public enum Direction
    {
        Left,
        Right
    }
}
