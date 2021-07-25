using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    public class InvoiceController : Controller
    {
        private static Invoice _ModelInvoice;
        public Invoice GetInvoice()
        {
            if (_ModelInvoice == null)
            {
                _ModelInvoice = new Invoice();

                _ModelInvoice.Description = "Description Edit";
                _ModelInvoice.Id = Guid.NewGuid();
                _ModelInvoice.Items = new List<LineItem>();

                LineItem lineItem = new LineItem();
                lineItem.Id = Guid.NewGuid();
                lineItem.InvoiceId = _ModelInvoice.Id;
                lineItem.Description = "LineItem Description1";
                lineItem.Account = "LineItem Account1";
                lineItem.Billable = "1";
                lineItem.Quantity = "1";
                lineItem.Cost = 100;
                _ModelInvoice.Items.Add(lineItem);

                LineItem lineItem2 = new LineItem();
                lineItem2.Id = Guid.NewGuid();
                lineItem2.InvoiceId = _ModelInvoice.Id;
                lineItem2.Description = "LineItem Description2";
                lineItem2.Account = "LineItem Account2";
                lineItem2.Billable = "1";
                lineItem2.Quantity = "1";
                lineItem2.Cost = 200;
                _ModelInvoice.Items.Add(lineItem2);
            }

            return _ModelInvoice;
        }

        public IActionResult Index()
        {
            Invoice model = GetInvoice();

            ViewData.Model = model;

            return View();
        }

        public IActionResult EditItem(string data)
        {
            LineItem itemDeserialize = JsonConvert.DeserializeObject<LineItem>(data);
            if (itemDeserialize.Id != null)
            {
                LineItem item = _ModelInvoice.Items.Find(a => a.Id == itemDeserialize.Id);
                _ModelInvoice.Items.Remove(item);
            }
            if (itemDeserialize.Id == null)  
            {
                itemDeserialize.Id = Guid.NewGuid();
            }
            _ModelInvoice.Items.Add(itemDeserialize);

            ViewData.Model = _ModelInvoice;

            return Content("OK");
        }

        public IActionResult Save(string data)
        {
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(data);
            _ModelInvoice.Id = invoice.Id;
            _ModelInvoice.Description = invoice.Description;

            return Content(JsonConvert.SerializeObject(_ModelInvoice));
        }


        [Route("Invoice/Switch")]
        public IActionResult Switch()
        {
            if (_ModelInvoice != null && _ModelInvoice.Items != null)
            {
                string strSwitch = "";
                foreach (var item in _ModelInvoice.Items)
                {
                    if (strSwitch == "")
                        strSwitch = item.Billable == "1" ? "0" : "1";

                    item.Billable = strSwitch;
                }
            }

            return Content("OK");
        }

        [Route("Invoice/DeleteItem/{id:Guid}")]
        public IActionResult DeleteItem(Guid id)
        {
            LineItem item = _ModelInvoice.Items.Find(a => a.Id == id);
            if (item != null)
            {
                _ModelInvoice.Items.Remove(item);
            }

            ViewData.Model = _ModelInvoice;

            return Content("OK");
        }
    }

    public class InvoiceInput
    {
        public string Description { get; set; }
        public string TotalAmount { get; set; }
        public string TotalBillable { get; set; }
    }

    public class LineItemInput
    {
        public Guid InvoiceId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public string Account { get; set; }
        public bool Billable { get; set; }
    }
}
