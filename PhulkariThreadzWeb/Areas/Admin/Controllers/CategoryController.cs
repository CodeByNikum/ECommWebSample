using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using PhulkariThreadz.Utility;

namespace PhulkariThreadzWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> _objCategoryList = _unitOfWork.Category.GetAll();
            return View(_objCategoryList);

        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category cat)
        {
            
            if (ModelState.IsValid)
            {

                _unitOfWork.Category.Add(cat);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }

            return View(cat);

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var Category = _db.Categories.Find(id);
            var Category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
           
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(cat);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }

            return View(cat);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var Category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var cat = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (cat == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(cat);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index");

        }

    }
}
