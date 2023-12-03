using AutoMapper;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using PL.Helpers;
using PL.ViewModels;

namespace PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var employees =  _unitOfWork.EmployeeRepository.GetAll();
            var employessVM = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees); 
            return View(employessVM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ImageName = DocumentSetting.UploadFile(model.ImageFile, "images");
                var employee = _mapper.Map<EmployeeViewModel, Employee>(model);
                _unitOfWork.EmployeeRepository.Add(employee);
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
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee is null)
                return NotFound();
            var departmentVM = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(departmentVM);
        }
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee is null)
                return NotFound();
            var employeeVM = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(employeeVM);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(employee);
                    int numberOfRowsEffected = _unitOfWork.Complete();
                    if (numberOfRowsEffected > 0)
                        return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }


        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee is null)
                return NotFound();
            var employeeVM = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(employeeVM);
        }
        [HttpPost]
        public IActionResult Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Delete(employee);
                    int numberOfRowsEffected = _unitOfWork.Complete();
                    if (numberOfRowsEffected > 0)
                    {
                        DocumentSetting.DeleteFile(employeeVM.ImageName, "images");
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }
    }
}
