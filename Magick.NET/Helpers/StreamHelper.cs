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

using System.IO;

namespace ImageMagick
{
  internal static class StreamHelper
  {
    internal static byte[] ToByteArray(Stream stream)
    {
      Throw.IfNull("stream", stream);

      MemoryStream memStream = stream as MemoryStream;
      if (memStream != null)
        return memStream.ToArray();

      if (stream.CanSeek)
      {
        int length = (int)stream.Length;
        if (length == 0)
          return new byte[0];

        byte[] result = new byte[length];
        stream.Read(result, 0, length);
        return result;
      }
      else
      {
        int bufferSize = 8192;
        using (MemoryStream tmpStream = new MemoryStream())
        {
          byte[] buffer = new byte[bufferSize];
          int length;
          while ((length = stream.Read(buffer, 0, bufferSize)) != 0)
          {
            tmpStream.Write(buffer, 0, length);
          }

          return tmpStream.ToArray();
        }
      }
    }
  }
}
