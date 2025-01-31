// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

namespace ImageMagick
{
    /// <summary>
    /// Draws a quadratic Bezier curve (using absolute coordinates) from the current point to (X, Y).
    /// The control point is assumed to be the reflection of the control point on the previous
    /// command relative to the current point. (If there is no previous command or if the previous
    /// command was not a PathQuadraticCurveToAbs, PathQuadraticCurveToRel,
    /// PathSmoothQuadraticCurveToAbs or PathSmoothQuadraticCurveToRel, assume the control point is
    /// coincident with the current point.). At the end of the command, the new current point becomes
    /// the final (X,Y) coordinate pair used in the polybezier.
    /// </summary>
    public sealed class PathSmoothQuadraticCurveToAbs : IPath, IDrawingWand
    {
        private readonly PointD _end;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathSmoothQuadraticCurveToAbs"/> class.
        /// </summary>
        /// <param name="x">X coordinate of final point.</param>
        /// <param name="y">Y coordinate of final point.</param>
        public PathSmoothQuadraticCurveToAbs(double x, double y)
          : this(new PointD(x, y))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathSmoothQuadraticCurveToAbs"/> class.
        /// </summary>
        /// <param name="end">Coordinate of final point.</param>
        public PathSmoothQuadraticCurveToAbs(PointD end)
        {
            _end = end;
        }

        /// <summary>
        /// Draws this instance with the drawing wand.
        /// </summary>
        /// <param name="wand">The want to draw on.</param>
        void IDrawingWand.Draw(DrawingWand wand) => wand?.PathSmoothQuadraticCurveToAbs(_end);
    }
}
