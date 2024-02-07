using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using PhulkariThreadz.Utility;
using System.ComponentModel.DataAnnotations;

namespace PhulkariThreadzWeb.Areas.Identity.Pages.Account.Manage
{
    public class AccountAddressModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountAddressModel(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Flat, House no., Building, Company, Apartment")]
            public string Address1 { get; set; }
            [Required]
            [Display(Name = "Area, Street, Sector, Village")]
            public string Address2 { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string State { get; set; }
            [Required]
            public string Country { get; set; }
            [Required]
            public Int32 PinCode { get; set; }
            
            public bool IsDefault { get; set; }
            public string Id { get; set; }


        }

        //public void Load(Task<IdentityUser> user)
        //{
        //    List<UserAddress> addresses = new List<UserAddress>();
        //    addresses = _unitOfWork.UserAddresses.GetAll(x => x.Id == user.Id.ToString()).ToList();

        //    if (addresses != null)
        //    {
        //        foreach (var input in Input)
        //        {
        //            foreach (var add in addresses)
        //            {
        //                input.Address1 = add.Address1;
        //                input.Address2 = add.Address2;
        //                input.City = add.City;
        //                input.State = add.State;
        //                input.Country = add.Country;
        //                input.PinCode = add.PinCode;
        //                input.IsDefault = add.IsDefault;
        //                input.Id = user.Id.ToString();
        //            }

        //        }
        //    }
        //    else
        //    {
        //        foreach (var input in Input)
        //        {
        //            input.Id = user.Id.ToString();
        //        }
        //    }
        //}


        public IActionResult OnGet()
        {
            var user = _userManager.GetUserId(User);

            if (user == null)
            {
                //return NotFound($"Please Login!.");
                return RedirectToRoute("loginRoute");
            }

            //Load(user);
            Input = new InputModel
            {
                Id = Convert.ToString(_userManager.GetUserId(User)),
            };
            return Page();

        }


        #region API CALLS

        public IActionResult OnDeleteAddressDelete(int UserAddressId, string UserId)
        {
            UserAddress userAddress = _unitOfWork.UserAddresses.GetFirstOrDefault(u => u.Id == UserId && u.UserAddress_Id == UserAddressId);

            _unitOfWork.UserAddresses.Remove(userAddress);
            _unitOfWork.Save();


            return new JsonResult("Success");
        }

        public IActionResult OnPostAddressSave(UserAddress data)
        {

            if (data.IsDefault)
            {
                List<UserAddress> userAddresses = _unitOfWork.UserAddresses.GetAll(u => u.Id == data.Id, null, false).ToList();
                if (userAddresses!= null)
                {
                    foreach (var item in userAddresses)
                    {
                        item.IsDefault = false;
                    }
                    _unitOfWork.UserAddresses.BulkUpdate(userAddresses);
                    //_unitOfWork.Save();
                }
                
            }

            _unitOfWork.UserAddresses.Add(data);
            _unitOfWork.Save();

            return new JsonResult("Success");
        }

        public IActionResult OnPostAddressUpdate(UserAddress data)
        {
            if (data.IsDefault)
            {
                List<UserAddress> userAddresses = _unitOfWork.UserAddresses.GetAll(u => u.Id == data.Id, null, false).ToList();
                if (userAddresses != null)
                {
                    foreach (var item in userAddresses)
                    {

                        if(item.UserAddress_Id != data.UserAddress_Id)
                        {
                            item.IsDefault = false;
                        }
                        else
                        {
                            item.Address1 = data.Address1;
                            item.Address2 = data.Address2;
                            item.City = data.City;
                            item.State = data.State;
                            item.Country = data.Country;
                            item.Id = data.Id;
                            item.PinCode = data.PinCode;
                            item.IsDefault = data.IsDefault;
                        }
                    }
                    _unitOfWork.UserAddresses.BulkUpdate(userAddresses);
                    _unitOfWork.Save();
                }

            }
            //_unitOfWork.UserAddresses.Update(data);
            

            return new JsonResult("Success");
        }

        public IActionResult OnGetAddressList()
        {
            List<UserAddress> AddressList = new List<UserAddress>();

            var user = _userManager.GetUserId(User);
            if (user == null)
            {
                return RedirectToRoute("loginRoute");

            }

            string userID = Convert.ToString(_userManager.GetUserId(User));

            AddressList = _unitOfWork.UserAddresses.GetAll(u=> u.Id == userID, null, false).ToList();

            return new JsonResult(AddressList);
        }

        public IActionResult OnGetCountryList()
        {
            List<Country> countries = new List<Country>();


            countries.Add(new Country
            {
                Name = Countries.Australia,
                Value = Countries.Australia
            });
            countries.Add(new Country
            {
                Name = Countries.Bangladesh,
                Value = Countries.Bangladesh
            });
            countries.Add(new Country
            {
                Name = Countries.Canada,
                Value = Countries.Canada
            });
            countries.Add(new Country
            {
                Name = Countries.Germany,
                Value = Countries.Germany
            });
            countries.Add(new Country
            {
                Name = Countries.India,
                Value = Countries.India
            });
            countries.Add(new Country
            {
                Name = Countries.New_Zealand,
                Value = Countries.New_Zealand
            });
            countries.Add(new Country
            {
                Name = Countries.Pakistan,
                Value = Countries.Pakistan
            });
            countries.Add(new Country
            {
                Name = Countries.United_States,
                Value = Countries.United_States
            });
            countries.Add(new Country
            {
                Name = Countries.United_Kingdom,
                Value = Countries.United_Kingdom
            });


            return new JsonResult(countries);
        }

        public IActionResult OnGetStateList()
        {
            List<State> states = new List<State>();
            states.Add(new State
            {
                Name = IndiaStates.Andhra_Pradesh,
                Value = IndiaStates.Andhra_Pradesh
            });
            states.Add(new State
            {
                Name = IndiaStates.Arunachal_Pradesh,
                Value = IndiaStates.Arunachal_Pradesh
            });
            states.Add(new State
            {
                Name = IndiaStates.Andaman_and_Nicobar_Islands,
                Value = IndiaStates.Andaman_and_Nicobar_Islands
            });

            states.Add(new State
            {
                Name = IndiaStates.Assam,
                Value = IndiaStates.Assam
            });
            states.Add(new State
            {
                Name = IndiaStates.Bihar,
                Value = IndiaStates.Bihar
            });
            states.Add(new State
            {
                Name = IndiaStates.Chhattisgarh,
                Value = IndiaStates.Chhattisgarh
            });
            states.Add(new State
            {
                Name = IndiaStates.Chandigarh,
                Value = IndiaStates.Chandigarh
            });
            states.Add(new State
            {
                Name = IndiaStates.Dadra_and_Nagar_Haveli,
                Value = IndiaStates.Dadra_and_Nagar_Haveli
            });
            states.Add(new State
            {
                Name = IndiaStates.Daman_and_Diu,
                Value = IndiaStates.Daman_and_Diu
            });
            states.Add(new State
            {
                Name = IndiaStates.Delhi,
                Value = IndiaStates.Delhi
            });
            states.Add(new State
            {
                Name = IndiaStates.Goa,
                Value = IndiaStates.Goa
            });
            states.Add(new State
            {
                Name = IndiaStates.Gujarat,
                Value = IndiaStates.Gujarat
            });
            states.Add(new State
            {
                Name = IndiaStates.Haryana,
                Value = IndiaStates.Haryana
            });
            states.Add(new State
            {
                Name = IndiaStates.Himachal_Pradesh,
                Value = IndiaStates.Himachal_Pradesh
            });
            states.Add(new State
            {
                Name = IndiaStates.Jammu_and_Kashmir,
                Value = IndiaStates.Jammu_and_Kashmir
            });
            states.Add(new State
            {
                Name = IndiaStates.Jharkhand,
                Value = IndiaStates.Jharkhand
            });
            states.Add(new State
            {
                Name = IndiaStates.Karnataka,
                Value = IndiaStates.Karnataka
            });
            states.Add(new State
            {
                Name = IndiaStates.Kerala,
                Value = IndiaStates.Kerala
            });
            states.Add(new State
            {
                Name = IndiaStates.Lakshadweep,
                Value = IndiaStates.Lakshadweep
            });
            states.Add(new State
            {
                Name = IndiaStates.Madhya_Pradesh,
                Value = IndiaStates.Madhya_Pradesh
            });
            states.Add(new State
            {
                Name = IndiaStates.Maharashtra,
                Value = IndiaStates.Maharashtra
            });
            states.Add(new State
            {
                Name = IndiaStates.Manipur,
                Value = IndiaStates.Manipur
            });
            states.Add(new State
            {
                Name = IndiaStates.Meghalaya,
                Value = IndiaStates.Meghalaya
            });
            states.Add(new State
            {
                Name = IndiaStates.Mizoram,
                Value = IndiaStates.Mizoram
            });
            states.Add(new State
            {
                Name = IndiaStates.Nagaland,
                Value = IndiaStates.Nagaland
            });
            states.Add(new State
            {
                Name = IndiaStates.Odisha,
                Value = IndiaStates.Odisha
            });
            states.Add(new State
            {
                Name = IndiaStates.Punjab,
                Value = IndiaStates.Punjab
            });
            states.Add(new State
            {
                Name = IndiaStates.Puducherry,
                Value = IndiaStates.Puducherry
            });
            states.Add(new State
            {
                Name = IndiaStates.Rajasthan,
                Value = IndiaStates.Rajasthan
            });
            states.Add(new State
            {
                Name = IndiaStates.Sikkim,
                Value = IndiaStates.Sikkim
            });
            states.Add(new State
            {
                Name = IndiaStates.Tamil_Nadu,
                Value = IndiaStates.Tamil_Nadu
            });
            states.Add(new State
            {
                Name = IndiaStates.Tripura,
                Value = IndiaStates.Tripura
            });
            states.Add(new State
            {
                Name = IndiaStates.Telangana,
                Value = IndiaStates.Telangana
            });
            states.Add(new State
            {
                Name = IndiaStates.Uttar_Pradesh,
                Value = IndiaStates.Uttar_Pradesh
            });
            states.Add(new State
            {
                Name = IndiaStates.Uttarakhand,
                Value = IndiaStates.Uttarakhand
            });
            states.Add(new State
            {
                Name = IndiaStates.West_Bengal,
                Value = IndiaStates.West_Bengal
            });
            return new JsonResult(states);
        }

        public class Country
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        public class State
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        #endregion


        //public IActionResult OnPost()
        //{
        //    var user =  _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound($"Please Login!.");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        Load(user);
        //        return Page();
        //    }

        //    List<UserAddress> addresses = new List<UserAddress>();
        //    addresses = _unitOfWork.UserAddresses.GetAll(x => x.Id == user.Id.ToString()).ToList();

        //    //if (addresses == null)
        //    //{
        //        UserAddress Address = new UserAddress(); 
        //        foreach (var add in addresses)
        //        {
        //            foreach (var item in Input)
        //            {
        //                Address.Address1 = item.Address1;
        //                Address.Address2 = item.Address2;
        //                Address.City = item.City;
        //                Address.State = item.State;
        //                Address.Country = item.Country;
        //                Address.PinCode = item.PinCode;
        //                Address.IsDefault = item.IsDefault;
        //                Address.Id = user.Id.ToString();
        //                addresses.Add(Address);
        //            }

        //        }
        //        _unitOfWork.UserAddresses.AddMultiple(addresses);
        //        _unitOfWork.Save();
        //    //}



        //    StatusMessage = "Your details has been updated";
        //    return RedirectToPage();



        //}

    }
}
