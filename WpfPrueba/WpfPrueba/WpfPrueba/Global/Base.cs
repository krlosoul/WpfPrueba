using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrueba.Global{
    public class Base{
        
        private Conexion _cx;

        protected Conexion cx{
            get { return _cx; }
        }

        protected Base(Conexion conexion){
            _cx = conexion == null ? new Conexion() : conexion;
        }
    }
}
