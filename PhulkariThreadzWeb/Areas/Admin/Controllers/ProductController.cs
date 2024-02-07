using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models.ViewModels;
using PhulkariThreadz.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhulkariThreadz.Models;

namespace PhulkariThreadzWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == id).ToList()
            };

            if (id == null || id == 0)
            {
                //Create product
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.ProductId == id);
                TempData["SubCategoryId"] = productVM.Product.SubCategoryId;

                SubCategory subCat = _unitOfWork.SubCategory.GetFirstOrDefault(i => i.SubCategoryId == productVM.Product.SubCategoryId);
                TempData["CategoryId"] = subCat.CategoryId;
                return View(productVM);
                
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj)
        {
           
            if (ModelState.IsValid)
            {
                if (obj.Product.ProductId == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    _unitOfWork.Save();
                    if (obj.File != null)
                    {
                        SaveImage(obj.File, obj.Product.ProductId);
                    }   
                    TempData["success"] = "Product created successfully!";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    _unitOfWork.Save();
                    if (obj.File != null)
                    {
                        SaveImage(obj.File, obj.Product.ProductId);
                    }
                    TempData["success"] = "Product updated successfully!";
                }
                return RedirectToAction("Index");

            }
            ProductVM productVM = new()
            {
                Product = new(),
                ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == obj.Product.ProductId).ToList()

            };
            return View(productVM);
        }

        public void SaveImage(IFormFile File, int ProductId)
        {
            ProductImage ImgObj = new ProductImage();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var formFile = File;
            if (formFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"Images\Products");
                var extension = Path.GetExtension(formFile.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    formFile.CopyTo(fileStreams);
                }
                ImgObj.ImageUrl = @"\Images\Products\" + fileName + extension;
                ImgObj.ProductId = ProductId;
                _unitOfWork.ProductImages.Add(ImgObj);
                _unitOfWork.Save();
            }

        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetCategoryList()
        {
            List<Category> Category = new List<Category>();
            var CategoryAll = _unitOfWork.Category.GetAll();

            foreach (var item in CategoryAll)
            {
                Category.Add(item);
            }

            return Json(new { data = Category });
        }


        [HttpGet]
        public IActionResult GetDropdownList(int? id)
        {
            var SubCategoryAll = _unitOfWork.SubCategory.GetAll(includeProperties: "Category");
            List<SubCategory> SubCategoryList = new List<SubCategory>();


            foreach (var item in SubCategoryAll)
            {
                if(item.CategoryId == id)
                {
                    SubCategoryList.Add(item);                
                }
            }

            return Json(new { data = SubCategoryList });
        }


        [HttpGet]
        public IActionResult GetAll(string? searchkey, int? subCatId)
        {
            

            if(!string.IsNullOrEmpty(searchkey) && subCatId==null)
            {
                var productList = _unitOfWork.Product.GetAll(x => x.ProductName.Contains(searchkey) || x.ProductDescription.Contains(searchkey), includeProperties: "SubCategory");
                return Json(new { data = productList });
            }
            else if(subCatId != null && string.IsNullOrEmpty(searchkey))
            {
                var productList = _unitOfWork.Product.GetAll(x => x.SubCategoryId == subCatId ,includeProperties: "SubCategory");
                return Json(new { data = productList });
            }
            else if(subCatId != null && !string.IsNullOrEmpty(searchkey))
            {
                var productList = _unitOfWork.Product.GetAll(x => (x.ProductName.Contains(searchkey) || x.ProductDescription.Contains(searchkey)) && x.SubCategoryId == subCatId, includeProperties: "SubCategory");
                return Json(new { data = productList });
            }
            else
            {
                var productList = _unitOfWork.Product.GetAll(includeProperties: "SubCategory");
                return Json(new { data = productList });
            }
        }

        [HttpDelete]

        public IActionResult Delete(int? id)
        {

            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.ProductId == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted successfully" });

        }




        #endregion


    }

}
