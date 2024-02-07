using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using PhulkariThreadz.Models.ViewModels;
using PhulkariThreadz.Utility;

namespace PhulkariThreadzWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductImageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductImageController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(int? productId, string? productName)
        {
            return View();
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetProductList(int SubCatId)
        {
            List<Product> Product = new List<Product>();
            var ProductAll = _unitOfWork.Product.GetAll();

            foreach (var item in ProductAll)
            {
                if(item.SubCategoryId == SubCatId)
                {
                    Product.Add(item);
                }
                
            }

            return Json(new { data = Product });
        }


        [HttpGet]
        public IActionResult GetImagesByProductId(int id)
        {
            List<ProductImage> ProductImages = new List<ProductImage>();
            var ProductImageAll = _unitOfWork.ProductImages.GetAll();

            foreach (var item in ProductImageAll)
            {
                if(item.ProductId == id)
                {
                    ProductImages.Add(item);
                }
            }

            return Json(new { data = ProductImages });
        }

        [HttpPost]

        public IActionResult SaveProductImage(List<IFormFile> files, int ProductId)
        {
            List<ProductImage> obj = new List<ProductImage>();
            
            //obj.IsMainImage = IsMainImage;
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (files.Count >0)
            {

                foreach (var aformFile in files)
                {
                    var formFile = aformFile;
                    if (formFile.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"Images\Products");
                        var extension = Path.GetExtension(formFile.FileName);

                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                          formFile.CopyTo(fileStreams);
                        }

                        obj.Add(new ProductImage
                        {
                            ImageUrl = @"\Images\Products\" + fileName + extension,
                            ProductId = ProductId
                        });
                    }
                   
                }

                //if (obj.ImageUrl != null)
                //{
                //    var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                //    if (System.IO.File.Exists(oldImagePath))
                //    {
                //        System.IO.File.Delete(oldImagePath);
                //    }
                //}
                _unitOfWork.ProductImages.AddMultiple(obj);
                _unitOfWork.Save();

            }

           

            return Json(new { success = true, message = "Image saved successfully" });
        }

        [HttpPost]

        public IActionResult SaveMainImage(int ProductId, int ProductImageId)
        {
            List<ProductImage> list = _unitOfWork.ProductImages.GetAll().ToList();

            foreach (var item in list)
            {
                if(item.ProductId == ProductId)
                {
                    if(item.ProductImgId == ProductImageId)
                    {
                        item.IsMainImage = true;
                    }
                    else
                    {
                        item.IsMainImage = false;
                    }
                }
            }
            _unitOfWork.Save();

            return Json(new { success = true, message = "SUCCESS" });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {

            var obj = _unitOfWork.ProductImages.GetFirstOrDefault(u => u.ProductImgId == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.ProductImages.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Image deleted successfully" });

        }

        [HttpGet]
        public IActionResult CheckMainImage(int ID)
        {
            bool retVal = false;

            var obj = _unitOfWork.ProductImages.GetAll();

            foreach (var item in obj)
            {
                if(item.ProductId == ID)
                {
                    if (item.IsMainImage)
                    {
                       retVal = true;
                       break;
                    }
                }
               
            }

            return Json(new { success = retVal, message = "Image deleted successfully" });

        }


        [HttpGet]
        public IActionResult GetGroupableCategoryList()
        {
            List<SubCategory> SubCategory = new List<SubCategory>();
            var SubCategoryAll = _unitOfWork.SubCategory.GetAll(includeProperties: "Category");

            foreach (var item in SubCategoryAll)
            {
                SubCategory.Add(item);
            }

            return Json(new { data = SubCategoryAll });
        }




        #endregion



    }
}
