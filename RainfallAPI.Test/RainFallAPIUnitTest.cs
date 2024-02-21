using NUnit.Framework;
using Moq;
using RainfallAPI.Controllers;
using RainfallAPI.Models;
using RainfallAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using RainfallAPI.Enum;

[TestFixture]
public class RainFallAPIUnitTest
{
    private Mock<IRainfallService> _rainfallServiceMock;
    private RainfallController _controller;

    [SetUp]
    public void Setup()
    {
        _rainfallServiceMock = new Mock<IRainfallService>();
        _controller = new RainfallController(_rainfallServiceMock.Object);
    }

    [Test]
    public async System.Threading.Tasks.Task GetRainfallReadings_ValidStationId_ReturnsOkResult()
    {
        // Arrange
        string stationId = "1";
        var count = 10;
        var expectedReadings = new List<RainfallReading>
            {
                new RainfallReading { DateMeasured = DateTime.Now.ToString(), AmountMeasured = 10.5m },
                new RainfallReading { DateMeasured = DateTime.Now.AddDays(-1).ToString(), AmountMeasured = 15.2m }
            };

        _rainfallServiceMock.Setup(s => s.GetRainfallReadingsAsync(stationId, count))
            .ReturnsAsync(expectedReadings);

        // Act
        var result = await _controller.GetRainfallReadingsAsync(stationId, count);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.IsInstanceOf<RainfallReadingResponse>(okResult.Value);
        var response = (RainfallReadingResponse)okResult.Value;
        Assert.AreEqual(expectedReadings, response.Readings);
    }

    [Test]
    public async System.Threading.Tasks.Task GetRainfallReadings_InvalidStationId_ReturnsBadRequestResult()
    {
        // Arrange
        string stationId = "a";
        var count = 10;

        // Act
        var result = await _controller.GetRainfallReadingsAsync(stationId, count);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.IsInstanceOf<ErrorResponse>(badRequestResult.Value);
        var errorResponse = (ErrorResponse)badRequestResult.Value;
        Assert.AreEqual(1, errorResponse.Error.Detail.Count);
        Assert.AreEqual(nameof(stationId), errorResponse.Error.Detail[0].PropertyName);
        Assert.IsTrue(errorResponse.Error.Detail[0].Message.Contains($"The {nameof(stationId)} is not valid"));
    }

    [Test]
    public async System.Threading.Tasks.Task GetRainfallReadings_NoReadings_ReturnsNoContentResult()
    {
        // Arrange
        string stationId = "1";
        var count = 10;
        var expectedReadings = new List<RainfallReading>();

        _rainfallServiceMock.Setup(s => s.GetRainfallReadingsAsync(stationId, count))
            .ReturnsAsync(expectedReadings);

        // Act
        var result = await _controller.GetRainfallReadingsAsync(stationId, count);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        var notFoundRequestResult = (NotFoundObjectResult)result;
        Assert.IsInstanceOf<ErrorResponse>(notFoundRequestResult.Value);
        var errorResponse = (ErrorResponse)notFoundRequestResult.Value;
        Assert.AreEqual(1, errorResponse.Error.Detail.Count);
        Assert.AreEqual(nameof(stationId), errorResponse.Error.Detail[0].PropertyName);
        Assert.IsTrue(errorResponse.Error.Detail[0].Message.Contains($"No readings found for the specified {nameof(stationId)}."));
    }

}