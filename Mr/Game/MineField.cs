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

        public static int SizeButton {
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


        public ListExt<BaseElem> Items { get; private set; }

        public MineField(int width, int height, int bombs)
        {
            this.Width = width;
            this.Height = height;
            this.Bombs = bombs;
            this.Items = new ListExt<BaseElem>();

            List<Coord> tmp_bomb = new List<Coord>();
            for (int i = 0; i < this.Bombs; i++)
            {
                Coord gen = Coord.GetRandomCoord(this.Width, this.Height);
                if (tmp_bomb.Contains(gen)) i--;
                else tmp_bomb.Add(gen);
            }

            for (int h = 0; h < this.Height; h++)
            {
                for(int w = 0; w < this.Width; w++)
                {
                    Coord tmp_coord = new Coord(w, h);
                    if (tmp_bomb.Contains(tmp_coord))
                    {
                        this.AddElement(new BombElem(tmp_coord));
                    }
                    else this.AddElement(new EmptyElem(tmp_coord));
                }
            }
        }

        private void AddElement(BaseElem elem)
        {
            elem.Event_select_model = new System.Action<SimpleModel>(this.Activate);
            this.Items.Add(elem);
        }

        private int Decrypt(Coord c)
        {
            return c.x + c.y * this.Width;
        }

        public void Activate(SimpleModel m)
        {
            if (m is EmptyElem empty)
            {
                int count_bombs = 0;
                List<int> tmp_fields = new List<int>();

                foreach(Coord c in empty.Coordinats.MaskResult())
                {
                    int tmp = this.Decrypt(c);
                    if (tmp >= 0 && tmp < this.Items.Count)
                    {
                        tmp_fields.Add(tmp);
                    }
                }


                foreach(int c in tmp_fields)
                {
                    if (this.Items[c] is BombElem) count_bombs++;
                }

                if (count_bombs == 0)
                {
                    foreach (int c in tmp_fields)
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
