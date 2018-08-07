using System;
using System.Collections.Generic;
using AdventureWorksSales.Models.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AdventureWorksSales.Models
{
    public class SalesOrderFactoryAlt : ISalesOrderFactory
    {
        private readonly IConfiguration Configuration;

        public SalesOrderFactoryAlt(IConfiguration configuration) {
            Configuration = configuration;
        }

        public List<SalesOrder> CreateCollection(DateTime startDateTime, DateTime endDateTime, int? numberOfItems = null)
        {
            List<SalesOrder> salesOrders = new List<SalesOrder>();

            salesOrders.Add(new SalesOrder
            {
                StoreName = "Fantasy Land",
                FirstName = "Fake",
                LastName = "Person",
                AccountNumber = "AW00011111",
                SalesOrderNumber = "SO11111",
                PurchaseOrderNumber = "PO11111111111",
                OrderDate = new DateTime(2011, 11, 11),
                DueDate = new DateTime(2011, 11, 26),
                TotalDue = 11.11M,
                ProductNumber = "PN-1111",
                OrderQty = 1,
                UnitPrice = 11.11M,
                LineTotal = 11.11M
            });

            salesOrders.Add(new SalesOrder
            {
                StoreName = "Imaginarium",
                FirstName = "Fictional",
                LastName = "Human",
                AccountNumber = "AW00022222",
                SalesOrderNumber = "SO22222",
                PurchaseOrderNumber = "PO22222222222",
                OrderDate = new DateTime(2012, 2, 22),
                DueDate = new DateTime(2012, 3, 7),
                TotalDue = 22.22M,
                ProductNumber = "PN-2222",
                OrderQty = 2,
                UnitPrice = 11.11M,
                LineTotal = 22.22M
            });

            return salesOrders;
        }
    }
}
