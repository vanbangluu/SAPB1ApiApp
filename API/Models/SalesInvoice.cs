using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class SalesInvoice
    {
        public int Id { get; set; }
        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string UOM { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public string TaxCode { get; set; }
        public double TaxPercentage { get; set; }
        public double TotalTaxAmount { get; set; }
        public double TotalAmountBfTax { get; set; }
        public double TotalAmountAfTax { get; set; }
    }
}