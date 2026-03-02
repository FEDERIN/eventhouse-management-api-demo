
namespace EventHouse.Management.Application.Exceptions
{
    public sealed class EntityAlreadyExistsException(
        string entityName,
        string fieldName,
        string fieldValue) : Exception($"{entityName} with {fieldName} '{fieldValue}' already exists.")
    {
        public string EntityName { get; } = entityName;
        public string FieldName { get; } = fieldName;
        public string FieldValue { get; } = fieldValue;
    }
}
