namespace MABS.Application.Features.DoctorFeatures.Common
{
    public record TitleExtendedDto
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
}