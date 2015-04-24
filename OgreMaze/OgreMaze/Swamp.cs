using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreMaze
{

    struct ExampleMaps
    {

        public static char[][] Example1, Example2;

        public static string[] Example1Pre = {"@@........", 
                                       "@@O.......",
                                       ".....O.O..",
                                       "..........",
                                       "..O.O.....",
                                       "..O....O.O",
                                       ".O........",
                                       "..........",
                                       ".....OO...",
                                       ".........$" };
        public static string[] Example2Pre = { "@@........",
                                       "@@O.......", 
                                       ".....O.O.",
                                       "..........",
                                       "..O.O.....",
                                       "..O....O.O",
                                       ".O........",
                                       "..........",
                                       ".....OO.O.",
                                       ".........$" };

        static ExampleMaps()
        {
            Convert(Example1Pre, out Example1);
            Convert(Example2Pre, out Example2);
        }

        private static void Convert(string[] src, out char[][] dst)
        {
            dst = new char[src.Length][];

            uint i = 0;
            foreach (string s in src)
            {
                //Debug.WriteLine("Example1[" + i + "++] = \"" + s + "\".ToCharArray();");
                dst[i++] = s.ToCharArray();
                //Debug.WriteLine("Sucess");
            }
        }
    }

    internal enum Terrain
    {
        Invalid,
        Empty,
        Sinkhole,
        Gold
    }

    internal class Space
    {
        private int XCoord, YCoord;
        private Swamp Swamp;
        private Terrain Terrain;

        public Space(int xCoord, int yCoord, Swamp swamp, Terrain terrain)
        {
            XCoord = xCoord;
            YCoord = yCoord;
            Swamp = swamp;
            Terrain = terrain;
        }

        public int X
        {
            get
            {
                return XCoord;
            }
        }

        public int Y
        {
            get
            {
                return YCoord;
            }
        }

        public Space North
        {
            get
            {
                return Swamp[YCoord - 1, XCoord];
            }
        }

        public Space East
        {
            get
            {
                return Swamp[YCoord, XCoord + 1];
            }
        }

        public Space South
        {
            get
            {
                return Swamp[YCoord + 1, XCoord];
            }
        }

        public Space West
        {
            get
            {
                return Swamp[YCoord, XCoord - 1];
            }
        }

        public bool Sinkhole
        {
            get
            {
                return Terrain == Terrain.Sinkhole;
            }
        }

        public bool Gold
        {
            get
            {
                return Terrain == Terrain.Gold;
            }
        }

        public bool OgreCanFindTheGoldHere
        {
            get
            {
                return Gold || East.Gold || South.Gold || South.East.Gold;  
            }
        }

        public bool OgreCanStandHere
        {
            get
            {
                return East != null && South != null && South.East != null && 
                    !Sinkhole && !East.Sinkhole && !South.Sinkhole && !South.East.Sinkhole;
            }
        }
    }

    internal class Swamp
    {
        public Space OgrePosition;
        public Space GoldPosition;
        private Space[,] Layout;
        internal char[][] Map;

        public Swamp(char[][] map)
        {
            Map = map;

            int yLen = map.Length;
            int xLen = map[0].Length;
            Layout = new Space[yLen, xLen];

            OgrePosition = GoldPosition = null;
            int x, y = 0;
            foreach (char[] s in map)
            {
                x = 0;
                foreach (char c in s)
                {
                    bool ogreIsHere = false, goldIsHere = false;
                    Terrain t;
                    switch (c)
                    {
                        case '@':
                            ogreIsHere = true;
                            goto case '.';
                        case '.':
                            t = Terrain.Empty;
                            break;
                        case '$':
                            goldIsHere = true;
                            t = Terrain.Gold;
                            break;
                        case 'O':
                            t = Terrain.Sinkhole;
                            break;
                        default:
                            //t = Terrain.Invalid;
                            throw new ArgumentException("Map contained invalid characters.");
                            break;
                    }


                    Layout[y, x] = new Space(x, y, this, t);
                    if (OgrePosition == null && ogreIsHere)
                    {
                        OgrePosition = Layout[y, x];
                    }
                    if (GoldPosition == null && goldIsHere)
                    {
                        GoldPosition = Layout[y, x];
                    }
                    x++;
                }
                y++;
            }
        }

        public Space this[int y, int x]
        {
            get
            {
                if (y < 0 || y >= Layout.GetLength(0) || x < 0 || x >= Layout.GetLength(1))
                {
                    return null;
                }
                return Layout[y, x];
            }
        }

        public static void PrintMap(char[][] map)
        {
            foreach (char[] s in map)
            {
                Console.WriteLine(s);
            }
        }
    }
}
