using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aplicação___Escola___Treinamento
{
    /// <summary>
    /// Lógica interna para AtualizaDadosAluno.xaml
    /// </summary>
    public partial class AtualizaDadosAluno : Window
    {
        public AtualizaDadosAluno()
        {
            InitializeComponent();
        }

        public void bnt_Atualizar(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
