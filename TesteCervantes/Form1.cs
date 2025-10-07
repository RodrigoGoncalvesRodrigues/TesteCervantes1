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
            
         
                List<Cadastro> todosCadastros = cadastro.GetAll();

                

                string mensagem = "Cadastros:\n\n";
                foreach (var item in todosCadastros)
                {
                    mensagem += $"ID: {item.Id} | Nome: {item.Nome} | Número: {item.Numero}\n";
                }

                MessageBox.Show(mensagem, "Lista de Cadastros");
            
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string numeroTexto = textBox2.Text.Trim();
            string nome = textBox1.Text.Trim();

            

            
            if (!long.TryParse(numeroTexto, out long numero))
            {
            
                return;
            }

           

            var cadastroParaDeletar = cadastro.GetByNomeNumero(nome, numero);
    

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

            

            
            if (!long.TryParse(numeroTexto, out long numero))
            {
                
                return;
            }

            var cadastroExistente = cadastro.GetByNomeNumero(nome, numero);
            if (cadastroExistente == null)
            {
                
                return;
            }

            string novoNome = Microsoft.VisualBasic.Interaction.InputBox(
                "Digite o novo nome:", "Atualizar Nome", nome);

            

            string novoNumeroTexto = Microsoft.VisualBasic.Interaction.InputBox(
                "Digite o novo telefone:", "Atualizar Telefone", numero.ToString());

            

            
            if (!long.TryParse(novoNumeroTexto, out long novoNumero))
            {
                
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
            

            string numeroTexto = textBox2.Text.Trim();
            

            
            if (!long.TryParse(numeroTexto, out long numero))
            {
                
                return;
            }

          
            string numeroStr = numero.ToString();
        

            
                Cadastro novoCadastro = cadastro.Post(nome, numero);
                if (novoCadastro != null)
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    MessageBox.Show("Cadastro criado com sucesso!");
                    CarregarDados();
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