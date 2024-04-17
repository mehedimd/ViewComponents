using Microsoft.AspNetCore.Mvc;
using ViewComponents.Models;
using ViewComponents.ViewModels;

namespace ViewComponents.Controllers
{
    public class VendorController : Controller
    {
        private APContext context;
        public VendorController(APContext _context)
        {
            this.context = _context;
        }
        public IActionResult Vendor(string name)
        {
            //if (name != null)
            //{
            //    var vendor = context.Vendors.Where(v => v.VendorState.ToLower().Equals(name.ToLower())).ToList();

            //    var vendorList = vendor.Join(context.Invoices, v => v.VendorId, i => i.VendorId, (v, i) => new { vndr = v, invc = i }).Select(s => new VendorInvoiceVM
            //    {
            //        VendorName = s.vndr.VendorName,
            //        VendorAddress = $"{s.vndr.VendorAddress1 ?? s.vndr.VendorAddress2},{s.vndr.VendorState},{s.vndr.VendorCity},{s.vndr.VendorZipCode}",
            //        InvoiceTotal = s.invc.InvoiceTotal,
            //        PaymentTotal = s.invc.PaymentTotal,
            //        TotalDue = s.invc.InvoiceTotal - s.invc.CreditTotal -s.invc.PaymentTotal

            //    });
            //    return View(vendorList);
            //}
            var result = (from a in context.Invoices
                          join b in context.Vendors on a.VendorId equals b.VendorId
                          group a by new { b.VendorId, b.VendorName, b.VendorAddress1, b.VendorAddress2, b.VendorCity, b.VendorState, b.VendorZipCode }
              into c
                          select new VendorInvoiceVM
                          {
                              VendorState = c.Key.VendorState,
                              VendorName = c.Key.VendorName,
                              VendorAddress = $"{c.Key.VendorAddress1 ?? c.Key.VendorAddress2}, {c.Key.VendorCity}, {c.Key.VendorZipCode}",
                              InvoiceTotal = c.Sum(b => b.InvoiceTotal),
                              PaymentTotal = c.Sum(b => b.PaymentTotal),
                              TotalDue = c.Sum(p => p.InvoiceTotal) - c.Sum(b => b.PaymentTotal) - c.Sum(b => b.CreditTotal)
                          }).Where(p=>p.VendorState.Equals(name));
            return View(result);
        }
    }
}
