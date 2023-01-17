﻿using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;
using Microsoft.Extensions.Logging;

namespace MABS.Application.CRUD.Readers.DoctorReader
{
    internal class DoctorReader : IDoctorReader
    {
        private readonly ILogger<DoctorReader> _logger;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorReader(ILogger<DoctorReader> logger, IDoctorRepository doctorRepository)
        {
            _logger = logger;
            _doctorRepository = doctorRepository;
        }

        public async Task<Doctor> GetDoctorByUUID(Guid uuid)
        {
            _logger.LogInformation($"Checking if doctor with id = {uuid} exists.");

            var doctor = await _doctorRepository.GetByUUID(uuid);
            if (doctor == null)
                throw new NotFoundException($"Doctor not found.", $"DoctorId = {uuid}");

            return doctor;
        }

        public async Task<Title> GetTitleById(int id)
        {
            _logger.LogInformation($"Checking if title with id = {id} exists.");

            var title = await _doctorRepository.GetTitleById(id);
            if (title == null)
                throw new DictionaryValueNotExistsException("Wrong TitleId.");

            return title;
        }

        public async Task<List<Specialty>> GetSpecialtiesByIds(List<int> ids)
        {
            _logger.LogInformation($"Checking if specialties with ids = [{string.Join(", ", ids.ToArray())}] exists.");

            var specalties = await _doctorRepository.GetSpecialtiesByIds(ids);
            if (specalties.Count != ids.Count)
                throw new DictionaryValueNotExistsException("Wrong Specailties.");

            return specalties;
        }
    }
}
