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
    public partial class RegistroVentaNew : Form
    {
        AppDTEntities db = new AppDTEntities();
        cmdGridNew lee = new cmdGridNew();
        int idTicket;
        int idEmpleado;
        public RegistroVentaNew(int _idTicket)
        {
            idTicket = _idTicket;
            InitializeComponent();
            calculaTotal();
            if (lblTotal.Text != "0")
                gridVenta.DataSource = lee.fillTickets(idTicket);
        }

        private void btnVender_Click(object sender, EventArgs e)
        {
            if (lblTotal.Text != "0")
                gridVenta.DataSource = lee.fillTickets(idTicket);

            agregaAventa(txtid.Text, txtprecio.Text, "1");
            gridVenta.DataSource = lee.fillTickets(idTicket);
            calculaTotal();
        }

        void buscaTarget()
        {
            var producto = db.PRODUCTOS_PV.Where(a => a.Prod_Codigo == barcode.Text).FirstOrDefault();
            //reader = conexion.select("Select nombre,precio,idProducto from productos where codigoBarras ='" + barcode.Text + "' ");
          try
            {
                txtnombre.Text = producto.Prod_Desc.ToString();
                txtprecio.Text = producto.Prod_Price.ToString();
                txtid.Text = producto.Prod_Id.ToString();
            }
            catch (Exception) {
                MessageBox.Show("codigo no existe");
            }
           
        }

        void agregaAventa(string Producto, string Precio, string Cant)
        {
            int product = Convert.ToInt32(Producto);
            int precio = Convert.ToInt32(Precio);
            decimal cant = Convert.ToDecimal(Cant);
            try
            {
                var Agrega = new VENTASTICKET_PV {
                    Prod_Id= product,
                    Venta_Cantidad= cant,
                    Prod_Price= precio,
                    Venta_Importe=(cant * precio),
                    Ticket_Id= idTicket

                };
                db.VENTASTICKET_PV.Add(Agrega);
                db.SaveChanges();
            }
            catch (System.Exception)
            {
                MessageBox.Show("No hay productos listos para agregar");
            }
        }

        void calculaTotal()
        {
            
            var Total = db.VENTASTICKET_PV.Where(t => t.Ticket_Id == idTicket).ToList();
            int contador = 0;
            foreach (var tot in Total)
            {
                contador +=Convert.ToInt16(Total[0].Venta_Importe);
            }
                lblTotal.Text = contador.ToString();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            IndexNew objindex = new IndexNew();
            objindex.Show();
            this.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Deseas facturar esta venta?",
               "Alerta!", MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, 0, false)
               == DialogResult.Yes)
            {
                using (var ctx = new AppDTEntities())
                {
                    var ticket = (from s in ctx.TICKETS_PV
                                where s.Ticket_Id == idTicket
                                  select s).FirstOrDefault();

                    ticket.Ticket_Status = "terminado";
                    ticket.Ticket_Subtotal = Convert.ToInt32(lblTotal.Text);

                    int num = ctx.SaveChanges();
                }

                IndexNew objmesas = new IndexNew();
                objmesas.Show();
                this.Close();

                //datosFactura factura = new datosFactura(idTicket);
                //factura.Show();
                //btnimprime_Click(sender, e);
                //MessageBox.Show("Facturando");
            }
            else
            {
                using (var ctx = new AppDTEntities())
                {
                    var ticket = (from s in ctx.TICKETS_PV
                                  where s.Ticket_Id == idTicket
                                  select s).FirstOrDefault();

                    ticket.Ticket_Status = "terminado";
                    ticket.Ticket_Subtotal =Convert.ToInt32(lblTotal.Text);

                    int num = ctx.SaveChanges();
                }

                IndexNew objmesas = new IndexNew();
                objmesas.Show();
                // btnimprime_Click(sender, e);
                this.Close();
            }
        }

        private void btnimprime_Click(object sender, EventArgs e)
        {
            Ticket ticket = new Ticket();

            //ticket.HeaderImage = "C:\imagen.jpg"; //esta propiedad no es obligatoria

            ticket.AddHeaderLine("Buffet Espiritu S.");
            //ticket.AddHeaderLine("EXPEDIDO EN:");
            //ticket.AddHeaderLine("AV. TAMAULIPAS NO. 5 LOC. 101");
            //ticket.AddHeaderLine("MEXICO, DISTRITO FEDERAL");
            //ticket.AddHeaderLine("RFC: CSI-020226-MV4");

            //El metodo AddSubHeaderLine es lo mismo al de AddHeaderLine con la diferencia
            //de que al final de cada linea agrega una linea punteada "=========="
            ticket.AddHeaderLine("Ticket # " + idTicket + " ");
            ticket.AddHeaderLine("\n");
            //ticket.AddHeaderLine("Le atendió: Prueba");
            ticket.AddHeaderLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

            //El metodo AddItem requeire 3 parametros, el primero es cantidad, el segundo es la descripcion
            //del producto y el tercero es el precio
            //ticket.AddItem("1", "Articulo 1", "15.00");
            //ticket.AddItem("2", "Articulo 2", "25.00");
            ticket.AddHeaderLine("\n");
            ticket.AddHeaderLine("----------------------");
            ticket.AddHeaderLine("\n");
            ticket.AddHeaderLine("Cant.     Desc.     Importe");
            ticket.AddHeaderLine("\n");
            //ticket.AddHeaderLine("Articulo 2");
            var reader = db.vis_fillTicket.Where(a => a.Ticket_Id == idTicket).ToList();
            foreach(var tick in reader){
                ticket.AddHeaderLine(" " + tick.Venta_Cantidad + "      " + tick.Prod_Desc + "      $" + tick.Venta_Importe);
                ticket.AddHeaderLine("\n");
            }
           
            ticket.AddHeaderLine("\n");
            ticket.AddHeaderLine("----------------------");
            ticket.AddHeaderLine("\n");
            ticket.AddHeaderLine("TOTAL: $" + lblTotal.Text);
            //El metodo AddTotal requiere 2 parametros, la descripcion del total, y el precio
            //ticket.AddTotal("SUBTOTAL", "29.75" );
            //ticket.AddTotal("IVA", "5.25" );
            //ticket.AddTotal("TOTAL", lblTotal.Text);
            //ticket.AddTotal("", "" ); //Ponemos un total en blanco que sirve de espacio
            //ticket.AddTotal("RECIBIDO", "50.00" );
            //ticket.AddTotal("CAMBIO", "15.00" );
            //ticket.AddTotal("", "" );//Ponemos un total en blanco que sirve de espacio
            //ticket.AddTotal("USTED AHORRO", "0.00" );

            //El metodo AddFooterLine funciona igual que la cabecera
            //ticket.AddFooterLine("EL CAFE ES NUESTRA PASION...");
            //ticket.AddFooterLine("VIVE LA EXPERIENCIA EN STARBUCKS");
            ticket.AddFooterLine("GRACIAS POR TU VISITA");

            //Y por ultimo llamamos al metodo PrintTicket para imprimir el ticket, este metodo necesita un
            //parametro de tipo string que debe de ser el nombre de la impresora.



            //Ticket ticket = new Ticket();

            //            ticket.AddHeaderLine("Buffet Espiritu Santo ");
            //            //ticket.AddHeaderLine("Calle");
            //            //ticket.AddHeaderLine("Colonia");
            //            //ticket.AddHeaderLine("RFC: CSI-020226-MV4");
            //            ticket.AddSubHeaderLine("Ticket: " + idTicket + " ");
            //            ticket.AddSubHeaderLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());


            //            reader = conexion.select("SELECT ventasticket.cantidad,productos.nombre, " +
            //                                     "ventasticket.importe FROM     " +
            //                                     "ventasticket INNER JOIN productos " +
            //                                     "ON (ventasticket.idProducto = productos.idProducto) where ventasticket.idTicket=" + idTicket + ";");
            //            while (reader.Read())
            //            {
            //               ticket.AddItem(reader[0].ToString(),"", reader[2].ToString());
            //            }

            //            reader.Close();
            //            ticket.AddSubHeaderLine("Total:" + lblTotal.Text + " ");
            //            //ticket.AddTotal("Total:",lblTotal.Text);
            //            ticket.AddFooterLine("GRACIAS POR SU VISITA");
            //            //ticket.AddFooterLine("Dios lo bendice");

            try
            {
                ticket.PrintTicket("Generic");
                btnCerrar_Click(sender, e);
            }
            catch (System.Exception)
            {
                btnCerrar_Click(sender, e);
                MessageBox.Show("No se encontro la impresora");
            }
        }

        private void btnAdult_Click(object sender, EventArgs e)
        {
            barcode.Text = "201701";
            buscaTarget();
            agregaAventa(txtid.Text, txtprecio.Text, "1");
            gridVenta.DataSource = lee.fillTickets(idTicket);
            calculaTotal();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            barcode.Text = "201702";
            buscaTarget();
            agregaAventa(txtid.Text, txtprecio.Text, "1");
            gridVenta.DataSource = lee.fillTickets(idTicket);
            calculaTotal();
        }

        private void btnllevar_Click(object sender, EventArgs e)
        {
            barcode.Text = "201703";
            buscaTarget();
            agregaAventa(txtid.Text, txtprecio.Text, "1");
            gridVenta.DataSource = lee.fillTickets(idTicket);
            calculaTotal();
        }

        private void barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                buscaTarget();
            }
        }

        private void gridVenta_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            int idProducto;
            idProducto = int.Parse(gridVenta.Rows[e.RowIndex].Cells[0].Value.ToString());
            cambiaProductosNew objForm = new cambiaProductosNew(idProducto, idTicket);
            objForm.ShowDialog();
            RegistroVentaNew objVino = new RegistroVentaNew(idTicket);
            objVino.Show();
            this.Close();
        }
    }
}
