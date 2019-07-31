public class JsonFilterConvert
{
    //'          \'
    //"          \"
    //\          \\
    ///          \/
    //\b         \\b
    //\f         \\f
    //\n         \\n
    //\r         \\n
    //\t         \\t
    //\u         \u
    public static string FilterConvert(string sourceString)
    {
        string destString = sourceString
                .Replace("'", "'")
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\");
        return destString;
    }
    public static string AddConvert(string sourceString)
    {
        string destString = sourceString
                .Replace("'", "\'")
                .Replace("\"", "\"")
                .Replace("\\", "\\\\");
        return destString;
    }
}
