using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.AppointmentModels;

namespace MABS.Application.ModelsExtensions.AppointmentModelsExtensions
{
    public static class AppointmentExtensions
    {
        public static async Task<Appointment> GetByUUIDAsync(this Appointment appointment, IAppointmentRepository appointmentRepository, Guid uuid)
        {
            appointment = await appointmentRepository.GetByUUIDAsync(uuid);
            if (appointment is null)
                throw new NotFoundException($"Nie znaleziono wizyty o podanym identyfikatorze.", $"AppointmentId = {uuid}");

            return appointment;
        }
    }
}
