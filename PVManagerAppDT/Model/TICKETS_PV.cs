//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PVManagerAppDT.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TICKETS_PV
    {
        public int Ticket_Id { get; set; }
        public System.DateTime Ticket_Date { get; set; }
        public double Ticket_Subtotal { get; set; }
        public int Ticket_Factura { get; set; }
        public int Sucu_Id { get; set; }
        public string Ticket_Status { get; set; }
    }
}