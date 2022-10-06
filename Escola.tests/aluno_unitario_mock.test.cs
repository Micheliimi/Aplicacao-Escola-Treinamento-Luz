using System.Reflection.PortableExecutable;
using Aplicação___Escola___Treinamento;
using Aplicação___Escola___Treinamento.Interfaces;
using Aplicação___Escola___Treinamento.Model.Database;
using Autofac.Extras.Moq;
using Moq;

namespace Escola.tests
{
    public class Aluno_teste_unitario
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
        public void Busca_alunos()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDatabase>()
                    .Setup(x => x.BuscaAlunos())
                    .Returns(MockAlunos());

                var cls = mock.Create<GenericDb>();
                var expected = MockAlunos();
                var actual = cls.BuscaAlunos();

                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);
                
                for(int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(expected[i].CodAluno, actual[i].CodAluno);
                    Assert.Equal(expected[i].NomeCompleto, actual[i].NomeCompleto);
                    Assert.Equal(expected[i].Serie, actual[i].Serie);
                }
            }
        }

        [Fact]
        public void Insere_aluno_com_dados_corretos()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var aluno = MockAlunos()[0];
                mock.Mock<IDatabase>()
                    .Setup(x => x.InsereAluno(aluno));

                var cls = mock.Create<GenericDb>();

                cls.InsereAluno(aluno);

                mock.Mock<IDatabase>()
                    .Verify(x => x.InsereAluno(aluno), Times.Exactly(1));
                
            }
        }

        [Fact]
        public void Insere_aluno_com_dados_incorretos()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var aluno = new Aluno()
                {
                    CodAluno = 4,
                    NomeCompleto = "H",
                    Serie = (Ano)7,
                };
           
                mock.Mock<IDatabase>()
                    .Setup(x => x.InsereAluno(aluno));
                var cls = mock.Create<GenericDb>();

                var exc = Assert.Throws<ArgumentException>(() => cls.InsereAluno(aluno));
                Assert.Equal("Dado inválido no campo de nome completo.", exc.Message);

            }
        }

      
        [Fact]
        public void Atualiza_aluno_com_dados_corretos()
        {
            using (var mock = AutoMock.GetLoose())
            {

                var aluno = MockAlunos()[0];
                mock.Mock<IDatabase>()
                    .Setup(x => x.AtualizaAluno(aluno));

                var cls = mock.Create<GenericDb>();

                cls.AtualizaAluno(aluno);

                mock.Mock<IDatabase>()
                    .Verify(x => x.AtualizaAluno(aluno), Times.Exactly(1));

            }
        }

        [Fact]
        public void Atualiza_aluno_com_dados_incorretos()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var aluno = new Aluno()
                {
                    CodAluno = 4,
                    NomeCompleto = "H",
                    Serie = (Ano)7,
                };

                mock.Mock<IDatabase>()
                    .Setup(x => x.AtualizaAluno(aluno));
                var cls = mock.Create<GenericDb>();

                var exc = Assert.Throws<ArgumentException>(() => cls.AtualizaAluno(aluno));
                Assert.Equal("Dado inválido no campo de nome completo.", exc.Message);

            }
        }

        [Fact]
        public void Remove_aluno()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var aluno = MockAlunos()[0];
                mock.Mock<IDatabase>()
                    .Setup(x => x.RemoveAluno(aluno));

                var cls = mock.Create<GenericDb>();

                cls.RemoveAluno(aluno);

                mock.Mock<IDatabase>()
                    .Verify(x => x.RemoveAluno(aluno), Times.Exactly(1));

            }
        }


    }
}