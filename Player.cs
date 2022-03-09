using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.KeyboardKey;
using ping_pong;

namespace ping_pong
{
    class Player {
        public float x;
        public float y;
        public int width;
        public int height;
        public Player(int x, int y, int width, int height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}