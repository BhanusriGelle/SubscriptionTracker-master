using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Web.Security;
using SubscriptionTracker.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;

namespace SubscriptionTracker.Controllers
{
    public class ServicesController : Controller
    {
        private SubTrackerContext db = new SubTrackerContext();

        // GET: Services
        public ActionResult Index()
        {
            
            return View(db.ServicesTable.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.ServicesTable.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServiceId,ServiceName,LogoUrl,PlanStatus,BillingTerm,Pricing,StartDate,ServiceType")] Service service)
        {
            if (ModelState.IsValid)
            {
                service.EndDate = service.StartDate.AddMonths(service.BillingTerm);
                string UserEmail = Session["Emailid"].ToString();
                service.User = db.UsersTable.Where(a => a.EmailId == UserEmail).FirstOrDefault();

                db.ServicesTable.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(service);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.ServicesTable.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServiceId,ServiceName,LogoUrl,PlanStatus,BillingTerm,Pricing,StartDate,ServiceType")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.ServicesTable.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.ServicesTable.Find(id);
            db.ServicesTable.Remove(service);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                SubTrackerContext db = new SubTrackerContext();
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/Pics/" + ImageName);
                file.SaveAs(physicalPath);

                Service service = new Service();

                string UserEmail = Session["Emailid"].ToString();
                service.User = db.UsersTable.Where(a => a.EmailId == UserEmail).FirstOrDefault();

                service.BillingTerm = Int32.Parse(Request.Form["BillingTerm"]);
                service.LogoUrl = ImageName;
                service.PlanStatus = "Active";
                service.Pricing = decimal.Parse(Request.Form["Pricing"]);
                service.ServiceName = Request.Form["ServiceName"];
                service.ServiceType = Request.Form["ServiceType"];
                service.StartDate = DateTime.Parse(Request.Form["StartDate"]);
                service.EndDate = service.StartDate.AddMonths(service.BillingTerm);


                db.ServicesTable.Add(service);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

    }
}
