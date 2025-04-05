﻿// Licensed to the .NET Foundation under one or more agreements.

namespace BlazorServerTemplate.Data.OPCUA
{
    public class WorkOrderVariable
    {
        public string AssetID { get; set; } = string.Empty;
        public Guid ID { get; set; } = Guid.NewGuid();
        public DateTime StartTime { get; set; } = DateTime.Now;
        public IList<WorkOrderStatusType> StatusComments { get; set; }=new List<WorkOrderStatusType>();
    }
}
