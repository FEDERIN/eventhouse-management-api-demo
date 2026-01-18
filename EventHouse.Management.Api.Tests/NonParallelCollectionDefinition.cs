
namespace EventHouse.Management.Api.Tests;


[CollectionDefinition("NonParallel", DisableParallelization = true)]
public sealed class NonParallelCollectionDefinition : ICollectionFixture<object>
{
}
