using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafariLib.Repositories.Models;

/// <summary>
///     Used as a base class for all entities in the system.
/// </summary>
/// <remarks>
///     <list>
///         <item>
///             Where Id is a Guid as a unique identifier for the entity.
///         </item>
///     </list>
/// </remarks>
public abstract class EntityWithGuid : BaseEntity
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
}