﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zero.NETCore.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zero.NETCore.Extensions.Tests
{
    [TestClass()]
    public class DateTimeExtensionsTests
    {
        [TestMethod()]
        public void ToTimeStringTest()
        {
            var s = 99999;
            _ = s.ToTimeString();
            Assert.Fail();
        }
    }
}