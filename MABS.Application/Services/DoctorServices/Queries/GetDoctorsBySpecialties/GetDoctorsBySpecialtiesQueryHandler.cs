using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.DoctorServices.Common;
using MABS.Application.ServicesExtensions.DoctorServiceExtensions;
using MABS.Domain.Models.DoctorModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Services.DoctorServices.Queries.GetDoctorsBySpecialties
{
    public class DoctorsBySpecialtiesQueryHandler : IRequestHandler<GetDoctorsBySpecialtiesQuery, PagedList<DoctorDto>>
    {
        private readonly ILogger<DoctorsBySpecialtiesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorsBySpecialtiesQueryHandler(
            ILogger<DoctorsBySpecialtiesQueryHandler> logger,
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
