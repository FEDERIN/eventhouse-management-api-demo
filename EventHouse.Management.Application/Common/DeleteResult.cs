
namespace EventHouse.Management.Application.Common
{
    public sealed class DeleteResult
    {
        public DeleteStatus Status { get; }
        private DeleteResult(DeleteStatus status)
        {
            Status = status;
        }

        public static DeleteResult Ok() => new(DeleteStatus.Ok);
        public static DeleteResult NotFoundResult() => new(DeleteStatus.NotFound);
    }

    public enum DeleteStatus
    {
        Ok = 1,
        NotFound = 2,
        HasAssociations = 3
    }
}
