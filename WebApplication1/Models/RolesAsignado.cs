using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class RolesAsignado
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public virtual Role Rol { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
