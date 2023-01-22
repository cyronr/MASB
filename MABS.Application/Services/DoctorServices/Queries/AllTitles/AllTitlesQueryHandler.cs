using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.DoctorServices.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Services.DoctorServices.Queries.AllTitles
{
    public class AllTitlesQueryHandler : IRequestHandler<AllTitlesQuery, List<TitleExtendedDto>>
    {
        private readonly ILogger<AllTitlesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;

        public AllTitlesQueryHandler(
            ILogger<AllTitlesQueryHandler> logger,
            IMapper mapper,
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<TitleExtendedDto>> Handle(AllTitlesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching all titles.");

            var specialties = await _doctorRepository.GetAllTitlesAsync();
            return specialties.Select(s => _mapper.Map<TitleExtendedDto>(s)).ToList();
        }

    }
}
