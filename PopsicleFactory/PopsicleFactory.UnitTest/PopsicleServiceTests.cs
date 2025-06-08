using System.Text.Json;
using Moq;
using PopsicleFactory.DataProvider.Entities.Repositories;
using PopsicleFactory.DataProvider.Models;
using PopsicleFactory.WebApi.Services;

namespace PopsicleFactory.UnitTest;

public class PopsicleServicesTest
{
    private readonly Mock<IPopsicleRepository> _mockRepositoy;
    private readonly PopsicleService _service;
    private static Popsicle popsicle1 = new Popsicle { Id = Guid.NewGuid(), Flavor = "Strawberry", Color = "Red", Quantity = 50, Price = 2 };
    private static Popsicle popsicle2 = new Popsicle { Id = Guid.NewGuid(), Flavor = "Blue Raspberry", Color = "Blue", Quantity = 30, Price = 2.49m };
    private static Popsicle popsicle3 = new Popsicle { Id = Guid.NewGuid(), Flavor = "Lime", Color = "Green", Quantity = 20, Price = 1.49m };
    private static Popsicle popsicle4 = new Popsicle { Id = Guid.NewGuid(), Flavor = "Mango", Color = "Orange", Quantity = 40, Price = 3 };
    private readonly IList<Popsicle> popsicles = [popsicle1, popsicle2, popsicle3, popsicle4];

    public PopsicleServicesTest()
    {
        _mockRepositoy = new Mock<IPopsicleRepository>();

        _service = new PopsicleService(_mockRepositoy.Object);
    }

    [Fact]
    public async void GetAll_ReturnsPopsiclesList()
    {
        _mockRepositoy.Setup(s => s.GetAll()).ReturnsAsync(popsicles);

        var result = await _service.GetAllPopsicles();

        _mockRepositoy.Verify(x => x.GetAll(), Times.Once);
        Assert.IsAssignableFrom<IEnumerable<Popsicle>>(result);
        Assert.Equal(popsicles.Count, result.Count());
    }

    [Fact]
    public async void GetAll_ReturnsPopsiclesWithEmptyList()
    {
        _mockRepositoy.Setup(s => s.GetAll()).ReturnsAsync([]);

        var result = await _service.GetAllPopsicles();

        _mockRepositoy.Verify(x => x.GetAll(), Times.Once);
        Assert.IsAssignableFrom<IEnumerable<Popsicle>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async void GetById_ReturnPopsicle()
    {
        _mockRepositoy.Setup(s => s.GetById(popsicle1.Id)).ReturnsAsync(popsicle1);

        var result = await _service.GetPopsicleById(popsicle1.Id);

        _mockRepositoy.Verify(x => x.GetById(popsicle1.Id), Times.Once);
        Assert.IsAssignableFrom<Popsicle>(result);
        Assert.NotNull(result);
        Assert.Equal(popsicle1.Id, result.Id);
    }

    [Fact]
    public async void GetById_ReturnNullObject()
    {
        var id = Guid.NewGuid();
        Popsicle? popsicle = null;
        _mockRepositoy.Setup(s => s.GetById(id)).ReturnsAsync(popsicle);

        var result = await _service.GetPopsicleById(id);
        _mockRepositoy.Verify(x => x.GetById(id), Times.Once);
        Assert.Null(result);
    }

    [Fact]
    public async void Create_ValidPopsicle_ReturnPopsicle()
    {
        _mockRepositoy.Setup(s => s.Create(popsicle2));
        _mockRepositoy.Setup(s => s.SaveChanges()).ReturnsAsync(true);

        var result = await _service.CreatePopsicle(popsicle2);

        _mockRepositoy.Verify(x => x.Create(popsicle2), Times.Once);
        _mockRepositoy.Verify(x => x.SaveChanges(), Times.Once);
        Assert.IsAssignableFrom<Popsicle>(result);
        Assert.NotNull(result);
        Assert.Equal(popsicle2, result);
    }

    [Fact]
    public async void UpdateInformation_ReturnPopsicle()
    {
        _mockRepositoy.Setup(s => s.Update(popsicle3.Id, popsicle3)).ReturnsAsync(popsicle3);
        _mockRepositoy.Setup(s => s.SaveChanges()).ReturnsAsync(true);

        var result = await _service.UpdateInformation(popsicle3.Id, popsicle3);

        _mockRepositoy.Verify(x => x.Update(popsicle3.Id, popsicle3), Times.Once);
        _mockRepositoy.Verify(x => x.SaveChanges(), Times.Once);
        Assert.IsAssignableFrom<Popsicle>(result);
        Assert.NotNull(result);
        Assert.Equal(popsicle3, result);
    }

    [Fact]
    public async void UpdateInformation_ReturnNull()
    {
        popsicle3.Id = Guid.NewGuid();
        Popsicle? popsicle = null;
        _mockRepositoy.Setup(s => s.Update(popsicle3.Id, popsicle3)).ReturnsAsync(popsicle);

        var result = await _service.UpdateInformation(popsicle3.Id, popsicle3);

        _mockRepositoy.Verify(x => x.Update(popsicle3.Id, popsicle3), Times.Once);
        _mockRepositoy.Verify(x => x.SaveChanges(), Times.Never);
        Assert.Null(result);
    }

    [Fact]
    public async void PartialUpdate_ReturnPopsicle()
    {
        var serializePopsicle = "{ \"Flavor\": \"Apple\" }";
        var updatedElement = JsonSerializer.Deserialize<JsonElement>(serializePopsicle);
        popsicle3.Flavor = "Apple";
        _mockRepositoy.Setup(s => s.PartialUpdate(popsicle3.Id, updatedElement)).ReturnsAsync(popsicle3);
        _mockRepositoy.Setup(s => s.SaveChanges()).ReturnsAsync(true);

        var result = await _service.PartialUpdate(popsicle3.Id, updatedElement);
        Assert.IsAssignableFrom<Popsicle>(result);
        Assert.NotNull(result);
        Assert.Equal(popsicle3.Flavor, result.Flavor);
    }

    [Fact]
    public async void PartialUpdate_ReturnNull()
    {
        popsicle3.Id = Guid.NewGuid();
        Popsicle? popsicle = null;
        _mockRepositoy.Setup(s => s.Update(popsicle3.Id, popsicle3)).ReturnsAsync(popsicle);

        var result = await _service.UpdateInformation(popsicle3.Id, popsicle3);

        _mockRepositoy.Verify(x => x.Update(popsicle3.Id, popsicle3), Times.Once);
        _mockRepositoy.Verify(x => x.SaveChanges(), Times.Never);
        Assert.Null(result);
    }

    [Fact]
    public async void DeletePopsicle_ReturnSuccess()
    {
        _mockRepositoy.Setup(s => s.Delete(popsicle4.Id)).ReturnsAsync(true);
        _mockRepositoy.Setup(s => s.SaveChanges()).ReturnsAsync(true);

        var result = await _service.DeletePopsicle(popsicle4.Id);

        _mockRepositoy.Verify(x => x.Delete(popsicle4.Id), Times.Once);
        _mockRepositoy.Verify(x => x.SaveChanges(), Times.Once);
        Assert.IsAssignableFrom<bool>(result);
        Assert.True(result);
    }

    [Fact]
    public async void DeletePopsicle_ReturnFailure()
    {
        _mockRepositoy.Setup(s => s.Delete(popsicle4.Id)).ReturnsAsync(false);

        var result = await _service.DeletePopsicle(popsicle4.Id);

        _mockRepositoy.Verify(x => x.Delete(popsicle4.Id), Times.Once);
        _mockRepositoy.Verify(x => x.SaveChanges(), Times.Never);
        Assert.IsAssignableFrom<bool>(result);
        Assert.False(result);
    }

    [Fact]
    public async void SearchPopsicle_ReturnsPopsicleList()
    {
        _mockRepositoy.Setup(s => s.Search(popsicle1.Flavor, null, null)).ReturnsAsync(popsicles);

        var result = await _service.Search(popsicle1.Flavor, null, null);

        _mockRepositoy.Verify(x => x.Search(popsicle1.Flavor, null, null), Times.Once);
        Assert.IsAssignableFrom<IEnumerable<Popsicle>>(result);
        Assert.Equal(popsicles.Count, result.Count());
    }

    [Fact]
    public async void SearchPopsicle_ReturnsPopsiclesWithEmptyList()
    {
        _mockRepositoy.Setup(s => s.Search(null, null, null)).ReturnsAsync([]);

        var result = await _service.Search(null, null, null);

        _mockRepositoy.Verify(x => x.Search(null, null, null), Times.Once);
        Assert.IsAssignableFrom<IEnumerable<Popsicle>>(result);
        Assert.Empty(result);
    }
}
