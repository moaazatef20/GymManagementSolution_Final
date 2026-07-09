using AutoMapper;
using GymManagement.BLL.ViewModels.BookingViewModels;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.BLL.ViewModels.PlansViewModels;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using GymManagement.BLL.ViewModels.TrainersViewModels;
using GymManagement.DAL.Models;
using GymManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ConfigureSessionMapping();
            ConfigureTrainerMapping();
            ConfigureMemberMapping();
            ConfigurePlanMapping();
            ConfigureMembershipMapping();
            ConfigureBookingMapping();
        }

        private void ConfigureSessionMapping()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()).ReverseMap();

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }

        private void ConfigureTrainerMapping()
        {
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNo} {src.Address.Street} {src.Address.City}"))
                .ReverseMap();

            CreateMap<Trainer, CreateTrainerViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNo))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ReverseMap();

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNo))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ReverseMap();
        }

        private void ConfigureMemberMapping()
        {
            CreateMap<Member, MembersViewModels>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNo}-{src.Address.Street}-{src.Address.City}"))
                .ForMember(dest => dest.PlanName, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipStartDate, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipEndDate, opt => opt.Ignore());

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNo = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => new HealthRecord
                {
                    BloodType = src.HealthRecordViewModel.BloodType,
                    Weight = src.HealthRecordViewModel.Weight,
                    Height = src.HealthRecordViewModel.Height,
                    Note = src.HealthRecordViewModel.Note
                }));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNo))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.Ignore())   
                .ForMember(dest => dest.Photo, opt => opt.Ignore()); 

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
        }

        private void ConfigurePlanMapping()
        {
            CreateMap<Plan, PlanViewModel>().ReverseMap();
            CreateMap<CreatePlanViewModel, Plan>();
            CreateMap<Plan, UpdatePlanViewModel>().ReverseMap();
        }

        private void ConfigureMembershipMapping()
        {
            CreateMap<Membership, MembershipViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name));

            CreateMap<Member, MemberSelectViewModel>();
            CreateMap<Plan, PlanSelectViewModel>();
        }

        private void ConfigureBookingMapping()
        {
            CreateMap<Booking, MemberForSessionViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreatedAt));

        }
    }
}