using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.DoctorServices.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Services.DoctorServices.Queries.AllDoctors
{
    public class AllDoctorsQueryHandler : IRequestHandler<AllDoctorsQuery, PagedList<DoctorDto>>
    {
        private readonly ILogger<AllDoctorsQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;

        public AllDoctorsQueryHandler(
            ILogger<AllDoctorsQueryHandler> logger,
            IMapper mapper,
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedList<DoctorDto>> Handle(AllDoctorsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching doctors with paging parameters = {query.PagingParameters.ToString()}.");

            var doctors = await _doctorRepository.GetAllAsync();

            return PagedList<DoctorDto>.ToPagedList(
                doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                query.PagingParameters.PageNumber,
                query.PagingParameters.PageSize
            );
        }

    }
}
