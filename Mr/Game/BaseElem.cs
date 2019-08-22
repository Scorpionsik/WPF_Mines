using CoreWPF.MVVM;
using CoreWPF.Utilites;
using Mr.Enums;

namespace Mr.Game
{
    public abstract class BaseElem : SimpleModel
    {
        public Coord Coordinats { get; private set; }

        private ElemStatus status;
        public ElemStatus Status
        {
            get { return this.status; }
            set
            {
                this.status = value;
                this.OnPropertyChanged("Symbol");
            }
        }

        private char symbol;
        public char Symbol
        {
            get
            {
                switch (this.Status)
                {
                    case ElemStatus.Question:
                        return '?';
                    case ElemStatus.Mark:
                        return '+';
                    case ElemStatus.Activate:
                        return this.symbol;
                    default:
                        return ' ';
                }
            }
            set
            {
                this.symbol = value;
                this.OnPropertyChanged("Symbol");
            }
        }

        public BaseElem(char symbol, Coord coordinats)
        {
            this.symbol = symbol;
            this.Coordinats = coordinats;
            this.status = ElemStatus.Default;
        }

        public override RelayCommand Command_select_model
        {
            get
            {
                /*
                if (this.Status != ElemStatus.Activate) return base.Command_select_model;
                else return null;*/
                return new RelayCommand(obj =>
                {
                    this.Status = ElemStatus.Activate;
                    base.Command_select_model.Execute(this);
                },
                (obj) => this.Status != ElemStatus.Activate
                );
            }
        }
    }
}
