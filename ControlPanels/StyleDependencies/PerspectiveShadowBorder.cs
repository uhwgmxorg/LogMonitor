namespace ControlPanels
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// A border that also shows a perspective shadow.
    /// </summary>
    public class PerspectiveShadowBorder : ContentControl
    {
        /// <summary>
        /// The perspective shadow color property.
        /// </summary>
        public static readonly DependencyProperty PerspectiveShadowColorProperty =
            DependencyProperty.Register("PerspectiveShadowColor", typeof(Color), typeof(PerspectiveShadowBorder), new PropertyMetadata(new PropertyChangedCallback(PerspectiveShadowColor_Changed)));

        /// <summary>
        /// The perspective shadow opacity property.
        /// </summary>
        public static readonly DependencyProperty PerspectiveShadowOpacityProperty =
            DependencyProperty.Register("PerspectiveShadowOpacity", typeof(double), typeof(PerspectiveShadowBorder), null);

        /// <summary>
        /// The Perspective shadow angle property.
        /// </summary>
        public static readonly DependencyProperty PerspectiveShadowAngleProperty =
            DependencyProperty.Register("PerspectiveShadowAngle", typeof(double), typeof(PerspectiveShadowBorder), new PropertyMetadata(new PropertyChangedCallback(PerspectiveShadowAngle_Changed)));

        /// <summary>
        /// The Perspective shadow spread property.
        /// </summary>
        public static readonly DependencyProperty PerspectiveShadowSpreadProperty =
            DependencyProperty.Register("PerspectiveShadowSpread", typeof(double), typeof(PerspectiveShadowBorder), new PropertyMetadata(new PropertyChangedCallback(PerspectiveShadowSpread_Changed)));

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PerspectiveShadowBorder), null);

        /// <summary>
        /// The shadow corner radius property.
        /// </summary>
        public static readonly DependencyProperty ShadowCornerRadiusProperty =
            DependencyProperty.Register("ShadowCornerRadius", typeof(CornerRadius), typeof(PerspectiveShadowBorder), null);

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(PerspectiveShadowBorder), null);

        /// <summary>
        /// Stores the skew transform.
        /// </summary>
        private SkewTransform skewTransform;

        /// <summary>
        /// Stores the scale transform.
        /// </summary>
        private ScaleTransform scaleTransform;

        /// <summary>
        /// The top out gradient stop.
        /// </summary>
        private GradientStop shadowOuterStop1;

        /// <summary>
        /// The bottom outer gradient stop.
        /// </summary>
        private GradientStop shadowOuterStop2;

        /// <summary>
        /// Stores the top gradient stop.
        /// </summary>
        private GradientStop shadowVertical1;

        /// <summary>
        /// Stores the bottom gradient stop.
        /// </summary>
        private GradientStop shadowVertical2;

        /// <summary>
        /// Stores the left gradient stop.
        /// </summary>
        private GradientStop shadowHorizontal1;

        /// <summary>
        /// Stores the right gradient stop.
        /// </summary>
        private GradientStop shadowHorizontal2;

        /// <summary>
        /// PerspectiveShadowBorder constructor.
        /// </summary>
        public PerspectiveShadowBorder()
        {
            DefaultStyleKey = typeof(PerspectiveShadowBorder);
            SizeChanged += new SizeChangedEventHandler(PerspectiveShadowBorder_SizeChanged);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the content is clipped.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool ClipContent
        {
            get { return (bool)GetValue(ClipContentProperty); }
            set { SetValue(ClipContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the border corner radius.
        /// This is a thickness, as there is a problem parsing CornerRadius types.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }

            set
            {
                SetValue(CornerRadiusProperty, value);

                CornerRadius shadowCornerRadius = new CornerRadius(
                    Math.Abs(value.TopLeft * 1.5),
                    Math.Abs(value.TopRight * 1.5),
                    Math.Abs(value.BottomRight * 1.5),
                    Math.Abs(value.BottomLeft * 1.5));
                SetValue(ShadowCornerRadiusProperty, shadowCornerRadius);
            }
        }

        /// <summary>
        /// Gets or sets the Perspective shadow color.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("the Perspective shadow color.")]
        public Color PerspectiveShadowColor
        {
            get { return (Color)GetValue(PerspectiveShadowColorProperty); }
            set { SetValue(PerspectiveShadowColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Perspective shadow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The Perspective shadow opacity.")]
        public double PerspectiveShadowOpacity
        {
            get { return (double)GetValue(PerspectiveShadowOpacityProperty); }
            set { SetValue(PerspectiveShadowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Perspective shadow angle.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The Perspective shadow angle.")]
        public double PerspectiveShadowAngle
        {
            get 
            { 
                return (double)GetValue(PerspectiveShadowAngleProperty); 
            }
            
            set 
            { 
                SetValue(
                    PerspectiveShadowAngleProperty, 
                    Math.Max(Math.Min(value, 89), -89)); 
            }
        }

        /// <summary>
        /// Gets or sets the Perspective shadow spread.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The Perspective shadow spread.")]
        public double PerspectiveShadowSpread
        {
            get { return (double)GetValue(PerspectiveShadowSpreadProperty); }
            set { SetValue(PerspectiveShadowSpreadProperty, value); }
        }

        /// <summary>
        /// Gets the parts from the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            skewTransform = (SkewTransform)GetTemplateChild("PART_PerspectiveShadowSkewTransform");
            scaleTransform = (ScaleTransform)GetTemplateChild("PART_PerspectiveShadowScaleTransform");
            shadowOuterStop1 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop1");
            shadowOuterStop2 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop2");
            shadowVertical1 = (GradientStop)GetTemplateChild("PART_ShadowVertical1");
            shadowVertical2 = (GradientStop)GetTemplateChild("PART_ShadowVertical2");
            shadowHorizontal1 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal1");
            shadowHorizontal2 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal2");

            UpdateStops(new Size(ActualWidth, ActualHeight));
            UpdateShadowAngle(PerspectiveShadowAngle);
        }

        /// <summary>
        /// Updates the gradient stops.
        /// </summary>
        /// <param name="size">The size of the control.</param>
        internal void UpdateStops(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                if (shadowHorizontal1 != null)
                {
                    shadowHorizontal1.Offset = PerspectiveShadowSpread / (size.Width + (PerspectiveShadowSpread * 2));
                }

                if (shadowHorizontal2 != null)
                {
                    shadowHorizontal2.Offset = 1 - (PerspectiveShadowSpread / (size.Width + (PerspectiveShadowSpread * 2)));
                }

                if (shadowVertical1 != null)
                {
                    shadowVertical1.Offset = PerspectiveShadowSpread / (size.Height + (PerspectiveShadowSpread * 2));
                }

                if (shadowVertical2 != null)
                {
                    shadowVertical2.Offset = 1 - (PerspectiveShadowSpread / (size.Height + (PerspectiveShadowSpread * 2)));
                }
            }
        }

        /// <summary>
        /// Updates the Perspective shadow color.
        /// </summary>
        /// <param name="color">The new color.</param>
        internal void UpdatePerspectiveShadowColor(Color color)
        {
            if (shadowVertical1 != null)
            {
                shadowVertical1.Color = color;
            }

            if (shadowVertical2 != null)
            {
                shadowVertical2.Color = color;
            }

            if (shadowOuterStop1 != null)
            {
                shadowOuterStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }

            if (shadowOuterStop2 != null)
            {
                shadowOuterStop2.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }
        }

        /// <summary>
        /// Updates the shadow angle.
        /// </summary>
        /// <param name="newAngle">The new angle</param>
        internal void UpdateShadowAngle(double newAngle)
        {
            if (skewTransform != null)
            {
                skewTransform.AngleX = Math.Min(Math.Max(newAngle, -45), 45);
            }

            if (scaleTransform != null)
            {
                scaleTransform.ScaleY = 1 - (Math.Abs(PerspectiveShadowAngle) / 89.0);
            }
        }

        /// <summary>
        /// Updates the Perspective shadow.
        /// </summary>
        /// <param name="dependencyObject">The Perspective shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void PerspectiveShadowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            PerspectiveShadowBorder perspectiveShadowBorder = (PerspectiveShadowBorder)dependencyObject;
            perspectiveShadowBorder.UpdatePerspectiveShadowColor((Color)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the Perspective shadow.
        /// </summary>
        /// <param name="dependencyObject">The Perspective shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void PerspectiveShadowSpread_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            PerspectiveShadowBorder perspectiveShadowBorder = (PerspectiveShadowBorder)dependencyObject;
            perspectiveShadowBorder.UpdateStops(new Size(perspectiveShadowBorder.ActualWidth, perspectiveShadowBorder.ActualHeight));
        }

        /// <summary>
        /// Updates the Perspective shadow.
        /// </summary>
        /// <param name="dependencyObject">The Perspective shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void PerspectiveShadowAngle_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            PerspectiveShadowBorder perspectiveShadowBorder = (PerspectiveShadowBorder)dependencyObject;
            perspectiveShadowBorder.UpdateShadowAngle((double)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the stops.
        /// </summary>
        /// <param name="sender">The Perspective shadow border.</param>
        /// <param name="e">Size changed event args.</param>
        private void PerspectiveShadowBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateStops(e.NewSize);
        }

        public double ShadowCornerRadius
        {
            get { return (double)GetValue(ShadowCornerRadiusProperty); }
        }
    }
}
