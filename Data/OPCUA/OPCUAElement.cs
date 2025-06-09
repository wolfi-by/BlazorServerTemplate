// Licensed to the .NET Foundation under one or more agreements.

namespace BlazorServerTemplate.Data.OPCUA
{
    public class OPCUAElement
    {
        public Guid ID { get; set; }
        public int Type { get; set; } = OPCUAElementType.None;
        public string Label { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public double Top { get; set; }
        public double Left { get; set; }
        
    }
}
