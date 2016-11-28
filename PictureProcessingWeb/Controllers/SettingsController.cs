using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PictureProcessingWeb.Models;


namespace PictureProcessingWeb.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            //sladit se Service1
            string connectionstring = "DefaultEndpointsProtocol=http;AccountName=frpictureprocessing;AccountKey=vouvk+Ls7SgNQ6Ua+Iqr5tBwF3RA9OSyNDQAGjlv463J1I2At0hhnrd9E4MUEwzC/tBomSnyNwWx8BCrvlKziA==";
            CloudStorageAccount LCloudStorageAccount = CloudStorageAccount.Parse(connectionstring);

            CloudTableClient LtableClient = LCloudStorageAccount.CreateCloudTableClient();
            CloudTable Ltable = LtableClient.GetTableReference("settings");

            if (!Ltable.Exists())
            {
                Ltable.Create();
                SettingsEntity LSettingsEntity = new SettingsEntity();

                LSettingsEntity.PartitionKey = "1";
                LSettingsEntity.RowKey = "1";

                LSettingsEntity.DiffLimit = 20;
                LSettingsEntity.GreyComparism = true;
                LSettingsEntity.TimeZone = "";
                LSettingsEntity.PixelSquareSize = 2;
                LSettingsEntity.PictureSourceAddress = "";
               

                // Create the TableOperation object that inserts the entity.
                TableOperation LinsertOperation = TableOperation.Insert(LSettingsEntity);

                // Execute the insert operation.
                Ltable.Execute(LinsertOperation);
            }

            var LQuery = new TableQuery<SettingsEntity>();

            List<SettingsEntity> LList = Ltable.ExecuteQuery(LQuery).ToList();

            return View(LList[0]);
        }
    }
}