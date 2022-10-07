using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicação___Escola___Treinamento.Interfaces;
using Aplicação___Escola___Treinamento.Model.Database;
using Aplicação___Escola___Treinamento;
using Autofac.Extras.Moq;
using Moq;

namespace Escola.tests
{
    public class Nota_teste_unitario
    {

        private List<MateriaNota> MockNotas()
        {
            List<MateriaNota> MockListaNota = new List<MateriaNota>
            {
                new MateriaNota
                {
                    CodNota = 1,
                    NomeMateria = (Materia)1,
                    Nota = 10,
                },
                new MateriaNota
                {
                    CodNota = 2,
                    NomeMateria = (Materia)2,
                    Nota = 0,
                },
                new MateriaNota
                {
                    CodNota = 3,
                    NomeMateria = (Materia)3,
                    Nota = 1,
                },
            };
            return MockListaNota;
        }

        [Fact]
        public void Busca_notas()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var aluno = new Aluno()
                {
                    CodAluno = 4,
                    NomeCompleto = "Homer",
                    Serie = (Ano)7,
                };

                mock.Mock<IDatabase>()
                    .Setup(x => x.BuscaNotas(aluno))
                    .Returns(MockNotas());

                var cls = mock.Create<GenericDb>();
                var expected = MockNotas().ToList(); ;
                var actual = cls.BuscaNotas(aluno).ToList(); ;

                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(expected[i].CodNota, actual[i].CodNota);
                    Assert.Equal(expected[i].NomeMateria, actual[i].NomeMateria);
                    Assert.Equal(expected[i].Nota, actual[i].Nota);
                }
            }
        }

        [Fact]
        public void Insere_nota_com_dados_corretos()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var aluno = new Aluno()
                {
                    CodAluno = 4,
                    NomeCompleto = "Homer",
                    Serie = (Ano)7,
                };
                var nota = MockNotas()[0];

                mock.Mock<IDatabase>()
                    .Setup(x => x.InsereNota(aluno, nota));

                var cls = mock.Create<GenericDb>();

                cls.InsereNota(aluno, nota);

                mock.Mock<IDatabase>()
                    .Verify(x => x.InsereNota(aluno, nota), Times.Exactly(1));

            }
        }

        [Fact]
        public void Insere_nota_com_dados_incorretos()
        {
            using (var mock = AutoMock.GetLoose())
            { 
                var aluno = new Aluno()
                {
                    CodAluno = 4,
                    NomeCompleto = "Homer",
                    Serie = (Ano)7,
                };
            
                var nota = new MateriaNota()
                {
                    CodNota = 3,
                    NomeMateria = (Materia)3,
                    Nota = -2,
                };

                mock.Mock<IDatabase>()
                    .Setup(x => x.InsereNota(aluno, nota));
                var cls = mock.Create<GenericDb>();

                var exc = Assert.Throws<ArgumentException>(() => cls.InsereNota(aluno, nota));
                Assert.Equal("Dado inválido no campo de nota.", exc.Message);

            }
        }


        [Fact]
        public void atualiza_nota_com_dados_corretos()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var nota = MockNotas()[0];
                mock.Mock<IDatabase>()
                    .Setup(x => x.AtualizaNota(nota));

                var cls = mock.Create<GenericDb>();

                cls.AtualizaNota(nota);

                mock.Mock<IDatabase>()
                    .Verify(x => x.AtualizaNota(nota), Times.Exactly(1));

            }
        }

        [Fact]
        public void atualiza_aluno_com_dados_incorretos()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var nota = new MateriaNota()
                {
                    CodNota = 3,
                    NomeMateria = (Materia)3,
                    Nota = -2,
                };

                mock.Mock<IDatabase>()
                    .Setup(x => x.AtualizaNota(nota));
                var cls = mock.Create<GenericDb>();

                var exc = Assert.Throws<ArgumentException>(() => cls.AtualizaNota(nota));
                Assert.Equal("Dado inválido no campo de nota.", exc.Message);

            }
        }

        [Fact]
        public void remove_nota()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var nota = MockNotas()[0];
                mock.Mock<IDatabase>()
                    .Setup(x => x.RemoveNota(nota));

                var cls = mock.Create<GenericDb>();

                cls.RemoveNota(nota);

                mock.Mock<IDatabase>()
                    .Verify(x => x.RemoveNota(nota), Times.Exactly(1));

            }
        }

    }
}
