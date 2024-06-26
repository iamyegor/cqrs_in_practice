using AutoMapper;
using CSharpFunctionalExtensions;
using DTOs;
using Logic.Services.Commands.EditPersonalInfo;
using Logic.Services.Commands.Register;
using Logic.Services.Commands.Unregister;
using Logic.Services.Queries.GetStudentsList;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : Controller
{
    private readonly Messages _messages;
    private readonly IMapper _mapper;

    public StudentController(Messages messages, IMapper mapper)
    {
        _messages = messages;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetList(string? enrolled, int? number)
    {
        GetStudentsListQuery query = new GetStudentsListQuery(enrolled, number);
        IEnumerable<StudentDto> studentsToReturn = _messages.Dispatch(query);

        return Ok(studentsToReturn);
    }

    [HttpPost]
    public IActionResult Register([FromBody] StudentForRegistrationDto studentDto)
    {
        RegisterCommand command = _mapper.Map<RegisterCommand>(studentDto);
        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Unregister(int id)
    {
        Result result = _messages.Dispatch(new UnregisterCommand(id));

        return FromResult(result);
    }

    [HttpPut("{studentId}")]
    public IActionResult EditPersonalInfo(
        int studentId,
        StudentForEditPersonalInfoDto studentForEditPersonalInfoDto
    )
    {
        EditPersonalInfoCommand command = _mapper.Map<EditPersonalInfoCommand>(
            studentForEditPersonalInfoDto
        );
        command.Id = studentId;

        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }
}
