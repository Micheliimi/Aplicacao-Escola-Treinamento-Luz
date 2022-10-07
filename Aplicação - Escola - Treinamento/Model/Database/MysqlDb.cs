using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Windows.Documents;
using System.Windows.Media;
using System.Diagnostics;
using Aplicação___Escola___Treinamento.Interfaces;
using Aplicação___Escola___Treinamento;
using System.ComponentModel;

namespace Aplicação___Escola___Treinamento.Database
{
    public class MysqlDb : IDatabase //, INotifyPropertyChanged
    {
        private static string _host;

        private static string _port;

        private static string _user;

        private static string _password;

        private static string _dbname;

        //private static MySqlConnection _connection;

        //private static MySqlCommand _command;

        private string _query;

        //private MySqlDataReader _reader;

        public MysqlDb()
        {
            //_Inicializa();
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void Notifica(string propertyName = null)
        //{
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //private void _Inicializa()
        //{
            //_host = "localhost";

            //_port = "3306";

            //_user = "root";

            //_password = "102030";

            //_dbname = "escola_db";

            //_connection = new MySqlConnection($"server={_host};user={_user};database={_dbname};port={_port};password={_password};");

        //}

        private string _strConnect()
        {
            _host = "localhost";

            _port = "3306";

            _user = "root";

            _password = "102030";

            _dbname = "escola_db";

           return $"server={_host};user={_user};database={_dbname};port={_port};password={_password};";
        }

        //private bool _OpenConnection()
        //{
            //try
            //{
                //_connection.Open();
                //return true;
            //} catch (MySqlException e)
            //{
                //MessageBox.Show(e.Message);
                //return false;
            //}
        //}

       // public List<Aluno> BuscaAlunos()
       // {
            //_query = $@"SELECT * FROM escola_db.alunos;";
        //    List<Aluno> lista = new List<Aluno>();
            
        //    try
        //    {
        //        using (MySqlConnection con = new MySqlConnection(_strConnect()))
        //        {
        //            con.Open();
        //            using(MySqlCommand cmd = new MySqlCommand(_query, con))
        //            {
        //                using (MySqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        Aluno aluno = new Aluno()
        //                        {
        //                            CodAluno = reader.GetInt32(0),
        //                            NomeCompleto = reader.GetString(1),
        //                            Serie = (Ano)reader.GetInt32(2),
        //                        };

        //                        lista.Add(aluno);
        //                    }
        //                    return lista;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public IEnumerable<Aluno> BuscaAlunos()
        {
            _query = $@"SELECT * FROM escola_db.alunos;";

            using (MySqlConnection con = new MySqlConnection(_strConnect()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(_query, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
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

            _query = $@"SELECT * FROM escola_db.aluno_materia_nota
            WHERE aluno_id = {alunoSelecionado.CodAluno};";

            using (MySqlConnection con = new MySqlConnection(_strConnect()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(_query, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
        
        
        //private void _CloseConnection()
        //{
            //_connection.Close();
        //}

        //public MySqlConnection Get_connection()
        //{
            //return _connection;
        //}

        public void InsereAluno (Aluno aluno)
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

            _ExecuteQuery (_query);
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
                using (MySqlConnection con = new MySqlConnection(_strConnect()))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
        }
    }
}
