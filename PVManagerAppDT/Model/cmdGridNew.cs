using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVManagerAppDT.Model
{
    class cmdGridNew
    {
        private BindingSource dbind = new BindingSource();
        AppDTEntities db = new AppDTEntities();


        public BindingSource fillUsuarios()
        {
            //conexion.connect();
            var reader = db.USUARIOS_PV;
            dbind.DataSource = reader;
            //reader.Close();
            return (dbind);
        }

        public BindingSource fillTickets(int idTicket)
        {
            var reader = db.vis_fillTickets.Where(t => t.Ticket == idTicket).ToList();
            dbind.DataSource = reader;
            //reader.Close();
            return (dbind);
        }

        public BindingSource fillTicketsPaquete(int idTicket)
        {
           // var reader = db.vis_fillGridVEnta.Where(t => t.Ticket == idTicket).ToList();
            //dbind.DataSource = reader;
            //reader.Close();
            return (dbind);
        }


    }
}
