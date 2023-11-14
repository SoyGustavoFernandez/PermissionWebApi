using System;
using System.Collections.Generic;

namespace API_Models.Models;

/// <summary>
/// Table where permits are recorded.
/// </summary>
public partial class TblPermission
{
    /// <summary>
    /// Unique ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Employee Forename.
    /// </summary>
    public string? EmployeeForename { get; set; }

    /// <summary>
    /// Employee Surename.
    /// </summary>
    public string? EmployeeSurename { get; set; }

    /// <summary>
    /// Permission Type.
    /// </summary>
    public int? PermissionType { get; set; }

    /// <summary>
    /// Permission granted on Date.
    /// </summary>
    public DateTime? PermissionDate { get; set; }

    /// <summary>
    /// Register status.
    /// </summary>
    public bool? Active { get; set; }

    public virtual TblPermissionType? PermissionTypeNavigation { get; set; }
}
