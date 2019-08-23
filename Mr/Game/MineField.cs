using Mr.MVVM;
using Mr.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace Mr.Game
{
    public class MineField : ViewModel
    {
        private CancellationTokenSource timer;
        private int sec;
        private int Sec
        {
            set
            {
                sec = value;
                if (sec == 60)
                {
                    this.sec = 0;
                    this.min++;
                }
                this.OnPropertyChanged("Time");
            }
        }
        private int min;

        private GameStatus status;
        public GameStatus Status
        {
            get { return this.status; }
            set
            {
                this.status = value;
                this.OnPropertyChanged("Status");
                this.GameOver();
            }
        }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Bombs { get; private set; }
        private int WinScore;
        private int NonActiveElems;
        private List<Coord> marks;
        
        public string Time
        {
            get
            {
                string tmp_send = "";
                if (this.min < 10) tmp_send += "0";
                tmp_send += this.min.ToString() + " : ";
                if (this.sec < 10) tmp_send += "0";
                tmp_send += this.sec;
                return tmp_send;
            }
        }

        public int Count
        {
            get
            {
                return this.Bombs - this.marks.Count;
            }
        }

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


        public ListExt<BaseElem> Items { get; private set; }

        public MineField(int width, int height, int bombs)
        {
            this.Width = width;
            this.Height = height;
            this.Bombs = bombs;
            this.Command_Generate.Execute(null);
        }

        private void GameOver()
        {
            if (this.Status != GameStatus.InGame) this.timer.Cancel();
            if (this.Status == GameStatus.Win) MessageBox.Show("Это победа!\nПоздравляю Вас!", "Сапер не взорвался, ура!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            else if (this.Status == GameStatus.Lose)
            {
                MessageBox.Show("Вы проиграли...", "БА-БАААААХ!", MessageBoxButton.OK, MessageBoxImage.Stop);
                foreach (BaseElem b in this.Items)
                {
                    if (b is BombElem)
                    {
                        if (b.Status != ElemStatus.Mark) b.Status = ElemStatus.Activate;
                        else b.Status = ElemStatus.TrueMark;
                    }
                }
            }
        }

        private async void GenegateField()
        {
            this.Items = new ListExt<BaseElem>();
            List<int> tmp_count = new List<int>();
            await Task.Run(() => 
            {
                
                for (int h = 0; h < this.Height; h++)
                {
                    for (int w = 0; w < this.Width; w++)
                    {
                        Coord tmp = new Coord(w, h);

                        this.Items.Add(this.BindElem(new EmptyElem(tmp)));
                        tmp_count.Add(this.Decrypt(tmp));
                    }
                }
            });

            await Task.Run(() =>
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int i = 0, b = this.Width * this.Height; i < this.Width * this.Height; i++, b--)
                    {
                        int rand = new Random().Next(b);
                        int save = tmp_count[rand];

                        tmp_count[rand] = tmp_count[b - 1];
                        tmp_count[b - 1] = save;
                    }
                }
            });

            List<int> tmp_list = new List<int>();
            await Task.Run(() =>
            {
                for (int i = 0; i < this.Bombs; i++)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        int index = i * ((this.Width * this.Height) / this.Bombs);
                        if (index >= (this.Width * this.Height) && index < 0) index = (this.Width * this.Height) - 1;
                        this.Items[tmp_count[index]] = this.BindElem(new BombElem(this.Items[tmp_count[index]].Coordinats));
                    });
                }
            });

            this.OnPropertyChanged("Items");
        }

        private BaseElem BindElem(BaseElem elem)
        {
            elem.Event_select_model = new System.Action<SimpleModel>(this.Activate);
            elem.Event_checkResult += new Action(this.CheckResult);
            elem.Command_changeStatus = this.Command_StatusChange;
            return elem; 
        }

        private int Decrypt(Coord c)
        {
            if (c.x >= 0 && c.x < this.Width && c.y >= 0 && c.y < this.Height) return c.x + c.y * this.Width;
            else return -1;
        }

        private async void StartTimer()
        {
            CancellationToken token = this.timer.Token;
            await Task.Run(() =>
            {
                while (true)
                { 
                    Thread.Sleep(1000);
                    if (token.IsCancellationRequested) return;
                    this.Sec = this.sec + 1;
                }
            });
        }

        private BaseElem FirstTimeSave(BombElem bomb)
        {
            int count = this.Decrypt(bomb.Coordinats), send_count = this.Decrypt(bomb.Coordinats);
            this.Items[send_count] = this.BindElem(new EmptyElem(bomb.Coordinats) { Status = ElemStatus.Activate });
            
            while (true)
            {
                count++;
                if (count > this.Items.Count - 1) count = 0;
                if(this.Items[count] is EmptyElem e)
                {
                    this.Items[count] = this.BindElem(new BombElem(e.Coordinats));
                    break;
                }
            }

            return this.Items[send_count];
        }

        public void Activate(SimpleModel m)
        {
            if(m is BaseElem e)
            {
                this.NonActiveElems--;
                if (e.Status == ElemStatus.Mark) this.RemoveMark(e.Coordinats);
                e.Status = ElemStatus.Activate;

                if(this.NonActiveElems == this.Width * this.Height - 1)
                {
                    if (e is BombElem b) m = this.FirstTimeSave(b);
                    this.StartTimer();
                }
            }
            
            if (m is EmptyElem empty)
            {
                int count_bombs = 0;
                List<int> tmp_fields = new List<int>();

                foreach(Coord c in empty.Coordinats.MaskResult())
                {
                    int tmp = this.Decrypt(c);
                    if (tmp >=0 && tmp < this.Items.Count)
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

                            //this.Items[c].Command_select_model?.Execute(this.Items[c]);
                            this.Activate(this.Items[c]);
                        }
                    }
                }
                else empty.Symbol = Convert.ToChar(count_bombs.ToString());
            }
            else if (m is BombElem bomb)
            {
                this.Status = GameStatus.Lose;
            }
        }

        private void CheckResult()
        {
            if (this.NonActiveElems == this.Bombs || this.WinScore == this.Bombs) this.Status = GameStatus.Win;
        }

        private void AddMark(Coord c)
        {
            this.marks.Add(c);
            if (this.Items[this.Decrypt(c)] is BombElem) this.WinScore++;
            this.CheckResult();
            this.OnPropertyChanged("Count");
        }

        private void RemoveMark(Coord c)
        {
            this.marks.Remove(c);
            if (this.Items[this.Decrypt(c)] is BombElem) this.WinScore--;
            this.OnPropertyChanged("Count");
        }

        public RelayCommand Command_Generate
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Task.Run(() =>
                    {
                        this.GenegateField();
                        if (this.timer != null) this.timer.Cancel();
                        this.timer = new CancellationTokenSource();
                        this.min = 0;
                        this.Sec = 0;
                        this.Status = GameStatus.InGame;
                        this.WinScore = 0;
                        this.NonActiveElems = this.Width * this.Height;
                        this.marks = new List<Coord>();
                        this.OnPropertyChanged("Count");
                    });
                });
            }
        }

        public RelayCommand<BaseElem> Command_StatusChange
        {
            get
            {
                return new RelayCommand<BaseElem>(obj =>
                {
                    switch (obj.Status)
                    {
                        case Enums.ElemStatus.Default:
                            if (this.Count > 0)
                            {
                                obj.Status = Enums.ElemStatus.Mark;
                                this.AddMark(obj.Coordinats);
                            }
                            else obj.Status = Enums.ElemStatus.Question;
                            break;
                        case Enums.ElemStatus.Mark:
                            obj.Status = Enums.ElemStatus.Question;
                            this.RemoveMark(obj.Coordinats);
                            break;
                        case Enums.ElemStatus.Question:
                            obj.Status = Enums.ElemStatus.Default;
                            break;
                    }
                },
                (obj) => obj.Status != Enums.ElemStatus.Activate
                );
            }
        }
    }
}
