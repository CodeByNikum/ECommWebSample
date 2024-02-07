using Microsoft.AspNetCore.Mvc;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using PhulkariThreadz.Models.ViewModels;
using System.Diagnostics;

namespace PhulkariThreadzWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<BannerImages> ImageList = new List<BannerImages>();
            ImageList = _unitOfWork.BannerImages.GetAll().OrderBy(u=> u.DisplayOrder).ToList();

            return View(ImageList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        #region API CALLS

        public IActionResult GetAllCategories()
        {
            List<Category> categories = new List<Category>();

            categories = _unitOfWork.Category.GetAll().ToList();

            return Json(new {data= categories });
        }


        public IActionResult GetProductsByCategory(int Id)
        {
            List<ProductsHomeVM> productsHomeVM = new List<ProductsHomeVM>();
            List<Product> products = new List<Product>();
            products = _unitOfWork.Product.GetAll(u => u.SubCategory.CategoryId == Id).ToList();

            List<ProductImage> productImages = new List<ProductImage>();


            Int32 productId = 0; 
            foreach (var item in products)
            {
                productId = item.ProductId;
                break;
            }

            
            string MainImageURL = "";
            
            foreach (var item in products)
            {
                ProductRating productRatings = new ProductRating();
                ProductsHomeVM tt = new ProductsHomeVM();
                productImages = _unitOfWork.ProductImages.GetAll(x => x.ProductId == item.ProductId).ToList();
                tt.ProductId = item.ProductId;
                tt.ProductName = item.ProductName;
                tt.Price = item.Price;
                tt.ProductDescription = item.ProductDescription;
                tt.ProductImages = productImages;
                tt.SubCategoryId = item.SubCategoryId;
                productRatings = _unitOfWork.ProductRatings.GetFirstOrDefault(x => x.ProductId == item.ProductId);
                tt.RatingCount = (productRatings == null) ? Convert.ToDecimal(5.0) : productRatings.RatingCount;

                foreach (var img in productImages)
                {
                    if (img.IsMainImage)
                    {
                        tt.MainImageUrl = img.ImageUrl;
                        break;
                    }
                }


                productsHomeVM.Add(tt);


            }

           
            return Json(new { data = productsHomeVM });

        }



        #endregion


    }
}