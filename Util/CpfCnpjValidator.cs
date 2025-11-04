using System.Text.RegularExpressions;

namespace Financeira.Validators
{
    public class CpfCnpjValidator
    {
        public bool IsValidCpfCnpj(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            var digits = Regex.Replace(value, @"\D", "");

            return digits.Length switch
            {
                11 => IsValidCpf(digits),
                14 => IsValidCnpj(digits),
                _ => false
            };
        }

        private bool IsValidCpf(string cpf)
        {
            if (Regex.IsMatch(cpf, @"^(\d)\1{10}$")) return false;

            int sum1 = 0, sum2 = 0;
            for (int i = 0; i < 9; i++)
            {
                int digit = cpf[i] - '0';
                sum1 += digit * (10 - i);
                sum2 += digit * (11 - i);
            }

            int check1 = (sum1 * 10) % 11;
            check1 = (check1 == 10) ? 0 : check1;
            sum2 += check1 * 2;
            int check2 = (sum2 * 10) % 11;
            check2 = (check2 == 10) ? 0 : check2;

            return check1 == (cpf[9] - '0') && check2 == (cpf[10] - '0');
        }

        private bool IsValidCnpj(string cnpj)
        {
            if (Regex.IsMatch(cnpj, @"^(\d)\1{13}$")) return false;

            int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int sum1 = 0, sum2 = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = cnpj[i] - '0';
                sum1 += digit * weights1[i];
                sum2 += digit * weights2[i];
            }

            int check1 = sum1 % 11;
            check1 = (check1 < 2) ? 0 : 11 - check1;
            sum2 += check1 * weights2[12];
            int check2 = sum2 % 11;
            check2 = (check2 < 2) ? 0 : 11 - check2;

            return check1 == (cnpj[12] - '0') && check2 == (cnpj[13] - '0');
        }
    }
}
