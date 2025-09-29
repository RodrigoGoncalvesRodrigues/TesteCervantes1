namespace TesteCervantes
{
    using Npgsql;
    using System.Data;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Form1 : Form
    {
        private Cadastro cadastro = new Cadastro();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarDados();
        }

        private void CarregarDados()
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<Cadastro> todosCadastros = cadastro.GetAll();

                if (todosCadastros.Count == 0)
                {
                    MessageBox.Show("Nenhum cadastro encontrado.");
                    return;
                }

                string mensagem = "Cadastros:\n\n";
                foreach (var item in todosCadastros)
                {
                    mensagem += $"ID: {item.Id} | Nome: {item.Nome} | Número: {item.Numero}\n";
                }

                MessageBox.Show(mensagem, "Lista de Cadastros");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string numeroTexto = textBox2.Text.Trim();
            string nome = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Digite um nome válido");
                return;
            }

            if (string.IsNullOrWhiteSpace(numeroTexto))
            {
                MessageBox.Show("Digite um telefone válido");
                return;
            }

            
            if (!long.TryParse(numeroTexto, out long numero))
            {
                MessageBox.Show("Telefone deve conter apenas números");
                return;
            }

            if (numero <= 0)
            {
                MessageBox.Show("Telefone deve ser maior que zero");
                return;
            }

            var cadastroParaDeletar = cadastro.GetByNomeNumero(nome, numero);
            if (cadastroParaDeletar == null)
            {
                MessageBox.Show("Cadastro não encontrado");
                return;
            }

            if (cadastro.Delete(cadastroParaDeletar.Id))
            {
                MessageBox.Show("Cadastro removido com sucesso!");
                textBox1.Clear();
                textBox2.Clear();
                CarregarDados();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string numeroTexto = textBox2.Text.Trim();
            string nome = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Digite um nome válido");
                return;
            }

            if (string.IsNullOrWhiteSpace(numeroTexto))
            {
                MessageBox.Show("Digite um telefone válido");
                return;
            }

            
            if (!long.TryParse(numeroTexto, out long numero))
            {
                MessageBox.Show("Telefone deve conter apenas números");
                return;
            }

            var cadastroExistente = cadastro.GetByNomeNumero(nome, numero);
            if (cadastroExistente == null)
            {
                MessageBox.Show("Cadastro não encontrado");
                return;
            }

            string novoNome = Microsoft.VisualBasic.Interaction.InputBox(
                "Digite o novo nome:", "Atualizar Nome", nome);

            if (string.IsNullOrWhiteSpace(novoNome))
            {
                MessageBox.Show("Nome não pode estar vazio");
                return;
            }

            string novoNumeroTexto = Microsoft.VisualBasic.Interaction.InputBox(
                "Digite o novo telefone:", "Atualizar Telefone", numero.ToString());

            if (string.IsNullOrWhiteSpace(novoNumeroTexto))
            {
                MessageBox.Show("Telefone não pode estar vazio");
                return;
            }

            
            if (!long.TryParse(novoNumeroTexto, out long novoNumero))
            {
                MessageBox.Show("Telefone deve conter apenas números");
                return;
            }

            if (novoNumero <= 0)
            {
                MessageBox.Show("Telefone deve ser maior que zero");
                return;
            }

            string numeroStr = novoNumero.ToString();
            if (numeroStr.Length < 10 || numeroStr.Length > 12)
            {
                MessageBox.Show("Telefone inválido! Deve ter entre 10 e 12 dígitos");
                return;
            }

            if (cadastro.Put(cadastroExistente.Id, novoNome, novoNumero))
            {
                MessageBox.Show("Cadastro atualizado com sucesso!");
                textBox1.Clear();
                textBox2.Clear();
                CarregarDados();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string nome = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("O nome não pode estar vazio");
                return;
            }

            string numeroTexto = textBox2.Text.Trim();
            if (string.IsNullOrWhiteSpace(numeroTexto))
            {
                MessageBox.Show("O telefone não pode estar vazio");
                return;
            }

            
            if (!long.TryParse(numeroTexto, out long numero))
            {
                MessageBox.Show("Telefone deve conter apenas números");
                return;
            }

            
            string numeroStr = numero.ToString();
            if (numeroStr.Length < 10 || numeroStr.Length > 12)
            {
                MessageBox.Show("Telefone deve ter entre 10 e 12 números");
                return;
            }

            if (numero <= 0)
            {
                MessageBox.Show("Telefone deve ser maior que zero");
                return;
            }

            try
            {
                Cadastro novoCadastro = cadastro.Post(nome, numero);
                if (novoCadastro != null)
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    MessageBox.Show("Cadastro criado com sucesso!");
                    CarregarDados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar: {ex.Message}");
            }
        }

        
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
    }
}