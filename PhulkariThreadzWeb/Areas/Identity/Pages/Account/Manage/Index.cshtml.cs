// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using PhulkariThreadz.Utility;

namespace PhulkariThreadzWeb.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork; 
        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            /// <summary>
            /// Added On 16 September, 2022
            /// </summary>
            /// Start Add;
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            public string Email { get; set; }   

            //[Required]
            //[Display(Name = "Flat, House no., Building, Company, Apartment")]
            //public string Address1 { get; set; }
            //[Required]
            //[Display(Name = "Area, Street, Sector, Village")]
            //public string Address2 { get; set; }
            //[Required]
            //public string City { get; set; }
            //[Required]
            //public string State { get; set; }
            //[Required]
            //public string Country { get; set; }
            //[Required]
            //public Int32 PinCode { get; set; }
            //public bool IsDefault { get; set; }
            public string Id { get; set; }
            ///Add End;
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var UserId = user.Id;
            var FirstName = user.UserName;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var Email = await _userManager.GetEmailAsync(user);

            UserDetail UserDetailObj = new UserDetail();

            UserDetailObj = _unitOfWork.UserDetails.GetAll(x => x.Id == user.Id, includeProperties: "IdentityUser").FirstOrDefault();

            Username = userName;

            if (UserDetailObj != null)
            {
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber,
                    FirstName = UserDetailObj.FirstName,
                    LastName = UserDetailObj.LastName,
                    Email = Email,
                    Id = user.Id,
                };
            }
            else
            {
                Input = new InputModel
                {
                    FirstName = FirstName,
                    PhoneNumber = phoneNumber,
                    Email = Email,
                    Id = user.Id,
                };
            }

            
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                return RedirectToRoute("loginRoute");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var Email = await _userManager.GetEmailAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.Email != Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email); 
                if (!setEmailResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set email.";
                    return RedirectToPage();
                }
            }
            UserDetail UserDetailObj = new UserDetail();
            var UserDetail = _unitOfWork.UserDetails.GetFirstOrDefault(u => u.Id == user.Id);

            if(UserDetail == null)
            {
                user.UserName = Input.FirstName;
                UserDetailObj.FirstName = Input.FirstName;
                UserDetailObj.LastName = Input.LastName;

                UserDetailObj.Id = user.Id;

                _unitOfWork.UserDetails.Add(UserDetailObj);
                _unitOfWork.Save();
            }
            else
            {
                user.UserName = Input.FirstName;
                UserDetailObj.FirstName = Input.FirstName;
                UserDetailObj.LastName = Input.LastName;
                UserDetailObj.Id = user.Id;

                _unitOfWork.UserDetails.Update(UserDetailObj);
                _unitOfWork.Save();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your details has been updated";
            return RedirectToPage();
        }





    }
}
