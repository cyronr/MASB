using AutoMapper;
using MediatR;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.FacilityServices.Common;
using MABS.Domain.Models.FacilityModels;
using Microsoft.Extensions.Logging;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;

namespace MABS.Application.Services.FacilityServices.Queries.GetFacilityById
{
    public class GetFacilityByIdQueryHandler : IRequestHandler<GetFacilityByIdQuery, FacilityDto>
    {
        private readonly ILogger<GetFacilityByIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IFacilityRepository _facilityRepository;

        public GetFacilityByIdQueryHandler(
            ILogger<GetFacilityByIdQueryHandler> logger,
            IMapper mapper,
            IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<FacilityDto> Handle(GetFacilityByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching facility with id = {query.Id}.");
            var facility = await new Facility().GetByUUIDAsync(_facilityRepository, query.Id);

            return _mapper.Map<FacilityDto>(facility);
        }

    }
}
