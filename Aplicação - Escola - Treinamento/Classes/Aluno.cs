using System;
using System.ComponentModel;

namespace Aplicação___Escola___Treinamento
{
    public class Aluno : /*INotifyPropertyChanged,*/ ICloneable
    {
        private string nomeCompleto;
        private int codAluno;
        private Ano serie;

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void Notifica(string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}


        public string NomeCompleto
        {
            get { return nomeCompleto; }
            set { nomeCompleto = value;  }
        }

        public int CodAluno
        {
            get { return codAluno; }
            set { codAluno = value;  }
        }

        public Ano Serie
        {
            get { return serie; }
            set { serie = value; }
        }


        public object Clone()
        {
            return MemberwiseClone();
        }

    }


}
