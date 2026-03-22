namespace EventHouse.Management.Api.Tests.Mappers;

public abstract class ApiEnumMapperTestBase<TContract, TDto>
    : ApiEnumMapperUnidirectionalTestBase<TContract, TDto>
    where TContract : struct, Enum
    where TDto : struct, Enum
{
    protected abstract TContract ToContractRequired(TDto dto);

    [Fact]
    public void ToContractRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = (TDto)Enum.ToObject(typeof(TDto), 999);
        Assert.Throws<ArgumentOutOfRangeException>(() => ToContractRequired(invalid));
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