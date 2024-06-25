using SafariLib.Repositories.Repository;
using Test.Utils.Data;
using Test.Utils.Data.Models;
using Test.Utils.Integration;

namespace SafariLib.EFRepositories.Tests.Repository;

public class RepositoryTest
{
    private readonly TestContext _context = new MemoryDatabase().Context;
    private IRepository<TestContext, UserWithGuid> _repository;

    [Fact]
    public void GetById_ShouldReturnUser()
    {
        // Arrange
        var user = Setup(new UserWithGuid());

        // Act
        var userById = _repository.GetByPrimaryKey(user.Id);

        // Assert
        Assert.NotNull(userById);
        Assert.Equal(user.Id, userById.Id);
    }

    [Fact]
    public async void GetByIdAsync_ShouldReturnUser()
    {
        // Arrange
        var user = Setup(new UserWithGuid());

        // Act
        var userById = await _repository.GetByPrimaryKeyAsync(user.Id);

        // Assert
        Assert.NotNull(userById);
        Assert.Equal(user.Id, userById.Id);
    }

    [Fact]
    public void Create_ShouldCreateEntity()
    {
        // Arrange
        Setup();
        var user = new UserWithGuid();

        // Act
        _repository.Create(user);
        _repository.Save();
        var createdUser = _repository.Get(u => u.Username == user.Username).FirstOrDefault();

        // Assert
        Assert.NotNull(createdUser);
        Assert.Equal(user.Username, createdUser.Username);
        Assert.True(createdUser?.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async void CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        Setup();
        var user = new UserWithGuid();

        // Act
        await _repository.CreateAsync(user);
        await _repository.SaveAsync();
        var createdUser = _repository.Get(u => u.Username == user.Username).FirstOrDefault();

        // Assert
        Assert.NotNull(createdUser);
        Assert.Equal(user.Username, createdUser.Username);
        Assert.True(createdUser?.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Update_ShouldUpdateUser()
    {
        // Arrange
        var user = Setup(new UserWithGuid());
        user.Username = "John Arbuckle";

        // Act
        _repository.Update(user);
        _repository.Save();
        var updatedUser = _repository.Get(u => u.Username == user.Username).FirstOrDefault()!;

        // Assert
        Assert.Equal(user.Username, updatedUser.Username);
        Assert.True(updatedUser.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Delete_ShouldDeleteUser()
    {
        // Arrange
        var user = Setup(new UserWithGuid());

        // Act
        var userToDelete = _repository.Get(u => u.Username == user.Username).FirstOrDefault();
        _repository.Delete(userToDelete!);
        _repository.Save();

        // Assert
        var deletedUser = _repository.Get(u => u.Username == user.Username).FirstOrDefault();
        Assert.True(deletedUser is null);
    }

    private UserWithGuid Setup(UserWithGuid userWithGuid)
    {
        Setup();
        _repository.Create(userWithGuid);
        _repository.Save();
        return _repository.Get(u => u.Username == userWithGuid.Username).FirstOrDefault()!;
    }

    private void Setup() => _repository = new Repository<TestContext, UserWithGuid>(_context);
}