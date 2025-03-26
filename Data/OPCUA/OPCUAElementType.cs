// Licensed to the .NET Foundation under one or more agreements.

using Ardalis.SmartEnum;

namespace BlazorServerTemplate.Data.OPCUA
{
    public sealed class OPCUAElementType:SmartEnum<OPCUAElementType>
    {
        public static readonly OPCUAElementType None = new OPCUAElementType(nameof(None), 0);
        public static readonly OPCUAElementType ToggleSwitch = new OPCUAElementType(nameof(ToggleSwitch), 1);
        public static readonly OPCUAElementType IntField = new OPCUAElementType(nameof(IntField), 2);
        public static readonly OPCUAElementType FloatField = new OPCUAElementType(nameof(FloatField), 3);

        private OPCUAElementType(string name, int value) : base(name, value)
        {
        }
    }
}
