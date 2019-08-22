using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mr.Game
{
    public class EmptyElem : BaseElem
    {
        public EmptyElem(Coord coordinats) : base('-', coordinats) { }
    }
}
