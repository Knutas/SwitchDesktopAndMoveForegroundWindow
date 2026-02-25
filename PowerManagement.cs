using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SwitchDesktopAndMoveForegroundWindow;

public static partial class PowerManagement
{
    private const uint PROCESS_POWER_THROTTLING_CURRENT_VERSION = 1;
    private const uint PROCESS_POWER_THROTTLING_EXECUTION_SPEED = 0x1;
    private const int ProcessPowerThrottling = 4;

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetProcessInformation(
        nint hProcess,
        int processInformationClass,
        ref PROCESS_POWER_THROTTLING_STATE processInformation,
        uint processInformationSize);

    /// <summary>
    /// Enables Windows Efficiency Mode for the current process.
    /// This lowers the process priority and enables power throttling (EcoQoS).
    /// </summary>
    public static void EnableEfficiencyMode()
    {
        var currentProcess = Process.GetCurrentProcess();

        currentProcess.PriorityClass = ProcessPriorityClass.Idle;

        var state = new PROCESS_POWER_THROTTLING_STATE
        {
            Version = PROCESS_POWER_THROTTLING_CURRENT_VERSION,
            ControlMask = PROCESS_POWER_THROTTLING_EXECUTION_SPEED,
            StateMask = PROCESS_POWER_THROTTLING_EXECUTION_SPEED
        };

        SetProcessInformation(
            currentProcess.Handle,
            ProcessPowerThrottling,
            ref state,
            (uint)Marshal.SizeOf<PROCESS_POWER_THROTTLING_STATE>());
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct PROCESS_POWER_THROTTLING_STATE
    {
        public uint Version;
        public uint ControlMask;
        public uint StateMask;
    }
}
