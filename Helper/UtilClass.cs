using System.Text.RegularExpressions;

namespace StudentManagement.Helper;
internal static class UtilClass
{
    public static bool IsValidUsername(string username)
    {
        // Define the regular expression pattern
        // ^[a-zA-Z]      -> Must start with a letter.
        // [a-zA-Z0-9_]{2,15} -> The following 2 to 15 characters can be letters, numbers, or underscores.
        // $             -> End of the string.
        string pattern = @"^[a-zA-Z][a-zA-Z0-9_]{2,15}$";

        // Check if the username is null or empty, and then if it matches the pattern
        if (string.IsNullOrEmpty(username))
        {
            return false;
        }

        return Regex.IsMatch(username, pattern);
    }

    public static bool IsValidPassword(string password)
    {
        // Tối thiểu 8 ký tự, ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, pattern);
    }

    public static bool IsValidEmail(string email)
    {
        // Định dạng email cơ bản
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, pattern);
    }

    public static bool IsValidAddress(string address)
    {
        // Địa chỉ: cho phép chữ, số, dấu phẩy, dấu chấm, khoảng trắng, tối thiểu 5 ký tự
        string pattern = @"^[\w\s,.-]{5,}$";
        return !string.IsNullOrEmpty(address) && Regex.IsMatch(address, pattern);
    }

    public static bool IsValidFullName(string fullName)
    {
        // Họ tên: chỉ cho phép chữ cái và khoảng trắng, tối thiểu 2 từ
        string pattern = @"^([A-Za-z]+(?:\s[A-Za-z]+)+)$";
        return !string.IsNullOrEmpty(fullName) && Regex.IsMatch(fullName, pattern);
    }

    public static bool IsValidPhone(string phone)
    {
        // Số điện thoại Việt Nam: bắt đầu bằng 0, có 10 hoặc 11 chữ số
        string pattern = @"^0\d{9,10}$";
        return !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, pattern);
    }

    public static bool IsValidDateOfBirth(string dob)
    {
        // Ngày sinh: định dạng YYYY-MM-DD và hợp lệ
        DateTime parsedDate;
        return DateTime.TryParseExact(dob, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedDate);
    }

}