namespace EcommerceWeb.Areas.Admin.Helpers
{
    public class HelpersString
    {
        //Hàm cắt chuỗi theo số lượng kí tự
        public static string TruncateString(string value, int characters)
        {
            if(value.Length > characters)
            {
                value = value.Substring(0, characters) + "...";
                return value;
            }
            else
            {
                return value;
            }
        }
    }
}
