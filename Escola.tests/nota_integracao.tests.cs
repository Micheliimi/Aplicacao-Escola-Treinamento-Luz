using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class Nota_teste_integracao
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
        public void busca_nota_lista()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var aluno = new Aluno()
                {
                    CodAluno = 4,
                    NomeCompleto = "Homer",
                    Serie = (Ano)7,
                };

                Mock<IDatabase> mockedDB = new Mock<IDatabase>();
                mockedDB.Setup(x => x.BuscaNotas(aluno)).Returns(MockNotas());

                GenericDb conn = new GenericDb(mockedDB.Object);


                ObservableCollection<MateriaNota> listaNota = new ObservableCollection<MateriaNota>(conn.BuscaNotas(aluno));

                var expected = MockNotas();
                var actual = listaNota;

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

    }
}
