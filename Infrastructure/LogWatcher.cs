using System.Diagnostics;

namespace Infrastructure;

public static class LogWatcher
{
    private static Process? _notepadProcess;

    public static Task WatchLogs(string filePath)
    {
        _notepadProcess = Process.Start("notepad.exe", filePath);

        // Start a new thread to run the timer
        var timerTask = Task.Run(async () =>
        {
            // Start a timer to check the file's last modified time every 5 seconds
            var timer = new Timer(CheckFileModified!, new object[] { _notepadProcess, filePath }, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            // Wait for the timer to be disposed
            var timerDisposed = new ManualResetEvent(false);
            await Task.Run(() => timerDisposed.WaitOne());

            // Close Notepad
            _notepadProcess.CloseMainWindow();
        });

        // Stop the timer and dispose of the event that signals the timer thread to exit
        // timerTask.Dispose();
        _notepadProcess.CloseMainWindow();
        return Task.CompletedTask;
    }

    private static void CheckFileModified(object state)
    {
        var args = (object[])state;
        var filePath = (string)args[1];

        var lastModifiedTime = File.GetLastWriteTime(filePath);

        if (lastModifiedTime != _notepadProcess?.StartTime)
        {
            // The file has been modified since Notepad was opened, so close and re-open the file
            _notepadProcess?.CloseMainWindow();
            _notepadProcess = Process.Start("notepad.exe", filePath);
        }
    }
}