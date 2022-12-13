using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace geekStore
{
    public partial class FrmVenda : Form
    {

        public FrmVenda()
        {
            InitializeComponent();
        }

        private void FrmVenda_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
            Venda venda = new Venda();
            List<Venda> vendas = venda.listavenda();
            dgvVenda.DataSource = vendas;
            btnConfirmaPagamento.Enabled = false;
            btnMostraTudo.Enabled = false;

        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNovoPedido_Click(object sender, EventArgs e)
        {
            FrmPedido pedido = new FrmPedido();
            pedido.Show();
            this.Close();
        }
        private void dgvVenda_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvVenda.Rows[e.RowIndex];
                row.Selected = true;
                txtId.Text = row.Cells[0].Value.ToString();
                btnConfirmaPagamento.Enabled = true;
            }
        }

        private void btnConfirmaPagamento_Click(object sender, EventArgs e)
        {
            try
            {
                int Id = Convert.ToInt32(txtId.Text.Trim());
                Venda venda = new Venda();
                venda.pagamento(Id);
                MessageBox.Show("Pagamento realizado com sucesso!", "Pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
                List<Venda> vendas = venda.listavenda();
                dgvVenda.DataSource = vendas;
                txtId.Text = "";
                btnConfirmaPagamento.Enabled = false;

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Erro!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLocaliza_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtId.Text.Trim());
            Venda venda = new Venda();
            List<Venda> vendas = venda.localiza(id);
            dgvVenda.DataSource = vendas;
            btnConfirmaPagamento.Enabled = true;
            btnMostraTudo.Enabled = true;
        }

        private void btnMostraTudo_Click(object sender, EventArgs e)
        {
            Venda venda = new Venda();
            List<Venda> vendas = venda.listavenda();
            dgvVenda.DataSource = vendas;
            btnConfirmaPagamento.Enabled = false;
            btnMostraTudo.Enabled = false;
        }
    }
}
