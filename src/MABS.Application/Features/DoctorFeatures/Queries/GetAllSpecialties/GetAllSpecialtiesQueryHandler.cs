﻿using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetAllSpecialties
{
    public class GetAllSpecialtiesQueryHandler : IRequestHandler<GetAllSpecialtiesQuery, List<SpecialityExtendedDto>>
    {
        private readonly ILogger<GetAllSpecialtiesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;

        public GetAllSpecialtiesQueryHandler(
            ILogger<GetAllSpecialtiesQueryHandler> logger,
            IMapper mapper,
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<SpecialityExtendedDto>> Handle(GetAllSpecialtiesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching all specialties.");

            var specialties = await _doctorRepository.GetAllSpecialtiesAsync();
            return specialties.Select(s => _mapper.Map<SpecialityExtendedDto>(s)).ToList();
        }

    }
}
