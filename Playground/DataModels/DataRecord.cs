using System;
using System.Collections.Generic;
using System.Text;

namespace Playground.DataModels
{
    /// <summary>
    /// This should combine the values from R and X to hold a unique record
    /// </summary>
    public class DataRecord(RValue rValue, XValue xValue)
    {
        /// <summary>
        /// This is the 'Y' coordinate, or amplitude in the plot
        /// </summary>
        public double R { get; } = rValue.R;
        public double Az { get; } = xValue.Az;
        public double El { get; } = xValue.El;
        public double Rg { get; } = xValue.Rg;
    }
}
