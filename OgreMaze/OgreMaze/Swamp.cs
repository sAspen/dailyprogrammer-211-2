using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreMaze
{

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
                return Swamp[XCoord - 1, YCoord];
            }
        }

        public Space East
        {
            get
            {
                return Swamp[XCoord, YCoord + 1];
            }
        }

        public Space South
        {
            get
            {
                return Swamp[XCoord + 1, YCoord];
            }
        }

        public Space West
        {
            get
            {
                return Swamp[XCoord, YCoord - 1];
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
    }

    internal class Swamp
    {

        public Space OgrePosition;
        public Space GoldPosition;
        private Space[,] Layout;

        public Swamp(string[] map)
        {
            int xLen = new System.Globalization.StringInfo(map[0]).LengthInTextElements;
            int yLen = map.Length;
            Layout = new Space[xLen, yLen];

            OgrePosition = GoldPosition = null;
            int x, y = 0;
            foreach (string s in map)
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


                    Layout[x, y] = new Space(x, y, this, t);
                    if (OgrePosition == null && ogreIsHere)
                    {
                        OgrePosition = Layout[x, y];
                    }
                    if (GoldPosition == null && goldIsHere)
                    {
                        GoldPosition = Layout[x, y];
                    }
                    x++;
                }
                y++;
            }
        }

        public Space this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Layout.GetLength(0) || y < 0 || y >= Layout.Rank)
                {
                    return null;
                }
                return Layout[x, y];
            }
        }
    }
}
