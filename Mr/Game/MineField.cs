using CoreWPF.MVVM;
using CoreWPF.Utitltes;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Mr.Game
{
    public class MineField : ViewModel
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Bombs { get; private set; }

        public int SizeButton {
            get
            {
                return 25;
            }
        }

        public int FieldWidht
        {
            get
            {
                return this.Width * SizeButton;
            }
        }

        public int FieldHeight
        {
            get
            {
                return this.Height * SizeButton;
            }
        }


        public Dictionary<Coord,BaseElem> Items { get; private set; }
        public Dictionary<BaseElem, Coord> InverseItems { get; private set; }

        public MineField(int width, int height, int bombs)
        {
            this.Width = width;
            this.Height = height;
            this.Bombs = bombs;
            this.Items = new Dictionary<Coord, BaseElem>();
            this.InverseItems = new Dictionary<BaseElem, Coord>();
            for(int h = 0; h < this.Height; h++)
            {
                for(int w = 0; w < this.Width; w++)
                {
                    //this.Items.Add(new Coord(w,h), new EmptyElem() { Event_select_model = new System.Action<SimpleModel>(this.Activate) });
                    this.AddElement(new Coord(w, h), new EmptyElem());
                }
            }

            List<Coord> tmp_bomb = new List<Coord>();
            for(int i = 0; i < this.Bombs; i++)
            {
                Coord gen = Coord.GetRandomCoord(this.Width, this.Height);
                if (tmp_bomb.Contains(gen)) i--;
                else tmp_bomb.Add(gen);
            }

            foreach(Coord c in tmp_bomb)
            {
                //this.Items[c] = new BombElem() { Event_select_model = new System.Action<SimpleModel>(this.Activate)};
                this.AddElement(c, new BombElem());
            }
        }

        private void AddElement(Coord coord, BaseElem elem)
        {
            elem.Event_select_model = new System.Action<SimpleModel>(this.Activate);
            if (this.Items.ContainsKey(coord))
            {
                this.InverseItems.Remove(this.Items[coord]);
                this.Items[coord] = elem;
                this.InverseItems.Add(elem, coord);
            }
            else
            {
                this.Items.Add(coord, elem);
                this.InverseItems.Add(elem, coord);
            }
        }

        public void Activate(SimpleModel m)
        {
            if (m is EmptyElem empty)
            {
                int count_bombs = 0;
                List<Coord> tmp_fields = new List<Coord>();

                foreach(Coord c in this.InverseItems[empty].MaskResult())
                {
                    if (this.Items.ContainsKey(c))
                    {
                        tmp_fields.Add(c);
                    }
                }


                foreach(Coord c in tmp_fields)
                {
                    if (this.Items[c] is BombElem) count_bombs++;
                }

                if (count_bombs == 0)
                {
                    foreach (Coord c in tmp_fields)
                    {
                        if (this.Items[c].Status != Enums.ElemStatus.Activate)
                        {

                            this.Items[c].Command_select_model?.Execute(this.Items[c]);
                        }
                    }
                }
                else empty.Symbol = Convert.ToChar(count_bombs.ToString());
            }
            else if (m is BombElem bomb)
            {
                MessageBox.Show("Boom!");
            }
        }
    }
}
