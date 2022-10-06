using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using Aplicação___Escola___Treinamento;

namespace Aplicação___Escola___Treinamento
{
    public class Aluno : INotifyPropertyChanged
    {
        private string nomeCompleto;
        private  int codAluno;
        private Ano serie;
        public Aluno()
        {
        }
        public Aluno(string nomeCompleto, int codAluno, Ano serie)
        {
            this.NomeCompleto = nomeCompleto;
            this.CodAluno = codAluno;
            this.Serie = serie;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notifica(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string NomeCompleto
        {
            get { return nomeCompleto; }
            set { nomeCompleto = value; Notifica(); }
        }

        public int CodAluno
        {
            get { return codAluno; }
            set { codAluno = value; Notifica(); }
        }

        public Ano Serie
        {
            get { return serie; }
            set { serie = value; Notifica(); }
        }


        //public object Clone()
        //{
            //Aluno alunoClone = new Aluno();
            //.NomeCompleto = this.nomeCompleto;
            //alunoClone.Serie = this.serie;
            //alunoClone.CodAluno = this.codAluno;
            //return alunoClone;
            //Aluno alunoClonado = (Aluno)this.MemberwiseClone();
            //return alunoClonado;
        //}

    }
   

}
