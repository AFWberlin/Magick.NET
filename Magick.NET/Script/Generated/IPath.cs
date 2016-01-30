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
    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
    private void ExecuteIPath(XmlElement element, Collection<IPath> paths)
    {
      switch(element.Name[0])
      {
        case 'a':
        {
          switch(element.Name[3])
          {
            case 'A':
            {
              ExecutePathArcAbs(element, paths);
              return;
            }
            case 'R':
            {
              ExecutePathArcRel(element, paths);
              return;
            }
          }
          break;
        }
        case 'c':
        {
          switch(element.Name[7])
          {
            case 'A':
            {
              ExecutePathCurveToAbs(element, paths);
              return;
            }
            case 'R':
            {
              ExecutePathCurveToRel(element, paths);
              return;
            }
          }
          break;
        }
        case 'l':
        {
          switch(element.Name[6])
          {
            case 'A':
            {
              ExecutePathLineToAbs(element, paths);
              return;
            }
            case 'H':
            {
              switch(element.Name[16])
              {
                case 'A':
                {
                  ExecutePathLineToHorizontalAbs(element, paths);
                  return;
                }
                case 'R':
                {
                  ExecutePathLineToHorizontalRel(element, paths);
                  return;
                }
              }
              break;
            }
            case 'R':
            {
              ExecutePathLineToRel(element, paths);
              return;
            }
            case 'V':
            {
              switch(element.Name[14])
              {
                case 'A':
                {
                  ExecutePathLineToVerticalAbs(element, paths);
                  return;
                }
                case 'R':
                {
                  ExecutePathLineToVerticalRel(element, paths);
                  return;
                }
              }
              break;
            }
          }
          break;
        }
        case 'm':
        {
          switch(element.Name[6])
          {
            case 'A':
            {
              ExecutePathMoveToAbs(element, paths);
              return;
            }
            case 'R':
            {
              ExecutePathMoveToRel(element, paths);
              return;
            }
          }
          break;
        }
        case 'q':
        {
          switch(element.Name[16])
          {
            case 'A':
            {
              ExecutePathQuadraticCurveToAbs(element, paths);
              return;
            }
            case 'R':
            {
              ExecutePathQuadraticCurveToRel(element, paths);
              return;
            }
          }
          break;
        }
        case 's':
        {
          switch(element.Name[6])
          {
            case 'C':
            {
              switch(element.Name[13])
              {
                case 'A':
                {
                  ExecutePathSmoothCurveToAbs(element, paths);
                  return;
                }
                case 'R':
                {
                  ExecutePathSmoothCurveToRel(element, paths);
                  return;
                }
              }
              break;
            }
            case 'Q':
            {
              switch(element.Name[22])
              {
                case 'A':
                {
                  ExecutePathSmoothQuadraticCurveToAbs(element, paths);
                  return;
                }
                case 'R':
                {
                  ExecutePathSmoothQuadraticCurveToRel(element, paths);
                  return;
                }
              }
              break;
            }
          }
          break;
        }
      }
      throw new NotImplementedException(element.Name);
    }
    private void ExecutePathArcAbs(XmlElement element, Collection<IPath> paths)
    {
      IEnumerable<PathArc> pathArcs_ = CreatePathArcs(element);
      paths.Add(new PathArcAbs(pathArcs_));
    }
    private void ExecutePathArcRel(XmlElement element, Collection<IPath> paths)
    {
      IEnumerable<PathArc> pathArcs_ = CreatePathArcs(element);
      paths.Add(new PathArcRel(pathArcs_));
    }
    private void ExecutePathCurveToAbs(XmlElement element, Collection<IPath> paths)
    {
      Hashtable arguments = new Hashtable();
      foreach (XmlAttribute attribute in element.Attributes)
      {
        if (attribute.Name == "controlPointEnd")
          arguments["controlPointEnd"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "controlPointStart")
          arguments["controlPointStart"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "end")
          arguments["end"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "x")
          arguments["x"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "x1")
          arguments["x1"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "x2")
          arguments["x2"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y")
          arguments["y"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y1")
          arguments["y1"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y2")
          arguments["y2"] = Variables.GetValue<double>(attribute);
      }
      if (OnlyContains(arguments, "controlPointStart", "controlPointEnd", "end"))
        paths.Add(new PathCurveToAbs((PointD)arguments["controlPointStart"], (PointD)arguments["controlPointEnd"], (PointD)arguments["end"]));
      else if (OnlyContains(arguments, "x1", "y1", "x2", "y2", "x", "y"))
        paths.Add(new PathCurveToAbs((double)arguments["x1"], (double)arguments["y1"], (double)arguments["x2"], (double)arguments["y2"], (double)arguments["x"], (double)arguments["y"]));
      else
        throw new ArgumentException("Invalid argument combination for 'curveToAbs', allowed combinations are: [controlPointStart, controlPointEnd, end] [x1, y1, x2, y2, x, y]");
    }
    private void ExecutePathCurveToRel(XmlElement element, Collection<IPath> paths)
    {
      Hashtable arguments = new Hashtable();
      foreach (XmlAttribute attribute in element.Attributes)
      {
        if (attribute.Name == "controlPointEnd")
          arguments["controlPointEnd"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "controlPointStart")
          arguments["controlPointStart"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "end")
          arguments["end"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "x")
          arguments["x"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "x1")
          arguments["x1"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "x2")
          arguments["x2"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y")
          arguments["y"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y1")
          arguments["y1"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y2")
          arguments["y2"] = Variables.GetValue<double>(attribute);
      }
      if (OnlyContains(arguments, "controlPointStart", "controlPointEnd", "end"))
        paths.Add(new PathCurveToRel((PointD)arguments["controlPointStart"], (PointD)arguments["controlPointEnd"], (PointD)arguments["end"]));
      else if (OnlyContains(arguments, "x1", "y1", "x2", "y2", "x", "y"))
        paths.Add(new PathCurveToRel((double)arguments["x1"], (double)arguments["y1"], (double)arguments["x2"], (double)arguments["y2"], (double)arguments["x"], (double)arguments["y"]));
      else
        throw new ArgumentException("Invalid argument combination for 'curveToRel', allowed combinations are: [controlPointStart, controlPointEnd, end] [x1, y1, x2, y2, x, y]");
    }
    private void ExecutePathLineToAbs(XmlElement element, Collection<IPath> paths)
    {
      IEnumerable<PointD> coordinates_ = CreatePointDs(element);
      paths.Add(new PathLineToAbs(coordinates_));
    }
    private void ExecutePathLineToHorizontalAbs(XmlElement element, Collection<IPath> paths)
    {
      double x_ = Variables.GetValue<double>(element, "x");
      paths.Add(new PathLineToHorizontalAbs(x_));
    }
    private void ExecutePathLineToHorizontalRel(XmlElement element, Collection<IPath> paths)
    {
      double x_ = Variables.GetValue<double>(element, "x");
      paths.Add(new PathLineToHorizontalRel(x_));
    }
    private void ExecutePathLineToRel(XmlElement element, Collection<IPath> paths)
    {
      IEnumerable<PointD> coordinates_ = CreatePointDs(element);
      paths.Add(new PathLineToRel(coordinates_));
    }
    private void ExecutePathLineToVerticalAbs(XmlElement element, Collection<IPath> paths)
    {
      double y_ = Variables.GetValue<double>(element, "y");
      paths.Add(new PathLineToVerticalAbs(y_));
    }
    private void ExecutePathLineToVerticalRel(XmlElement element, Collection<IPath> paths)
    {
      double y_ = Variables.GetValue<double>(element, "y");
      paths.Add(new PathLineToVerticalRel(y_));
    }
    private void ExecutePathMoveToAbs(XmlElement element, Collection<IPath> paths)
    {
      PointD coordinate_ = Variables.GetValue<PointD>(element, "coordinate");
      paths.Add(new PathMoveToAbs(coordinate_));
    }
    private void ExecutePathMoveToRel(XmlElement element, Collection<IPath> paths)
    {
      PointD coordinate_ = Variables.GetValue<PointD>(element, "coordinate");
      paths.Add(new PathMoveToRel(coordinate_));
    }
    private void ExecutePathQuadraticCurveToAbs(XmlElement element, Collection<IPath> paths)
    {
      Hashtable arguments = new Hashtable();
      foreach (XmlAttribute attribute in element.Attributes)
      {
        if (attribute.Name == "controlPoint")
          arguments["controlPoint"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "end")
          arguments["end"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "x")
          arguments["x"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "x1")
          arguments["x1"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y")
          arguments["y"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y1")
          arguments["y1"] = Variables.GetValue<double>(attribute);
      }
      if (OnlyContains(arguments, "controlPoint", "end"))
        paths.Add(new PathQuadraticCurveToAbs((PointD)arguments["controlPoint"], (PointD)arguments["end"]));
      else if (OnlyContains(arguments, "x1", "y1", "x", "y"))
        paths.Add(new PathQuadraticCurveToAbs((double)arguments["x1"], (double)arguments["y1"], (double)arguments["x"], (double)arguments["y"]));
      else
        throw new ArgumentException("Invalid argument combination for 'quadraticCurveToAbs', allowed combinations are: [controlPoint, end] [x1, y1, x, y]");
    }
    private void ExecutePathQuadraticCurveToRel(XmlElement element, Collection<IPath> paths)
    {
      Hashtable arguments = new Hashtable();
      foreach (XmlAttribute attribute in element.Attributes)
      {
        if (attribute.Name == "controlPoint")
          arguments["controlPoint"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "end")
          arguments["end"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "x")
          arguments["x"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "x1")
          arguments["x1"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y")
          arguments["y"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y1")
          arguments["y1"] = Variables.GetValue<double>(attribute);
      }
      if (OnlyContains(arguments, "controlPoint", "end"))
        paths.Add(new PathQuadraticCurveToRel((PointD)arguments["controlPoint"], (PointD)arguments["end"]));
      else if (OnlyContains(arguments, "x1", "y1", "x", "y"))
        paths.Add(new PathQuadraticCurveToRel((double)arguments["x1"], (double)arguments["y1"], (double)arguments["x"], (double)arguments["y"]));
      else
        throw new ArgumentException("Invalid argument combination for 'quadraticCurveToRel', allowed combinations are: [controlPoint, end] [x1, y1, x, y]");
    }
    private void ExecutePathSmoothCurveToAbs(XmlElement element, Collection<IPath> paths)
    {
      Hashtable arguments = new Hashtable();
      foreach (XmlAttribute attribute in element.Attributes)
      {
        if (attribute.Name == "controlPoint")
          arguments["controlPoint"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "end")
          arguments["end"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "x")
          arguments["x"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "x2")
          arguments["x2"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y")
          arguments["y"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y2")
          arguments["y2"] = Variables.GetValue<double>(attribute);
      }
      if (OnlyContains(arguments, "controlPoint", "end"))
        paths.Add(new PathSmoothCurveToAbs((PointD)arguments["controlPoint"], (PointD)arguments["end"]));
      else if (OnlyContains(arguments, "x2", "y2", "x", "y"))
        paths.Add(new PathSmoothCurveToAbs((double)arguments["x2"], (double)arguments["y2"], (double)arguments["x"], (double)arguments["y"]));
      else
        throw new ArgumentException("Invalid argument combination for 'smoothCurveToAbs', allowed combinations are: [controlPoint, end] [x2, y2, x, y]");
    }
    private void ExecutePathSmoothCurveToRel(XmlElement element, Collection<IPath> paths)
    {
      Hashtable arguments = new Hashtable();
      foreach (XmlAttribute attribute in element.Attributes)
      {
        if (attribute.Name == "controlPoint")
          arguments["controlPoint"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "end")
          arguments["end"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "x")
          arguments["x"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "x2")
          arguments["x2"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y")
          arguments["y"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y2")
          arguments["y2"] = Variables.GetValue<double>(attribute);
      }
      if (OnlyContains(arguments, "controlPoint", "end"))
        paths.Add(new PathSmoothCurveToRel((PointD)arguments["controlPoint"], (PointD)arguments["end"]));
      else if (OnlyContains(arguments, "x2", "y2", "x", "y"))
        paths.Add(new PathSmoothCurveToRel((double)arguments["x2"], (double)arguments["y2"], (double)arguments["x"], (double)arguments["y"]));
      else
        throw new ArgumentException("Invalid argument combination for 'smoothCurveToRel', allowed combinations are: [controlPoint, end] [x2, y2, x, y]");
    }
    private void ExecutePathSmoothQuadraticCurveToAbs(XmlElement element, Collection<IPath> paths)
    {
      Hashtable arguments = new Hashtable();
      foreach (XmlAttribute attribute in element.Attributes)
      {
        if (attribute.Name == "end")
          arguments["end"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "x")
          arguments["x"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y")
          arguments["y"] = Variables.GetValue<double>(attribute);
      }
      if (OnlyContains(arguments, "end"))
        paths.Add(new PathSmoothQuadraticCurveToAbs((PointD)arguments["end"]));
      else if (OnlyContains(arguments, "x", "y"))
        paths.Add(new PathSmoothQuadraticCurveToAbs((double)arguments["x"], (double)arguments["y"]));
      else
        throw new ArgumentException("Invalid argument combination for 'smoothQuadraticCurveToAbs', allowed combinations are: [end] [x, y]");
    }
    private void ExecutePathSmoothQuadraticCurveToRel(XmlElement element, Collection<IPath> paths)
    {
      Hashtable arguments = new Hashtable();
      foreach (XmlAttribute attribute in element.Attributes)
      {
        if (attribute.Name == "end")
          arguments["end"] = Variables.GetValue<PointD>(attribute);
        else if (attribute.Name == "x")
          arguments["x"] = Variables.GetValue<double>(attribute);
        else if (attribute.Name == "y")
          arguments["y"] = Variables.GetValue<double>(attribute);
      }
      if (OnlyContains(arguments, "end"))
        paths.Add(new PathSmoothQuadraticCurveToRel((PointD)arguments["end"]));
      else if (OnlyContains(arguments, "x", "y"))
        paths.Add(new PathSmoothQuadraticCurveToRel((double)arguments["x"], (double)arguments["y"]));
      else
        throw new ArgumentException("Invalid argument combination for 'smoothQuadraticCurveToRel', allowed combinations are: [end] [x, y]");
    }
  }
}
