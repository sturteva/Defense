using System;
using System.Diagnostics;
namespace ProcessMem
{
    class MemoryManip
    {
        //https://stackoverflow.com/questions/648410/how-can-i-list-all-processes-running-in-windows
        private Process[] processlist = Process.GetProcesses();

        public void displayProcess()
        {
            foreach (Process theprocess in processlist)
            {
                Console.WriteLine("Process: {0} PID: {1}", theprocess.ProcessName, theprocess.Id);
            }
        }

       
    }
}