namespace MASB.API.Requests.DoctorRequests
{
    public abstract record UpsertDoctorRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int TitleId { get; set; }
        public List<int> Specialties { get; set; }
    }
}
