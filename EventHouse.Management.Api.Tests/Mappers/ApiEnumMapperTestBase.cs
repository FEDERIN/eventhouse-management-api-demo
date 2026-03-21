namespace EventHouse.Management.Api.Tests.Mappers;

public abstract class ApiEnumMapperTestBase<TContract, TDto>
    where TContract : struct, Enum
    where TDto : struct, Enum
{
    protected abstract TDto ToApplicationRequired(TContract contract);
    protected abstract TDto? ToApplicationOptional(TContract? contract);
    protected abstract TContract ToContractRequired(TDto dto);

    [Fact]
    public void ToApplicationRequired_WhenInvalidContract_ThrowsArgumentOutOfRangeException()
    {
        var invalid = (TContract)Enum.ToObject(typeof(TContract), 999);
        Assert.Throws<ArgumentOutOfRangeException>(() => ToApplicationRequired(invalid));
    }

    [Fact]
    public void ToApplicationOptional_WhenInvalidContract_ThrowsArgumentOutOfRangeException()
    {
        var invalid = (TContract)Enum.ToObject(typeof(TContract), 999);
        TContract? nullable = invalid;
        Assert.Throws<ArgumentOutOfRangeException>(() => ToApplicationOptional(nullable));
    }

    [Fact]
    public void ToContractRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = (TDto)Enum.ToObject(typeof(TDto), 999);
        Assert.Throws<ArgumentOutOfRangeException>(() => ToContractRequired(invalid));
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

    [Fact]
    public void ToApplicationOptional_WhenHasValue_ReturnsMappedDto()
    {
        foreach (var contractValue in Enum.GetValues<TContract>())
        {
            var result = ToApplicationOptional(contractValue);
            Assert.Equal(contractValue.ToString(), result?.ToString());
        }
    }

    [Fact]
    public void ToContractRequired_WhenValidDto_ReturnsMappedContract()
    {
        foreach (var dtoValue in Enum.GetValues<TDto>())
        {
            var result = ToContractRequired(dtoValue);
            Assert.Equal(dtoValue.ToString(), result.ToString());
        }
    }
}