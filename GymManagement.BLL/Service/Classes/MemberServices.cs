using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using GymManagement.Models;


namespace GymManagement.BLL.Service.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachementServices _attachementServices;

        public MemberServices(IUnitOfWork unitOfWork, IMapper mapper ,IAttachementServices attachementServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachementServices = attachementServices;
        }

        public async Task<IEnumerable<MembersViewModels>> GetAllMembers(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            return _mapper.Map<IEnumerable<MembersViewModels>>(members);
        }

        public async Task<MembersViewModels?> GetMemberDetailsAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null) return null;

            var memberDetails = _mapper.Map<Member, MembersViewModels>(member);

            var ActiveMembership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(m => m.MemberId == member.Id && m.EndDate > DateTime.Now, ct);
            if (ActiveMembership is not null)
            {
                var ActivePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(ActiveMembership.PlanId, ct);
                memberDetails.PlanName = ActivePlan?.Name;
                memberDetails.MembershipStartDate = ActiveMembership.StartDate.ToShortDateString();
                memberDetails.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();
            }

            return memberDetails;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int id, CancellationToken ct = default)
        {
            var memberHealthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == id, ct);
            if (memberHealthRecord is null) return null;

            return _mapper.Map<HealthRecord, HealthRecordViewModel>(memberHealthRecord);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null) return null;

            return _mapper.Map<Member, MemberToUpdateViewModel>(member);
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            if (await _unitOfWork.GetRepository<Member>().AnyAsync(e => e.Email == model.Email, ct)) return Result.Validation("Email is already exist");
            else if (await _unitOfWork.GetRepository<Member>().AnyAsync(e => e.Email == model.Phone, ct)) return Result.Validation("Phone is already exist");

            var member = _mapper.Map<CreateMemberViewModel, Member>(model);
            var photoName = await _attachementServices.UploadAsync(model.Photo.OpenReadStream(), model.Photo.FileName, "MembersPhotos", ct);

            if (string.IsNullOrEmpty(photoName)) return Result.Fail("Failed to Upload Photo");
            member.Photo = photoName;


            await _unitOfWork.GetRepository<Member>().AddAsync(member, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Create Member");
        }

        public async Task<Result> UpdateMemberAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null) return Result.NotFound("Member Not Found");

            else if (await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != id, ct))
                return Result.Validation("Email is Already Exist");

            else if (await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct))
                return Result.Validation("Phone is Already Exist");

            _mapper.Map(model, member);
            member.UpdatedAt = DateTime.Now;

            await _unitOfWork.GetRepository<Member>().UpdateAsync(member, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Update Member");
        }

        public async Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null) return Result.NotFound("Member Not Found");

            var bookings = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == id && b.Session.EndDate > DateTime.UtcNow, ct);
            if (bookings) return Result.Fail("Can't Delete Member Until Session Ended");

            await _unitOfWork.GetRepository<Member>().DeleteAsync(member, ct);
            if (member.Photo is not null)
                _attachementServices.Delete(member.Phone, "MembersPhotos");

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Fail Delete");
        }
    }
}