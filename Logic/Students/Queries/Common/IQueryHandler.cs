namespace Logic.Students.Queries.Common;

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public TResult Handle(TQuery query);
}
