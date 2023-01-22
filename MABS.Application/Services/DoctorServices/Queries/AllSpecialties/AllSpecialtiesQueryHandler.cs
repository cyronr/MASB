using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.DoctorServices.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Services.DoctorServices.Queries.AllSpecialties
{
    public class AllSpecialtiesQueryHandler : IRequestHandler<AllSpecialtiesQuery, List<SpecialityExtendedDto>>
    {
        private readonly ILogger<AllSpecialtiesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;

        public AllSpecialtiesQueryHandler(
            ILogger<AllSpecialtiesQueryHandler> logger,
            IMapper mapper,
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<SpecialityExtendedDto>> Handle(AllSpecialtiesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching all specialties.");

            var specialties = await _doctorRepository.GetAllSpecialtiesAsync();
            return specialties.Select(s => _mapper.Map<SpecialityExtendedDto>(s)).ToList();
        }

    }
}
