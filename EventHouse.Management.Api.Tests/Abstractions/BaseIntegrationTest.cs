namespace EventHouse.Management.Api.Tests.Abstractions;

public abstract class BaseIntegrationTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly CustomWebApplicationFactory Factory = factory;

    protected readonly HttpClient Client = factory.CreateDefaultClient(new AuthedHandler(factory));
}