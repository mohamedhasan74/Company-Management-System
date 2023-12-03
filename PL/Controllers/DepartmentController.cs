using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using AutoMapper;
using DAL.Models;
using PL.ViewModels;

namespace PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            var departmentsVM = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(departmentsVM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel model)
        {
            if(ModelState.IsValid)
            {
                var department = _mapper.Map<DepartmentViewModel, Department>(model);
                _unitOfWork.DepartmentRepository.Add(department);
                int numberOfRowEffected = _unitOfWork.Complete();
                if (numberOfRowEffected > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if(department is null)
                return NotFound();
            var departmentVM = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(departmentVM);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null)
                return NotFound();
            var departmentVM = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(departmentVM);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(department);
                    int numberOfRowsEffected = _unitOfWork.Complete();
                    if (numberOfRowsEffected > 0)
                        return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null)
                return NotFound();
            var departmentVM = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(departmentVM);
        }
        [HttpPost]
        public IActionResult Delete([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Delete(department);
                    int numberOfRowsEffected = _unitOfWork.Complete();
                    if (numberOfRowsEffected > 0)
                        return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }
    }
}
