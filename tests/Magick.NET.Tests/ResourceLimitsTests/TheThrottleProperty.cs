﻿// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

using ImageMagick;
using Xunit;

namespace Magick.NET.Tests
{
    public partial class ResourceLimitsTests
    {
        [CollectionDefinition(nameof(RunTestsSeparately))]
        public class TheThrottleProperty
        {
            [Fact]
            public void ShouldHaveTheCorrectValue()
            {
                Assert.Equal(0U, ResourceLimits.Throttle);
            }

            [Fact]
            public void ShouldReturnTheCorrectValueWhenChanged()
            {
                var throttle = ResourceLimits.Throttle;

                ResourceLimits.Throttle = 1U;
                Assert.Equal(1U, ResourceLimits.Throttle);
                ResourceLimits.Throttle = throttle;
            }
        }
    }
}
