namespace ControlPanels
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Content control that draws a glow around its inside.
    /// </summary>
    public class InnerGlowBorder : ContentControl
    {
        /// <summary>
        /// The inner glow opacity property.
        /// </summary>
        public static readonly DependencyProperty InnerGlowOpacityProperty =
            DependencyProperty.Register("InnerGlowOpacity", typeof(double), typeof(InnerGlowBorder), null);

        /// <summary>
        /// The inner glow size property.
        /// </summary>
        public static readonly DependencyProperty InnerGlowSizeProperty =
            DependencyProperty.Register("InnerGlowSize", typeof(Thickness), typeof(InnerGlowBorder), new PropertyMetadata(new PropertyChangedCallback(InnerGlowSize_Changed)));

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(InnerGlowBorder), null);

        /// <summary>
        /// The inner glow color.
        /// </summary>
        public static readonly DependencyProperty InnerGlowColorProperty =
            DependencyProperty.Register("InnerGlowColor", typeof(Color), typeof(InnerGlowBorder), new PropertyMetadata(new PropertyChangedCallback(InnerGlowColor_Changed)));

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(InnerGlowBorder), null);

        /// <summary>
        /// The content z-index property.
        /// </summary>
        public static readonly DependencyProperty ContentZIndexProperty =
            DependencyProperty.Register("ContentZIndex", typeof(int), typeof(InnerGlowBorder), null);

        /// <summary>
        /// Stores the left glow border.
        /// </summary>
        private Border leftGlow;

        /// <summary>
        /// Stores the top glow border.
        /// </summary>
        private Border topGlow;

        /// <summary>
        /// Stores the right glow border.
        /// </summary>
        private Border rightGlow;

        /// <summary>
        /// Stores the bottom glow border.
        /// </summary>
        private Border bottomGlow;

        /// <summary>
        /// Stores the left glow stop 0;
        /// </summary>
        private GradientStop leftGlowStop0;

        /// <summary>
        /// Stores the left glow stop 1;
        /// </summary>
        private GradientStop leftGlowStop1;

        /// <summary>
        /// Stores the top glow stop 0;
        /// </summary>
        private GradientStop topGlowStop0;

        /// <summary>
        /// Stores the top glow stop 1;
        /// </summary>
        private GradientStop topGlowStop1;

        /// <summary>
        /// Stores the right glow stop 0;
        /// </summary>
        private GradientStop rightGlowStop0;

        /// <summary>
        /// Stores the right glow stop 1.
        /// </summary>
        private GradientStop rightGlowStop1;

        /// <summary>
        /// Stores the bottom glow stop 0;
        /// </summary>
        private GradientStop bottomGlowStop0;

        /// <summary>
        /// Stores the bottom glow stop 1.
        /// </summary>
        private GradientStop bottomGlowStop1;

        /// <summary>
        /// InnerGlowBorder constructor.
        /// </summary>
        public InnerGlowBorder()
        {
            DefaultStyleKey = typeof(InnerGlowBorder);
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
        /// Gets or sets the content z-index. 0 for behind shadow, 1 for in-front.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Set 0 for behind the shadow, 1 for in front.")]
        public int ContentZIndex
        {
            get { return (int)GetValue(ContentZIndexProperty); }
            set { SetValue(ContentZIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inner glow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow opacity.")]
        public double InnerGlowOpacity
        {
            get { return (double)GetValue(InnerGlowOpacityProperty); }
            set { SetValue(InnerGlowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inner glow color.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow color.")]
        public Color InnerGlowColor
        {
            get
            {
                return (Color)GetValue(InnerGlowColorProperty);
            }

            set
            {
                SetValue(InnerGlowColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the inner glow size.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow size.")]
        public Thickness InnerGlowSize
        {
            get
            {
                return (Thickness)GetValue(InnerGlowSizeProperty);
            }

            set
            {
                SetValue(InnerGlowSizeProperty, value);
                UpdateGlowSize(value);
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
            }
        }

        /// <summary>
        /// Gets the template parts out.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            leftGlow = GetTemplateChild("PART_LeftGlow") as Border;
            topGlow = GetTemplateChild("PART_TopGlow") as Border;
            rightGlow = GetTemplateChild("PART_RightGlow") as Border;
            bottomGlow = GetTemplateChild("PART_BottomGlow") as Border;

            leftGlowStop0 = GetTemplateChild("PART_LeftGlowStop0") as GradientStop;
            leftGlowStop1 = GetTemplateChild("PART_LeftGlowStop1") as GradientStop;
            topGlowStop0 = GetTemplateChild("PART_TopGlowStop0") as GradientStop;
            topGlowStop1 = GetTemplateChild("PART_TopGlowStop1") as GradientStop;
            rightGlowStop0 = GetTemplateChild("PART_RightGlowStop0") as GradientStop;
            rightGlowStop1 = GetTemplateChild("PART_RightGlowStop1") as GradientStop;
            bottomGlowStop0 = GetTemplateChild("PART_BottomGlowStop0") as GradientStop;
            bottomGlowStop1 = GetTemplateChild("PART_BottomGlowStop1") as GradientStop;

            UpdateGlowColor(InnerGlowColor);
            UpdateGlowSize(InnerGlowSize);
        }

        /// <summary>
        /// Updates the inner glow color.
        /// </summary>
        /// <param name="color">The new color.</param>
        internal void UpdateGlowColor(Color color)
        {
            if (leftGlowStop0 != null)
            {
                leftGlowStop0.Color = color;
            }

            if (leftGlowStop1 != null)
            {
                leftGlowStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }

            if (topGlowStop0 != null)
            {
                topGlowStop0.Color = color;
            }

            if (topGlowStop1 != null)
            {
                topGlowStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }

            if (rightGlowStop0 != null)
            {
                rightGlowStop0.Color = color;
            }

            if (rightGlowStop1 != null)
            {
                rightGlowStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }

            if (bottomGlowStop0 != null)
            {
                bottomGlowStop0.Color = color;
            }

            if (bottomGlowStop1 != null)
            {
                bottomGlowStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }
        }

        /// <summary>
        /// Sets the glow size.
        /// </summary>
        /// <param name="newGlowSize">The new glow size.</param>
        internal void UpdateGlowSize(Thickness newGlowSize)
        {
            if (leftGlow != null)
            {
                leftGlow.Width = Math.Abs(newGlowSize.Left);
            }

            if (topGlow != null)
            {
                topGlow.Height = Math.Abs(newGlowSize.Top);
            }

            if (rightGlow != null)
            {
                rightGlow.Width = Math.Abs(newGlowSize.Right);
            }

            if (bottomGlow != null)
            {
                bottomGlow.Height = Math.Abs(newGlowSize.Bottom);
            }
        }

        /// <summary>
        /// Updates the inner glow color when the DP changes.
        /// </summary>
        /// <param name="dependencyObject">The inner glow border.</param>
        /// <param name="eventArgs">The new property event args.</param>
        private static void InnerGlowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            InnerGlowBorder innerGlowBorder = (InnerGlowBorder)dependencyObject;
            innerGlowBorder.UpdateGlowColor((Color)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the glow size.
        /// </summary>
        /// <param name="dependencyObject">The inner glow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void InnerGlowSize_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            InnerGlowBorder innerGlowBorder = (InnerGlowBorder)dependencyObject;
            innerGlowBorder.UpdateGlowSize((Thickness)eventArgs.NewValue);
        }
    }
}
