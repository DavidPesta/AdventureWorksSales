using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksSales.Models;
using AdventureWorksSales.Utilities;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AdventureWorksSales.Resources
{
    public class Column : SpreadsheetBuilder.ColumnTemplate<SalesOrder> { }

    public class SalesOrderExcelFile
    {
        private List<SalesOrder> SalesOrders;
        private IConfiguration Configuration;
        private HttpContext HttpContext;

        public SalesOrderExcelFile(List<SalesOrder> salesOrders, IConfiguration configuration, HttpContext httpContext)
        {
            SalesOrders = salesOrders;
            Configuration = configuration;
            HttpContext = httpContext;
        }

        public FileContentResult CreateFileContentResult()
        {
            var pkg = new ExcelPackage();
            var wbk = pkg.Workbook;
            var sheet = wbk.Worksheets.Add(Configuration["WorksheetName"]);

            var normalStyle = "Normal";

            var columns = new[]
            {
                new Column { Title = "Sold At", Style = normalStyle, Action = i => i.StoreName ?? "N/A" },
                new Column { Title = "Sold To", Style = normalStyle, Action = i => i.FirstName + " " + i.LastName },
                new Column { Title = "Account Number", Style = normalStyle, Action = i => i.AccountNumber ?? "N/A" },
                new Column { Title = "Invoice #", Style = normalStyle, Action = i => i.SalesOrderNumber ?? "N/A" },
                new Column { Title = "Customer PO #", Style = normalStyle, Action = i => i.PurchaseOrderNumber ?? "N/A" },
                new Column { Title = "Order Date", Style = normalStyle, Action = i => i.OrderDate.ToString("yyyy-MM-dd") },
                new Column { Title = "Due Date", Style = normalStyle, Action = i => i.DueDate.ToString("yyyy-MM-dd") },
                new Column { Title = "Invoice Total", Style = normalStyle, Action = i => i.TotalDue.ToString("0.##") },
                new Column { Title = "Product Number", Style = normalStyle, Action = i => i.ProductNumber ?? "N/A" },
                new Column { Title = "Order Qty", Style = normalStyle, Action = i => i.OrderQty },
                new Column { Title = "Unit Net", Style = normalStyle, Action = i => i.UnitPrice.ToString("0.##") },
                new Column { Title = "Line Total", Style = normalStyle, Action = i => i.LineTotal.ToString("0.##") }
            };

            sheet.SaveData(columns, SalesOrders);

            var bytes = pkg.GetAsByteArray();

            HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            FileContentResult result = new FileContentResult(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = Configuration["DownloadFilename"]
            };

            return result;
        }
    }
}
