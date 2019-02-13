using System;
using System.Diagnostics;


namespace ProcessMem
{
    class MemoryLook
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

        public void runningThreads()
        {
            string input = "";
            Console.WriteLine("Please enter the PID for the Process: ");
            input = Console.ReadLine();
            int getPID = 0;
            while(!int.TryParse(input, out getPID))
            {
                Console.WriteLine("Please ensure PID is an integer:");
                input = Console.ReadLine();
            }

            string allThreads = string.Empty;

            foreach (Process theprocess in processlist)
            {
                //https://social.msdn.microsoft.com/Forums/vstudio/en-US/905ecf98-57fb-4c7b-abb1-3b9489a6c98e/getting-list-of-running-threads?forum=csharpgeneral
                if (theprocess.Id == getPID)
                {
                    //Get threads
                    ProcessThreadCollection threads = theprocess.Threads;

                    foreach(ProcessThread thread in threads)
                    {
                        allThreads += string.Format("Thread Id: {0}, ThreadState: {1}\r\n", thread.Id, thread.ThreadState);
                    }

                    Console.WriteLine(allThreads);
                }
            }
            
        }

        public void allModules()
        {
            string input = "";
            Console.WriteLine("Please enter the PID for the Process: ");
            input = Console.ReadLine();
            int getPID = 0;
            while (!int.TryParse(input, out getPID))
            {
                Console.WriteLine("Please ensure PID is an integer:");
                input = Console.ReadLine();
            }

            string allMods = "";
            Console.WriteLine("Module Name | BaseAddress | EntryPointAddress");

            foreach (Process theprocess in processlist)
            {
                if (theprocess.Id == getPID)
                {
                    ProcessModuleCollection moduleCollection = theprocess.Modules;
                    foreach (ProcessModule module in moduleCollection)
                    {
                        string baseAddress = module.BaseAddress.ToString("X");
                        string entryAddress = module.EntryPointAddress.ToString("X");
                        allMods += $"{module.ModuleName}  | {baseAddress}  |  {entryAddress}\r\n";
                    }

                    Console.WriteLine(allMods);
                }
            }

        }

      

        
    }
}