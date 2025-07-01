using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CrystalReport.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult Invoice()
        {
            ReportDocument rd = new ReportDocument();
            // Load the report file
            rd.Load("G:\\6\\Project\\TFC\\TFC\\CrystalReport\\Reports\\InvoiceReport.rpt");
            // Set the data source for the report
            var ModelContext = new List<object>
            {
                new { InvoiceNumber = "INV001", CustomerName = "John Doe", Amount = 100.00, Date = DateTime.Now }
            };
            //rd.SetDataSource(ModelContext);
            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf", "Invoice.pdf");
        }
    }
}