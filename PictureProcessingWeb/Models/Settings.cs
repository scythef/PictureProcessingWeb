using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;


namespace PictureProcessingWeb.Models
{
    public class SettingsEntity : TableEntity
    {
        // 0 - 100%
        public int DiffLimit { get; set; }
        // 1 - ... 1=1x1, 2=2x2, 3=3x3, ...
        public int PixelSquareSize { get; set; }
        // eliminate color influence
        public bool GreyComparism { get; set; }
        public string TimeZone { get; set; }
        public string PictureSourceAddress { get; set; }

        // to do: start time, end time, time to next picture

    }
}