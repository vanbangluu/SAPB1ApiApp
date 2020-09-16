using API.Models;
using System;
using System.Collections.Generic;
using SAPbobsCOM;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace API.Controllers
{
    [RoutePrefix("Sap")]
    public class SapController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        [Route("ConnectSAP")]
        [HttpGet]
        public IHttpActionResult ConnectSAP()
        {
            try
            {
                ServerConnection connection = new ServerConnection();
                // attempt connection; 0 = success
                if (connection.Connect() == 0)
                {
                    var companyName = connection.GetCompany().CompanyName;
                    connection.GetCompany().Disconnect();
                    return Success(companyName);
                }
                else
                {
                    throw new Exception(connection.GetErrorCode() + " : " + connection.GetErrorMessage());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetCustomers")]
        public IHttpActionResult GetCustomers()
        {
            try
            {
                ServerConnection connection = new ServerConnection();
                if (connection.Connect() == 0)
                {
                    Recordset oRecordSet;
                    oRecordSet = connection.GetCompany().GetBusinessObject(BoObjectTypes.BoRecordset);

                    oRecordSet.DoQuery("SELECT DISTINCT CardCode as 'Customer Code', CardName as 'Name' FROM OCRD");

                    List<Customer> customers = new List<Customer>();

                    while (!(oRecordSet.EoF == true))
                    {
                        if (oRecordSet.EoF == false)
                        {
                            var customer = new Customer();
                            customer.Id = oRecordSet.Fields.Item(0).Value;
                            customer.CustomerCode = oRecordSet.Fields.Item(0).Value;
                            customer.CustomerName = oRecordSet.Fields.Item(1).Value;

                            customers.Add(customer);
                        }

                        oRecordSet.MoveNext();
                    }

                    int retVal = 0;
                    var success = "Started at " + DateTime.Now;

                    if (retVal == 0)
                    {
                        success += " Ended at " + DateTime.Now;
                        connection.GetCompany().Disconnect();
                        return Json(customers);
                    }
                    else
                    {
                        connection.GetCompany().Disconnect();
                        throw new Exception(connection.GetCompany().GetLastErrorCode() + " : " + connection.GetCompany().GetLastErrorDescription());
                    }
                }
                else
                {
                    throw new Exception(connection.GetErrorCode() + " : " + connection.GetErrorMessage());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("AddCustomer")]
        [HttpPost]
        public IHttpActionResult AddCustomer(FormDataCollection formData)
        {
            try
            {
                ServerConnection connection = new ServerConnection();
                // attempt connection; 0 = success
                if (connection.Connect() == 0)
                {
                    int retVal = 0;
                    var success = "Started at " + DateTime.Now;

                    BusinessPartners businessPartner = null;
                    businessPartner = connection.GetCompany().GetBusinessObject(BoObjectTypes.oBusinessPartners);

                    businessPartner.CardCode = formData["CardCode"];
                    businessPartner.CardName = formData["CardName"];
                    businessPartner.Phone1 = formData["Phone1"];
                    businessPartner.CardType = BoCardTypes.cCustomer;
                    businessPartner.SubjectToWithholdingTax = BoYesNoEnum.tNO;
                    retVal = businessPartner.Add();
                    //
                    if (retVal == 0)
                    {
                        success += " Ended at " + DateTime.Now;
                        connection.GetCompany().Disconnect();
                        return Success(success);
                    }
                    else
                    {
                        connection.GetCompany().Disconnect();
                        throw new Exception(connection.GetCompany().GetLastErrorCode() + " : " + connection.GetCompany().GetLastErrorDescription());
                    }
                }
                else
                {
                    throw new Exception(connection.GetErrorCode() + " : " + connection.GetErrorMessage());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetItems")]
        public IHttpActionResult GetItems()
        {
            try
            {
                var connection = new ServerConnection();
                if (connection.Connect() == 0)
                {
                    Recordset oRecordSet;
                    oRecordSet = connection.GetCompany().GetBusinessObject(BoObjectTypes.BoRecordset);

                    oRecordSet.DoQuery("SELECT DISTINCT T0.ItemCode as 'Item Code', T0.ItemName as 'Item', T0.ItemType as 'Item Group', T0.ItmsGrpCod as 'UoM Group' FROM OITM T0");

                    List<Item> items = new List<Item>();

                    while (!(oRecordSet.EoF == true))
                    {
                        if (oRecordSet.EoF == false)
                        {
                            var item = new Item();

                            item.ItemCode = oRecordSet.Fields.Item(0).Value;
                            item.ItemName = oRecordSet.Fields.Item(1).Value;
                            item.ItemType = oRecordSet.Fields.Item(2).Value;
                            item.UoMGroup = oRecordSet.Fields.Item(3).Value;

                            items.Add(item);
                        }

                        oRecordSet.MoveNext();
                    }

                    int retVal = 0;
                    var success = "Started at " + DateTime.Now;

                    if (retVal == 0)
                    {
                        success += " Ended at " + DateTime.Now;
                        connection.GetCompany().Disconnect();
                        return Json(items);
                    }
                    else
                    {
                        connection.GetCompany().Disconnect();
                        throw new Exception(connection.GetCompany().GetLastErrorCode() + " : " + connection.GetCompany().GetLastErrorDescription());
                    }
                }
                else
                {
                    throw new Exception(connection.GetErrorCode() + " : " + connection.GetErrorMessage());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetSalesInvoice")]
        public IHttpActionResult GetSalesInvoice()
        {
            try
            {
                ServerConnection connection = new ServerConnection();
                if (connection.Connect() == 0)
                {
                    Recordset oRecordSet;
                    oRecordSet = connection.GetCompany().GetBusinessObject(BoObjectTypes.BoRecordset);

                    //oRecordSet.DoQuery("select top 5 v.docnum,v.docdate,v.cardcode,v.CardName from oinv v where v.docentry in (select v1.DocEntry from inv1 v1 where v1.BaseType ='-1') order by v.docnum desc");

                    oRecordSet.DoQuery("SELECT DISTINCT	" +
                        "T1.DocNum as 'Invoice No.', T1.DocDate as 'Invoice Date', " +
                        "T1.CardCode as 'Customer Code', T1.CardName as 'Customer', " +
                        "T0.ItemCode as 'Item Code', T0.Dscription as 'Item', T0.UomCode as 'UOM', " +
                        "T0.Quantity, T0.Price as 'Rate', T0.TaxCode as 'Tax Code', T0.VatPrcnt as 'Tax %', " +
                        "T0.VatSum as 'Tax Amount', T0.LineTotal as 'Amount BF Tax', T0.GTotal as 'Amount AF Tax' " +
                        "FROM INV1 T0 LEFT JOIN OINV T1 ON T1.DocEntry = T0.[DocEntry] " +
                        "WHERE T1.[DocDate] >= '11/13/2019' AND  T1.[DocDate] <= '12/01/2019'");

                    List<SalesInvoice> salesInvoices = new List<SalesInvoice>();

                    while ((!oRecordSet.EoF))
                    {
                        if (oRecordSet.EoF == false)
                        {
                            var salesInvoice = new SalesInvoice();

                            salesInvoice.InvoiceNumber = oRecordSet.Fields.Item(0).Value;
                            salesInvoice.InvoiceDate = oRecordSet.Fields.Item(1).Value;
                            salesInvoice.CustomerCode = oRecordSet.Fields.Item(2).Value;
                            salesInvoice.CustomerName = oRecordSet.Fields.Item(3).Value;
                            salesInvoice.ItemCode = oRecordSet.Fields.Item(4).Value;
                            salesInvoice.ItemDescription = oRecordSet.Fields.Item(5).Value;
                            salesInvoice.UOM = oRecordSet.Fields.Item(6).Value;
                            salesInvoice.Quantity = oRecordSet.Fields.Item(7).Value;
                            salesInvoice.Rate = oRecordSet.Fields.Item(8).Value;
                            salesInvoice.TaxCode = oRecordSet.Fields.Item(9).Value;
                            salesInvoice.TaxPercentage = oRecordSet.Fields.Item(10).Value;
                            salesInvoice.TotalTaxAmount = oRecordSet.Fields.Item(11).Value;
                            salesInvoice.TotalAmountBfTax = oRecordSet.Fields.Item(12).Value;
                            salesInvoice.TotalAmountAfTax = oRecordSet.Fields.Item(13).Value;

                            salesInvoices.Add(salesInvoice);
                        }
                        oRecordSet.MoveNext();
                    }

                    int retVal = 0;
                    var success = "Started at " + DateTime.Now;

                    if (retVal == 0)
                    {
                        success += " Ended at " + DateTime.Now;
                        connection.GetCompany().Disconnect();
                        return Json(salesInvoices);
                    }
                    else
                    {
                        connection.GetCompany().Disconnect();
                        throw new Exception(connection.GetCompany().GetLastErrorCode() + " : " + connection.GetCompany().GetLastErrorDescription());
                    }
                }
                else
                {
                    throw new Exception(connection.GetErrorCode() + " : " + connection.GetErrorMessage());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("PostSalesInvoiceToUDT")]
        public IHttpActionResult PostSalesInvoiceToUDT(FormDataCollection formData)
        {
            try
            {
                ServerConnection connection = new ServerConnection();
                if (connection.Connect() == 0)
                {
                    int retVal = 0;
                    var success = "Started at " + DateTime.Now;

                    SalesInvoice salesInvoice = new SalesInvoice();

                    salesInvoice.InvoiceNumber = Convert.ToInt32(formData["InvoiceNumber"]);
                    salesInvoice.InvoiceDate = Convert.ToDateTime(formData["InvoiceDate"]);
                    salesInvoice.CustomerCode = formData["CustomerCode"];
                    salesInvoice.CustomerName = formData["CustomerName"];
                    salesInvoice.ItemCode = formData["ItemCode"];
                    salesInvoice.ItemDescription = formData["ItemDescription"];
                    salesInvoice.UOM = formData["UOM"];
                    salesInvoice.Quantity = Convert.ToDouble(formData["Quantity"]);
                    salesInvoice.Rate = Convert.ToDouble(formData["Rate"]);
                    salesInvoice.TaxCode = formData["TaxCode"];
                    salesInvoice.TaxPercentage = Convert.ToDouble(formData["TaxPercentage"]);
                    salesInvoice.TotalTaxAmount = Convert.ToDouble(formData["TotalTaxAmount"]);
                    salesInvoice.TotalAmountBfTax = Convert.ToDouble(formData["TotalAmountBfTax"]);
                    salesInvoice.TotalAmountAfTax = Convert.ToDouble(formData["TotalAmountAfTax"]);

                    _context.SalesInvoices.Add(salesInvoice);
                    _context.SaveChanges();

                    if (retVal == 0)
                    {
                        success += " Ended at " + DateTime.Now;
                        connection.GetCompany().Disconnect();
                        return Success(success);
                    }
                    else
                    {
                        connection.GetCompany().Disconnect();
                        throw new Exception(connection.GetCompany().GetLastErrorCode() + " : " + connection.GetCompany().GetLastErrorDescription());
                    }
                }
                else
                {
                    throw new Exception(connection.GetErrorCode() + " : " + connection.GetErrorMessage());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IHttpActionResult Success()
        {
            return Json(new { Success = true });
        }
        public IHttpActionResult Success(string msg)
        {
            return Json(new { Success = true, message = msg });
        }
    }
}