using Mr.MVVM;
using Mr.Enums;
using System;

namespace Mr.Game
{
    public abstract class BaseElem : SimpleModel
    {
        public Coord Coordinats { get; private set; }

        private event Action event_checkResult;
        public event Action Event_checkResult
        {
            add { this.event_checkResult += value; }
            remove { this.event_checkResult -= value; }
        }

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
                        return 'X';
                    case ElemStatus.Activate:
                    case ElemStatus.TrueMark:
                        return this.symbol;
                    default:
                        return ' ';
                        //return this.symbol;
                }
            }
            set
            {
                this.symbol = value;
                this.OnPropertyChanged("Symbol");
            }
        }

        public BaseElem(char s, Coord coord)
        {
            this.symbol = s;
            this.Coordinats = coord;
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
                    if(this.Status != ElemStatus.Mark)
                    { 
                        base.Command_select_model?.Execute(this);
                        this.event_checkResult?.Invoke();
                    }
                },
                (obj) => this.Status != ElemStatus.Activate
                );
            }
        }

        private RelayCommand<BaseElem> command_changeStatus;
        public RelayCommand<BaseElem> Command_changeStatus
        {
            get { return this.command_changeStatus; }
            set
            {
                this.command_changeStatus = value;
            }
        }
    }
}
