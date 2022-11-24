using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace WindowsFormsPIMVIII
{
   
    public partial class Form1 : Form
    {

        MySqlConnection Conexao_Com_DB;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Bem vindo ao sistema da HighTech", "HighTech");
        }
        class PessoaDAO
        {
            private string nome;
            public string Nome
            {
                get { return nome; }
                set { nome = value; }
            }
            private string senha;
            public string Senha
            {
                get { return senha; }
                set { senha = value; }
            }
        }
        class Endereco
        {
            public string logradouro;
            public int numero;
            public int cep;
            public string bairro;
            public string cidade;
            public string estado;
            /*public Endereco (string logradouro, int numero, int cep, string bairro, string cidade, string estado)
            {
             this.logradouro = logradouro;
             this.numero = numero;
             this.cep = cep;
             this.bairro = bairro;
             this.cidade = cidade;
             this.estado = estado;
            }*/
        }
        class TipoTelefone
        {
            //public int id
            public int id_telefone;
            //public string tipo; - Definido no Banco de dados - (1) Telefone e (2) Celular
        }
        class Telefone
        {
            //public int id;
            public int numero;
            public int ddd;
            public TipoTelefone tipo;
            public Telefone(TipoTelefone tipo)
            {
                this.tipo = tipo;
            }
        }
        class Pessoa
        {
            //public int id;
            public string nome;
            public long cpf;
            public Endereco endereco;
            public Telefone telefone;
            public Pessoa(Endereco endereco, Telefone telefone)
            {
                this.endereco = endereco;
                this.telefone = telefone;
            }
        }
        private void inserinfo_Click(object sender, EventArgs e)
        {
            /*Criação de novo objeto Pessoa*/
            Endereco end_Pessoa = new Endereco();
            TipoTelefone tipotelefone_Pessoa = new TipoTelefone();
            Telefone telefone_pessoa = new Telefone(tipotelefone_Pessoa);
            Pessoa pessoa = new Pessoa(end_Pessoa, telefone_pessoa);

            /* Definindo Pessoa */
            pessoa.nome = txtNome.Text;
            pessoa.cpf = Int64.Parse(txtCPF.Text );
            /* Definindo Endereço da Pessoa */
            end_Pessoa.logradouro = txtLogradouro.Text;
            end_Pessoa.numero = Int32.Parse(txtNumero.Text);
            end_Pessoa.cep = Int32.Parse(txtCEP.Text);
            end_Pessoa.bairro = txtBairro.Text;
            end_Pessoa.cidade = txtCidade.Text;
            end_Pessoa.estado = txtEstado.Text;
            /* Definindo Telefone da Pessoa */
            telefone_pessoa.ddd = Int32.Parse(txtNumeroTelefone.Text);
            telefone_pessoa.numero = Int32.Parse(txtDDD.Text);

            try
            {
                /* Definindo Fonte de Dados (Banco de dados) */
                string data_source = "datasource=localhost;username=root;password=;database=conexao";
                /* Estabelecendo nova conexão (Banco de dados) */
                Conexao_Com_DB = new MySqlConnection(data_source);

                tipotelefone_Pessoa.id_telefone = Check_telefone_tipo();

                string sql_script =

                    /* Início do script */
                    "INSERT INTO pessoa(nome, cpf)" +
                    "VALUES('" + pessoa.nome + "','" + pessoa.cpf + "');" +

                    "INSERT INTO endereco(logradouro, numero, cep, bairro, cidade, estado)" +
                    "VALUES('" + end_Pessoa.logradouro + "','" + end_Pessoa.numero + "','" + end_Pessoa.cep +
                    "','" + end_Pessoa.bairro + "','" + end_Pessoa.cidade + "','" + end_Pessoa.estado + "');" +

                    "INSERT INTO telefone(numero, ddd, tipo)" +
                    "VALUES('" + telefone_pessoa.numero + "','" + telefone_pessoa.ddd + "','" + tipotelefone_Pessoa.id_telefone + "');";
                    /* Final do script */

                MySqlCommand EnviarComando = new MySqlCommand(sql_script, Conexao_Com_DB);
                MySqlCommand Info_Leitura = EnviarComando;

                Conexao_Com_DB.Open();

                Info_Leitura.ExecuteReader();

                MessageBox.Show("Sucesso na operação [Op 1 - Inserir dados].");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Conexao_Com_DB.Close();
            }
        }
        private void atuinfo_Click(object sender, EventArgs e)
        {

        }
        private void excluinfo_Click(object sender, EventArgs e)
        {
            try
            {
                /* Definindo Fonte de Dados (Banco de dados) */
                string data_source = "datasource=localhost;username=root;password=;database=conexao";
                /* Estabelecendo nova conexão (Banco de dados) */
                Conexao_Com_DB = new MySqlConnection(data_source);

                int id_remover = Int32.Parse(textIDRemover.Text);

                string sql_script =

                    /* Início do script de exclusão de dados*/
                    "DELETE FROM pessoa WHERE ID=" + id_remover + ";" +
                    "DELETE FROM endereco WHERE ID=" + id_remover + ";" +
                    "DELETE FROM pessoa_telefone WHERE ID=" + id_remover + ";" +
                    "DELETE FROM telefone WHERE ID=" + id_remover + ";";
                    /* Final do script de exclusão de dados*/

                MySqlCommand EnviarComando = new MySqlCommand(sql_script, Conexao_Com_DB);
                MySqlCommand Info_Leitura = EnviarComando;

                Conexao_Com_DB.Open();

                Info_Leitura.ExecuteReader();

                MessageBox.Show("Sucesso na operação [Op 3 - Excluir dados].");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Conexao_Com_DB.Close();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private int Check_telefone_tipo()
        {

            if ((radioTelefone.Checked == false) && (radioCelular.Checked == true))
            {
                return 1;
            }
            else if ((radioTelefone.Checked == true) && (radioCelular.Checked = false))
            {
                return 2;
            }
            else
            {
                MessageBox.Show("Você deve selecionar o tipo de telefone.");
            }
            return 0;
        }

    }

}
