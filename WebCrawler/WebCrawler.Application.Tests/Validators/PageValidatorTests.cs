using WebCrawler.Application.Validators;
using Xunit;

namespace WebCrawler.Application.Tests.Validators;

public class PageValidatorTests
{
    [Fact]
    public void GetValidPageNumber_NumberBiggerThanMinValue_ShouldReturNumber()
    {
        var testPageNumber = 5;

        var result = PageValidator.GetValidPageNumber(testPageNumber);

        Assert.Equal(testPageNumber, result);
    }

    [Fact]
    public void GetValidPageNumber_NumberEqualMinValue_ShouldReturnMinValue()
    {
        var testPageNumber = 1;

        var result = PageValidator.GetValidPageNumber(testPageNumber);

        Assert.Equal(1, result);
    }

    [Fact]
    public void GetValidPageNumber_NumberLessThanMinValue_ShouldReturnMinValue()
    {
        var testPageNumber = -1;

        var result = PageValidator.GetValidPageNumber(testPageNumber);

        Assert.Equal(1, result);
    }

    [Fact]
    public void GetValidPageSize_NumberBiggerThanMinValue_ShouldReturnNumber()
    {
        var testPageSize = 2;

        var result = PageValidator.GetValidPageSize(testPageSize);

        Assert.Equal(2, result);
    }

    [Fact]
    public void GetValidPageSize_NumberEqualMinValue_ShouldReturnMinValue()
    {
        var testPageSize = 1;

        var result = PageValidator.GetValidPageSize(testPageSize);

        Assert.Equal(1, result);
    }
    [Fact]
    public void GetValidPageSize_NumberLessThanMinValue_ShouldReturnStandartValue()
    {
        var testPageSize = -1;

        var result = PageValidator.GetValidPageSize(testPageSize);

        Assert.Equal(5, result);
    }
}
