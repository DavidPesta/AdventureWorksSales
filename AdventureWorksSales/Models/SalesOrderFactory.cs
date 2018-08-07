using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureWorksSales.Entities;
using AdventureWorksSales.Models.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AdventureWorksSales.Models
{
    public class SalesOrderFactory : ISalesOrderFactory
    {
        private readonly IConfiguration Configuration;

        public SalesOrderFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<SalesOrder> CreateCollection(DateTime startDateTime, DateTime endDateTime, int? numberOfItems = null)
        {
            List<SalesOrder> salesOrders = new List<SalesOrder>();
            
            using (var db = new ApplicationDbContext(Configuration["DatabaseConnectionString"]))
            {
                var salesOrderHeaders = db.SalesOrderHeader
                    .Include(header => header.Customer)
                        .ThenInclude(customer => customer.Person)
                    .Include(header => header.Customer)
                        .ThenInclude(customer => customer.Store)
                    .Include(header => header.SalesOrderDetail)
                    .Where(header => header.DueDate >= startDateTime && header.DueDate <= endDateTime)
                    .OrderBy(header => header.DueDate);
                    //.Take((int)numberOfItems); // This won't work for us. It limits the number of salesOrderHeader records BEFORE expanding them with the inner foreach below. 

                int counter = 0;
                foreach (SalesOrderHeader salesOrderHeader in salesOrderHeaders)
                {
                    foreach (var soDetail in salesOrderHeader.SalesOrderDetail)
                    {
                        SalesOrder salesOrder = new SalesOrder();

                        if (salesOrderHeader.Customer != null)
                        {
                            if (salesOrderHeader.Customer.Store != null)
                            {
                                salesOrder.StoreName = salesOrderHeader.Customer.Store.Name;
                            }

                            if (salesOrderHeader.Customer.Person != null)
                            {
                                salesOrder.FirstName = salesOrderHeader.Customer.Person.FirstName;
                                salesOrder.LastName = salesOrderHeader.Customer.Person.LastName;
                            }

                            salesOrder.AccountNumber = salesOrderHeader.AccountNumber;
                        }

                        salesOrder.SalesOrderNumber = salesOrderHeader.SalesOrderNumber;
                        salesOrder.PurchaseOrderNumber = salesOrderHeader.PurchaseOrderNumber;
                        salesOrder.OrderDate = salesOrderHeader.OrderDate;
                        salesOrder.DueDate = salesOrderHeader.DueDate;
                        salesOrder.TotalDue = salesOrderHeader.TotalDue;

                        salesOrder.ProductNumber = db.Product.Where(p => p.ProductId == soDetail.ProductId).Select(p => p.ProductNumber).Single();
                        salesOrder.OrderQty = soDetail.OrderQty;
                        salesOrder.UnitPrice = soDetail.UnitPrice;
                        salesOrder.LineTotal = soDetail.LineTotal;

                        salesOrders.Add(salesOrder);

                        counter++;
                        if (numberOfItems != null && counter >= numberOfItems)
                        {
                            return salesOrders;
                        }
                    }
                }
            }

            return salesOrders;
        }
    }
}
