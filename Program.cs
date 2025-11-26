using SwitchDesktopAndMoveForegroundWindow;
using TinyHotKey;

using var tinyHotKey = new TinyHotKeyInstance();

var modifiers = Modifier.Control | Modifier.Alt | Modifier.Windows;
RegisterDesktopHotKey(modifiers, Key.Left, Desktop.Direction.Left);
RegisterDesktopHotKey(modifiers, Key.Right, Desktop.Direction.Right);

// Keep the application running until the process is killed
var waitHandle = new ManualResetEvent(false);
waitHandle.WaitOne();

void RegisterDesktopHotKey(Modifier modifiers, Key key, Desktop.Direction direction)
{
    var registration = tinyHotKey.RegisterHotKey(modifiers, key, () =>
    {
        Desktop.MoveActiveWindow(direction);

        return Task.CompletedTask;
    });

    if (!registration.IsRegistered)
    {
        // Failed to register the hotkey, probably because it's already in use
        Environment.Exit(1);
    }
}
