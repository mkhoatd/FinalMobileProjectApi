namespace Api.Interface;

public class BaseResponse<T>
{
    public required string Status { get; set; }
    public required T Data { get; set; }
    public required string Message { get; set; }
}