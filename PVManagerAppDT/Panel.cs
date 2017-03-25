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
    public partial class Panel : Form

    {
        AppDTEntities db = new AppDTEntities();
        cmdGridNew lee = new cmdGridNew();
        int idTicket;
        int idEmpleado;
        public Panel(int _idTicket)
        {
            idTicket = _idTicket;
            InitializeComponent();
            calculaTotal();
            if (lblTotal.Text != "0")
            {
                gridVenta.DataSource = lee.fillTickets(idTicket);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
            var desc = db.sp_Stock_Paquete(1).ToList();
            int op = Convert.ToInt32(desc[0].Value);
           
                if (op == 1)
                {
                    MessageBox.Show("No hay productos listos para agregar");
                }
                else
                {
                   
                    var paq = db.PAQUETE_PV.Where(a => a.Paqu_Id == 1).ToList();
                    //pedido_sugerido(paq[0].Paqu_Id, 1, 1);
                    agregaAventa(paq[0].Paqu_Id, paq[0].Paqu_Price, 1);
                    gridVenta.DataSource = lee.fillTickets(idTicket);
                    calculaTotal();
                }
                
            }
            catch(Exception qe)
            {
                MessageBox.Show("No hay productos listos para agregar");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("B2");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("B3");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("B4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
           try
            {
                int a = GetStockSimple(1, 2).add;
                if (a <= 0)
                {
                    MessageBox.Show("No hay productos listos para agregar");
                }
                else
                {
                    var producto = db.PRODUCTOS_PV.Where(b => b.Prod_Id == 1).ToList();
                    agregaAventa(producto[0].Prod_Id, producto[0].Prod_Price, 1);
                    gridVenta.DataSource = lee.fillTickets(idTicket);
                    calculaTotal();
                }
            }
            catch (Exception qe)
            {
                MessageBox.Show("No hay productos listos para agregar");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("extra2");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("extra3");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("extra4");
        }

        private static PedidoSugerido GetStockSimple(int Producto, int Cant)
        {

            using (var db = new AppDTEntities())
            {

                var stock = db.PRODUCTOS_PV.Where(a => a.Prod_Id == Producto).ToList();
                var s1 = Convert.ToInt32(stock[0].Prod_Stock);
                if (s1 <= 2)
                {
                    decimal st = Convert.ToDecimal(stock[0].Prod_Stock);
                    string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    var ps = new PEDIDO_SUGERIDO_PV
                    {
                        Prod_Id = Producto,
                        Sucu_Id = "1",
                        Prod_Stock = st,
                        Prod_date = date,
                        Pedi_Status = 0

                    };
                    db.PEDIDO_SUGERIDO_PV.Add(ps);
                    db.SaveChanges();

                }

                int resta = s1 - Cant;
                if (resta > 0)
                {
                    var producto = (from s in db.PRODUCTOS_PV
                                    where s.Prod_Id == Producto
                                    select s).FirstOrDefault();

                    producto.Prod_Stock = resta;
                    // producto.Ticket_Subtotal = Convert.ToInt32(lblTotal.Text);

                    int num = db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("No cuentas con stock suficiente");
                }

                var result = new PedidoSugerido
                {
                    add = resta
                };
                return result;

            }

           
           
        }

        void pedido_sugerido(int Producto,int Cant,int Tipo)
        {

            if (Tipo == 1)
            {
                //var desc = db.sp_Stock_Paquete(Producto).ToList();
                var desc = db.test_appdt(1).ToList();
                var w = desc[0].Value;

            }//si es venta por paquete
            else
            {
                using (var ctx = new AppDTEntities())
                {

                    var stock = db.PRODUCTOS_PV.Where(a => a.Prod_Id == Producto).ToList();
                    var s1 = Convert.ToInt32(stock[0].Prod_Stock);
                    if (s1 <= 2)
                    {
                        decimal st = Convert.ToDecimal(stock[0].Prod_Stock);
                        string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                        var ps = new PEDIDO_SUGERIDO_PV
                        {
                            Prod_Id = Producto,
                            Sucu_Id = "1",
                            Prod_Stock = st,
                            Prod_date = date,
                            Pedi_Status = 0

                        };
                        db.PEDIDO_SUGERIDO_PV.Add(ps);
                        db.SaveChanges();

                    }

                    int resta = s1 - Cant;
                    if (resta > 0)
                    {
                        var producto = (from s in ctx.PRODUCTOS_PV
                                        where s.Prod_Id == Producto
                                        select s).FirstOrDefault();

                        producto.Prod_Stock = resta;
                        // producto.Ticket_Subtotal = Convert.ToInt32(lblTotal.Text);

                        int num = ctx.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("No cuentas con stock suficiente");
                    }

                }
            }//si es solo

        }

        void agregaAventa(int Producto, int Precio, int Cant)
        {
            try
            {

                var Agrega = new VENTASTICKET_PV
                {
                    Prod_Id = Producto,
                    Venta_Cantidad = Cant,
                    Prod_Price = Precio,
                    Venta_Importe = (Cant * Precio),
                    Ticket_Id = idTicket

                };
                db.VENTASTICKET_PV.Add(Agrega);
                db.SaveChanges();

               

            }
            catch (Exception e)
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
                contador += Convert.ToInt16(Total[0].Venta_Importe);
            }
            lblTotal.Text = contador.ToString();
        }

        private void button9_Click(object sender, EventArgs e)
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
                    ticket.Ticket_Subtotal = Convert.ToInt32(lblTotal.Text);

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
            var reader = db.vis_fillTickets.Where(a => a.Ticket == idTicket).ToList();
            foreach (var tick in reader)
            {
                ticket.AddHeaderLine(" " + tick.Cantidad + "      " + tick.Descripcion + "      $" + tick.Importe);
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
                //btnCerrar_Click(sender, e);
            }
            catch (System.Exception)
            {
               // btnCerrar_Click(sender, e);
                MessageBox.Show("No se encontro la impresora");
            }
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
                    ticket.Ticket_Subtotal = Convert.ToInt32(lblTotal.Text);

                    int num = ctx.SaveChanges();
                }

                IndexNew objmesas = new IndexNew();
                objmesas.Show();
                // btnimprime_Click(sender, e);
                this.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

        }
    }
}
