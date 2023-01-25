namespace MABS.API.Responses.FacilityResponses
{
    public record StreetTypeResponse
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
}
