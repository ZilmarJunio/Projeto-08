﻿using System;
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

                    /* Início do script */
                    "INSERT INTO pessoa(id, nome, cpf, endereco, telefones)" +
                    "VALUES('" + setID + "','" + pessoa.nome + "','" + pessoa.cpf + "','" + setID + "','"+ setID + "');" +

                    "INSERT INTO endereco(id, logradouro, numero, cep, bairro, cidade, estado)" +
                    "VALUES('" + setID + "','" + end_Pessoa.logradouro + "','" + end_Pessoa.numero + "','" + end_Pessoa.cep +
                    "','" + end_Pessoa.bairro + "','" + end_Pessoa.cidade + "','" + end_Pessoa.estado + "');" +

                    "INSERT INTO telefone(id, numero, ddd, tipo)" +
                    "VALUES('" + setID + "','" + telefone_pessoa.numero + "','" + telefone_pessoa.ddd + "','" + tipotelefone_Pessoa.id_telefone + "');"+
                    "INSERT INTO pessoa_telefone(id_pessoa, id_telefone)" +
                    "VALUES('"+ setID + "','"+ setID + "');";
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
                "SELECT id, nome, cpf FROM pessoa;" +
                "SELECT numero, ddd, tipo FROM telefone;" +
                "SELECT logradouro, numero, cep, bairro, cidade, estado tipo FROM endereco;";
                /* Final do script de exclusão de dados*/

                MySqlCommand EnviarComando = new MySqlCommand(sql_script, Conexao_Com_DB);

                Conexao_Com_DB.Open();

                MySqlDataReader reader = EnviarComando.ExecuteReader();

                listView1.Items.Clear();

                while(reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
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
