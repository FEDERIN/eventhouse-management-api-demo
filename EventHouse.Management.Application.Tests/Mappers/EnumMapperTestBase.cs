namespace EventHouse.Management.Application.Tests.Mappers;

public abstract class EnumMapperTestBase<TDomainEnum, TDtoEnum>
    where TDomainEnum : struct, Enum
    where TDtoEnum : struct, Enum
{
    protected abstract TDomainEnum ToDomainRequired(TDtoEnum dto);
    protected abstract TDomainEnum? ToDomainOptional(TDtoEnum? dto);
    protected abstract TDtoEnum ToApplicationRequired(TDomainEnum domain);

    [Fact]
    public void ToDomainRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = (TDtoEnum)Enum.ToObject(typeof(TDtoEnum), 999);
        Assert.Throws<ArgumentOutOfRangeException>(() => ToDomainRequired(invalid));
    }

    [Fact]
    public void ToApplicationRequired_WhenInvalidDomain_ThrowsArgumentOutOfRangeException()
    {
        var invalid = (TDomainEnum)Enum.ToObject(typeof(TDomainEnum), 999);
        Assert.Throws<ArgumentOutOfRangeException>(() => ToApplicationRequired(invalid));
    }

    [Fact]
    public void ToDomainOptional_WhenNull_ReturnsNull()
    {
        Assert.Null(ToDomainOptional(null));
    }

    [Fact]
    public void ToDomainRequired_WhenValidDto_ReturnsMappedDomain()
    {
        foreach (var dtoValue in Enum.GetValues<TDtoEnum>())
        {
            var result = ToDomainRequired(dtoValue);

            Assert.Equal(dtoValue.ToString(), result.ToString());
        }
    }

    [Fact]
    public void ToDomainOptional_WhenHasValue_ReturnsMappedDomain()
    {
        foreach (var dtoValue in Enum.GetValues<TDtoEnum>())
        {
            var result = ToDomainOptional(dtoValue);
            Assert.Equal(dtoValue.ToString(), result?.ToString());
        }
    }

    [Fact]
    public void ToApplicationRequired_WhenValidDomain_ReturnsMappedApp()
    {
        foreach (var domainValue in Enum.GetValues<TDomainEnum>())
        {
            var result = ToApplicationRequired(domainValue);
            Assert.Equal(domainValue.ToString(), result.ToString());
        }
    }
}