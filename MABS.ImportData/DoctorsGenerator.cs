using System.Data.SqlClient;
using System.Globalization;

namespace MABS.GenerateData;

public static class DoctorsGenerator
{
    public static void GenerateDotors(int count, string connectionString)
    {
        List<string> lastnames_m = GetListOfMaleLastNames();
        List<string> lastnames_f = GetListOfFemaleLastNames();
        List<string> firstnames_m = new List<string>();
        List<string> firstnames_f = new List<string>();
        
        var firstnames = GetListOfFirstNames();
        firstnames.Where(f => f.Item2 == "M").ToList().ForEach(x => firstnames_m.Add(x.Item1));
        firstnames.Where(f => f.Item2 == "K").ToList().ForEach(x => firstnames_f.Add(x.Item1));

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            Random rnd = new Random();
            conn.Open();

            for (int i = 0; i <= count; i++)
            {
                bool isMale = i % 2 == 0 ? true : false;
                int titleId = rnd.Next(1, 4);
                string firstname;
                string lastname;
                
                if (isMale)
                {
                    firstname = firstnames_m[rnd.Next(0, firstnames_m.Count-1)];
                    lastname = lastnames_m[rnd.Next(0, lastnames_m.Count - 1)];
                }
                else
                {
                    firstname = firstnames_f[rnd.Next(0, firstnames_f.Count - 1)];
                    lastname = lastnames_f[rnd.Next(0, lastnames_f.Count - 1)];
                }

                int doctorId = InsertDoctorToDatabase(conn, textInfo.ToTitleCase(firstname.ToLower()), textInfo.ToTitleCase(lastname.ToLower()), titleId);
                var speclaties = GenerateSpecalties();
                foreach(int specailty in speclaties)
                {
                    InsertDoctorSpecialtyToDatabase(conn, doctorId, specailty);
                }
                
            }

            conn.Close();
        }

        Console.WriteLine($"Created {count} doctors.");
    }

    private static int InsertDoctorToDatabase(SqlConnection conn, string firstname, string lastname, int titleId)
    {
        string cmdInsertIntoDoctors =
            "insert into Doctors (UUID, StatusId, FirstName, LastName, TitleId) " +
            "output INSERTED.ID " +
            "values (@uuid, @statusId, @firstname, @lastname, @titleId)";

        using (SqlCommand cmd = new SqlCommand(cmdInsertIntoDoctors, conn))
        {
            cmd.Parameters.AddWithValue("@uuid", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@statusId", 1);
            cmd.Parameters.AddWithValue("@firstname", firstname);
            cmd.Parameters.AddWithValue("@lastname", lastname);
            cmd.Parameters.AddWithValue("@titleId", titleId);

            int insertedId = (int)cmd.ExecuteScalar();
            return insertedId;
        }
    }

    private static void InsertDoctorSpecialtyToDatabase(SqlConnection conn, int doctorId, int specialtyId)
    {
        string cmdInsertIntoDoctors =
            "insert into DoctorsSpecialties (DoctorId, SpecialtyId)" +
            "values (@doctorId, @specialtyId)";

        using (SqlCommand cmd = new SqlCommand(cmdInsertIntoDoctors, conn))
        {
            cmd.Parameters.AddWithValue("@doctorId", doctorId);
            cmd.Parameters.AddWithValue("@specialtyId", specialtyId);

            cmd.ExecuteNonQuery();
        }
    }

    private static List<int> GenerateSpecalties()
    {
        Random rnd = new Random();
        List<int> specalties = new List<int>();

        int count = rnd.Next(1, 3);
        while(specalties.Count != count)
        {
            int temp = rnd.Next(1, 10);
            if (!specalties.Contains(temp))
                specalties.Add(temp);
        }

        return specalties;
    }

    private static List<(string, string)> GetListOfFirstNames()
    {
        List<(string, string)> names = new List<(string, string)>();

        using (var reader = new StreamReader(@"..\..\..\..\help\imiona.csv"))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (values[0] != String.Empty && values[1] != String.Empty)
                    names.Add((values[0], values[2]));
            }
        }

        return names;
    }

    private static List<string> GetListOfMaleLastNames()
    {
        List<string> names = new List<string>();

        using (var reader = new StreamReader(@"..\..\..\..\help\nazwiska_m.csv"))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (values[0] != String.Empty)
                    names.Add(values[0]);
            }
        }

        return names;
    }

    private static List<string> GetListOfFemaleLastNames()
    {
        List<string> names = new List<string>();

        using (var reader = new StreamReader(@"..\..\..\..\help\nazwiska_f.csv"))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (values[0] != String.Empty)
                    names.Add(values[0]);
            }
        }

        return names;
    }
}
