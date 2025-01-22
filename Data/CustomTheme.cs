// Licensed to the .NET Foundation under one or more agreements.

using System.ComponentModel.DataAnnotations;
using MudBlazor;

namespace BlazorServerTemplate.Data
{
    public class CustomTheme
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public MudTheme Theme { get; set; } = default!;
    }
}
