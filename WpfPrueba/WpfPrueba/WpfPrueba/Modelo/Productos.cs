using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrueba.Global;
using System.Data;

namespace WpfPrueba.Modelo
{
    public class Productos : Base{
        public Productos(Conexion cn = null) : base(cn) { }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Valor { get; set; }
        public int Cantidad { get; set; }

        public List<Productos> GetDatos(){
            List<Productos> _Productos = null;
            DataSet ds = cx.Obtener("SP_CONS_PRODUCTOS");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0){
                _Productos = new List<Productos>();
                DataTable dt = ds.Tables[0];
                var list = from i in dt.AsEnumerable()
                           select new Productos{
                               Codigo = i.Field<string>("Codigo"),
                               Nombre = i.Field<string>("Nombre"),
                               Valor = i.Field<int>("Valor"),
                               Cantidad = i.Field<int>("Cantidad")
                           };
                _Productos = list.ToList();
            }
            return _Productos;
        }
    }
}
