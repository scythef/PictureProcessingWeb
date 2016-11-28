using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PictureProcessingWeb.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;



namespace PictureProcessingWeb.Controllers
{
    public class PicturesController : Controller
    {
//        private PictureDBContext db = new PictureDBContext();

        // GET: Pictures
        public ActionResult Index(string aPartitionKey = "")
        {
            //sladit se Service1
            string connectionstring = "DefaultEndpointsProtocol=http;AccountName=frpictureprocessing;AccountKey=vouvk+Ls7SgNQ6Ua+Iqr5tBwF3RA9OSyNDQAGjlv463J1I2At0hhnrd9E4MUEwzC/tBomSnyNwWx8BCrvlKziA==";
            CloudStorageAccount LCloudStorageAccount = CloudStorageAccount.Parse(connectionstring);

            CloudTableClient LtableClient = LCloudStorageAccount.CreateCloudTableClient();
            CloudTable Ltable = LtableClient.GetTableReference("pictures");

            CloudBlobClient LBlobClient = LCloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer LBlobContainer = LBlobClient.GetContainerReference("pictures");
            CloudBlockBlob LBlockBlob;

            if (aPartitionKey == "")
            {
                aPartitionKey = DateTime.UtcNow.ToString("yyyy-MM-dd");
            }
            TempData["date"] = new DateTime(int.Parse(aPartitionKey.Substring(0, 4)), int.Parse(aPartitionKey.Substring(5, 2)), int.Parse(aPartitionKey.Substring(8, 2)), 0, 0, 0).ToLongDateString();
            TempData["year"] = aPartitionKey.Substring(0, 4);
            TempData["month"] = aPartitionKey.Substring(5, 2);
            TempData["day"] = aPartitionKey.Substring(8, 2);


            //if (aPartitionKey == DateTime.UtcNow.ToString("yyyy-MM-dd"))
            //{
            //    TempData["Date"] = "Today";
            //}
            //else
            //{
            //    if (aPartitionKey == DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd"))
            //    {
            //        TempData["Date"] = "Yesterday";
            //    }
            //    else
            //    {


            //    }

            //}

            var LQuery = new TableQuery<PictureEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, aPartitionKey));

            List<PictureEntity> LList = Ltable.ExecuteQuery(LQuery).ToList();
            foreach (PictureEntity aPicEnt in LList)
            {
                LBlockBlob = LBlobContainer.GetBlockBlobReference(aPicEnt.Filename);
                aPicEnt.Uri = LBlockBlob.Uri.ToString();
                DateTime LDT = new DateTime().AddHours(aPicEnt.Hour).AddMinutes(aPicEnt.Minute);
                aPicEnt.HHmm = LDT.ToString("HH:mm");

            }

            return View(LList);

            //return View(db.Pictures.ToList());
        }

        // GET: Pictures/Details/5
        public ActionResult Details(string id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            //sladit se Service1
            string connectionstring = "DefaultEndpointsProtocol=http;AccountName=frpictureprocessing;AccountKey=vouvk+Ls7SgNQ6Ua+Iqr5tBwF3RA9OSyNDQAGjlv463J1I2At0hhnrd9E4MUEwzC/tBomSnyNwWx8BCrvlKziA==";
            CloudStorageAccount LCloudStorageAccount = CloudStorageAccount.Parse(connectionstring);

            CloudTableClient LtableClient = LCloudStorageAccount.CreateCloudTableClient();
            CloudTable Ltable = LtableClient.GetTableReference("pictures");

            CloudBlobClient LBlobClient = LCloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer LBlobContainer = LBlobClient.GetContainerReference("pictures");
            CloudBlockBlob LBlockBlob;

            string LPK = id.ToString().Substring(0, 10);
            int LH = int.Parse(id.ToString().Substring(10, 2));
            int LM = int.Parse(id.ToString().Substring(12, 2));

            var LQuery = new TableQuery<PictureEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, LPK));
            List<PictureEntity> LList = Ltable.ExecuteQuery(LQuery).ToList();

            for (var ai = LList.Count() - 1; ai >= 0 ; ai--)
            {
                if ((LList[ai].Hour != LH) || (LList[ai].Minute != LM))
                {
                    LList.RemoveAt(ai);
                }
            }

            foreach (PictureEntity aPicEnt in LList)
            {
                LBlockBlob = LBlobContainer.GetBlockBlobReference(aPicEnt.Filename);
                aPicEnt.Uri = LBlockBlob.Uri.ToString();
                DateTime LDT = new DateTime().AddHours(aPicEnt.Hour).AddMinutes(aPicEnt.Minute);
                aPicEnt.HHmm = LDT.ToString("HH:mm");
            }

            //Picture picture = db.Pictures.Find(id);
            //if (picture == null)
            //{
            //    return HttpNotFound();
            //}
            return View(LList);
        }

        // GET: Pictures/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,yyyyMMdd,HHmmssffff,Year,Month,Day,Hour,Minute,Second,Milisecond,Filename,Diff")] Picture picture)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Pictures.Add(picture);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(picture);
        //}

        // GET: Pictures/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Picture picture = db.Pictures.Find(id);
        //    if (picture == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(picture);
        //}

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,yyyyMMdd,HHmmssffff,Year,Month,Day,Hour,Minute,Second,Milisecond,Filename,Diff")] Picture picture)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(picture).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(picture);
        //}

        // GET: Pictures/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Picture picture = db.Pictures.Find(id);
        //    if (picture == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(picture);
        //}

        // POST: Pictures/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Picture picture = db.Pictures.Find(id);
        //    db.Pictures.Remove(picture);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
