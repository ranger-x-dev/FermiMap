using System;

namespace FermiMap
{
    class Perlin
    {
        /*
        //Function to linearly interpolate between a0 and a1
        // Weight w should be in the range [0.0, 1.0]
        float interpolate(float a0, float a1, float w)
        {
            // // You may want clamping by inserting:
            // if (0.0 > w) return a0;
            // if (1.0 < w) return a1;
            //
            return (a1 - a0) * w + a0;
            // // Use this cubic interpolation [[Smoothstep]] instead, for a smooth appearance:
            // return (a1 - a0) * (3.0 - w * 2.0) * w * w + a0;
            //
            // // Use [[Smootherstep]] for an even smoother result with a second derivative equal to zero on boundaries:
            // return (a1 - a0) * ((w * (w * 6.0 - 15.0) + 10.0) * w * w * w) + a0;
            //
        }

        struct vector2 {
            float x, y;
        }

        vector2 randomGradient(int ix, int iy)
        {
            // No precomputed gradients mean this works for any number of grid coordinates
            const int w = 8 * sizeof(int);
            const int s = w / 2; // rotation width
            int a = ix, b = iy;
            a *= 3284157443; b ^= a << s | a >> w - s;
            b *= 1911520717; a ^= b << s | b >> w - s;
            a *= 2048419325;
            double random = a * (3.14159265 / ~(~0u >> 1)); // in [0, 2*Pi]
            vector2 v;
            v.x = Math.Cos(random); v.y = Math.Sin(random);
            return v;
        }
        */

        //Perlin Noise implementation
        int[] p = { 151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36,
                      103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0,
                      26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56,
                      87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
                      77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55,
                      46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132,
                      187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109,
                      198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126,
                      255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183,
                      170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43,
                      172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112,
                      104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162,
                      241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106,
                      157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205,
                      93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180 };

        double fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        double lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }

        double grad(int hash, double x, double y, double z)
        {
            int h = hash & 15;
            double u = h < 8 ? x : y,
                   v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        /// x0, y0 and z0 can be any real numbers, but the result is
        /// zero if they are all integers.
        /// The result is probably in [-1.0, 1.0].
        public double noise(double x, double y, double z)
        {
            int X = (int)Math.Floor(x) & 255,
                Y = (int)Math.Floor(y) & 255,
                Z = (int)Math.Floor(z) & 255;
            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);
            double u = fade(x),
                   v = fade(y),
                   w = fade(z);
            int A = p[X] + Y, AA = p[A] + Z, AB = p[A + 1] + Z,
                B = p[X + 1] + Y, BA = p[B] + Z, BB = p[B + 1] + Z;

            return lerp(w, lerp(v, lerp(u, grad(p[AA], x, y, z),
                                           grad(p[BA], x - 1, y, z)),
                                   lerp(u, grad(p[AB], x, y - 1, z),
                                           grad(p[BB], x - 1, y - 1, z))),
                           lerp(v, lerp(u, grad(p[AA + 1], x, y, z - 1),
                                           grad(p[BA + 1], x - 1, y, z - 1)),
                                   lerp(u, grad(p[AB + 1], x, y - 1, z - 1),
                                           grad(p[BB + 1], x - 1, y - 1, z - 1))));
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //init vars
            string widthInput;
            string heightInput;

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

                //Test Perlin Noise
                Perlin p = new Perlin();

                /*
                for (int i = 0; i < 1000; i++)
                {
                    Console.WriteLine("Perlin noise: " + p.noise(3.14, i*0.01, 2));
                }
                Console.WriteLine("Perlin test complete.\n");
                */

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

                double[,] tilemap = new double[width, height];

                /*
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
                */

                //randomly assign terrain
                for (int i = 0; i < width; i++)
                {
                    //randomly assign terrain
                    for (int j = 0; j < height; j++)
                    {
                        //Console.WriteLine(i + ", " + j);
                        //tilemap[i, j] = i * j;
                        tilemap[i, j] = p.noise(3.14*i/width, 1.45*j/height, 0.1);
                    }
                }

                ///OCEAN BORDER
                /*
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
                */

                Console.WriteLine("Tilemap generated.\n");
                Console.WriteLine("Would you like to see the tilemap? (Y/N? or X to exit)\n");
                choice = Console.ReadLine();

                if(choice == "y" || choice == "Y")
                {
                    
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
                   

                    
                    //show an ASCII tilemap
                    Console.WriteLine("Here's an ASCII render...\n");

                    //generate an ascii map
                    for (int i = 0; i < height; i++)
                    {
                        Console.WriteLine("");
                        for (int j = 0; j < width; j++)
                        {
                            /*
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
                            */
                            double tileValue = 3*tilemap[j, i]+0.5;
                            if (tileValue < 0.2)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            } 
                            else if (tileValue < 0.4)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }
                            else if (tileValue < 0.6)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else if (tileValue < 0.8)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            }
                            else if (tileValue < 1.0)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
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
