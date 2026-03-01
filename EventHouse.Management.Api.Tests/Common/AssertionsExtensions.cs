using EventHouse.Management.Api.Contracts.Common;
using FluentAssertions;

namespace EventHouse.Management.Api.Tests.Common;

public static class AssertionsExtensions
{
public static void ShouldHaveValidPaginationLinks<T>(this PagedResult<T> page, int currentPage, int expectedPageSize)
{
    page.Links.Should().NotBeNull("el objeto de enlaces (links) no debe ser nulo");
    
    // 1. Validar el enlace actual (Self)
    page.Links!.Self.Should().Contain($"page={currentPage}");
    page.Links.Self.Should().Contain($"pageSize={expectedPageSize}");

    // 2. Validar el enlace a la siguiente página (Next)
    if (page.TotalCount > (currentPage * expectedPageSize))
    {
        page.Links.Next.Should().NotBeNull("porque hay más elementos en la siguiente página");
        page.Links.Next.Should().Contain($"page={currentPage + 1}");
    }
    else
    {
        page.Links.Next.Should().BeNull("porque estamos en la última página");
    }

    // 3. Validar el enlace a la página anterior (Previous)
    if (currentPage > 1)
    {
        page.Links.Previous.Should().NotBeNull("porque existe una página anterior");
        page.Links.Previous.Should().Contain($"page={currentPage - 1}");
    }
    else
    {
        page.Links.Previous.Should().BeNull("porque estamos en la primera página y no hay anterior");
    }
}
}
