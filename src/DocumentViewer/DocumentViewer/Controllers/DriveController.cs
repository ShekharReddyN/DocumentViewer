using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using DocumentViewer.Models;
using DocumentViewer.Repository;

namespace DocumentViewer.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.Default)]
    //[RoutePrefix("Drive")]
    public class DriveController : Controller
    {
        private readonly IDriveRepository driveRepository;
        
         
        public DriveController()
        {
            driveRepository = new DriveRepository();
        }
        // GET: Drive
        public ActionResult Index(string rootId)
        {

          //  var files = driveRepository.GetDirectoryFileList("Temp", Session["AccessToken"].ToString());

            var fileColl = driveRepository.GetFileList(Session["AccessToken"].ToString(), rootId);

            var viewModelCollection = (from file in fileColl.files
                                      select new FileViewModel()
                                      {
                                          Id = file.id,
                                          Name = file.name,
                                          Type = file.mimeType == "application/vnd.google-apps.folder" ? "Folder" : "File"
                                      }).ToList();



            
            return View(viewModelCollection);
        }

        [HttpPost]
        //[Route("DeleteFile/{id}")]
        public ActionResult DeleteFile(string id)
        {
            var isFileDeleted = driveRepository.DeleteFile(Session["AccessToken"].ToString(),id);
            return View(isFileDeleted);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                   var isFileUploaded = driveRepository.UploadFile(file, Session["AccessToken"].ToString());
                }
                ViewBag.Message = "File Uploaded Successfully!!";
            }
            catch(Exception ex)
            {
                ViewBag.Message = "File upload failed!!";
            }
            return View();
        }

        [HttpPost]
        public ActionResult OpenFolder(string id)
        {
            return RedirectToAction("Index","Drive", new { rootId = id });
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public FileResult DownloadFile(string id)
        {

            var _file = driveRepository.GetFile(id, Session["AccessToken"].ToString());
            var fileResult = new FileContentResult(_file.ReadAsByteArrayAsync().Result, _file.Headers.ContentType.MediaType);
            return fileResult; 



        }
    }
}