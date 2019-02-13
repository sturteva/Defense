using System;
namespace ProcessMem
{
    class Menu
    {
        public int display()
        {
            string choice = "";

            
            Console.WriteLine("--------Please Enter Option---------------");
            Console.WriteLine("\t1 - Enumerate all running processes");
            Console.WriteLine("\t2 - List all running threads within process boundary");
            Console.WriteLine("\t3 - Enumerate all the loaded modules within the processes");
            Console.WriteLine("\t4 - Show All the Executable Pages within the process");
            Console.WriteLine("\t5 - Read Memory of a Process");
            Console.WriteLine("\t6 - Exit");
            Console.Write("Your Choice? ");

            choice = Console.ReadLine();
            int cleanChoice = 0;

            //Ensuring that an Integer is inputted.  Did not do Range verification because menu will just repeat if not within range. 
            while(!int.TryParse(choice, out cleanChoice))
            {

                Console.Write("Please Select an Option 1 through 6: ");
                choice = Console.ReadLine();                
            }

            return cleanChoice;
        }
    }
}