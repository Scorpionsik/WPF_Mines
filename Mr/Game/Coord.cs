using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mr.Game
{
    public struct Coord
    {
        public int x;
        public int y;

        private readonly static List<Coord> mask = new List<Coord>()
        {
            new Coord(-1,0),
            new Coord(-1,-1),
            new Coord(0,-1),
            new Coord(1,-1),
            new Coord(1,0),
            new Coord(1,1),
            new Coord(0,1),
            new Coord(-1,1)
        };

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public List<Coord> MaskResult()
        {
            List<Coord> tmp_send = new List<Coord>();
            foreach (Coord m in mask)
            {
                tmp_send.Add(new Coord(this.x + m.x, this.y + m.y));
            }
            return tmp_send;
        }

        public static Coord GetRandomCoord(int width, int height)
        {
            Random r = new Random();
            int tmp_x = r.Next(width);
            int tmp_y = r.Next(height);
            return new Coord(tmp_x, tmp_y);
        }
    }
}
