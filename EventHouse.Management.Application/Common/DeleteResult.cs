
namespace EventHouse.Management.Application.Common
{
    public sealed class DeleteResult
    {
        public DeleteStatus Status { get; }
        public bool Success => Status == DeleteStatus.Ok;
        public bool NotFound => Status == DeleteStatus.NotFound;
        public bool HasAssociations => Status == DeleteStatus.HasAssociations;
        private DeleteResult(DeleteStatus status)
        {
            Status = status;
        }

        public static DeleteResult Ok() => new(DeleteStatus.Ok);
        public static DeleteResult NotFoundResult() => new(DeleteStatus.NotFound);
        public static DeleteResult HasAssociationsResult() => new(DeleteStatus.HasAssociations);
    }

    public enum DeleteStatus
    {
        Ok = 1,
        NotFound = 2,
        HasAssociations = 3
    }
}
