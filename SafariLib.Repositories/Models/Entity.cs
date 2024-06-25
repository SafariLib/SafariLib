using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafariLib.Repositories.Models;

/// <summary>
///     Used as a base class for all entities in the system.
/// </summary>
/// <remarks>
///     <list>
///         <item>
///             Where Id is an Int as a unique identifier for the entity.
///         </item>
///     </list>
/// </remarks>
public abstract class Entity : BaseEntity
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
}