using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.ServicesExtensions.DoctorServiceExtensions
{
    public static class DoctorExtensions
    {
        public static async Task<Doctor> GetByUUIDAsync(this Doctor doctor, IDoctorRepository doctorRepository, Guid uuid)
        {
            doctor = await doctorRepository.GetByUUIDAsync(uuid);
            if (doctor is null)
                throw new NotFoundException($"Doctor not found.", $"DoctorId = {uuid}");

            return doctor;
        }
    }
}
