using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrueba.Global;
using System.Data;

namespace WpfPrueba.Modelo
{
    public class Facturas : Base{
        public Facturas(Conexion cn = null) : base(cn) { }

        #region LlenarGridDetalle
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public int Unidad { get; set; }
        public int Total { get; set; }
        #endregion

        public int GetIdCabecera(string IdCliente){
            int IdCabecera = 0;
            DataSet ds = cx.Obtener("SP_SAVE_CABECERA",cx.Parametros("IdCliente",IdCliente,DbType.String));
            if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0){
                DataRow dr = ds.Tables[0].Rows[0];
                IdCabecera = Convert.ToInt32(dr.Field<object>("IdCabecera"));
            }
            return IdCabecera;
        }

        public void SetDetalle(int IdCabecera, string IdProducto, int Cantidad, int ValorUnidad, int ValorTotal)
        {
            cx.Ejecutar("SP_SAVE_DETALLE",
                cx.Parametros("IdCabecera", IdCabecera, DbType.Int64),
                cx.Parametros("IdProducto",IdProducto,DbType.String),
                cx.Parametros("Cantidad",Cantidad,DbType.Int32),
                cx.Parametros("ValorUnidad", ValorUnidad, DbType.Int32),
                cx.Parametros("ValorTotal", ValorTotal, DbType.Int32));        
        }
    }
}