namespace Kryptering
{
    public class User
    {
        static string? name;

        public static string? Name
        {
            get => name;
            set => name = value;
        }
    }
}