using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicação___Escola___Treinamento.Interfaces;
using MySql.Data.MySqlClient;
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

            _port = "5432";

            _user = "postgres";

            _password = "102030";

            _dbname = "escola_db";

            return $"Server={_host};Username={_user};Database={_dbname};Port={_port};Password={_password};";
        }

      
        public IEnumerable<Aluno> BuscaAlunos()
        {
            _query = $@"SELECT * FROM alunos;";

            using (NpgsqlConnection con = new NpgsqlConnection(_strConnect()))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(_query, con))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Aluno()
                            {
                                CodAluno = reader.GetInt32(0),
                                NomeCompleto = reader.GetString(1),
                                Serie = (Ano)reader.GetInt32(2),
                            };

                        }
                    }
                }
            }
        }
        public IEnumerable<MateriaNota> BuscaNotas(Aluno alunoSelecionado)

        {

            _query = $@"SELECT * FROM aluno_materia_nota
            WHERE aluno_id = {alunoSelecionado.CodAluno};";
            using (NpgsqlConnection con = new NpgsqlConnection(_strConnect()))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(_query, con))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                                yield return new MateriaNota()
                            {
                                CodNota = reader.GetInt32(0),
                                NomeMateria = (Materia)reader.GetInt32(2),
                                Nota = reader.GetInt32(3),
                            };

                        }
                    }
                }
            }
        }

        public void InsereAluno(Aluno aluno)
        {
            _query = $@"INSERT INTO alunos(nome_completo, serie_id)
                VALUES
                ('{aluno.NomeCompleto}', '{(int)aluno.Serie}')";

            _ExecuteQuery(_query);
        }


        public void RemoveAluno(Aluno aluno)
        {
            _query = $@"DELETE
                FROM alunos
                WHERE id = '{aluno.CodAluno}'";

            _ExecuteQuery(_query);
        }

        public void AtualizaAluno(Aluno aluno)
        {
            _query = $@"UPDATE
                alunos
                SET nome_completo = '{aluno.NomeCompleto}', serie_id = '{(int)aluno.Serie}'
                WHERE id = '{aluno.CodAluno}'";

            _ExecuteQuery(_query);
        }

        public void InsereNota(Aluno aluno, MateriaNota nota)
        {
            _query = $@"INSERT INTO aluno_materia_nota(aluno_id, materia_id, nota)
                VALUES ('{aluno.CodAluno}', '{(int)nota.NomeMateria}', '{nota.Nota}')";

            _ExecuteQuery(_query);
        }

        public void AtualizaNota(MateriaNota nota)
        {
            _query = $@"UPDATE aluno_materia_nota
                SET materia_id = '{(int)nota.NomeMateria}', nota = '{nota.Nota}'
                WHERE id = '{nota.CodNota}'";

            _ExecuteQuery(_query);
        }
        public void RemoveNota(MateriaNota nota)
        {
            _query = $@"DELETE
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
                CREATE TABLE alunos (
                id SERIAL PRIMARY KEY NOT NULL,
                nome_completo VARCHAR(50) NOT NULL,
                serie_id INTEGER,
                FOREIGN KEY (serie_id) REFERENCES series (id)
	            ON DELETE CASCADE ON UPDATE CASCADE);";

            _ExecuteQuery(_query);

            _query = $@"
                CREATE TABLE aluno_materia_nota (
                id SERIAL PRIMARY KEY NOT NULL,
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
            _query = @"DROP TABLE IF EXISTS aluno_materia_nota;
                       DROP TABLE IF EXISTS alunos;";
            _ExecuteQuery(_query);

        }

        private void _ExecuteQuery(string query)
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
    }
}
