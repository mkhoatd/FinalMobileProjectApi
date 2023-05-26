using Api.Endpoints.Admin.Dtos;
using Api.Interface;
using Api.Service.AdminServices;

using Data.Entities;

namespace Api.Endpoints.Admin;

public record CreateClassroomRequest
{
    public required string Subject { get; init; }
    public required int TeacherId { get; init; }

    public required string Description { get; init; }

    public required int ClassroomSize { get; init; }
    public required List<StudySessionAdminRequestDto> StudySessions { get; init; }
}

public class CreateClassroomResponse : BaseResponse<CreateClassroomResponseData>
{
}

public record CreateClassroomResponseData
{
    public string Result { get; init; }
}

public class CreateClassroomRequestValidator : Validator<CreateClassroomRequest>
{
    public CreateClassroomRequestValidator()
    {
        RuleFor(x => x.TeacherId).NotEmpty();
        RuleFor(x => x.ClassroomSize).NotEmpty();
        RuleFor(x => x.StudySessions).NotEmpty()
            .Must(ss =>
            {
                foreach (StudySessionAdminRequestDto session in ss)
                {
                    try
                    {
                        if (!TimeSpan.TryParse(session.StartTime, out TimeSpan dummyOutput1) ||
                            !TimeSpan.TryParse(session.StartTime, out TimeSpan dummyOutput2))
                        {
                            return false;
                        }
                    }
                    catch (FormatException ex)
                    {
                        return false;
                    }
                }

                return true;
            })
            .WithMessage("Start time and end time must be in format HH:mm:ss or HH:mm")
            .Must(ss =>
            {
                foreach (StudySessionAdminRequestDto session in ss)
                {
                    if (!Enum.IsDefined(typeof(DayOfWeek), session.DayOfWeek))
                    {
                        return false;
                    }
                }

                return true;
            })
            .WithMessage("Day of week must be either Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday");
        RuleFor(x => x.StudySessions).NotEmpty().Must(ss =>
            {
                foreach (StudySessionAdminRequestDto session in ss)
                {
                    try
                    {
                        if (TimeSpan.Parse(session.StartTime) >= TimeSpan.Parse(session.EndTime))
                        {
                            return false;
                        }
                    }
                    catch (FormatException ex)
                    {
                        return false;
                    }
                }

                return true;
            })
            .WithMessage("Start time must be earlier than end time");

        RuleFor(x => x.Subject).NotEmpty()
            .Must(sn => Enum.IsDefined(typeof(SubjectName), sn))
            .WithMessage("Subject name must be either Toán, Văn, Anh, Lý, Hóa, Sinh");
        RuleFor(x => x.Description).NotEmpty();
    }
}

public class CreateClassroomEndpoint : Endpoint<CreateClassroomRequest, CreateClassroomResponse>
{
    private readonly IAdminService _adminService;

    public CreateClassroomEndpoint(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public override void Configure()
    {
        Post("/admin/create-classroom");
        Roles(RoleName.Admin.ToString());
    }

    public override async Task HandleAsync(CreateClassroomRequest req, CancellationToken ct)
    {
        List<StudySessionAdminDto> studySessionAdminDtos = req.StudySessions.Select(ss =>
            new StudySessionAdminDto
            {
                DayOfWeek = Enum.Parse<DayOfWeek>(ss.DayOfWeek),
                StartTime = TimeSpan.Parse(ss.StartTime),
                EndTime = TimeSpan.Parse(ss.EndTime)
            }).ToList();
        bool res = await _adminService.CreateClassroomAsync(Enum.Parse<SubjectName>(req.Subject),
            req.TeacherId, req.Description, req.ClassroomSize, studySessionAdminDtos);
        if (res == false)
        {
            await SendErrorsAsync(cancellation: ct);
        }
        else
        {
            await SendOkAsync(
                new CreateClassroomResponse
                {
                    Status = "Ok",
                    Message = "Success",
                    Data = new CreateClassroomResponseData { Result = "Success" }
                },
                ct);
        }
    }
}