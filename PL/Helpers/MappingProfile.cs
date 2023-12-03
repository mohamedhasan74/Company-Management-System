using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using PL.ViewModels;

namespace PL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
        }
    }
}
