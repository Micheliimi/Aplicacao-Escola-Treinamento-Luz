using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicação___Escola___Treinamento.Interfaces;

namespace Aplicação___Escola___Treinamento.Model.Database
{
    public class GenericDb
    {
        private IDatabase db;

         public GenericDb(IDatabase newData)
        {
            db = newData;
        }

        //public async Task<List<Aluno>> BuscaAlunos()
        //{
        //    return await db.BuscaAlunos();
        //}

        public IEnumerable<Aluno> BuscaAlunos()
        {
            return db.BuscaAlunos();
        }

        public IEnumerable<MateriaNota> BuscaNotas(Aluno aluno)
        {
            return db.BuscaNotas(aluno);
        }
        public void InsereAluno(Aluno aluno)
        {
            if (_ValidaNome(aluno) == false)
            {
                throw new ArgumentException("Dado inválido no campo de nome completo.");
            } else
            {
                db.InsereAluno(aluno);
            }
             
        }

        public void RemoveAluno(Aluno aluno)
        {
            if (aluno != null)
            {
                db.RemoveAluno(aluno);
            } else
            {
                throw new ArgumentException("Nenhum aluno selecionado!");
            }
            
        }

        public void AtualizaAluno(Aluno aluno)
        {
            if (aluno != null)
            {
                if (_ValidaNome(aluno) == false)
                {
                    throw new ArgumentException("Dado inválido no campo de nome completo.");
                }
                else
                {
                    db.AtualizaAluno(aluno);
                }
            } else
            {
                throw new ArgumentException("Nenhum aluno selecionado!");
            }
           
            
        }

        public void InsereNota(Aluno aluno, MateriaNota nota)
        {
            if (aluno != null)
            {
                if (_ValidaNota(nota) == false)
                {
                    throw new ArgumentException("Dado inválido no campo de nota.");
                }
                else
                {
                    db.InsereNota(aluno, nota);
                }
            } else
            {
                throw new ArgumentException("Nenhum aluno selecionado!");
            }
                
            
        }

        public void AtualizaNota(MateriaNota nota)
        {
            if(nota != null)
            {
                if (_ValidaNota(nota) == false)
                {
                    throw new ArgumentException("Dado inválido no campo de nota.");
                }
                else
                {
                    db.AtualizaNota(nota);
                }
            } else
            {
                throw new ArgumentException("Nenhuma nota selecionada!");
            }
           
           
        }

        public void RemoveNota(MateriaNota nota)
        {
            if (nota != null)
            {
                db.RemoveNota(nota);
            } else
            {
                throw new ArgumentException("Nenhuma nota selecionada!");
            }
           
        }

        public void ResetaTabelas()
        {
            db.ResetaTabelas(); 
        }

        private bool _ValidaNome(Aluno aluno)
        {
            if (aluno.NomeCompleto == null || aluno.NomeCompleto.Length < 3) return false;
            return true;
        }

        private bool _ValidaNota(MateriaNota nota)
        {
            if (nota.Nota < 0 || nota.Nota > 10) return false;
            return true;
        }

    }
}
