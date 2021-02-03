using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrueba.Global;
using System.Data;

namespace WpfPrueba.Modelo
{
    public class Clientes : Base{
        public Clientes(Conexion cn = null) : base(cn) { }

        public string Cedula {get;set;}
        public string Nombre {get;set;}
        public string Apellido {get;set;}
        public string Direccion {get;set;}
        public string Telefono {get;set;}

        public List<Clientes> GetDatos()
        {
            List<Clientes> _Clientes = null;
            DataSet ds = cx.Obtener("SP_CONS_CLIENTE");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                _Clientes = new List<Clientes>();
                DataTable dt = ds.Tables[0];
                var list = from i in dt.AsEnumerable()
                           select new Clientes
                           {
                               Cedula = i.Field<string>("Cedula"),
                               Nombre = i.Field<string>("Nombre"),
                               Apellido = i.Field<string>("Apellido"),
                               Direccion = i.Field<string>("Direccion"),
                               Telefono = i.Field<string>("Telefono")
                           };
                _Clientes = list.ToList();
            }
            return _Clientes;
        }
    }
}
