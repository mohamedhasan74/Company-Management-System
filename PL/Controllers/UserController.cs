using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.ViewModels;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchTerm)
        {
            List<UserViewModel> usersList= new List<UserViewModel>();
            if(string.IsNullOrEmpty(searchTerm))
            {
                //var users = await _userManager.Users.ToListAsync();
                //var usersMapped = _mapper.Map<List<ApplicationUser>, List<UserViewModel>>(users);
                //return View(usersMapped);
                var users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    UserName = U.UserName,
                    Email = U.Email,
                    PhoneNumber = U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync();
                return View(users);
            }else
            {
                var user = _userManager.FindByEmailAsync(searchTerm).Result;
                var userMapped = _mapper.Map<ApplicationUser, UserViewModel>(user);
                usersList.Add(userMapped);
                return View(usersList);
            }

        }

        public async Task<IActionResult> Details(string id)
        {
            if (id is null)
                return BadRequest();
            var user = _userManager.FindByIdAsync(id).Result;
            if (user is null)
                return NotFound();
            var userVM = _mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(userVM);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if(id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            var userVM = _mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(userVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.PhoneNumber = userViewModel.PhoneNumber;
                var updateUser = await _userManager.UpdateAsync(user);
                if (updateUser.Succeeded)
                    return RedirectToAction(nameof(Index));
                foreach (var error in updateUser.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(userViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            var userVM = _mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(userVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel userViewModel)
        {
            if(id != userViewModel.Id) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            var delUser = await _userManager.DeleteAsync(user);
            if(delUser.Succeeded) return RedirectToAction(nameof(Index));
            foreach(var error in delUser.Errors)
                ModelState.AddModelError(string.Empty,error.Description);
            return View(userViewModel);
        }
    }
}
