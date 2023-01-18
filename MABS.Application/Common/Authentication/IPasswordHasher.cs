namespace MABS.Application.Common.Authentication
{
    public interface IPasswordHasher
    {
        public void GeneratePassword(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
