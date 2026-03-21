namespace EventHouse.Management.Api.Tests.Mappers;

public abstract class ApiEnumMapperUnidirectionalTestBase<TContract, TDto>
    where TContract : struct, Enum
    where TDto : struct, Enum
{
    protected abstract TDto ToApplicationRequired(TContract contract);
    protected abstract TDto? ToApplicationOptional(TContract? contract);

    [Fact]
    public void ToApplicationRequired_WhenInvalidContract_ThrowsArgumentOutOfRangeException()
    {
        var invalid = (TContract)Enum.ToObject(typeof(TContract), 999);
        Assert.Throws<ArgumentOutOfRangeException>(() => ToApplicationRequired(invalid));
    }

    [Fact]
    public void ToApplicationOptional_WhenNull_ReturnsNull()
    {
        Assert.Null(ToApplicationOptional(null));
    }

    [Fact]
    public void ToApplicationRequired_WhenValidContract_ReturnsMappedDto()
    {
        foreach (var contractValue in Enum.GetValues<TContract>())
        {
            var result = ToApplicationRequired(contractValue);
            Assert.Equal(contractValue.ToString(), result.ToString());
        }
    }
}