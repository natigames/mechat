using System;
namespace mechat.core
{
    public static class Config
    {
        public static string MainEndpoint =
            "http://localhost:7071";
        public static string NegotiateEndpoint =
            $"{MainEndpoint}/api/negotiate";
        public static string MessagesEndpoint =
            $"{MainEndpoint}/api/Messages";
    }
}
