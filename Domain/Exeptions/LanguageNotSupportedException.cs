namespace Domain.Exeptions
{
    public class LanguageNotSupportedException : Exception
    {
        public LanguageNotSupportedException(string language)
            : base($"The selected language '{language}' is not supported for the requested operation.")
        {

        }
    }
}
