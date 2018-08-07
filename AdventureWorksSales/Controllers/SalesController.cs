using System;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksSales.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using AdventureWorksSales.Utilities;
using AdventureWorksSales.Models;
using AdventureWorksSales.Resources;

namespace AdventureWorksSales.Controllers
{
    [Route("api/[controller]")]
    public class SalesController : Controller
    {
        IConfiguration Configuration;
        ISalesOrderFactory SalesOrderFactory;

        public SalesController(IConfiguration configuration, ISalesOrderFactory salesOrderFactory)
        {
            Configuration = configuration;
            SalesOrderFactory = salesOrderFactory;
        }

        // GET api/sales/{startDate}/{endDate}
        [HttpGet("{startDate}/{endDate}")]
        public JsonResult Get(string startDate, string endDate)
        {
            DateTime startDateTime = DateTime.Parse(startDate);
            DateTime endDateTime = DateTime.Parse(endDate);

            var salesOrders = SalesOrderFactory.CreateCollection(startDateTime, endDateTime, int.Parse(Configuration["NumerOfItems"]));

            return Json(salesOrders);
        }

        // GET api/sales/download/{startDate}/{endDate}
        [HttpGet("download/{startDate}/{endDate}")]
        public FileResult GetExcelDownload(string startDate, string endDate)
        {
            DateTime startDateTime = DateTime.Parse(startDate);
            DateTime endDateTime = DateTime.Parse(endDate);

            var salesOrders = SalesOrderFactory.CreateCollection(startDateTime, endDateTime);

            var salesOrderExcelFile = new SalesOrderExcelFile(salesOrders, Configuration, HttpContext);

            return salesOrderExcelFile.CreateFileContentResult();
        }
    }
}
