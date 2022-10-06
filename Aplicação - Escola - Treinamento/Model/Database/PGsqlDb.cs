using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicação___Escola___Treinamento.Interfaces;
using Npgsql;

namespace Aplicação___Escola___Treinamento.Model.Database
{
    internal class PGsqlDb : IDatabase
    {
        private static string _host;

        private static string _port;

        private static string _user;

        private static string _password;

        private static string _dbname;

        private string _query;


        public PGsqlDb(){}

        private string _strConnect()
        {
            _host = "localhost";

            _port = "3306";

            _user = "root";

            _password = "102030";

            _dbname = "escola_db";

            return $"Server={_host};Username={_user};Databasee={_dbname};Port={_port};Password={_password};";
        }

        public List<Aluno> BuscaAlunos()
        {
            _query = $@"SELECT * FROM escola_db.alunos;";
            List<Aluno> lista = new List<Aluno>();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(_strConnect()))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(_query, con))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Aluno aluno = new Aluno()
                                {
                                    CodAluno = reader.GetInt32(0),
                                    NomeCompleto = reader.GetString(1),
                                    Serie = (Ano)reader.GetInt32(2),
                                };

                                lista.Add(aluno);
                            }
                            return lista;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MateriaNota> BuscaNotas(Aluno alunoSelecionado)

        {

            _query = $@"SELECT * FROM escola_db.aluno_materia_nota
            WHERE aluno_id = {alunoSelecionado.CodAluno};";
            List<MateriaNota> lista = new List<MateriaNota>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(_strConnect()))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(_query, con))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MateriaNota nota = new MateriaNota()
                                {
                                    CodNota = reader.GetInt32(0),
                                    NomeMateria = (Materia)reader.GetInt32(2),
                                    Nota = reader.GetInt32(3),
                                };
                                lista.Add(nota);

                            }
                            return lista;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereAluno(Aluno aluno)
        {
            _query = $@"INSERT INTO escola_db.alunos(nome_completo, serie_id)
                VALUES
                ('{aluno.NomeCompleto}', '{(int)aluno.Serie}')";

            _ExecuteQuery(_query);
        }


        public void RemoveAluno(Aluno aluno)
        {
            _query = $@"DELETE
                FROM  escola_db.alunos
                WHERE id = '{aluno.CodAluno}'";

            _ExecuteQuery(_query);
        }

        public void AtualizaAluno(Aluno aluno)
        {
            _query = $@"UPDATE
                escola_db.alunos
                SET nome_completo = '{aluno.NomeCompleto}', serie_id = '{(int)aluno.Serie}'
                WHERE id = '{aluno.CodAluno}'";

            _ExecuteQuery(_query);
        }

        public void InsereNota(Aluno aluno, MateriaNota nota)
        {
            _query = $@"USE escola_db;
                INSERT INTO aluno_materia_nota(aluno_id, materia_id, nota)
                VALUES ('{aluno.CodAluno}', '{(int)nota.NomeMateria}', '{nota.Nota}')";

            _ExecuteQuery(_query);
        }

        public void AtualizaNota(MateriaNota nota)
        {
            _query = $@"USE escola_db;
                UPDATE aluno_materia_nota
                SET materia_id = '{(int)nota.NomeMateria}', nota = '{nota.Nota}'
                WHERE id = '{nota.CodNota}'";

            _ExecuteQuery(_query);
        }
        public void RemoveNota(MateriaNota nota)
        {
            _query = $@"USE escola_db;
                        DELETE
                        FROM aluno_materia_nota
                        WHERE id = '{nota.CodNota}'";

            _ExecuteQuery(_query);
        }

        public void ResetaTabelas()
        {
            _DropaTabelas();
            _CriaTabelas();
        }
        private void _CriaTabelas()
        {

            _query = $@"
                CREATE TABLE escola_db.alunos (
                id INTEGER auto_increment PRIMARY KEY NOT NULL,
                nome_completo VARCHAR(50) NOT NULL,
                serie_id INTEGER,
                FOREIGN KEY (serie_id) REFERENCES series (id)
	            ON DELETE CASCADE ON UPDATE CASCADE);";

            _ExecuteQuery(_query);

            _query = $@"
                CREATE TABLE escola_db.aluno_materia_nota (
                id INTEGER auto_increment PRIMARY KEY NOT NULL,
                aluno_id INTEGER,
                FOREIGN KEY (aluno_id) REFERENCES alunos (id)
	            ON DELETE CASCADE ON UPDATE CASCADE,
                materia_id INTEGER,
                FOREIGN KEY (materia_id) REFERENCES materias (id)
	            ON DELETE CASCADE ON UPDATE CASCADE,
                nota INTEGER NOT NULL);";

            _ExecuteQuery(_query);
        }
        private void _DropaTabelas()
        {
            _query = @"DROP TABLE IF EXISTS escola_db.aluno_materia_nota;
                       DROP TABLE IF EXISTS escola_db.alunos;";
            _ExecuteQuery(_query);

        }

        private void _ExecuteQuery(string query)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(_strConnect()))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)

            {
                throw ex;
            }
        }
    }
}
