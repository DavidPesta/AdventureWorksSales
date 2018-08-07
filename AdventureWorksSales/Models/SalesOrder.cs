using System;

namespace AdventureWorksSales.Models
{
    public class SalesOrder
    {
        public string StoreName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string SalesOrderNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public Decimal TotalDue { get; set; }
        public string ProductNumber { get; set; }
        public int OrderQty { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal LineTotal { get; set; }
    }
}
