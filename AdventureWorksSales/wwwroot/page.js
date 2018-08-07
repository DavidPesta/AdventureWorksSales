$(function () {
    new Pikaday({
        field: document.getElementById('startDate'),
        firstDay: 1,
        minDate: new Date(2000, 12, 31),
        maxDate: new Date(2040, 12, 31),
        yearRange: [2000, 2040]
    });

    new Pikaday({
        field: document.getElementById('endDate'),
        firstDay: 1,
        minDate: new Date(2000, 12, 31),
        maxDate: new Date(2040, 12, 31),
        yearRange: [2000, 2040]
    });

    function SalesRecord(data) {
        this.StoreName = data.storeName ? data.storeName : "N/A";
        this.FirstName = data.firstName ? data.firstName : "";
        this.LastName = data.lastName ? data.lastName : "";
        this.AccountNumber = data.accountNumber ? data.accountNumber : "N/A";
        this.SalesOrderNumber = data.salesOrderNumber ? data.salesOrderNumber : "N/A";
        this.PurchaseOrderNumber = data.purchaseOrderNumber ? data.purchaseOrderNumber : "N/A";

        var orderDate = moment(data.orderDate).format("YYYY-MM-DD");
        this.OrderDate = data.orderDate ? orderDate : "N/A";

        var dueDate = moment(data.dueDate).format("YYYY-MM-DD");
        this.DueDate = data.dueDate ? dueDate : "N/A";

        this.TotalDue = data.totalDue ? data.totalDue.toFixed(2) : "N/A";
        this.ProductNumber = data.productNumber ? data.productNumber : "N/A";
        this.OrderQty = data.orderQty ? data.orderQty : "N/A";
        this.UnitPrice = data.unitPrice ? data.unitPrice.toFixed(2) : "N/A";
        this.LineTotal = data.lineTotal ? data.lineTotal.toFixed(2) : "N/A";
    }

    function SalesViewModel() {
        var self = this;

        self.startDate = ko.observable();
        self.endDate = ko.observable();
        self.salesData = ko.observableArray([]);

        self.startDate(moment().subtract(1, 'months').startOf('month').format('YYYY-MM-DD'));
        self.endDate(moment().subtract(1, 'months').endOf('month').format('YYYY-MM-DD'));

        self.validateData = function () {
            if (self.startDate() == "" || self.endDate() == "") {
                alert("Start Date and End Date must both be populated.");
                return false;
            }

            if (moment(self.startDate()).isAfter(self.endDate())) {
                alert("Start Date must come before End Date.");
                return false;
            }

            return true;
        }

        self.fetchData = function () {
            if (self.validateData()) {
                $.getJSON("/api/sales/" + self.startDate() + "/" + self.endDate(), function (data) {
                    if (data.length == 0) {
                        alert("Date range produced no sales data results.");
                    }

                    var salesData = $.map(data, function (item) { return new SalesRecord(item) });
                    self.salesData(salesData);
                    console.log(self.salesData());
                });
            }
        }

        self.downloadExcel = function () {
            if (self.validateData()) {
                var iframe = document.createElement('iframe');
                iframe.id = "iframeid";
                iframe.style.display = 'none';
                document.body.appendChild(iframe);
                iframe.src = "/api/sales/download/" + self.startDate() + "/" + self.endDate();
            }
        }
    }

    ko.applyBindings(new SalesViewModel());
});