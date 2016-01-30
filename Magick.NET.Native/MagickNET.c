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

#include "Stdafx.h"
#include "MagickNET.h"

MAGICK_NET_EXPORT const char *MagickNET_Features_Get(void)
{
  return GetMagickFeatures();
}

MAGICK_NET_EXPORT void MagickNET_SetEnv(const char *name, const char *value)
{
  _putenv_s(name, value);
}

MAGICK_NET_EXPORT void MagickNET_SetRandomSeed(const unsigned long seed)
{
  SetRandomSecretKey(seed);
}

MAGICK_NET_EXPORT void MagickNET_SetLogDelegate(const MagickLogMethod method)
{
  SetLogMethod(method);
}

MAGICK_NET_EXPORT void MagickNET_SetLogEvents(const char *events)
{
  SetLogEventMask(events);
}

MAGICK_NET_EXPORT MagickBooleanType MagickNET_SetUseOpenCL(const MagickBooleanType value, ExceptionInfo **exception)
{
  MagickBooleanType
    result;

  MAGICK_NET_GET_EXCEPTION;
  result = MagickFalse;
  if (!value)
    InitImageMagickOpenCL(MAGICK_OPENCL_OFF, NULL, NULL, exceptionInfo);
  else
    result = InitImageMagickOpenCL(MAGICK_OPENCL_DEVICE_SELECT_AUTO, NULL, NULL, exceptionInfo);
  MAGICK_NET_SET_EXCEPTION;
  return result;
}