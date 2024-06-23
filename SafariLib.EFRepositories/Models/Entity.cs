using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafariLib.EFRepositories.Models;

/// <summary>
///     Used as a base class for all entities in the system.
/// </summary>
/// <remarks>
///     Adds an Id, CreatedAt, and UpdatedAt property to all entities.
/// </remarks>
public abstract class Entity
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Column("created_at")] [Required] public DateTime CreatedAt { get; init; }

    [Column("updated_at")] public DateTime? UpdatedAt { get; init; }
}