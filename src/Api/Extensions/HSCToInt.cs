using System.Net;

namespace Api.Extensions;

public static class HSCToInt
{
    public static int Value(this HttpStatusCode @this)
    {
        return (int)@this;
    }
}