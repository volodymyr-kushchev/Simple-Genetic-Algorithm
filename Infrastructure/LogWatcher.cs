
using System.Diagnostics;

namespace Infrastructure
{
    public class LogWatcher
    {
        private static Process notepadProcess;

        public static async Task WatchLogs(string filePath)
        {
            //Process.Start("notepad.exe", filePath);

            notepadProcess = Process.Start("notepad.exe", filePath);

            // Start a new thread to run the timer
            Task timerTask = Task.Run(async () =>
            {
                // Start a timer to check the file's last modified time every 5 seconds
                Timer timer = new Timer(CheckFileModified, new object[] { notepadProcess, filePath }, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

                // Wait for the timer to be disposed
                ManualResetEvent timerDisposed = new ManualResetEvent(false);
                await Task.Run(() => timerDisposed.WaitOne());

                // Close Notepad
                notepadProcess.CloseMainWindow();
            });

            // Stop the timer and dispose of the event that signals the timer thread to exit
            timerTask.Dispose();
            notepadProcess.CloseMainWindow();
        }

        private static void CheckFileModified(object state)
        {
            object[] args = (object[])state;
            //Process notepadProcess = (Process)args[0];
            string filePath = (string)args[1];

            DateTime lastModifiedTime = File.GetLastWriteTime(filePath);

            if (lastModifiedTime != notepadProcess.StartTime)
            {
                // The file has been modified since Notepad was opened, so close and re-open the file
                notepadProcess.CloseMainWindow();
                notepadProcess = Process.Start("notepad.exe", filePath);
            }
        }
    }
}
