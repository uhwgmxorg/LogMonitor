﻿namespace ControlPanels
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// A border that also shows a radial shadow below it.
    /// </summary>
    public class RadialShadowBorder : ContentControl
    {
        /// <summary>
        /// The drop shadow color property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowColorProperty =
            DependencyProperty.Register("RadialShadowColor", typeof(Color), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(RadialShadowColor_Changed)));

        /// <summary>
        /// The drop shadow opacity property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowOpacityProperty =
            DependencyProperty.Register("RadialShadowOpacity", typeof(double), typeof(RadialShadowBorder), null);

        /// <summary>
        /// The drop shadow distance property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowVerticalOffsetProperty =
            DependencyProperty.Register("RadialShadowVerticalOffset", typeof(double), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(RadialShadowVerticalOffset_Changed)));

        /// <summary>
        /// The drop shadow angle property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowWidthProperty =
            DependencyProperty.Register("RadialShadowWidth", typeof(double), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(RadialShadowWidth_Changed)));

        /// <summary>
        /// The drop shadow spread property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowSpreadProperty =
            DependencyProperty.Register("RadialShadowSpread", typeof(double), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(RadialShadowSpread_Changed)));

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RadialShadowBorder), null);

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(RadialShadowBorder), null);

        /// <summary>
        /// The inner shadow gradient stop.
        /// </summary>
        private GradientStop shadowInnerStop;

        /// <summary>
        /// The outer shadow gradient stop.
        /// </summary>
        private GradientStop shadowOuterStop;

        /// <summary>
        /// Stores the shadow translate transform.
        /// </summary>
        private TranslateTransform shadowTranslate;

        /// <summary>
        /// Stores the shadow scale;
        /// </summary>
        private ScaleTransform shadowScale;

        /// <summary>
        /// Stores the shadow.
        /// </summary>
        private Ellipse shadow;

        /// <summary>
        /// RadialShadowBorder constructor.
        /// </summary>
        public RadialShadowBorder()
        {
            DefaultStyleKey = typeof(RadialShadowBorder);
            SizeChanged += new SizeChangedEventHandler(RadialShadowBorder_SizeChanged);
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
            }
        }

        /// <summary>
        /// Gets or sets the drop shadow color.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow color.")]
        public Color RadialShadowColor
        {
            get { return (Color)GetValue(RadialShadowColorProperty); }
            set { SetValue(RadialShadowColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow opacity.")]
        public double RadialShadowOpacity
        {
            get { return (double)GetValue(RadialShadowOpacityProperty); }
            set { SetValue(RadialShadowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow distance.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow vertical offset.")]
        public double RadialShadowVerticalOffset
        {
            get { return (double)GetValue(RadialShadowVerticalOffsetProperty); }
            set { SetValue(RadialShadowVerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow angle.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow width.")]
        public double RadialShadowWidth
        {
            get { return (double)GetValue(RadialShadowWidthProperty); }
            set { SetValue(RadialShadowWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow spread.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow spread.")]
        public double RadialShadowSpread
        {
            get { return (double)GetValue(RadialShadowSpreadProperty); }
            set { SetValue(RadialShadowSpreadProperty, value); }
        }

        /// <summary>
        /// Gets the parts out of the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            shadow = (Ellipse)GetTemplateChild("PART_Shadow");
            shadowInnerStop = (GradientStop)GetTemplateChild("PART_ShadowInnerStop");
            shadowOuterStop = (GradientStop)GetTemplateChild("PART_ShadowOuterStop");
            shadowTranslate = (TranslateTransform)GetTemplateChild("PART_ShadowTranslate");
            shadowScale = (ScaleTransform)GetTemplateChild("PART_ShadowScale");

            UpdateShadowSize(new Size(ActualWidth, ActualHeight));
            UpdateStops(RadialShadowSpread);
            UpdateShadowScale(RadialShadowWidth);
            UpdateShadowVerticalOffset(RadialShadowVerticalOffset);
        }

        /// <summary>
        /// Updates the shadow size.
        /// </summary>
        /// <param name="newSize">The new control size.</param>
        internal void UpdateShadowSize(Size newSize)
        {
            if (shadow != null)
            {
                shadow.Height = newSize.Height / 3;
                shadow.Margin = new Thickness(0, 0, 0, -shadow.Height / 2);
            }
        }

        /// <summary>
        /// Updates the shadow scale;
        /// </summary>
        /// <param name="scaleX">The scale X of the shadow.</param>
        internal void UpdateShadowScale(double scaleX)
        {
            if (shadowScale != null)
            {
                shadowScale.ScaleX = scaleX;
            }
        }

        /// <summary>
        /// Updates the gradient stops offset.
        /// </summary>
        /// <param name="spread">The spread of the stops.</param>
        internal void UpdateStops(double spread)
        {
            if (shadowInnerStop != null)
            {
                shadowInnerStop.Offset = spread;
            }
        }

        /// <summary>
        /// Updates the shadow color.
        /// </summary>
        /// <param name="color">The new shadow color.</param>
        internal void UpdateShadowColor(Color color)
        {
             if (shadowInnerStop != null)
            {
                shadowInnerStop.Color = color;
            }

            if (shadowOuterStop != null)
            {
                shadowOuterStop.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }
        }

        /// <summary>
        /// Updates the vertical offset of the shadow.
        /// </summary>
        /// <param name="verticalOffset">The vertical offset.</param>
        internal void UpdateShadowVerticalOffset(double verticalOffset)
        {
            if (shadowTranslate != null)
            {
                shadowTranslate.Y = Math.Max(0, RadialShadowVerticalOffset);
            }
        }

        /// <summary>
        /// Updates the radial shadow.
        /// </summary>
        /// <param name="dependencyObject">The radial shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void RadialShadowVerticalOffset_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            RadialShadowBorder radialShadowBorder = (RadialShadowBorder)dependencyObject;
            radialShadowBorder.UpdateShadowVerticalOffset((double)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the radial shadow.
        /// </summary>
        /// <param name="dependencyObject">The radial shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void RadialShadowWidth_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            RadialShadowBorder radialShadowBorder = (RadialShadowBorder)dependencyObject;
            radialShadowBorder.UpdateShadowScale((double)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the radial shadow.
        /// </summary>
        /// <param name="dependencyObject">The radial shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void RadialShadowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            RadialShadowBorder radialShadowBorder = (RadialShadowBorder)dependencyObject;
            radialShadowBorder.UpdateShadowColor((Color)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the radial shadow.
        /// </summary>
        /// <param name="dependencyObject">The radial shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void RadialShadowSpread_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            RadialShadowBorder radialShadowBorder = (RadialShadowBorder)dependencyObject;
            radialShadowBorder.UpdateStops((double)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the radial shadow size.
        /// </summary>
        /// <param name="sender">The radial shadow border.</param>
        /// <param name="e">Size changed event args.</param>
        private void RadialShadowBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateShadowSize(e.NewSize);
        }
    }
}
