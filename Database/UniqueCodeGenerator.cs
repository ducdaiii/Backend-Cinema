namespace CinemaHD.Database
{
    public class UniqueCodeGenerator
    {
        private static readonly Random _random = new Random();
        private static readonly HashSet<string> _existingCodes = new HashSet<string>();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateUniqueCode(int length = 20)
        {
            string code;
            do
            {
                code = new string(Enumerable.Repeat(_chars, length)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            } while (_existingCodes.Contains(code));

            _existingCodes.Add(code);
            return code;
        }
    }
}
