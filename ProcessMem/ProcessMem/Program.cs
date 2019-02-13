using System;

namespace ProcessMem
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu mymenu = new Menu();

            //Continue doing the Menu until Exit is selected
            int choice = mymenu.display();
            while(choice != 6)
            {
                MemoryManip myMem = new MemoryManip();
                switch(choice)
                {

                    case 1:
                        myMem.displayProcess();
                        break;

                    case 2:

                        break;

                    case 3:

                        break;

                    case 4:

                        break;

                    case 5:

                        break;

                    default:
                        Console.WriteLine("Option must be between 1 and 6");
                        break;
                }

                //After everything else, redisplay the menu
                choice = mymenu.display();
            }
        }
    }
}