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
        public int id { get; set; }
        public DateTime data { get; set; }
        public string cliente { get; set; }
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
                c.id = (int)dr["Id"];
                c.data = (DateTime)dr["data_venda"];
                c.cliente = dr["nome"].ToString();
                c.total = (decimal)dr["total"];
                li.Add(c);
            }
            dr.Close();
            con.Close();
            return li;
        }

        public void pagamento (int id)
        {
            string sql = "UPDATE venda SET situacao = 'Finalizado' WHERE Id='" + id + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public List<Venda> localiza (int id)
        {
            List<Venda> lo = new List<Venda>();
            string sql = "SELECT v.Id, v.data_venda, c.nome, v.total FROM venda v join cliente c on c.Id = v.id_cliente WHERE v.situacao = 'Aberta' and v.Id = '" + id + "';\r\n";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Venda c = new Venda();
                c.id = (int)dr["Id"];
                c.data = (DateTime)dr["data_venda"];
                c.cliente = dr["nome"].ToString();
                c.total = (decimal)dr["total"];
                lo.Add(c);
            }
            dr.Close();
            con.Close();
            return lo;

        }
    }
}
