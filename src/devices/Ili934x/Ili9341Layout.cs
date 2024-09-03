// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Ili934x
{
    /// <summary> Values that represent orientations. </summary>
    public enum Orientation : byte
    {
        /// <summary> Default or normal orientation. </summary>
        PortraitNormal = Ili9341.PortraitNormal,

        /// <summary> Rotated 90 degrees clockwise. </summary>
        LandscapeNormal = Ili9341.LandscapeNormal,

        /// <summary> Rotated 180 degrees clockwise. </summary>
        PortraitFlipped = Ili9341.PortraitFlipped,

        /// <summary> Rotated 270 degrees clockwise. </summary>
        LandscapeFlipped = Ili9341.LandscapeFlipped
    }

    /// <summary>
    /// The ILI9341 is a QVGA (Quarter VGA) driver integrated circuit that is used to control 240×320 VGA LCD screens.
    /// </summary>
    public partial class Ili9341
    {
        internal const byte PortraitNormal =
            (byte)(MemoryAccessControlFlags.MX | MemoryAccessControlFlags.BGR);
        internal const byte PortraitFlipped =
            (byte)(MemoryAccessControlFlags.MY | MemoryAccessControlFlags.BGR);
        internal const byte LandscapeNormal =
            (byte)(MemoryAccessControlFlags.MV | MemoryAccessControlFlags.BGR);
        internal const byte LandscapeFlipped =
            (byte)(MemoryAccessControlFlags.MV | MemoryAccessControlFlags.BGR | MemoryAccessControlFlags.MY | MemoryAccessControlFlags.MX);

        /// <summary>
        /// Width of the screen, in pixels
        /// </summary>
        /// <remarks>This is of type int, because all image sizes use int, even though this can never be negative</remarks>
        public override int ScreenWidth => _orientation switch
        {
            Orientation.PortraitNormal => _sx,
            Orientation.LandscapeNormal => _sy,
            Orientation.PortraitFlipped => _sx,
            Orientation.LandscapeFlipped => _sy,
            _ => _sx
        };

        /// <summary>
        /// Height of the screen, in pixels
        /// </summary>
        /// <remarks>This is of type int, because all image sizes use int, even though this can never be negative</remarks>
        public override int ScreenHeight => _orientation switch
        {
            Orientation.PortraitNormal => _sy,
            Orientation.LandscapeNormal => _sx,
            Orientation.PortraitFlipped => _sy,
            Orientation.LandscapeFlipped => _sx,
            _ => _sy
        };
    }
}
