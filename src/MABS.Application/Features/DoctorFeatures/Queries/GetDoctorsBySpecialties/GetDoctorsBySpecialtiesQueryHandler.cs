using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetDoctorsBySpecialties
{
    public class GetDoctorsBySpecialtiesQueryHandler : IRequestHandler<GetDoctorsBySpecialtiesQuery, PagedList<DoctorDto>>
    {
        private readonly ILogger<GetDoctorsBySpecialtiesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;

        public GetDoctorsBySpecialtiesQueryHandler(
            ILogger<GetDoctorsBySpecialtiesQueryHandler> logger,
            IMapper mapper,
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedList<DoctorDto>> Handle(GetDoctorsBySpecialtiesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching doctors with specialties = {string.Join(", ", query.Specialties.ToArray())}, with paging parameters = {query.PagingParameters.ToString()}.");

            var doctors = await _doctorRepository.GetBySpecaltiesAsync(query.Specialties);
            return PagedList<DoctorDto>.ToPagedList(
                doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                query.PagingParameters.PageNumber,
                query.PagingParameters.PageSize
            );
        }

    }
}
