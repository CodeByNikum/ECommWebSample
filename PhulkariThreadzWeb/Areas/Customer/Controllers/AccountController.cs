using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using PhulkariThreadz.Models.ViewModels;
using System.Security.Claims;

namespace PhulkariThreadzWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<IdentityUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var user =  _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //var userName = _userManager.GetUserNameAsync(user);
            //var phoneNumber = _userManager.GetPhoneNumberAsync(user);


            return View();
        }


        public IActionResult Wishlist()
        {
            List<ProductsHomeVM> productsHomeVM = new List<ProductsHomeVM>();
            //Getting user Id from login 
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            List<Product> Products = new List<Product>();
            List<UserWishList> Wishlist = _unitOfWork.Wishlist.GetAll(u => u.ApplicationUserId == claim.Value).ToList();
            List<ProductImage> productImages = new List<ProductImage>();

            if (Wishlist != null)
            {
                foreach (var item in Wishlist)
                {
                    Product P = new Product();
                    P = _unitOfWork.Product.GetFirstOrDefault(u => u.ProductId == item.ProductId);
                    Products.Add(P);
                }

                foreach (var item in Products)
                {
                    ProductRating productRatings = new ProductRating();
                    ProductsHomeVM tt = new ProductsHomeVM();
                    productImages = _unitOfWork.ProductImages.GetAll(x => x.ProductId == item.ProductId).ToList();
                    tt.ProductId = item.ProductId;
                    tt.ProductName = item.ProductName;
                    tt.Price = item.Price;
                    tt.ProductDescription = item.ProductDescription;
                    tt.ProductImages = productImages;
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
            }


            return View(productsHomeVM);
        }

        public IActionResult OrderHistory()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddressSave(UserAddress data)
        {





            return new JsonResult("Success");
        }




    }
}
