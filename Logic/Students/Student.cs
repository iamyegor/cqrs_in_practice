using Logic.Students.Common;

namespace Logic.Students;

public class Student : AggregateRoot
{
    public string Name { get; set; }
    public string Email { get; set; }

    private readonly IList<Enrollment> _enrollments = new List<Enrollment>();
    public IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();
    private readonly List<Enrollment> _removedEnrollments = [];

    private readonly IList<Disenrollment> _disenrollments = new List<Disenrollment>();
    public IReadOnlyList<Disenrollment> Disenrollments => _disenrollments.ToList();

    private Student()
        : base(0) { }

    public Student(string name, string email)
        : base(0)
    {
        Name = name;
        Email = email;
    }

    public Enrollment? GetEnrollment(int index)
    {
        if (_enrollments.Count > index)
        {
            return _enrollments[index];
        }

        return null;
    }

    public void Disenroll(Enrollment enrollment, string comment)
    {
        _enrollments.Remove(enrollment);
        _removedEnrollments.Add(enrollment);

        var disenrollment = new Disenrollment(enrollment.Student, enrollment.Course, comment);
        _disenrollments.Add(disenrollment);
    }

    public IEnumerable<Enrollment> PopRemovedEnrollments()
    {
        IEnumerable<Enrollment> copy = _removedEnrollments.ToList();

        _removedEnrollments.Clear();

        return copy;
    }

    public void Enroll(Course course, Grade grade)
    {
        if (_enrollments.Count >= 2)
        {
            throw new Exception("Cannot have more than 2 enrollments");
        }

        var enrollment = new Enrollment(this, course, grade);
        _enrollments.Add(enrollment);
    }
}
