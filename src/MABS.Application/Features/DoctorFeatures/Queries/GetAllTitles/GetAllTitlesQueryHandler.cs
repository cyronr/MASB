using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetAllTitles
{
    public class GetAllTitlesQueryHandler : IRequestHandler<GetAllTitlesQuery, List<TitleExtendedDto>>
    {
        private readonly ILogger<GetAllTitlesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;

        public GetAllTitlesQueryHandler(
            ILogger<GetAllTitlesQueryHandler> logger,
            IMapper mapper,
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<TitleExtendedDto>> Handle(GetAllTitlesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching all titles.");

            var specialties = await _doctorRepository.GetAllTitlesAsync();
            return specialties.Select(s => _mapper.Map<TitleExtendedDto>(s)).ToList();
        }

    }
}
