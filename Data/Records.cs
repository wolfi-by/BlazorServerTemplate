// Licensed to the .NET Foundation under one or more agreements.

using System.ComponentModel.DataAnnotations;

namespace BlazorServerTemplate.Data
{
    public record QuantityRecord(double Value, string QuantityName, string UnitName , string Id);
    public record QuantityMappingRecord(double Value, string QuantityName, string UnitName, double MinValue, double MaxValue, string id);

    public class QuantityDto()
    {
        [Key]
        public Guid Id { get; set; }
        public double Value { get; set; }
        public string QuantityName { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;

        public QuantityRecord ToRecord(QuantityDto quantityDto)
        {
            return new QuantityRecord(quantityDto.Value, quantityDto.QuantityName, quantityDto.UnitName, quantityDto.Id.ToString());
        }
        public static QuantityDto FromRecord(QuantityRecord record)
        {
            return new QuantityDto()
            {
                Value = record.Value,
                QuantityName = record.QuantityName,
                UnitName = record.UnitName
            };
        }
    }
    public class QuantityMappingDto()
    {
        [Key]
        public Guid Id { get; set; }
        public double Value { get; set; }
        public string QuantityName { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public QuantityMappingRecord ToRecord(QuantityMappingDto record)
        {
            return new QuantityMappingRecord(record.Value, record.QuantityName, record. UnitName, record. MinValue, record .MaxValue, record.Id.ToString());
        }
        public static QuantityMappingDto FromRecord(QuantityMappingRecord record)
        {
            return new QuantityMappingDto()
            {
                Value = record.Value,
                QuantityName = record.QuantityName,
                UnitName = record.UnitName,
                MinValue = record.MinValue,
                MaxValue = record.MaxValue
            };
        }
    }
}
