// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Ili934x
{
    /// <summary> Control flags for Memory Access Control (36h). </summary>
    internal enum MemoryAccessControlFlags : byte
    {
        /// <summary> Row Address Order. </summary>
        MY = 0b10000000,

        /// <summary> Column Address Order. </summary>
        MX = 0b01000000,

        /// <summary> Row / Column Exchange. </summary>
        MV = 0b00100000,

        /// <summary> Vertical Refresh Order.<br/>LCD vertical refresh direction control. </summary>
        ML = 0b00010000,

        /// <summary> RGB-BGR Order Color selector switch control.<br/>
        /// (0=RGB color filter panel, 1=BGR color filter panel).</summary>
        BGR = 0b00001000,

        /// <summary> Horizontal Refresh ORDER.<br/>LCD horizontal refreshing direction control. </summary>
        MH = 0b00000100

    }
}
