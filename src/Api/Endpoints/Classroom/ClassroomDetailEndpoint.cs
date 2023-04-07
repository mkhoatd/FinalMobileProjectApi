namespace Api.Endpoints.Classroom;



public record ClassroomDetailRequest
{
    [BindFrom("id")]
    public Guid ClassroomId { get; init; }
}

public record Data
{}


// public class ClassroomDetailEndpoint : Endpoint<ClassroomDetailRequest, >
// {
//     
//     
// }