using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            Venda venda = new Venda();
            List<Venda> vendas = venda.listavenda();
            dgvVenda.DataSource = vendas;
            btnFinalizaVenda.Enabled = false;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNovoPedido_Click(object sender, EventArgs e)
        {
            FrmPedido pedido = new FrmPedido();
            pedido.Show();
        }

        private void btnFinalizaVenda_Click(object sender, EventArgs e)
        {

        }

        private void dgvVenda_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvVenda.Rows[e.RowIndex];
                row.Selected = true;
                txtId.Text = row.Cells[0].Value.ToString();
                btnFinalizaVenda.Enabled = true;
            }
        }
    }
}
