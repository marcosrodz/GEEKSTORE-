﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace geekStore
{
    public partial class FrmPedido : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\PROJETOS\geek\geekStore\DbGeek.mdf;Integrated Security=True");

        public FrmPedido()
        {
            InitializeComponent();
        }
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void CarregacomboBoxCliente()
        {
            string cli = "SELECT id, nome FROM Cliente";
            SqlCommand cmd = new SqlCommand(cli, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cli, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "cliente");
            cbxCliente.ValueMember = "Id";
            cbxCliente.DisplayMember = "nome";
            cbxCliente.DataSource = ds.Tables["cliente"];
            con.Close();
        }

        public void CarregaCbxProduto()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            string pro = "SELECT Id, nome from Produto";
            SqlCommand cmd = new SqlCommand(pro, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(pro, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "produto");
            cbxProduto.ValueMember = "Id";
            cbxProduto.DisplayMember = "nome";
            cbxProduto.DataSource = ds.Tables["produto"];
            con.Close();
        }

        private void FrmPedido_Load(object sender, EventArgs e)
        {
            lblEstoque.BackColor = Color.Transparent;
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            CarregacomboBoxCliente();
            cbxProduto.Enabled = false;
            txtIdProduto.Enabled = false;
            txtQuantidade.Enabled = false;
            txtValor.Enabled = false;
            btnFinalizar.Enabled = false;
            btnConfirmaItem.Enabled = false;
            btnNovoItem.Enabled = false;
            btnExcluirItem.Enabled = false;

        }

        private void btnNovoPedido_Click(object sender, EventArgs e)
        {
            cbxProduto.Enabled = true;
            CarregaCbxProduto();
            txtIdProduto.Enabled = true;
            txtQuantidade.Enabled = true;
            txtValor.Enabled = true;
            btnConfirmaItem.Enabled = true;
            dgvPedido.Columns.Add("ID", "ID");
            dgvPedido.Columns.Add("Produto", "Produto");
            dgvPedido.Columns.Add("Quantidade", "Quantidade");
            dgvPedido.Columns.Add("Valor", "Valor");
            dgvPedido.Columns.Add("Total", "Total");

        }

        private void cbxProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            SqlCommand cmd = new SqlCommand("SELECT * FROM Produto Where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", cbxProduto.SelectedValue);
            cmd.CommandType = CommandType.Text;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtValor.Text = dr["preco"].ToString();
                txtIdProduto.Text = dr["Id"].ToString();
                txtQuantidade.Focus();
                dr.Close();
                con.Close();
            }
            con.Open();
            SqlCommand cmd2 = new SqlCommand("Quantidade_Produto", con);
            cmd2.Parameters.AddWithValue("@Id", txtIdProduto.Text);
            cmd2.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                lblEstoque.Text = dr2["quantidade"].ToString();
                dr2.Close();
                con.Close();
            }
        }

        private void btnConfirmaItem_Click(object sender, EventArgs e)
        {
            var repetido = false;
            foreach (DataGridViewRow dr in dgvPedido.Rows)
            {
                if (txtIdProduto.Text == Convert.ToString(dr.Cells[0].Value))
                {
                    repetido = true;
                }
            }
            if (repetido == false)
            {
                DataGridViewRow item = new DataGridViewRow();
                item.CreateCells(dgvPedido);
                item.Cells[0].Value = txtIdProduto.Text;
                item.Cells[1].Value = cbxProduto.Text;
                item.Cells[2].Value = txtQuantidade.Text;
                item.Cells[3].Value = txtValor.Text;
                item.Cells[4].Value = Convert.ToDecimal(txtValor.Text) * Convert.ToDecimal(txtQuantidade.Text);
                dgvPedido.Rows.Add(item);
                decimal soma = 0;
                foreach (DataGridViewRow dr in dgvPedido.Rows)
                {
                    soma += Convert.ToDecimal(dr.Cells[4].Value);
                    txtTotal.Text = Convert.ToString(soma);
                }
                btnFinalizar.Enabled = true;
                btnNovoItem.Enabled = true;
            }
            else
            {
                MessageBox.Show("Produto já cadastrado!", "Produto repetido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNovoItem_Click(object sender, EventArgs e)
        {
            txtIdProduto.Text = "";
            txtValor.Text = "";
            txtQuantidade.Text = "";
            cbxCliente.Text = "";
            btnNovoItem.Enabled = false;
        }

        private void dgvPedido_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dgvPedido.Rows[e.RowIndex];
            cbxProduto.Text = row.Cells[1].Value.ToString();
            txtIdProduto.Text = row.Cells[0].Value.ToString();
            txtQuantidade.Text = row.Cells[2].Value.ToString();
            txtValor.Text = row.Cells[3].Value.ToString();
            btnExcluirItem.Enabled = true;
        }

        private void btnExcluirItem_Click(object sender, EventArgs e)
        {
            int linha = dgvPedido.CurrentRow.Index;
            dgvPedido.Rows.RemoveAt(linha);
            dgvPedido.Refresh();
            cbxProduto.Text = "";
            txtIdProduto.Text = "";
            txtQuantidade.Text = "";
            txtValor.Text = "";
            decimal soma = 0;
            foreach (DataGridViewRow dr in dgvPedido.Rows)
                soma += Convert.ToDecimal(dr.Cells[4].Value);
            txtTotal.Text = Convert.ToString(soma);
        }

        private void txtQuantidade_Leave(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Quantidade_Produto", con);
            cmd.Parameters.AddWithValue("@Id", txtIdProduto.Text);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            int valor1 = 0;
            bool conversaoSucedida = int.TryParse(txtQuantidade.Text, out valor1);
            if (dr.Read())
            {
                int valor2 = Convert.ToInt32(dr["quantidade"].ToString());
                if (valor1 > valor2)
                {
                    MessageBox.Show("Não tem quantidade suficiente!", "Estoque insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQuantidade.Text = "";
                    txtQuantidade.Focus();
                }
            }
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("InserirVenda", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id_pessoa", SqlDbType.NChar).Value = cbxCliente.SelectedValue;
            cmd.Parameters.AddWithValue("@total", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTotal.Text);
            cmd.Parameters.AddWithValue("@data_venda", SqlDbType.Date).Value = DateTime.Now;
            cmd.Parameters.AddWithValue("@situacao", SqlDbType.NChar).Value = "Aberta";
            cmd.ExecuteNonQuery();
            string idvenda = "SELECT IDENT_CURRENT('Venda') AS id_venda";
            SqlCommand cmd2 = new SqlCommand(idvenda, con);
            Int32 idvenda2 = Convert.ToInt32(cmd2.ExecuteScalar());
            foreach (DataGridViewRow dr in dgvPedido.Rows)
            {
                SqlCommand cmditens = new SqlCommand("InserirItensPedidos", con);
                cmditens.CommandType = CommandType.StoredProcedure;
                cmditens.Parameters.AddWithValue("@id_venda", SqlDbType.Int).Value = idvenda2;
                cmditens.Parameters.AddWithValue("@id_produto", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[0].Value);
                cmditens.Parameters.AddWithValue("@quantidade", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[2].Value);
                cmditens.Parameters.AddWithValue("@valor_unitario", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[3].Value);
                cmditens.Parameters.AddWithValue("@valor_total", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[4].Value);
                cmditens.ExecuteNonQuery();
            }
            con.Close();
            dgvPedido.Rows.Clear();
            dgvPedido.Refresh();
            txtTotal.Text = "";
            MessageBox.Show("Pedido realizado com sucesso!", "Pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnVoltar_Click_1(object sender, EventArgs e)
        {
            FrmVenda venda = new FrmVenda();
            venda.Show();
            this.Close();
        }

    }
}
