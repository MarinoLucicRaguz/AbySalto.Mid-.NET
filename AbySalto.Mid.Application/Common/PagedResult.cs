namespace AbySalto.Mid.Application.Common
{
   public record PagedResult<T> (IReadOnlyList<T> Items, int Total, int Skip, int Limit);
}
