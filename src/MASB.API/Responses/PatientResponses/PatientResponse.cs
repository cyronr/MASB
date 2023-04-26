namespace MASB.API.Responses.PatientResponses
{
    public record PatientResponse
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
