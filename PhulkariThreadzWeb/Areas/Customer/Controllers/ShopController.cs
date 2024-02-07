
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using PhulkariThreadz.Models.ViewModels;
using PhulkariThreadz.Utility;
using System.Security.Claims;

namespace PhulkariThreadzWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShopController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public MiniCartViewModel MiniCartVM { get; set; }
        public ShopController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        

        public IActionResult Index()
        {
            List<ProductsHomeVM> productsHomeVM = new List<ProductsHomeVM>();
            List<Product> products = new List<Product>();
            products = _unitOfWork.Product.GetAll().ToList();
            List<ProductImage> productImages = new List<ProductImage>();

            //Int32 productId = 0;
            //foreach (var item in products)
            //{
            //    productId = item.ProductId;
            //    break;
            //}
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
                productRatings = _unitOfWork.ProductRatings.GetFirstOrDefault(x => x.ProductId == item.ProductId);
                tt.RatingCount = (productRatings == null) ? Convert.ToDecimal(5.0) : productRatings.RatingCount;

                //foreach (var CatItem in CategoryList)
                //{
                //    CategoryHomeVM catVM = new CategoryHomeVM();
                //    catVM.Id = CatItem.Id;
                //    catVM.Name = CatItem.Name;
                //    catVM.SubCategories = _unitOfWork.SubCategory.GetAll(x => x.CategoryId == CatItem.Id).ToList();
                //    tt.Categories = new List<CategoryHomeVM> { };
                //}

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


            TempData["ProductCount"] = productsHomeVM.Count;

            return View(productsHomeVM);

        }

        public IActionResult ProductDetailView(int productId)
        {

            ProductImage Images = _unitOfWork.ProductImages.GetFirstOrDefault(x => x.ProductId == productId && x.IsMainImage == true);
            ProductVM productVM = new()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.ProductId == productId, includeProperties: "SubCategory"),
                ProductImages = _unitOfWork.ProductImages.GetAll(x => x.ProductId == productId).ToList(),
                MainImageUrl = Images.ImageUrl,
                RatingCount = (_unitOfWork.ProductRatings.GetFirstOrDefault(x => x.ProductId == productId) == null) ? Convert.ToDecimal(5.0) : _unitOfWork.ProductRatings.GetFirstOrDefault(x => x.ProductId == productId).RatingCount,
            };


            return View(productVM);
        }



        public IActionResult Cart()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            List<ShoppingCart> cartFromDb = _unitOfWork.ShoppingCart.GetAll(
                  u => u.ApplicationUserId == claim.Value).ToList();

            if (cartFromDb.Count>0)
            {
                MiniCartVM = new MiniCartViewModel
                {
                    ListCart = _unitOfWork.ShoppingCart.GetAll(U => U.ApplicationUserId == claim.Value, includeProperties: "Product").ToList(),

                };
                foreach (var cart in MiniCartVM.ListCart)
                {
                    cart.Price = cart.Product.Price;
                    cart.MainImageUrl = _unitOfWork.ProductImages.GetFirstOrDefault(u => u.ProductId == cart.ProductId && u.IsMainImage == true).ImageUrl;
                    MiniCartVM.TotalPrice += (cart.Price * cart.Count);
                }

                return View(MiniCartVM);
            }


            return View(MiniCartVM);
        }

        [HttpPost]
        [ActionName("Cart")]
        public IActionResult CartPost(string submitButton, string clearAll, string delete)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(clearAll))
            {
                IEnumerable<ShoppingCart> cartFromDb = _unitOfWork.ShoppingCart.GetAll(
                   u => u.ApplicationUserId == claim.Value);

                _unitOfWork.ShoppingCart.RemoveRange(cartFromDb);
                _unitOfWork.Save();
                return View(MiniCartVM);
            }

            else if (!String.IsNullOrEmpty(delete))
            {
                Int32 Id = Convert.ToInt32(delete.Split("(")[1].Split(")")[0]);


                List<ShoppingCart> CartList = _unitOfWork.ShoppingCart.GetAll().ToList();

                foreach (var item in CartList)
                {
                    if (item.Id == Id)
                    {
                        _unitOfWork.ShoppingCart.Remove(item);
                        _unitOfWork.Save();
                        break;
                    }
                }

            }
            else if (!String.IsNullOrEmpty(submitButton))
            {
                var ProductId = submitButton.Split("(")[1].Split(")")[0];
                string Case = submitButton.Split("(")[0].ToString();


                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    u => u.ApplicationUserId == claim.Value && u.ProductId == Convert.ToInt32(ProductId));

                switch (Case)
                {
                    case "increase":
                        _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, 1);
                        _unitOfWork.Save();
                        break;
                    case "decrease":
                        if (cartFromDb.Count == 1)
                        {
                            _unitOfWork.ShoppingCart.Remove(cartFromDb);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            _unitOfWork.ShoppingCart.DecrementCount(cartFromDb, 1);
                            _unitOfWork.Save();
                        }
                        break;

                    default:
                        break;
                }

            }
            MiniCartVM = new MiniCartViewModel
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(U => U.ApplicationUserId == claim.Value, includeProperties: "Product").ToList(),

            };
            foreach (var cart in MiniCartVM.ListCart)
            {
                cart.Price = cart.Product.Price;
                cart.MainImageUrl = _unitOfWork.ProductImages.GetFirstOrDefault(u => u.ProductId == cart.ProductId && u.IsMainImage == true).ImageUrl;
                MiniCartVM.TotalPrice += (cart.Price * cart.Count);
            }

            return View(MiniCartVM);


        }



        public IActionResult EmptyCart()
        {

            return View();

        }


        #region API CALLS
        public class RemoveSubCat
        {
            public int ID { get; set; }
        }
        [HttpGet]
        public IActionResult ProductBox01(int Value, string sortby, string subCatList)
        {
            List<RemoveSubCat> SubCatList = new List<RemoveSubCat>();
            var List = JsonConvert.DeserializeObject<List<RemoveSubCat>>(subCatList);
            if (List != null)
            {
                foreach (var item in List)
                {
                    RemoveSubCat subCatItem = new RemoveSubCat();
                    subCatItem.ID = item.ID;
                    SubCatList.Add(subCatItem);
                }
            }

            List<ProductsHomeVM> productsHomeVM = new List<ProductsHomeVM>();
            List<Product> products = new List<Product>();
            products = _unitOfWork.Product.GetAll().ToList();
            List<ProductImage> productImages = new List<ProductImage>();


            Int32 productId = 0;
            foreach (var item in products)
            {
                productId = item.ProductId;
                break;
            }
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
                tt.TotalCount = products.Count;
                productsHomeVM.Add(tt);
            }


            List<ProductsHomeVM> filteredList = new List<ProductsHomeVM>();
            filteredList = productsHomeVM.Skip(0).Take(Value).ToList();

            if (SubCatList != null)
            {
                foreach (var subCat in SubCatList)
                {
                    bool contains = filteredList.Any(p => p.SubCategoryId == subCat.ID);
                    if (contains)
                    {
                        var itemToRemove = filteredList.Single(r => r.SubCategoryId == subCat.ID);
                        if (itemToRemove != null)
                        {
                            filteredList.Remove(itemToRemove);
                        }
                    }
                }
            }

            if (sortby == "rating")
            {
                return PartialView("_ProductView", filteredList.OrderBy(u => u.RatingCount).ToList());
            }
            if (sortby == "price")
            {
                return PartialView("_ProductView", filteredList.OrderBy(u => u.Price).ToList());
            }


            return PartialView("_ProductView", filteredList);
        }


        [HttpGet]
        public IActionResult CategoryListBox()
        {
            List<CategoryHomeVM> CategoryHomeVM = new List<CategoryHomeVM>();
            List<Category> CategoryList = _unitOfWork.Category.GetAll().ToList();
            List<Product> Products = _unitOfWork.Product.GetAll(includeProperties: "SubCategory").ToList();

            foreach (var item in CategoryList)
            {
                List<SubCategory> SubCategoryList = _unitOfWork.SubCategory.GetAll(x => x.CategoryId == item.Id).ToList();
                CategoryHomeVM CM = new CategoryHomeVM();
                CM.Id = item.Id;
                CM.Name = item.Name;
                CM.SubCategories = (from p in Products
                                    join s in SubCategoryList
                                     on p.SubCategoryId equals s.SubCategoryId
                                    select s).ToList();
                //CM.SubCategories = _unitOfWork.SubCategory.GetAll(x => x.CategoryId == item.Id).ToList();
                CategoryHomeVM.Add(CM);
            }

            return PartialView("_CategoryListView", CategoryHomeVM);
        }

        [HttpGet]

        public IActionResult QuickViewModalBox(int productId)
        {
            ProductImage Images = _unitOfWork.ProductImages.GetFirstOrDefault(x => x.ProductId == productId && x.IsMainImage == true);
            ProductVM productVM = new()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.ProductId == productId, includeProperties: "SubCategory"),
                ProductImages = _unitOfWork.ProductImages.GetAll(x => x.ProductId == productId).ToList(),
                MainImageUrl = Images.ImageUrl
            };

            return PartialView("_QuickViewModal", productVM);
        }


        [HttpGet]
        public IActionResult MiniCartView()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            MiniCartVM = new MiniCartViewModel   
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(U => U.ApplicationUserId == claim.Value, includeProperties: "Product").ToList(),
               
            };
            foreach (var cart in MiniCartVM.ListCart)
            {
                cart.Price =  cart.Product.Price;
                cart.MainImageUrl = _unitOfWork.ProductImages.GetFirstOrDefault(u => u.ProductId == cart.ProductId && u.IsMainImage == true).ImageUrl;
                MiniCartVM.TotalPrice += (cart.Price * cart.Count);
            }

            return PartialView("_MiniCartView", MiniCartVM);
        }


        [HttpGet]
        public IActionResult GetCartCount()
        {

            //Getting user Id from login 
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            decimal TotalPrice = 0;
            int CartCount = 0;
            List<ShoppingCart> CartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product").ToList();

            if (CartList != null)
            {

                foreach (var item in CartList)
                {
                    item.Price = item.Product.Price;
                }
                foreach (var item in CartList)
                {
                    TotalPrice += item.Price * item.Count;

                }

                CartCount = CartList.Count;
            }


            return Json(new { CartCount = CartCount, TotalPrice = TotalPrice });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToCart(Int32 ProductId, int Count)
        {

            //Getting user Id from login 
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            ShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.ProductId = ProductId;
            shoppingCart.Count = Count;
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);
            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                //HttpContext.Session.SetInt32(SD.SessionCart,
                //    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);

            }
            else
            {

                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, Count);
                _unitOfWork.Save();
            }

            decimal TotalPrice = 0;
            int CartCount = 0;
            List<ShoppingCart> CartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product").ToList();

            if (CartList != null)
            {

                foreach (var item in CartList)
                {
                    item.Price = item.Product.Price;
                }
                foreach (var item in CartList)
                {

                    TotalPrice += item.Price * item.Count;

                }

                CartCount = CartList.Count;
            }
            return Json(new { Message = "Success", CartCount = CartCount, TotalPrice = TotalPrice });
        }


        [HttpDelete]

        public IActionResult RemoveFromCart(int Id)
        {

            List<ShoppingCart> CartList = _unitOfWork.ShoppingCart.GetAll().ToList();

            foreach (var item in CartList)
            {
                if(item.Id == Id)
                {
                    _unitOfWork.ShoppingCart.Remove(item);
                    _unitOfWork.Save();
                    break;
                }
            }


            return Json(new { Message = "Success"});
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToWishlist(Int32 ProductId)
        {
            //Getting user Id from login 
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<UserWishList> Wishlist = _unitOfWork.Wishlist.GetAll(u=> u.ApplicationUserId == claim.Value).ToList();
            UserWishList wl = new UserWishList();
            bool IfExist = false;
            if (Wishlist != null)
            {
                foreach (var item in Wishlist)
                {
                    if(item.ProductId == ProductId)
                    {
                        //item already added in wishlist
                        IfExist = true;
                        break;
                    }                        
                   
                }
            }
            if (!IfExist)
            {
                wl.ProductId = ProductId;
                wl.ApplicationUserId = claim.Value;
                _unitOfWork.Wishlist.Add(wl);
                _unitOfWork.Save();
            }

            List<UserWishList> ReturningWL = _unitOfWork.Wishlist.GetAll(u => u.ApplicationUserId == claim.Value).ToList();

            return Json(new { Message = "Success", Wishlist = ReturningWL });
        }




        #endregion


    }
}
