using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using PhulkariThreadz.Utility;

namespace PhulkariThreadzWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SettingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public SettingsController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region APICALLS
        [HttpPost]
        public IActionResult AddBannerImages(List<IFormFile> files)
        {
            List<BannerImages> obj = new List<BannerImages>();

            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (files.Count > 0)
            {

                foreach (var aformFile in files)
                {
                    var formFile = aformFile;
                    if (formFile.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"Images\Banners");
                        var extension = Path.GetExtension(formFile.FileName);

                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            formFile.CopyTo(fileStreams);
                        }

                        obj.Add(new BannerImages
                        {
                            ImageURl = @"\Images\Banners\" + fileName + extension,
                            BannerText = "", BannerSubText = "", DisplayOrder =1,
                            BannerTextFontColor ="", BannerSubTextFontColor= "", Link =""
                        });
                    }

                }

                _unitOfWork.BannerImages.AddMultiple(obj);
                _unitOfWork.Save();

            }

            return Json(new { success = true, message = "Banner Images added successfully" });
        }

        [HttpGet]
        public IActionResult GetAllImages(int id)
        {
            List<BannerImages> ImageList = new List<BannerImages>();

            ImageList = _unitOfWork.BannerImages.GetAll().ToList();

            return Json(new { data = ImageList });
        }

        [HttpDelete]
        public IActionResult DeleteImage(int? id)
        {

            var obj = _unitOfWork.BannerImages.GetFirstOrDefault(u => u.BannerImageId == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageURl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.BannerImages.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Image deleted successfully" });

        }

        [HttpPost]
        public IActionResult SaveImageDetail(int BannerImageId, int DisplayOrder, string Link)
        {
            BannerImages ImgObj = _unitOfWork.BannerImages.GetFirstOrDefault(u => u.BannerImageId == BannerImageId);

            ImgObj.DisplayOrder = DisplayOrder;
            //ImgObj.BannerText = BannerText == null ? "" : BannerText;
            //ImgObj.BannerSubText = BannerSubText == null ? "" : BannerSubText;
            ImgObj.Link = Link == null ? "" : Link;

            _unitOfWork.BannerImages.Update(ImgObj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Details upated successfully" });
        }

        #endregion

    }
}
