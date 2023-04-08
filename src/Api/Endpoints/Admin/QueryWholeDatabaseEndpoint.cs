// using Api.Interface;
//
// using Data;
// using Data.Entities;
// using Data.Entities.Interfaces;
//
// using Microsoft.EntityFrameworkCore;
//
// namespace Api.Endpoints.Admin;
//
// public record QueryWholeDatabaseRequest
// {
//     [FromClaim("UserID")] public int UserId { get; init; }
//     [FromClaim("Role")] public RoleName RoleName { get; init; }
// }
//
// public record QueryWholeDatabaseData
// {
//     public List<User> Users { get; init; }
//     public List<Teacher> Teachers { get; init; }
//     public List<Data.Entities.Classroom> Classrooms { get; init; }
//     public List<Student> Students { get; init; }
//     public List<StudySession> Sessions { get; init; }
//     public List<Data.Entities.Admin> Admins { get; init; }
// }
//
// public class QueryWholeDatabaseEndpoint : Endpoint<QueryWholeDatabaseRequest, BaseResponse<QueryWholeDatabaseData>>
// {
//     private readonly TutorDbContext _dbContext;
//
//     public QueryWholeDatabaseEndpoint(TutorDbContext dbContext)
//     {
//         _dbContext = dbContext;
//     }
//
//     public override void Configure()
//     {
//         Get("/admin/query-whole-database");
//         Roles(RoleName.Admin.ToString());
//     }
//
//     public override async Task HandleAsync(QueryWholeDatabaseRequest req, CancellationToken ct)
//     {
//         var response = new BaseResponse<QueryWholeDatabaseData>
//         {
//             Status = "1",
//             Message = "Success",
//             Data = new QueryWholeDatabaseData
//             {
//                 Users = await _dbContext.Users.ToListAsync(ct),
//                 Teachers = await _dbContext.Teachers.ToListAsync(ct),
//                 Classrooms = await _dbContext.Classrooms.ToListAsync(ct),
//                 Students = await _dbContext.Students.ToListAsync(ct),
//                 Sessions = await _dbContext.Sessions.ToListAsync(ct),
//                 Admins = await _dbContext.Admins.ToListAsync(ct),
//             }
//         };
//         await SendOkAsync(response, cancellation: ct);
//     }
// }