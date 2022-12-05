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
   /* PIM VIII - Concluído*/
    public partial class Form1 : Form
    {

        MySqlConnection Conexao_Com_DB;

        public Form1()
        {
            InitializeComponent();
            Refresh_pessoas();

            //Ajustes dos lists view
            listView1.View = View.Details; listView2.View = View.Details; listView3.View = View.Details;
            listView1.LabelEdit = true; listView2.LabelEdit = true; listView3.LabelEdit = true;
            listView1.AllowColumnReorder = true; listView2.AllowColumnReorder = true; listView3.AllowColumnReorder = true;
            listView1.FullRowSelect = true; listView2.FullRowSelect = true; listView3.FullRowSelect = true;
            listView1.GridLines = true; listView2.GridLines = true; listView3.GridLines = true;

            //View-1
            listView1.Columns.Add("ID", 30, HorizontalAlignment.Left);
            listView1.Columns.Add("Nome", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("CPF", 60, HorizontalAlignment.Left);
            //View-2
            listView2.Columns.Add("Número", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("DDD", 40, HorizontalAlignment.Left);
            listView2.Columns.Add("Tipo_Tel", 80, HorizontalAlignment.Left);
            //View-3
            listView3.Columns.Add("Logradouro", 100, HorizontalAlignment.Left);
            listView3.Columns.Add("Número", 80, HorizontalAlignment.Left);
            listView3.Columns.Add("CEP", 60, HorizontalAlignment.Left);
            listView3.Columns.Add("Bairro", 60, HorizontalAlignment.Left);
            listView3.Columns.Add("Cidade", 60, HorizontalAlignment.Left);
            listView3.Columns.Add("Estado", 60, HorizontalAlignment.Left);

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
            PessoaDAO user = new PessoaDAO();

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

                string sql_script_setID =
                /* Início do script de exclusão de dados*/
                    "SELECT MAX(id) as id FROM pessoa;";
                /* Final do script de exclusão de dados*/

                Conexao_Com_DB.Open();

                MySqlCommand EnviarComando_setID = new MySqlCommand(sql_script_setID, Conexao_Com_DB);

                MySqlDataReader reader = EnviarComando_setID.ExecuteReader();

                int setID = 0;

                try {
                    while (reader.Read())
                    {
                    setID = reader.GetInt16(0);
                    setID++;
                    }
                }
                catch
                {
                setID = 1;
                }

                string sql_script =

                    /* Início do script INSERIR INFO.*/
                    "INSERT INTO pessoa(id, nome, cpf, endereco, telefones)" +
                    "VALUES(" + setID + ",'" + pessoa.nome + "'," + pessoa.cpf + "," + setID + ","+ setID + ");" +

                    "INSERT INTO endereco(id, logradouro, numero, cep, bairro, cidade, estado)" +
                    "VALUES(" + setID + ",'" + end_Pessoa.logradouro + "'," + end_Pessoa.numero + "," + end_Pessoa.cep +
                    ",'" + end_Pessoa.bairro + "','" + end_Pessoa.cidade + "','" + end_Pessoa.estado + "');" +

                    "INSERT INTO telefone(id, numero, ddd, tipo)" +
                    "VALUES(" + setID + "," + telefone_pessoa.numero + "," + telefone_pessoa.ddd + "," + tipotelefone_Pessoa.id_telefone + ");"+
                    "INSERT INTO pessoa_telefone(id_pessoa, id_telefone)" +
                    "VALUES("+ setID + ","+ setID + ");";
                    /* Final do script INSERIR INFO. */

                reader.Close();

                MySqlCommand EnviarComando = new MySqlCommand(sql_script, Conexao_Com_DB);
                MySqlCommand Info_Leitura = EnviarComando;

                Info_Leitura.ExecuteReader();

                Refresh_pessoas();
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

                Conexao_Com_DB.Open();

                string sql_script =

                    /* Início do script ATUALIZAR INFO.*/
                    "UPDATE pessoa SET nome = '" + pessoa.nome + "', cpf = " + pessoa.cpf +
                    " WHERE id="+ textIDAtualizar.Text + ";" +

                    "UPDATE endereco SET logradouro = '" + end_Pessoa.logradouro + "', numero = " + end_Pessoa.numero + ", cep = " + end_Pessoa.cep +
                    ", bairro = '" + end_Pessoa.bairro + "', cidade = '" + end_Pessoa.cidade + "', estado = '" + end_Pessoa.estado +
                    "' WHERE id=" + textIDAtualizar.Text + ";" +

                    "UPDATE telefone SET numero = " + telefone_pessoa.numero + ", ddd = " + telefone_pessoa.ddd + ", tipo = '" + tipotelefone_Pessoa.id_telefone +
                    "' WHERE id=" + textIDAtualizar.Text + ";";
                    /* Final do script ATUALIZAR INFO.*/

                MySqlCommand EnviarComando = new MySqlCommand(sql_script, Conexao_Com_DB);
                MySqlCommand Info_Leitura = EnviarComando;

                Info_Leitura.ExecuteReader();

                Refresh_pessoas();
                MessageBox.Show("Sucesso na operação [Op 2 - Atualizar dados].");

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

                    /* Início do script EXCLUIR DADOS*/
                    "DELETE FROM endereco WHERE ID=" + id_remover + ";" +
                    "DELETE FROM telefone WHERE ID=" + id_remover + ";" +
                    "DELETE FROM pessoa WHERE ID=" + id_remover + ";" +
                    "DELETE FROM pessoa_telefone WHERE id_pessoa=" + id_remover + ";" +
                    "DELETE FROM pessoa_telefone WHERE id_telefone=" + id_remover + ";";
                    /* Final do script EXCLUIR DADOS*/

                MySqlCommand EnviarComando = new MySqlCommand(sql_script, Conexao_Com_DB);
                MySqlCommand Info_Leitura = EnviarComando;

                Conexao_Com_DB.Open();

                Info_Leitura.ExecuteReader();

                Refresh_pessoas();
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
        private int Check_telefone_tipo()
        {

            if ((radioCelular.Checked == true))
            {
                return 1;
            }
            if ((radioTelefone.Checked == true))
            {
                return 2;
            }
            else
            {
                MessageBox.Show("Você deve selecionar o tipo de telefone.");
            }
            return 2;
        }
        private void Refresh_pessoas()
        {
            try
            {
                /* Definindo Fonte de Dados (Banco de dados) */
                string data_source = "datasource=localhost;username=root;password=;database=conexao";
                /* Estabelecendo nova conexão (Banco de dados) */
                Conexao_Com_DB = new MySqlConnection(data_source);

                //Scripts de EXIBIR DADOS ->
                string sql_script_Pessoa =
                    "SELECT id, nome, cpf FROM pessoa;";

                string sql_script_Telefone =
                    "SELECT numero, ddd, tipo FROM telefone;";

                string sql_script_Endereco =
                    "SELECT logradouro, numero, cep, bairro, cidade, estado FROM endereco;";

                MySqlCommand EnviarComando = new MySqlCommand(sql_script_Pessoa, Conexao_Com_DB);
                MySqlCommand EnviarComando2 = new MySqlCommand(sql_script_Telefone, Conexao_Com_DB);
                MySqlCommand EnviarComando3 = new MySqlCommand(sql_script_Endereco, Conexao_Com_DB);

                Conexao_Com_DB.Open();

                MySqlDataReader reader_Pessoa = EnviarComando.ExecuteReader();
                listView1.Items.Clear();

                while (reader_Pessoa.Read())
                {
                    string[] row =
                    {
                        reader_Pessoa.GetString(0),
                        reader_Pessoa.GetString(1),
                        reader_Pessoa.GetString(2),
                    };

                    var linha_listView1 = new ListViewItem(row);
                    listView1.Items.Add(linha_listView1);

                }

                reader_Pessoa.Close();

                MySqlDataReader reader_Telefone = EnviarComando2.ExecuteReader();
                listView2.Items.Clear();

                while (reader_Telefone.Read())
                {
                    string[] row1 =
                      {
                        reader_Telefone.GetString(0),
                        reader_Telefone.GetString(1),
                        reader_Telefone.GetString(2),
                    };

                    var linha_listView2 = new ListViewItem(row1);
                    listView2.Items.Add(linha_listView2);
                }

                reader_Telefone.Close();

                MySqlDataReader reader_Endereco = EnviarComando3.ExecuteReader();
                listView3.Items.Clear();

                while (reader_Endereco.Read())
                {
                    string[] row2 =
                      {
                        reader_Endereco.GetString(0),
                        reader_Endereco.GetString(1),
                        reader_Endereco.GetString(2),
                        reader_Endereco.GetString(3),
                        reader_Endereco.GetString(4),
                        reader_Endereco.GetString(5),
                    };

                    var linha_listView3 = new ListViewItem(row2);
                    listView3.Items.Add(linha_listView3);
                }

                reader_Endereco.Close();

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
