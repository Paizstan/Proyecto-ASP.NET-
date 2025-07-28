using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
}
