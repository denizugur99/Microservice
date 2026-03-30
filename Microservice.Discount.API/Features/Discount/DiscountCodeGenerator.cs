namespace Microservice.Discount.API.Features.Discount
{
    public class DiscountCodeGenerator
    {
        private static readonly Random _random = new Random();
        private const string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// 10 karakterli discount kodu üretir (Validator'a uygun)
        /// Örnek: A3F9K2L7XM
        /// </summary>
        public static string GenerateCode()
        {
            const int length = 10; // Validator'da 10 karakter zorunlu

            var chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = _characters[_random.Next(_characters.Length)];
            }
            return new string(chars);
        }
    }
}
