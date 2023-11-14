using System;
using System.Collections.Generic;

namespace API_Models.Models;

/// <summary>
/// Table showing the types of permits.
/// </summary>
public partial class TblPermissionType
{
    /// <summary>
    /// Unique ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Permission description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Register status.
    /// </summary>
    public bool? Active { get; set; }

    public virtual ICollection<TblPermission> TblPermissions { get; set; } = new List<TblPermission>();
}
