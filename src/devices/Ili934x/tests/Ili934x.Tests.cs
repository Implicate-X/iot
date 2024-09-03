// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.Gpio;
using System.Device.Spi;
using System.Drawing;
using Iot.Device.Ili934x;
using Iot.Device.Graphics;
using Moq;
using Xunit;

namespace Ili934x.Tests
{
    public class Ili9342Test : IDisposable
    {
        private readonly DummySpiDriver _spiMock;
        private readonly GpioController _gpioController;
        private readonly Mock<MockableGpioDriver> _gpioDriverMock;
        private readonly Mock<IImageFactory> _imageFactoryMock;

        private Ili9342? _testee;

        public Ili9342Test()
        {
            _spiMock = new DummySpiDriver();
            _gpioDriverMock = new Mock<MockableGpioDriver>(MockBehavior.Loose);
            _imageFactoryMock = new Mock<IImageFactory>(MockBehavior.Strict);
            BitmapImage.RegisterImageFactory(_imageFactoryMock.Object);
            _gpioDriverMock.CallBase = true;
            _gpioController = new GpioController(PinNumberingScheme.Logical, _gpioDriverMock.Object);
        }

        [Fact]
        public void Init()
        {
            _gpioDriverMock.Setup(x => x.OpenPinEx(15));
            _gpioDriverMock.Setup(x => x.IsPinModeSupportedEx(It.Is<int>(y => y == 15 || y == 2 || y == 3), PinMode.Output)).Returns(true);
            _gpioDriverMock.Setup(x => x.OpenPinEx(2));
            _gpioDriverMock.Setup(x => x.OpenPinEx(3));
            _gpioDriverMock.Setup(x => x.WriteEx(15, It.IsAny<PinValue>()));
            _testee = new Ili9342(_spiMock, 15, 2, 3, 4096, _gpioController);

            Assert.NotEmpty(_spiMock.Data);
        }

        [Fact]
        public void Size()
        {
            Init();
            Assert.Equal(320, _testee!.ScreenWidth);
            Assert.Equal(240, _testee.ScreenHeight);
        }

        [Fact]
        public void SendImage()
        {
            Init();

            _imageFactoryMock.Setup(x => x.CreateBitmap(It.IsAny<int>(), It.IsAny<int>(), PixelFormat.Format32bppArgb)).Returns(
                new Func<int, int, PixelFormat, BitmapImage>((w, h, pf) =>
                {
                    var m = new Mock<BitmapImage>(MockBehavior.Loose, w, h, w * 4, pf)
                    {
                        CallBase = true
                    };
                    return m.Object;
                }));
            using BitmapImage bmp = _testee!.GetBackBufferCompatibleImage();

            Assert.Equal(320, bmp.Width);
            Assert.Equal(240, bmp.Height);

            bmp.SetPixel(0, 0, Color.White);
            _spiMock.Data.Clear();
            _testee.DrawBitmap(bmp);
            _testee.SendFrame(true);

            // 11 bytes setup + 2 bytes per pixel (this is raw SPI data, not including any possible Arduino headers)
            Assert.Equal(11 + 320 * 240 * 2, _spiMock.Data.Count);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
