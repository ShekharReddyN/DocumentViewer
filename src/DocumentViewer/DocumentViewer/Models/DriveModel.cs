using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentViewer.Models
{
    public class DriveModel
    {
        public string kind { get; set; }
        public bool incompleteSearch { get; set; }

        public string nextPageToken { get; set; }
        public List<DriveFile> files { get; set; }
    }
}