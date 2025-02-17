// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

namespace ImageMagick
{
    /// <summary>
    /// Draws a cubic Bezier curve from the current point to (x,y) using absolute coordinates. The
    /// first control point is assumed to be the reflection of the second control point on the
    /// previous command relative to the current point. (If there is no previous command or if the
    /// previous command was not an PathCurveToAbs, PathCurveToRel, PathSmoothCurveToAbs or
    /// PathSmoothCurveToRel, assume the first control point is coincident with the current point.)
    /// (x2,y2) is the second control point (i.e., the control point at the end of the curve). At
    /// the end of the command, the new current point becomes the final (x,y) coordinate pair used
    /// in the polybezier.
    /// </summary>
    public sealed class PathSmoothCurveToAbs : IPath, IDrawingWand
    {
        private readonly PointD _controlPoint;
        private readonly PointD _end;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathSmoothCurveToAbs"/> class.
        /// </summary>
        /// <param name="x2">X coordinate of second point.</param>
        /// <param name="y2">Y coordinate of second point.</param>
        /// <param name="x">X coordinate of final point.</param>
        /// <param name="y">Y coordinate of final point.</param>
        public PathSmoothCurveToAbs(double x2, double y2, double x, double y)
          : this(new PointD(x2, y2), new PointD(x, y))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathSmoothCurveToAbs"/> class.
        /// </summary>
        /// <param name="controlPoint">Coordinate of second point.</param>
        /// <param name="end">Coordinate of final point.</param>
        public PathSmoothCurveToAbs(PointD controlPoint, PointD end)
        {
            _controlPoint = controlPoint;
            _end = end;
        }

        /// <summary>
        /// Draws this instance with the drawing wand.
        /// </summary>
        /// <param name="wand">The want to draw on.</param>
        void IDrawingWand.Draw(DrawingWand wand) => wand?.PathSmoothCurveToAbs(_controlPoint, _end);
    }
}
