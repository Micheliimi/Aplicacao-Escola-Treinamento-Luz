using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicação___Escola___Treinamento.Interfaces
{
    public interface IDatabase
    {
        //Task<List<Aluno>> BuscaAlunos();
        IEnumerable<Aluno> BuscaAlunos();
        IEnumerable<MateriaNota> BuscaNotas(Aluno aluno);
        void InsereAluno(Aluno aluno);
        //Task InsereAluno(Aluno aluno);
        void RemoveAluno(Aluno aluno);
        void AtualizaAluno(Aluno aluno);
        void InsereNota(Aluno aluno, MateriaNota nota);
        void AtualizaNota(MateriaNota nota);
        void RemoveNota(MateriaNota nota);

        void ResetaTabelas();

    }
}
