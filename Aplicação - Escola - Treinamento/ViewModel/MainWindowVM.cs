using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Aplicação___Escola___Treinamento.Database;
using Aplicação___Escola___Treinamento.Model.Database;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Utilities.Collections;

namespace Aplicação___Escola___Treinamento
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public ObservableCollection<Aluno> listaAluno { get; set; }
        public ObservableCollection<MateriaNota> listaNotas { get; set; }
        public ICommand Add { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Update { get; private set; }
        public ICommand ViewGrades {  get; private set; }  
        public ICommand AddGrade { get; private set; }
        public ICommand UpdateGrade {  get; private set; } 
        public ICommand RemoveGrade { get; private set; }
        private Aluno _alunoSelecionado;
        public MateriaNota NotaSelecionada { get; set; }

        private readonly GenericDb _conn;

        //private MySqlCommand command;

        //private string sql;
        public MainWindowVM()
        {
            _conn = new GenericDb(new MysqlDb());
            try
            {
               listaAluno = new ObservableCollection<Aluno>(_conn.BuscaAlunos());
            } 
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }


        //_conn.ResetaTabelas();
        IniciaComandos();
        }

        public Aluno AlunoSelecionado
        {
            get { return _alunoSelecionado; }
            set { _alunoSelecionado = value;
                if (value != null)
                {
                    listaNotas = new ObservableCollection<MateriaNota>(_conn.BuscaNotas(_alunoSelecionado));
                    Notifica();
                }
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notifica(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void IniciaComandos()
        {
            Add = new RelayCommand((object _) =>
            {
                Aluno novoAluno = new Aluno();
                CadastroAluno tela = new CadastroAluno();
                tela.DataContext = novoAluno;
                tela.ShowDialog();
                
                try
                {
                    _conn.InsereAluno(novoAluno);
                    listaAluno = new ObservableCollection<Aluno>(_conn.BuscaAlunos());
                    Notifica();

                } catch (Exception ex)
                {
                    MessageBox.Show("" + ex);
                }

            });

            Remove = new RelayCommand((object _) =>
            {
                
                try
                {
                    _conn.RemoveAluno(AlunoSelecionado);
                    listaAluno = new ObservableCollection<Aluno>(_conn.BuscaAlunos());
                    Notifica();
                    listaNotas.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex);
                }


            }, (object _) => this.AlunoSelecionado != null);


            Update = new RelayCommand((object _) =>
            {

                AtualizaDadosAluno telaAtualiza = new AtualizaDadosAluno();
                telaAtualiza.DataContext = AlunoSelecionado;
                bool? verificaBotao = telaAtualiza.ShowDialog();

                if (verificaBotao.HasValue && verificaBotao.Value)
                {
                    try
                    {
                        _conn.AtualizaAluno(AlunoSelecionado);

                        listaAluno = new ObservableCollection<Aluno>(_conn.BuscaAlunos());
                        Notifica();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex);
                    }
                }

            }, (object _) => this.AlunoSelecionado != null);

            AddGrade = new RelayCommand((object _) =>
            {
                MateriaNota novaNota = new MateriaNota();


                AdicionaNota tela = new AdicionaNota();

                tela.DataContext = novaNota;
                tela.ShowDialog();

                try
                {
                    _conn.InsereNota(AlunoSelecionado, novaNota);

                    listaNotas = new ObservableCollection<MateriaNota>(_conn.BuscaNotas(AlunoSelecionado));
                    Notifica();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex);
                }

            }, (object _) => this.AlunoSelecionado != null);

            UpdateGrade = new RelayCommand((object _) =>
            {
                EditaNota tela = new EditaNota();
                tela.DataContext = NotaSelecionada;
                
                bool? verificaBotao = tela.ShowDialog();
                
                if (verificaBotao.HasValue && verificaBotao.Value)
                {
                    try
                    {
                        _conn.AtualizaNota(NotaSelecionada);

                        listaNotas = new ObservableCollection<MateriaNota>(_conn.BuscaNotas(AlunoSelecionado));
                        Notifica();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex);
                    }

                }

            }, (object _) => this.NotaSelecionada != null);

            RemoveGrade = new RelayCommand((object _) =>
            {
                try
                {
                    _conn.RemoveNota(NotaSelecionada);

                    listaNotas = new ObservableCollection<MateriaNota>(_conn.BuscaNotas(AlunoSelecionado));
                    Notifica();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex);
                }

            }, (object _) => this.NotaSelecionada != null);

        }

    }
}
