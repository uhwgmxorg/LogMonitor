namespace ControlPanels
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Content control that draws and outer glow around itself.
    /// </summary>
    public class OuterGlowBorder : ContentControl
    {
        /// <summary>
        /// The outer glow opacity property.
        /// </summary>
        public static readonly DependencyProperty OuterGlowOpacityProperty =
            DependencyProperty.Register("OuterGlowOpacity", typeof(double), typeof(OuterGlowBorder), null);

        /// <summary>
        /// The outer glow size property.
        /// </summary>
        public static readonly DependencyProperty OuterGlowSizeProperty =
            DependencyProperty.Register("OuterGlowSize", typeof(double), typeof(OuterGlowBorder), null);

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(OuterGlowBorder), null);

        /// <summary>
        /// The shadow corner radius property.
        /// </summary>
        public static readonly DependencyProperty ShadowCornerRadiusProperty =
            DependencyProperty.Register("ShadowCornerRadius", typeof(CornerRadius), typeof(OuterGlowBorder), null);

        /// <summary>
        /// The outer glow color.
        /// </summary>
        public static readonly DependencyProperty OuterGlowColorProperty =
            DependencyProperty.Register("OuterGlowColor", typeof(Color), typeof(OuterGlowBorder), new PropertyMetadata(new PropertyChangedCallback(OuterGlowColor_Changed)));

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(OuterGlowBorder), null);

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
        /// Stores the outer glow border.
        /// </summary>
        private Border outerGlowBorder;

        /// <summary>
        /// Out glow border constructor.
        /// </summary>
        public OuterGlowBorder()
        {
            DefaultStyleKey = typeof(OuterGlowBorder);
            SizeChanged += new SizeChangedEventHandler(OuterGlowContentControl_SizeChanged);
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
        /// Gets or sets the outer glow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The outer glow opacity.")]
        public double OuterGlowOpacity
        {
            get { return (double)GetValue(OuterGlowOpacityProperty); }
            set { SetValue(OuterGlowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the outer glow size.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The outer glow size.")]
        public double OuterGlowSize
        {
            get 
            { 
                return (double)GetValue(OuterGlowSizeProperty); 
            }

            set 
            { 
                SetValue(OuterGlowSizeProperty, value);
                UpdateGlowSize(OuterGlowSize);
                UpdateStops(new Size(ActualWidth, ActualHeight));
            }
        }

        /// <summary>
        /// Gets or sets the outer glow color.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The outer glow color.")]
        public Color OuterGlowColor
        {
            get
            {
                return (Color)GetValue(OuterGlowColorProperty);
            }

            set
            {
                SetValue(OuterGlowColorProperty, value);
            }
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
        /// Gets the parts out of the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            shadowOuterStop1 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop1");
            shadowOuterStop2 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop2");
            shadowVertical1 = (GradientStop)GetTemplateChild("PART_ShadowVertical1");
            shadowVertical2 = (GradientStop)GetTemplateChild("PART_ShadowVertical2");
            shadowHorizontal1 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal1");
            shadowHorizontal2 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal2");
            outerGlowBorder = (Border)GetTemplateChild("PART_OuterGlowBorder");
            UpdateGlowSize(OuterGlowSize);
            UpdateGlowColor(OuterGlowColor);
        }

        /// <summary>
        /// Updates the glow size.
        /// </summary>
        /// <param name="size">The new size.</param>
        internal void UpdateGlowSize(double size)
        {
            if (outerGlowBorder != null)
            {
                outerGlowBorder.Margin = new Thickness(-Math.Abs(size));
            }
        }

        /// <summary>
        /// Updates the outer glow color.
        /// </summary>
        /// <param name="color">The new color.</param>
        internal void UpdateGlowColor(Color color)
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
        /// Updates the outer glow color when the DP changes.
        /// </summary>
        /// <param name="dependencyObject">The outer glow border.</param>
        /// <param name="eventArgs">The new property event args.</param>
        private static void OuterGlowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            OuterGlowBorder outerGlowBorder = (OuterGlowBorder)dependencyObject;
            outerGlowBorder.UpdateGlowColor((Color)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the gradient stops on the drop shadow.
        /// </summary>
        /// <param name="sender">The outer glow border.</param>
        /// <param name="e">Size changed event args.</param>
        private void OuterGlowContentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateStops(e.NewSize);
        }

        /// <summary>
        /// Updates the gradient stops.
        /// </summary>
        /// <param name="size">The size of the control.</param>
        private void UpdateStops(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                if (shadowHorizontal1 != null)
                {
                    shadowHorizontal1.Offset = Math.Abs(OuterGlowSize) / (size.Width + Math.Abs(OuterGlowSize) + Math.Abs(OuterGlowSize));
                }

                if (shadowHorizontal2 != null)
                {
                    shadowHorizontal2.Offset = 1 - (Math.Abs(OuterGlowSize) / (size.Width + Math.Abs(OuterGlowSize) + Math.Abs(OuterGlowSize)));
                }

                if (shadowVertical1 != null)
                {
                    shadowVertical1.Offset = Math.Abs(OuterGlowSize) / (size.Height + Math.Abs(OuterGlowSize) + Math.Abs(OuterGlowSize));
                }

                if (shadowVertical2 != null)
                {
                    shadowVertical2.Offset = 1 - (Math.Abs(OuterGlowSize) / (size.Height + Math.Abs(OuterGlowSize) + Math.Abs(OuterGlowSize)));
                }
            }
        }

        /// <summary>
        /// ShadowCornerRadius
        /// </summary>
        public double ShadowCornerRadius
        {
            get { return (double)GetValue(ShadowCornerRadiusProperty); }
        }
    }
}
