using SafariLib.Repositories.Repository;
using SafariLib.Repositories.RepositoryService;
using Test.Utils.Data;
using Test.Utils.Data.Models;
using Test.Utils.Integration;

namespace SafariLib.EFRepositories.Tests.RepositoryService;

public class RepositoryServiceTest
{
    private readonly TestContext _context = new MemoryDatabase().Context;
    private IRepositoryService<User> _repository;
    private IRepositoryService<UserWithGuid> _repositoryWithGuid;

    [Fact]
    public void GetById_WithInt_ShouldReturnEntity()
    {
        // Arrange
        var user = Setup(new User());

        // Act
        var userById = _repository.GetById(user.Id);

        // Assert
        Assert.NotNull(userById);
        Assert.Equal(user.Id, userById.Value!.Id);
    }

    [Fact]
    public async void GetByIdAsync_WithInt_ShouldReturnEntity()
    {
        // Arrange
        var user = Setup(new User());

        // Act
        var userById = await _repository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(userById);
        Assert.Equal(user.Id, userById.Value!.Id);
    }

    [Fact]
    public void GetById_WithGuid_ShouldReturnEntity()
    {
        // Arrange
        var user = Setup(new UserWithGuid());

        // Act
        var userById = _repositoryWithGuid.GetById(user.Id);

        // Assert
        Assert.NotNull(userById);
        Assert.Equal(user.Id, userById.Value!.Id);
    }

    [Fact]
    public async void GetByIdAsync_WithGuid_ShouldReturnEntity()
    {
        // Arrange
        var user = Setup(new UserWithGuid());

        // Act
        var userById = await _repositoryWithGuid.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(userById);
        Assert.Equal(user.Id, userById.Value!.Id);
    }

    private User Setup(User user)
    {
        Setup();
        _repository.Create(user);
        _repository.Save();
        return _repository.Get(u => u.Username == user.Username).Value!.FirstOrDefault()!;
    }

    private UserWithGuid Setup(UserWithGuid userWithGuid)
    {
        Setup();
        _repositoryWithGuid.Create(userWithGuid);
        _repositoryWithGuid.Save();
        return _repositoryWithGuid.Get(u => u.Username == userWithGuid.Username).Value!.FirstOrDefault()!;
    }


    private void Setup()
    {
        _repository = new RepositoryService<TestContext, User>(new Repository<TestContext, User>(_context));
        _repositoryWithGuid =
            new RepositoryService<TestContext, UserWithGuid>(new Repository<TestContext, UserWithGuid>(_context));
    }
}