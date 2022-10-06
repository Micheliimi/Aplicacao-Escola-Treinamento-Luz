using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicação___Escola___Treinamento;
using Aplicação___Escola___Treinamento.Interfaces;
using Aplicação___Escola___Treinamento.Model.Database;
using Autofac.Extras.Moq;
using Moq;

namespace Escola.tests
{
    public class Aluno_teste_integracao
    {
        private List<Aluno> MockAlunos()
        {
            List<Aluno> output = new List<Aluno>
            {
                new Aluno
                {
                    CodAluno = 1,
                    NomeCompleto = "Michele Gusmão",
                    Serie = (Ano)2,
                },
                new Aluno
                {
                    CodAluno = 2,
                    NomeCompleto = "Guilherme Missias",
                    Serie = (Ano)4,
                },
                new Aluno
                {
                    CodAluno = 3,
                    NomeCompleto = "Homer",
                    Serie = (Ano)7,
                },
            };
            return output;
        }

        [Fact]
        public void busca_alunos_lista()
        {
            using (var mock = AutoMock.GetLoose())
            {
				Mock<IDatabase> mockedDB = new Mock<IDatabase>();
				mockedDB.Setup(x => x.BuscaAlunos()).Returns(MockAlunos());

                GenericDb conn = new GenericDb(mockedDB.Object);


				ObservableCollection<Aluno> listaAluno = new ObservableCollection<Aluno>(conn.BuscaAlunos());

                var expected = MockAlunos();
                var actual = listaAluno;

                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(expected[i].CodAluno, actual[i].CodAluno);
                    Assert.Equal(expected[i].NomeCompleto, actual[i].NomeCompleto);
                    Assert.Equal(expected[i].Serie, actual[i].Serie);
                }
            }
        }

	}
}
