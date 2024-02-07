using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using PhulkariThreadz.Models.ViewModels;
using PhulkariThreadz.Utility;

namespace PhulkariThreadzWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SubCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            SubCategoryVM SubCatVM = new()
            {
                SubCategory = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
           return View(SubCatVM);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubCategoryVM SubCat)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SubCategory.Add(SubCat.SubCategory);
                _unitOfWork.Save();
                TempData["success"] = "Sub Category created successfully!";
                return RedirectToAction("Index");
            }

            return View(SubCat);
        }

        public IActionResult Edit(int? id)
        {
            SubCategoryVM SubCatVM = new()
            {
                SubCategory = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var Category = _db.Categories.Find(id);
            SubCatVM.SubCategory = _unitOfWork.SubCategory.GetFirstOrDefault(u => u.SubCategoryId == id);
          
            return View(SubCatVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SubCategoryVM SubCat)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.SubCategory.Update(SubCat.SubCategory);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }

            return View(SubCat);

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string? searchkey)
        {
            object SubCategoryList;
            if (searchkey != null)
            {
                SubCategoryList = _unitOfWork.SubCategory.GetAll(x=> x.SubCategoryName.Contains(searchkey)|| x.Category.Name.Contains(searchkey), includeProperties: "Category");
            }
            else
            {
                SubCategoryList = _unitOfWork.SubCategory.GetAll(includeProperties: "Category");
            }
            return Json(new { data = SubCategoryList });
        }

        [HttpDelete]

        public IActionResult Delete(int? id)
        {

            var obj = _unitOfWork.SubCategory.GetFirstOrDefault(u => u.SubCategoryId == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            
            _unitOfWork.SubCategory.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted successfully" });

        }

        #endregion

    }
}
