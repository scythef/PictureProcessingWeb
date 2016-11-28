using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Data.Entity;



namespace PictureProcessingWeb.Models
{
    //sjednotit s tridou Service1.PictureEntity
    public class PictureEntity : TableEntity
    {
        public int ID { get; set; }
        public string yyyyMMdd { get; set; }
        public string HHmmssffff { get; set; }
        public string HHmm { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int Milisecond { get; set; }
        public string Filename { get; set; }
        public string Uri { get; set; }
        public int Diff { get; set; }
    }

    public class Picture
    {
        public int ID { get; set; }
        public string yyyyMMdd { get; set; }
        public string HHmmssffff { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int Milisecond { get; set; }
        public string Filename { get; set; }
        public int Diff { get; set; }
    }

    public class PictureDBContext : DbContext
    {
        public DbSet<Picture> Pictures { get; set; }
    }

}