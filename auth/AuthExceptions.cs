namespace auth {
    public class AuthException : Exception
    {
        public AuthException(string message) : base(message) { }
    }

    public class AuthForbiddenException : AuthException
    {
        public AuthForbiddenException(string message): base(message) { }
    }
}
