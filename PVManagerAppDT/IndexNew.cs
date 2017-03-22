using PVManagerAppDT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVManagerAppDT
{
    public partial class IndexNew : Form
    {
        AppDTEntities db = new AppDTEntities();
        public IndexNew()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTienda_Click(object sender, EventArgs e)
        {
            if (this.btnTienda.BackColor == Color.Orange)
            {
                var context = new AppDTEntities();

                var CrearTicket = new TICKETS_PV //Make sure you have a table called test in DB
                {
                    Ticket_Subtotal = 0,
                    Ticket_Factura = 0,
                    Ticket_Date = DateTime.Now,
                    Sucu_Id=1,
                    Ticket_Status= "abierto"
                };

                context.TICKETS_PV.Add(CrearTicket);
                context.SaveChanges();

                var idTicket = db.TICKETS_PV.Max(t => t.Ticket_Id);
                
                //RegistroVentaNew objVino = new RegistroVentaNew(idTicket);
                Panel objVino = new Panel(idTicket);
                objVino.Show();

            }
            else
            {
                //RegistroVentaNew objVino = new RegistroVentaNew(int.Parse(lblTicketTienda.Text));
                Panel objVino = new Panel(int.Parse(lblTicketTienda.Text));
                objVino.Show();
            }
            this.Close();
        }

        private void IndexNew_Load(object sender, EventArgs e)
        {

            var max=0;
            try{
                max = db.TICKETS_PV.Max(a => a.Ticket_Id);
            }
            catch (Exception)
            {
                max = 1;
            }
           

            var Disponible = db.TICKETS_PV.Where(a => a.Ticket_Status == "abierto" && a.Ticket_Id == max).ToList();
               if (Disponible.Count() == 0)
            {
               

            }
               else if(Disponible[0].Ticket_Status == "abierto")
                {
                    btnTienda.BackColor = Color.YellowGreen;
                    lblTicketTienda.BackColor = Color.YellowGreen;
                    lblTicketTienda.Text = max.ToString();
                }
        }
    }
}
