using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.FacilityFeatures.Queries.GetFacilityDoctors
{
    public class GetFacilityDoctorsQueryHandler : IRequestHandler<GetFacilityDoctorsQuery, PagedList<DoctorDto>>
    {
        private readonly ILogger<GetFacilityDoctorsQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IFacilityRepository _facilityRepository;

        public GetFacilityDoctorsQueryHandler(
            ILogger<GetFacilityDoctorsQueryHandler> logger,
            IMapper mapper,
            IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedList<DoctorDto>> Handle(GetFacilityDoctorsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching doctors for facility (Id = {query.FacilityId}) with paging parameters = {query.PagingParameters.ToString()}.");

            _logger.LogDebug($"Fetching facility with id = {query.FacilityId}");
            var facility = await _facilityRepository.GetWithAllDoctorsByUUIDAsync(query.FacilityId);
            if (facility is null)
                throw new NotFoundException("Facility not found.", $"FacilityId = {query.FacilityId}");

            return PagedList<DoctorDto>.ToPagedList(
                facility.Doctors.Select(f => _mapper.Map<DoctorDto>(f)).ToList(),
                query.PagingParameters.PageNumber,
                query.PagingParameters.PageSize
            );
        }

    }
}
