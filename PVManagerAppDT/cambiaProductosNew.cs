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
    public partial class cambiaProductosNew : Form
    {
        AppDTEntities db = new AppDTEntities();
        int idTicket;
        int idProducto;

        public cambiaProductosNew(int _idProducto, int _idTicket)
        {
            idTicket = _idTicket;
            idProducto = _idProducto;
            InitializeComponent();
            var reader = db.vis_fillProduct.Where(a => a.Ticket_Id == idTicket && a.Prod_Id == idProducto).ToList();
            txtProducto.Text = reader[0].Prod_Desc;
            txtCantidad.Text = reader[0].Venta_Cantidad.ToString();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            cambiaCantidad();
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                var itemToRemove = db.VENTASTICKET_PV.SingleOrDefault(x => x.Ticket_Id == idTicket && x.Prod_Id== idProducto); //returns a single item.

                if (itemToRemove != null)
                {
                    db.VENTASTICKET_PV.Remove(itemToRemove);
                    db.SaveChanges();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cambiaCantidad()
        {
            try
            {
                //var itemToRemove = db.VENTASTICKET_PV.SingleOrDefault(x => x.Ticket_Id == idTicket && x.Prod_Id == idProducto); //returns a single item.
                //if (itemToRemove != null)
                //{
                //    var entity = new VENTASTICKET_PV {
                //        Venta_Cantidad= Convert.ToDecimal(txtCantidad.Text)
                //};

                //    db.VENTASTICKET_PV.Attach(entity);
                //    db.SaveChanges();
                //}

                ///

                VENTASTICKET_PV stud;
                using (var ctx = new AppDTEntities())
                {
                    stud = ctx.VENTASTICKET_PV.Where(s => s.Ticket_Id == idTicket && s.Prod_Id == idProducto).FirstOrDefault<VENTASTICKET_PV>();
                }

                //2. change student name in disconnected mode (out of ctx scope)
                if (stud != null)
                {
                    stud.Venta_Cantidad = Convert.ToDecimal(txtCantidad.Text);
                }
                this.Close();
            }
            catch (Exception es)
            {
                lblError.Visible = true;
            }
        }
    }
}
