﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Obsidian.Util
{
    public struct Velocity
    {
        public short X;
        public short Y;
        public short Z;

        public Velocity(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}