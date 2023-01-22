using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.DoctorServices.Common;
using MABS.Application.ServicesExtensions.DoctorServiceExtensions;
using MABS.Domain.Models.DoctorModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Services.DoctorServices.Queries.DoctorById
{
    public class GetDoctorByIdQueryHandler : IRequestHandler<DoctorByIdQuery, DoctorDto>
    {
        private readonly ILogger<GetDoctorByIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;

        public GetDoctorByIdQueryHandler(
            ILogger<GetDoctorByIdQueryHandler> logger,
            IMapper mapper,
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<DoctorDto> Handle(DoctorByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching doctor with id = {query.Id}.");
            var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, query.Id);

            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}
