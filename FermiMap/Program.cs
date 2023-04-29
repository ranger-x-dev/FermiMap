using System;

namespace FermiMap
{
    class Program
    {
        static void Main(string[] args)
        {
            //init vars
            string widthInput = String.Empty;
            string heightInput = String.Empty;

            int width = 0;
            int height = 0;

            //make new random number generator
            Random rnd = new Random();

            //prompt choice
            string choice = "";

            //app loop
            while (choice != "x" && choice != "X")
            {
                // Set the Foreground color to blue
                Console.ForegroundColor = ConsoleColor.White;

                //Get width of room
                Console.WriteLine("Enter the width of the room: ");
                widthInput = Console.ReadLine();

                try
                {
                    width = Int32.Parse(widthInput);
                    Console.WriteLine("width: " + width);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse '{widthInput}'");
                }
                // Output: Unable to parse ''



                //Get height of room
                Console.WriteLine("Enter the height of the room: ");
                heightInput = Console.ReadLine();

                try
                {
                    height = Int32.Parse(heightInput);
                    Console.WriteLine("width: " + height);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse '{heightInput}'");
                }
                // Output: Unable to parse ''

                Console.WriteLine("Room (w x h): " + width + " x " + height);
                Console.WriteLine("Tile size: 16 px");
                Console.WriteLine("Room resolution: " + 16 * width + " x " + 16 * height);

                Console.WriteLine("Generating array...");

                int[,] tilemap = new int[width, height];

                //randomly assign terrain
                for (int i = 0; i < width; i++)
                {
                    //randomly assign terrain
                    for (int j = 0; j < height; j++)
                    {
                        //Console.WriteLine(i + ", " + j);
                        //tilemap[i, j] = i * j;
                        tilemap[i, j] = rnd.Next(0,5);
                    }
                }

                //add ocean to borders TOP
                int oceanBorder = rnd.Next(12, 18);

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < oceanBorder; j++)
                    {
                        oceanBorder = rnd.Next(12, 18);
                        tilemap[i, j] = 0;
                    }
                }

                //add ocean to borders BOTTOM
   
                for (int i = 0; i < width; i++)
                {
                    for (int j = height-oceanBorder; j < height; j++)
                    {
                        oceanBorder = rnd.Next(12, 18);
                        tilemap[i, j] = 0;
                    }
                }

                //add ocean to borders left
                oceanBorder = rnd.Next(12, 18);
                for (int i = 0; i < oceanBorder; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        oceanBorder = rnd.Next(12, 18);
                        tilemap[i, j] = 0;
                    }
                }

                //add ocean to borders left
                oceanBorder = rnd.Next(12, 18);
                for (int i = width-oceanBorder; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        oceanBorder = rnd.Next(12, 18);
                        tilemap[i, j] = 0;
                    }
                }

                Console.WriteLine("Tilemap generated.\n");
                Console.WriteLine("Would you like to see the tilemap? (Y/N? or X to exit)\n");
                choice = Console.ReadLine();

                if(choice == "y" || choice == "Y")
                {
                    /*
                    //Display all values for tilemap
                    for (int i = 0; i < height; i++)
                    {
                        Console.WriteLine("");
                        for (int j = 0; j < width; j++)
                        {
                            Console.Write(tilemap[j,i]+", ");
                        }
                    }
                    Console.WriteLine("");
                    */

                    //show an ASCII tilemap
                    Console.WriteLine("Here's an ASCII render...\n");

                    //generate an ascii map
                    for (int i = 0; i < height; i++)
                    {
                        Console.WriteLine("");
                        for (int j = 0; j < width; j++)
                        {
                            switch(tilemap[j,i])
                            {
                                case 0: 
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    break;
                                case 1:
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    break;
                                case 2:
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    break;
                                case 3:
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    break;
                                case 4:
                                    Console.ForegroundColor = ConsoleColor.White;
                                    break;
                            }
                            Console.Write("█");
                        }
                    }
                    Console.WriteLine("\n");
                }
            }
        }
    }
}
