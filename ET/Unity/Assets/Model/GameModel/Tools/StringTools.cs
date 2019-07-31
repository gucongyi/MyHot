using System.Text.RegularExpressions;

public class StringTools
{
    public static bool isStartNum(string sourceString)
    {
        bool isStartWithNum = false;
        Regex regNum = new Regex("^[0-9]");
        if (regNum.IsMatch(sourceString))
        {
            isStartWithNum = true;
        }
        return isStartWithNum;
    }
}
