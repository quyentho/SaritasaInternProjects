namespace UnrealEstate.Models.ViewModels.ResponseViewModels
{
    public class AuthenticationResponse
    {
        public AuthenticationResponseStatus ResponseStatus { get; set; }

        public string Message { get; set; }
    }

    public enum AuthenticationResponseStatus
    {
        Success,
        Error,
        Fail
    }
}