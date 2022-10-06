﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicação___Escola___Treinamento;

namespace Aplicação___Escola___Treinamento
{
    public class MateriaNota : INotifyPropertyChanged
    {
        private int codNota;
        private Materia nomeMateria;
        private int nota = 0;

        public MateriaNota()
        {
        }
        public MateriaNota(Materia nomeMateria, int nota, int codNota)
        {
            this.NomeMateria = nomeMateria;
            this.Nota = nota;
            this.CodNota = codNota;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notifica(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Materia NomeMateria
        {
            get { return nomeMateria; }
            set { nomeMateria = value; }
        }

        public int Nota
        {
            get { return nota; }
            set { nota = value; }
        }

        public int CodNota
        {
            get { return codNota; }
            set { codNota = value; }
        }
     
    }

}
