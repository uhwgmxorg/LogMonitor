namespace ControlPanels
{
    using System;
    using System.Windows;

    /// <summary>
    /// Class with trig and other helper functions.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Converts degress into radians.
        /// </summary>
        /// <param name="degrees">The degree value.</param>
        /// <returns>The degrees as radians.</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        /// <summary>
        /// Converts radians to degress.
        /// </summary>
        /// <param name="radians">The radians value.</param>
        /// <returns>The radians as degrees.</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        /// <summary>
        /// Gets a point offset by a distance and angle (in degrees).
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <param name="distance">The distance.</param>
        /// <returns>The offset point.</returns>
        public static Point GetOffset(double angle, double distance)
        {
            double x = Math.Cos(MathHelper.DegreesToRadians(angle)) * distance;
            double y = Math.Tan(MathHelper.DegreesToRadians(angle)) * x;
            return new Point(x, y);
        }

        /// <summary>
        /// Gets the angle between to points.
        /// </summary>
        /// <param name="offset">The offset point.</param>
        /// <returns>The angle for the offset.</returns>
        public static double GetAngleFromOffset(Point offset)
        {
            double opposite = Math.Abs(offset.Y);
            double adjacent = Math.Abs(offset.X);

            if (offset.Y < 0)
            {
                opposite = -opposite;
            }

            double angle = MathHelper.RadiansToDegrees(Math.Atan(opposite / adjacent));
            if (double.IsNaN(angle))
            {
                return 0.0;
            }

            if (offset.X < 0)
            {
                angle = 90 + (90 - angle);
            }

            return angle;
        }
    }
}
