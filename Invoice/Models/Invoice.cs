
using System;
using System.Collections.Generic;

namespace Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        // return the total amount of the invoice
        public decimal? TotalAmount
        {
            get
            {
                if (Items == null)
                {
                    return null;
                }

                decimal totalAmount = 0;
                foreach (var item in Items)
                {
                    totalAmount += item.Cost;
                }
                return totalAmount;
            }
        }
        // return the total amount of the billable invoice
        public decimal? TotalBillable
        {
            get
            {
                if (Items == null)
                {
                    return null;
                }

                decimal totalBillable = 0;
                foreach (var item in Items)
                {
                    if (item.Billable=="1")
                    {
                        totalBillable += item.Cost;
                    }
                }
                return totalBillable;
            }
        }

        public List<LineItem> Items;
    }

    public class LineItem
    {
        public Guid? Id { get; set; }
        public string Description { get; set; }
        public Guid InvoiceId { get; set; }
        public string Account { get; set; }
        public string Quantity { get; set; }
        public decimal Cost { get; set; }
        public string Billable { get; set; }
    }
}