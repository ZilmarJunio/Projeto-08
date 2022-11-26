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
            listView1.View = View.Details;
            listView1.LabelEdit = true;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Add("ID", 30, HorizontalAlignment.Left);
            listView1.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("CPF", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("ID_Tel", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("Número", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("DDD", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("Tipo_Tel", 70, HorizontalAlignment.Left);

            listView1.Columns.Add("ID_Endereço", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("Logradouro", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("Número", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("CEP", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("Bairro", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("Cidade", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("Estado", 60, HorizontalAlignment.Left);

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
            pessoa.cpf = Int64.Parse(txtCPF.Text);
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

                string sql_script_lastID =
                /* Início do script de exclusão de dados*/
                "SELECT LAST_INSERT_ID();";
                /* Final do script de exclusão de dados*/

                Conexao_Com_DB.Open();

                MySqlCommand EnviarComando_lastID = new MySqlCommand(sql_script_lastID, Conexao_Com_DB);

                MySqlDataReader reader = EnviarComando_lastID.ExecuteReader();

                int lastID=1;

                while (reader.Read())
                {
                    lastID = reader.GetInt16(0);
                    lastID++;
                }

                string sql_script =

                    /* Início do script */
                    "INSERT INTO pessoa(id, nome, cpf, endereco, telefones)" +
                    "VALUES('" + lastID + "','" + pessoa.nome + "','" + pessoa.cpf + "','" + lastID + "','"+ lastID + "');" +

                    "INSERT INTO endereco(id, logradouro, numero, cep, bairro, cidade, estado)" +
                    "VALUES('" + lastID + "','" + end_Pessoa.logradouro + "','" + end_Pessoa.numero + "','" + end_Pessoa.cep +
                    "','" + end_Pessoa.bairro + "','" + end_Pessoa.cidade + "','" + end_Pessoa.estado + "');" +

                    "INSERT INTO telefone(id, numero, ddd, tipo)" +
                    "VALUES('" + lastID + "','" + telefone_pessoa.numero + "','" + telefone_pessoa.ddd + "','" + 1 + "');"+
                    "INSERT INTO pessoa_telefone(id_pessoa, id_telefone)" +
                    "VALUES('"+ lastID + "','"+ lastID + "');";
                /* Final do script */

                reader.Close();

                MySqlCommand EnviarComando = new MySqlCommand(sql_script, Conexao_Com_DB);
                MySqlCommand Info_Leitura = EnviarComando;

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
                    "DELETE FROM endereco WHERE ID=" + id_remover + ";" +
                    "DELETE FROM telefone WHERE ID=" + id_remover + ";" +
                    "DELETE FROM pessoa WHERE ID=" + id_remover + ";" +
                    "DELETE FROM pessoa_telefone WHERE id_pessoa=" + id_remover + " and id_telefone=" + id_remover + ";";
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

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                /* Definindo Fonte de Dados (Banco de dados) */
                string data_source = "datasource=localhost;username=root;password=;database=conexao";
                /* Estabelecendo nova conexão (Banco de dados) */
                Conexao_Com_DB = new MySqlConnection(data_source);

                string sql_script =

                /* Início do script de exclusão de dados*/
                "SELECT * FROM pessoa;" +
                "SELECT * FROM telefone;"+
                "SELECT * FROM endereco;";
                /* Final do script de exclusão de dados*/

                Conexao_Com_DB.Open();

                MySqlCommand EnviarComando = new MySqlCommand(sql_script, Conexao_Com_DB);

                MySqlDataReader reader = EnviarComando.ExecuteReader();

                listView1.Items.Clear();

                while(reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetString(8),
                        reader.GetString(9),
                        reader.GetString(10),
                        reader.GetString(11),
                        reader.GetString(12),
                        reader.GetString(13),
                        reader.GetString(14),
                    };
                    var linha_listView1 = new ListViewItem(row);
                    listView1.Items.Add(linha_listView1);
                }

                MessageBox.Show("Sucesso na operação [Op 4 - Exibir Dados].");

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
    }

}
