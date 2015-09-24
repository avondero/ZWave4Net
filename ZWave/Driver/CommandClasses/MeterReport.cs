﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZWave.Driver.Communication;

namespace ZWave.Driver.CommandClasses
{
    public class MeterReport : NodeReport
    {
        public readonly MeterType Type;
        public readonly float Value;
        public readonly string Unit;
        public readonly byte Scale;

        internal MeterReport(Node node, byte[] payload) : base(node)
        {
            Type = (MeterType)(payload[0]);
            var ignore = default(byte);
            Value = PayloadConverter.ParseSensorValue(payload.Skip(1).ToArray(), out ignore, out Scale, out ignore);
            Unit = GetUnit(Type, Scale);
        }

        private static string GetUnit(MeterType type, byte scale)
        {
            var electricityUnits = new[] { "kWh", "kVAh", "W", "pulses", "V", "A", "Power Factor", "" };
            var gasUnits = new[] { "cubic meters", "cubic feet", "", "pulses", "", "", "", "" };
            var waterUnits = new[] { "cubic meters", "cubic feet", "US gallons",  "pulses", "", "", "", ""};

            switch (type)
            {
                case MeterType.Electric: return electricityUnits[scale];
                case MeterType.Gas: return gasUnits[scale];
                case MeterType.Water: return waterUnits[scale];
                default: return string.Empty;
            }
        }

        public override string ToString()
        {
            return $"Type:{Type}, Value:\"{Value} {Unit}\"";
        }
    }
}
