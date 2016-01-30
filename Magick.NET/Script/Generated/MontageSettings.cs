//=================================================================================================
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;

#if Q8
using QuantumType = System.Byte;
#elif Q16
using QuantumType = System.UInt16;
#elif Q16HDRI
using QuantumType = System.Single;
#else
#error Not implemented!
#endif

namespace ImageMagick
{
  public sealed partial class MagickScript
  {
    private MontageSettings CreateMontageSettings(XmlElement element)
    {
      if (element == null)
        return null;
      MontageSettings result = new MontageSettings();
      result.BackgroundColor = Variables.GetValue<MagickColor>(element, "backgroundColor");
      result.BorderColor = Variables.GetValue<MagickColor>(element, "borderColor");
      result.BorderWidth = Variables.GetValue<Int32>(element, "borderWidth");
      result.FillColor = Variables.GetValue<MagickColor>(element, "fillColor");
      result.Font = Variables.GetValue<String>(element, "font");
      result.FontPointsize = Variables.GetValue<Int32>(element, "fontPointsize");
      result.FrameGeometry = Variables.GetValue<MagickGeometry>(element, "frameGeometry");
      result.Geometry = Variables.GetValue<MagickGeometry>(element, "geometry");
      result.Gravity = Variables.GetValue<Gravity>(element, "gravity");
      result.Label = Variables.GetValue<String>(element, "label");
      result.Shadow = Variables.GetValue<Boolean>(element, "shadow");
      result.StrokeColor = Variables.GetValue<MagickColor>(element, "strokeColor");
      result.TextureFileName = Variables.GetValue<String>(element, "textureFileName");
      result.TileGeometry = Variables.GetValue<MagickGeometry>(element, "tileGeometry");
      result.Title = Variables.GetValue<String>(element, "title");
      result.TransparentColor = Variables.GetValue<MagickColor>(element, "transparentColor");
      return result;
    }
  }
}
