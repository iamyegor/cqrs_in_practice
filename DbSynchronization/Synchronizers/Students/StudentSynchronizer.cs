using Dapper;
using DbSynchronization.ConnectionStrings;
using DbSynchronization.Synchronizers.Students.Models;
using DbSynchronization.Synchronizers.Students.Repositories;
using Mapster;
using Npgsql;
using Serilog;

namespace DbSynchronization.Synchronizers.Students;

public class StudentSynchronizer
{
    private readonly CommandDbStudentRepository _studentRepository;
    private readonly CommandDbSynchronizationRepository _syncRepository;
    private readonly CommandDbOutboxRepository _outboxRepository;

    public StudentSynchronizer(
        CommandDbStudentRepository studentRepository,
        CommandDbSynchronizationRepository syncRepository,
        CommandDbOutboxRepository outboxRepository
    )
    {
        _studentRepository = studentRepository;
        _syncRepository = syncRepository;
        _outboxRepository = outboxRepository;
    }

    public void Sync()
    {
        if (!IsSyncNeeded())
        {
            return;
        }

        using var connection = new NpgsqlConnection(CommandDbConnectionString.Value);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            IReadOnlyList<StudentInCommandDb> students = GetUpdatedStudents(transaction);

            List<StudentInQueryDb> mappedStudents = students.Adapt<List<StudentInQueryDb>>();

            _outboxRepository.Save(mappedStudents, "Student", transaction);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "StudentSynchronizer caught the exception");
            transaction.Rollback();
        }
    }

    private bool IsSyncNeeded()
    {
        string query = "select is_sync_required from sync where name = 'Student'";

        using var connection = new NpgsqlConnection(CommandDbConnectionString.Value);
        return connection.QuerySingle<bool>(query);
    }

    private IReadOnlyList<StudentInCommandDb> GetUpdatedStudents(NpgsqlTransaction transaction)
    {
        int syncRowVersion = _syncRepository.GetRowVersionFor("Student", transaction);

        IReadOnlyList<StudentInCommandDb> students = _studentRepository.GetForSyncAndResetFlags(
            transaction
        );

        _syncRepository.ResetSyncFlagFor("Student", syncRowVersion, transaction);

        return students;
    }
}
