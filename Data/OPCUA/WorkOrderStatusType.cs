// Licensed to the .NET Foundation under one or more agreements.

namespace BlazorServerTemplate.Data.OPCUA
{
    public class WorkOrderStatusType
    {
        public string Actor { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
