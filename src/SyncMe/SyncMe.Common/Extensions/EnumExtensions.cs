using System.ComponentModel;
using System.Globalization;

namespace SyncMe.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum e)
    {
        if (e != null)
        {
            Type type = e.GetType();
            Array values = Enum.GetValues(type);
            int num = Convert.ToInt32(e, CultureInfo.InvariantCulture);
            foreach (int item in values)
            {
                if (item == num)
                {
                    return (type.GetMember(type.GetEnumName(item))[0].GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).FirstOrDefault() as DescriptionAttribute)?.Description;
                }
            }
        }

        return null;
    }
}
