using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using DriverAPIClient.Repository;

namespace DriverAPIClient.Controllers
{
    public class DriveController : ApiController
    {
        private readonly IDriveRepository _driverRepository = null;
        public DriveController()
        {
            _driverRepository = new DriveRepository();
        }
        public DriveController(IDriveRepository repository)
        {
            this._driverRepository = repository;
        }

        [HttpGet]
        [Route("files/{folderName}")]
        public IHttpActionResult GetDirectoryFileList(string folderName)
        {
            var temp = this._driverRepository.GetDirectoryFileList(folderName);
            var fileCol = from d in temp.files
                          select new { FileName = d.name, FileType = d.mimeType,Id = d.id };
            return Json(fileCol);

        }

    }
}
