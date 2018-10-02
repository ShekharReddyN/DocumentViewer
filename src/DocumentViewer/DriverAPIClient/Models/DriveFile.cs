using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriverAPIClient.Models
{
    public class DriveFile
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string mimeType { get; set; }
        public List<string> parents { get; set; }
    }
}