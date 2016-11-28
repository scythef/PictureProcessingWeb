using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using PictureProcessingAPI.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PictureProcessingAPI.Controllers
{
    public class ValuesController : ApiController
    {
        private static Dictionary<int, Picture> mockData = new Dictionary<int, Picture>();
        //static ValuesController()
        //{
        //    mockData.Add(0, new Picture { ID = 0, yyyyMMdd = "20160101", HHmmssffff = "0101011111", Year=2016, Month=1, Day=1, Hour=1, Minute=1, Second=1, Milisecond=1111, Diff=50, Filename="filename1" } );
        //    mockData.Add(0, new Picture { ID = 1, yyyyMMdd = "20161220", HHmmssffff = "2002021011", Year=2016, Month=12, Day=20, Hour=20, Minute=20, Second=2, Milisecond=1011, Diff=40, Filename="filename2" } );
        //}

        private static void CheckCallerId()
        {
            // Uncomment following lines for service principal authentication
            //string currentCallerClientId = ClaimsPrincipal.Current.FindFirst("appid").Value;
            //string currentCallerServicePrincipalId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //if (currentCallerClientId != trustedCallerClientId || currentCallerServicePrincipalId != trustedCallerServicePrincipalId)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, ReasonPhrase = "The appID or service principal ID is not the expected value." });
            //}
        }




        // GET api/values
        [SwaggerOperation("GetAll")]
        public IEnumerable<Picture> Get(string aYYYY_MM_DD)
        {


            //mockData.Add(0, new Picture { ID = 0, yyyyMMdd = "20160101", HHmmssffff = "0101011111", Year = 2016, Month = 1, Day = 1, Hour = 1, Minute = 1, Second = 1, Milisecond = 1111, Diff = 50, Filename = "filename1" });
            //mockData.Add(1, new Picture { ID = 1, yyyyMMdd = "20161220", HHmmssffff = "2002021011", Year = 2016, Month = 12, Day = 20, Hour = 20, Minute = 20, Second = 2, Milisecond = 1011, Diff = 40, Filename = "filename2" });




            CheckCallerId();

            string connectionstring = "DefaultEndpointsProtocol=http;AccountName=frpictureprocessing;AccountKey=vouvk+Ls7SgNQ6Ua+Iqr5tBwF3RA9OSyNDQAGjlv463J1I2At0hhnrd9E4MUEwzC/tBomSnyNwWx8BCrvlKziA==";
            CloudStorageAccount LCloudStorageAccount = CloudStorageAccount.Parse(connectionstring);

            CloudTableClient LtableClient = LCloudStorageAccount.CreateCloudTableClient();
            CloudTable Ltable = LtableClient.GetTableReference("pictures");

            CloudBlobClient LBlobClient = LCloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer LBlobContainer = LBlobClient.GetContainerReference("pictures");
            CloudBlockBlob LBlockBlob;


            var LQuery = new TableQuery<PictureEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, aYYYY_MM_DD));

            mockData.Clear();
            List<PictureEntity> LList = Ltable.ExecuteQuery(LQuery).ToList();
            foreach (PictureEntity aPicEnt in LList)
            {
                LBlockBlob = LBlobContainer.GetBlockBlobReference(aPicEnt.Filename);
                aPicEnt.Uri = LBlockBlob.Uri.ToString();
                DateTime LDT = new DateTime().AddHours(aPicEnt.Hour).AddMinutes(aPicEnt.Minute);
                aPicEnt.HHmm = LDT.ToString("HH:mm");
                mockData.Add(mockData.Count, new Picture { ID = mockData.Count, PartitionKey = aPicEnt.PartitionKey, yyyyMMdd = aPicEnt.yyyyMMdd, HHmmssffff = aPicEnt.HHmmssffff, Year = aPicEnt.Year, Month = aPicEnt.Month, Day = aPicEnt.Day, Hour = aPicEnt.Hour, Minute = aPicEnt.Minute, Second = aPicEnt.Second, Milisecond = aPicEnt.Milisecond, Diff = aPicEnt.Diff, Filename = aPicEnt.Filename, Url = aPicEnt.Uri });
            }

            //return new string[] { "value1", "value2" };

            //            return mockData.Values.Where(m => m.Owner == owner || owner == "*");
            return mockData.Values;
//            return mockData.Values.Where(m => m.Year == aYear && m.Month == aMonth && m.Day == aDay);

        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public Picture Get(int id)
        {
            CheckCallerId();

            return mockData.Values.Where(m => m.ID == id).First();

            
        }

        // POST api/values
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public void Post(Picture aPicture)
        {
            CheckCallerId();

            aPicture.ID = mockData.Count > 0 ? mockData.Keys.Max() + 1 : 1;
            mockData.Add(aPicture.ID, aPicture);
        }

        // PUT api/values/5
        [SwaggerOperation("Update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Put(int aID, Picture aPicture)
        {
            CheckCallerId();

            Picture xpic = mockData.Values.First(a => a.ID == aPicture.ID);
            if (aPicture != null && xpic != null)
            {
    //            xpic.Description = aPicture.Description;
            }
        }

        // DELETE api/values/5
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Delete(int aID)
        {
            CheckCallerId();

            Picture pic = mockData.Values.First(a => a.ID == aID);
            if (pic != null)
            {
                mockData.Remove(pic.ID);
            }
        }

        
        

    }
}
