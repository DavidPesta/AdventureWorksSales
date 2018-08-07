using AdventureWorksSales.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksSales.Models.Interfaces
{
    public interface ISalesOrderFactory
    {
        List<SalesOrder> CreateCollection(DateTime startDateTime, DateTime endDateTime, int? numberOfItems = null);
    }
}
