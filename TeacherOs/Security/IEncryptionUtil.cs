namespace TeacherOs.Security
{
    public interface IEncryptionUtil
    {
        string EncryptPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
