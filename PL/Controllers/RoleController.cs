using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.ViewModels;
using System.Data;

namespace PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesMapped = _mapper.Map<List<IdentityRole>, List<RoleViewModel>>(roles);
            return View(rolesMapped);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var role = _mapper.Map<RoleViewModel, IdentityRole>(model);
                var roleCreated = await _roleManager.CreateAsync(role);
                if (roleCreated.Succeeded)
                    return RedirectToAction(nameof(Index));
                foreach (var error in roleCreated.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id is null) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if(role is null) return NotFound();
            var roleMapped = _mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(roleMapped);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            var roleMapped = _mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(roleMapped);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if(id != model.Id) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            role.Name = model.Name;
            var roleUpdate = await _roleManager.UpdateAsync(role);
            if (roleUpdate.Succeeded)
                return RedirectToAction(nameof(Index));
            foreach(var error in roleUpdate.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id is null) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            var roleMapped = _mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(roleMapped);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            var roleDeleted = await _roleManager.DeleteAsync(role);
            if (roleDeleted.Succeeded)
                return RedirectToAction(nameof(Index));
            foreach (var error in roleDeleted.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }
        public async Task<IActionResult> AddOrRemoveUsersInRole(string roleId)
        {
            if(roleId is null) return BadRequest();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            var usersInRole = new List<UserInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                UserInRoleViewModel userInRole = new UserInRoleViewModel()
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                };
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }else
                {
                    userInRole.IsSelected= false;
                }
                usersInRole.Add(userInRole);
            }
            ViewBag.roleId = roleId;
            return View(usersInRole);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveUsersInRole(List<UserInRoleViewModel> userInRoleViewModels, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return BadRequest();
            if (ModelState.IsValid)
            {
                foreach (var user in userInRoleViewModels)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (user.IsSelected && !(await _userManager.IsInRoleAsync(appUser, role.Name)))
                    {
                        var roleAdd = await _userManager.AddToRoleAsync(appUser, role.Name);
                        if (!roleAdd.Succeeded)
                        {
                            foreach (var error in roleAdd.Errors)
                                ModelState.AddModelError(string.Empty, error.Description);
                            return View(userInRoleViewModels);
                        }
                    }
                    else if (!user.IsSelected && (await _userManager.IsInRoleAsync(appUser, role.Name)))
                    {
                        var roleRemove = await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        if (!roleRemove.Succeeded)
                        {
                            foreach (var error in roleRemove.Errors)
                                ModelState.AddModelError(string.Empty, error.Description);
                            return View(userInRoleViewModels);
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userInRoleViewModels);
        }
    }
}
