﻿//=================================================================================================
// Copyright 2013-2016 Dirk Lemstra <https://magick.codeplex.com/>
//
// Licensed under the ImageMagick License (the "License"); you may not use this file except in 
// compliance with the License. You may obtain a copy of the License at
//
//   http://www.imagemagick.org/script/license.php
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//=================================================================================================

using ImageMagick;
using ImageMagick.ImageOptimizers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magick.NET.Tests
{
  [TestClass]
  public class GifOptimizerTests : IImageOptimizerTests
  {
    private const string _Category = "GifOptimizer";

    protected override ILosslessImageOptimizer CreateLosslessImageOptimizer()
    {
      return new GifOptimizer();
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_InvalidArguments()
    {
      Test_LosslessCompress_InvalidArguments();
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_LosslessCompress()
    {
      Test_LosslessCompress(Files.RoseSparkleGIF, false);
      Test_LosslessCompress(Files.FujiFilmFinePixS1ProGIF);
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_LosslessCompress_InvalidFile()
    {
      Test_LosslessCompress_InvalidFile(Files.InvitationTif);
    }
  }
}
