using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace geekStore
{
    class Venda
    {
        public int idVenda { get; set; }
        public DateTime data { get; set; }
        public string nomeCliente { get; set; }
        public decimal total { get; set; }


        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\PROJETOS\geek\geekStore\DbGeek.mdf;Integrated Security=True");

        public List<Venda> listavenda()
        {
            List<Venda> li = new List<Venda>();
            string sql = "SELECT v.Id, v.data_venda, c.nome, v.total FROM venda v join cliente c on c.Id = v.id_cliente WHERE v.situacao = 'Aberta';\r\n";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Venda c = new Venda();
                c.idVenda = (int)dr["Id"];
                c.data = (DateTime)dr["data_venda"];
                c.nomeCliente = dr["nome"].ToString();
                c.total = (decimal)dr["total"];
                li.Add(c);
            }
            dr.Close();
            con.Close();
            return li;
        }
    }
}
