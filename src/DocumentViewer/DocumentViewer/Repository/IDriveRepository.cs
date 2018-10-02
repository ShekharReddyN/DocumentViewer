using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DocumentViewer.Models;

namespace DocumentViewer.Repository
{
    interface IDriveRepository
    {
        DriveModel GetDirectoryFileList(string directoryName,string accessToken);

        DriveModel GetFileList(string acToken, string rootId = "");
        bool DeleteFile(string token,string id);
        bool UploadFile(HttpPostedFileBase file, string v);
        StreamContent GetFile(string id, string acsToken);
    }
}
