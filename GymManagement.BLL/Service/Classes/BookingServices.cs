using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.BookingViewModels;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.BLL.Service.Classes
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberSelectViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            return _mapper.Map<IEnumerable<MemberSelectViewModel>>(members);
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetBookingsForSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBookingsForSessionWithMemberAsync(sessionId, ct);
            return _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
        }

        public async Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(model.SessionId, ct);
            if (session is null) return Result.NotFound("Session Not Found");

            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(model.MemberId, ct);
            if (member is null) return Result.NotFound("Member Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Validation("Cannot book a session that has already started or finished");

            var hasActiveMembership = await _unitOfWork.GetRepository<Membership>()
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate >= DateTime.Now, ct);
            if (!hasActiveMembership) return Result.Validation("Member must have an active membership to book a session");

            var bookedCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(model.SessionId, ct);
            if (bookedCount >= session.Capacity) return Result.Validation("Session is fully booked");

            var alreadyBooked = await _unitOfWork.GetRepository<Booking>()
                .AnyAsync(b => b.MemberId == model.MemberId && b.SessionId == model.SessionId, ct);
            if (alreadyBooked) return Result.Validation("Member already booked this session");

            var booking = new Booking
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId,
                IsAttended = false
            };

            await _unitOfWork.GetRepository<Booking>().AddAsync(booking, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Create Booking");
        }

        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.GetRepository<Booking>()
                .FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, ct);
            if (booking is null) return Result.NotFound("Booking Not Found");

            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);
            if (session is null) return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Validation("Cannot cancel a booking once the session has started");

            await _unitOfWork.GetRepository<Booking>().DeleteAsync(booking, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Cancel Booking");
        }

        public async Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.GetRepository<Booking>()
                .FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, ct);
            if (booking is null) return Result.NotFound("Booking Not Found");

            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);
            if (session is null) return Result.NotFound("Session Not Found");

            var isOngoing = session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now;
            if (!isOngoing) return Result.Validation("Attendance can only be marked for ongoing sessions");

            booking.IsAttended = true;
            await _unitOfWork.GetRepository<Booking>().UpdateAsync(booking, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Mark Attendance");
        }
    }
}
