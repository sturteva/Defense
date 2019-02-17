using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;



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
            while (!int.TryParse(input, out getPID))
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

                    foreach (ProcessThread thread in threads)
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

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        public enum AllocationProtectEnum : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        public enum StateEnum : uint
        {
            MEM_COMMIT = 0x1000,
            MEM_FREE = 0x10000,
            MEM_RESERVE = 0x2000
        }

        public enum TypeEnum : uint
        {
            MEM_IMAGE = 0x1000000,
            MEM_MAPPED = 0x40000,
            MEM_PRIVATE = 0x20000
        }

        // REQUIRED STRUCTS
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

       /* public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }*/

        public void pages()
        {
            // saving the values as long ints so I won't have to do a lot of casts later
            //long MaxAddress = 0x7fffffff;
            //long address = 0;

            string input = "";
            Console.WriteLine("Please enter the PID for the Process: ");
            input = Console.ReadLine();
            int getPID = 0;
            while (!int.TryParse(input, out getPID))
            {
                Console.WriteLine("Please ensure PID is an integer:");
                input = Console.ReadLine();
            }

            Process process = null;
            foreach (Process theprocess in processlist)
            {
                if (theprocess.Id == getPID)
                {
                    process = theprocess;
                }
            }
            if (process == null)
                return;


            string allMods = "";
            Console.WriteLine("====LIST OF EXECUTEABLE PROTECTIONS====");
            Console.WriteLine("PAGE_EXECUTE = 0x10");
            Console.WriteLine("PAGE_EXECUTE_READ = 0x20");
            Console.WriteLine("PAGE_EXECUTE_READWRITE = 0x40");
            Console.WriteLine("PAGE_EXECUTE_WRITECOPY = 0x80");
            Console.WriteLine("==================================");
            Console.WriteLine("Executable Name | BaseAddress | EntryPointAddress | Executable Protection");


            ProcessModuleCollection moduleCollection = process.Modules;
            foreach (ProcessModule module in moduleCollection)
            {
                        MEMORY_BASIC_INFORMATION m;
                        int result = VirtualQueryEx(process.Handle, module.BaseAddress, out m, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
                
                        //Ensure it did not fail & that the AllocationProtection is an executable
                        if(result != 0 && (m.AllocationProtect == 0x00000010 || m.AllocationProtect == 0x00000020 || m.AllocationProtect == 0x00000040 || m.AllocationProtect == 0x00000080)) 
                        {
                            string baseAddress = "0x"+ module.BaseAddress.ToString("X");
                            string entryAddress = "0x"+ module.EntryPointAddress.ToString("X");
                            string allocation = "0x" + m.AllocationProtect.ToString("X");
                            allMods += $"{module.ModuleName}  | {baseAddress}  |  {entryAddress} | {allocation} \r\n";
                        }
            }

            Console.WriteLine(allMods);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
                IntPtr hProcess,
                IntPtr lpBaseAddress,
                 byte[] lpBuffer,
                Int32 nSize,
                out IntPtr lpNumberOfBytesRead);

        public void memoryScan()
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

            Process process = null;
            foreach (Process theprocess in processlist)
            {
                if (theprocess.Id == getPID)
                {
                    process = theprocess;
                }
            }
            if (process == null)
                return;


            UTF8Encoding utf8 = new UTF8Encoding();
            string lineMem = "";
            string ascii = "";
            ProcessModuleCollection moduleCollection = process.Modules;
            foreach (ProcessModule module in moduleCollection)
            {
                    
                
                IntPtr numRead = (IntPtr)0;
                byte[] buffer = new byte[0x1000];
                bool read = ReadProcessMemory(process.Handle, module.BaseAddress, buffer, 0x1000,out numRead);

                //Ensure it did not fail & that the AllocationProtection is an executable
                if (read)
                {
                    int counter = 0;
                    foreach (byte number in buffer)
                    {
                        if (counter < 7)
                        {
                            lineMem += number.ToString("X2") + " ";
                            string tempString = utf8.GetString(new[] { number });
                                               
                            ascii += tempString;
                            counter++;
                        }

                        else
                        {
                            lineMem += number.ToString("X2");
                            Console.WriteLine(lineMem + " | " + ascii);
                            
                            lineMem = "";
                            ascii = "";
                            counter = 0;
                        }
                    }
                }
            }

            
            
          

        }
    }
}

