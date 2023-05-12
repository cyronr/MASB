namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockDoctorRepository
{
    public static Mock<IDoctorRepository> GetDoctorRepository()
    {
        var mockDoctors = PrepareListOfMockDoctors();
        var mockTitles = PrepareListOfMockTitles();
        var mockSpecialties = PrepareListOfMockSpecialties();

        return new Mock<IDoctorRepository>().SetupRepository(mockDoctors, mockTitles, mockSpecialties);
    }

    public static Mock<IDoctorRepository> GetEmptyDoctorRepository()
    {
        var mockDoctors = new List<Doctor>();
        var mockTitles = new List<Title>();
        var mockSpecialties = new List<Specialty>();

        return new Mock<IDoctorRepository>().SetupRepository(mockDoctors, mockTitles, mockSpecialties);
    }

    public static List<Doctor> PrepareListOfMockDoctors()
    {
        List<Title> mockTitles = PrepareListOfMockTitles();
        List<Specialty> mockSpecalties = PrepareListOfMockSpecialties();

        return new List<Doctor>
        {
            new Doctor
            {
                Id = 1,
                UUID= Guid.NewGuid(),
                StatusId = DoctorStatus.Status.Active,
                Status = new DoctorStatus
                {
                    Id = DoctorStatus.Status.Active,
                    Name = "Active"
                },
                Firstname = "Mock_Firstname_1",
                Lastname = "Mock_Lastname_1",
                Title = mockTitles.Single(t => t.Id == 1),
                Specialties =
                {
                    mockSpecalties.Single(t => t.Id == 1),
                    mockSpecalties.Single(t => t.Id == 2)
                }
            },
            new Doctor
            {
                Id = 2,
                UUID= Guid.Parse(Consts.Active_Doctor_UUID),
                StatusId = DoctorStatus.Status.Active,
                Status = new DoctorStatus
                {
                    Id = DoctorStatus.Status.Active,
                    Name = "Active"
                },
                Firstname = "Mock_Firstname_2",
                Lastname = "Mock_Lastname_2",
                Title = mockTitles.Single(t => t.Id == 2),
                Specialties =
                {
                    mockSpecalties.Single(t => t.Id == 3),
                    mockSpecalties.Single(t => t.Id == 4)
                }
            },
            new Doctor
            {
                Id = 3,
                UUID= Guid.NewGuid(),
                StatusId = DoctorStatus.Status.Active,
                Status = new DoctorStatus
                {
                    Id = DoctorStatus.Status.Active,
                    Name = "Active"
                },
                Firstname = "Mock_Firstname_3",
                Lastname = "Mock_Lastname_3",
                Title = mockTitles.Single(t => t.Id == 1),
                Specialties =
                {
                    mockSpecalties.Single(t => t.Id == 3),
                    mockSpecalties.Single(t => t.Id == 5)
                }
            },
            new Doctor
            {
                Id = 4,
                UUID= Guid.NewGuid(),
                StatusId = DoctorStatus.Status.Deleted,
                Status = new DoctorStatus
                {
                    Id = DoctorStatus.Status.Deleted,
                    Name = "Deleted"
                },
                Firstname = "Mock_Firstname_4_Deleted",
                Lastname = "Mock_Lastname_4_Deleted",
                Title = mockTitles.Single(t => t.Id == 1),
                Specialties =
                {
                    mockSpecalties.Single(t => t.Id == 3),
                    mockSpecalties.Single(t => t.Id == 5)
                }
            }
        };
    }

    private static List<Title> PrepareListOfMockTitles()
    {
        return new List<Title>
        {
            new Title
            {
                Id = 1,
                ShortName = "MockTitle1",
                Name= "Mock Testing Title 1"
            },
            new Title
            {
                Id = 2,
                ShortName = "MockTitle2",
                Name= "Mock Testing Title 2"
            }
        };
    }

    private static List<Specialty> PrepareListOfMockSpecialties()
    {
        return new List<Specialty>
        {
            new Specialty
            {
                Id = 1,
                Name= "Mock Speciality 1"
            },
            new Specialty
            {
                Id = 2,
                Name= "Mock Speciality 2"
            },
            new Specialty
            {
                Id = 3,
                Name= "Mock Speciality 3"
            },
            new Specialty
            {
                Id = 4,
                Name= "Mock Speciality 4"
            },
            new Specialty
            {
                Id = 5,
                Name= "Mock Speciality 5"
            }
        };
    }

}

