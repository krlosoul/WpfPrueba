using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace WpfPrueba.Global
{
    public class Conexion
    {

        private SqlConnection cnn;

        public Conexion(){
            var  CadenaConexion = ConfigurationManager.ConnectionStrings["CnString"];
            cnn = new SqlConnection(CadenaConexion.ConnectionString);
        }

        public void Ejecutar(String procedimiento, params IDataParameter[] parametros){
            SqlCommand cmd = new SqlCommand();
            cnn.Open();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedimiento;
            if (parametros != null)
            {
                for (int i = 0; i < parametros.Length; i++)
                {
                    cmd.Parameters.Add(parametros[i]);
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Se ha presentado un error al realizar la operación con la base de datos:\r\n" + ex.Message);
            }
        }

        public DataSet Obtener(String procedimiento, params IDataParameter[] parametros){
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adaptador = new SqlDataAdapter();
            DataSet dst = new DataSet();
            try
            {
                cnn.Open();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procedimiento;
                if (parametros != null)
                {
                    for (int i = 0; i < parametros.Length; i++)
                    {
                        cmd.Parameters.Add(parametros[i]);
                    }
                }
                adaptador.SelectCommand = cmd;
                adaptador.Fill(dst);
                cnn.Close();
                return dst;
            }
            catch (Exception ex)
            {
                throw new Exception("Se ha presentado un error al realizar la operación con la base de datos:\r\n" + ex.Message);
            }
        }

        public DbParameter Parametros(String clave, object valor, DbType tipo){
            SqlParameter param = new SqlParameter();
            param.DbType = tipo;
            param.Value = valor;
            param.ParameterName = clave;
            return param;
        }

    }
}
