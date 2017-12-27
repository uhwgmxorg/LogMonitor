namespace ControlPanels
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// A border that clips its contents.
    /// </summary>
    public class ClippingBorder : ContentControl
    {
        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ClippingBorder), new PropertyMetadata(new PropertyChangedCallback(CornerRadius_Changed)));

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(ClippingBorder), new PropertyMetadata(new PropertyChangedCallback(ClipContent_Changed)));

        /// <summary>
        /// Stores the top left content control.
        /// </summary>
        private ContentControl topLeftContentControl;

        /// <summary>
        /// Stores the top right content control.
        /// </summary>
        private ContentControl topRightContentControl;

        /// <summary>
        /// Stores the bottom right content control.
        /// </summary>
        private ContentControl bottomRightContentControl;

        /// <summary>
        /// Stores the bottom left content control.
        /// </summary>
        private ContentControl bottomLeftContentControl;

        /// <summary>
        /// Stores the clip responsible for clipping the top left corner.
        /// </summary>
        private RectangleGeometry topLeftClip;

        /// <summary>
        /// Stores the clip responsible for clipping the top right corner.
        /// </summary>
        private RectangleGeometry topRightClip;

        /// <summary>
        /// Stores the clip responsible for clipping the bottom right corner.
        /// </summary>
        private RectangleGeometry bottomRightClip;

        /// <summary>
        /// Stores the clip responsible for clipping the bottom left corner.
        /// </summary>
        private RectangleGeometry bottomLeftClip;

        /// <summary>
        /// Stores the main border.
        /// </summary>
        private Border border;

        /// <summary>
        /// ClippingBorder constructor.
        /// </summary>
        public ClippingBorder()
        {
            DefaultStyleKey = typeof(ClippingBorder);
            SizeChanged += new SizeChangedEventHandler(ClippingBorder_SizeChanged);
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
        /// Gets or sets a value indicating whether the content is clipped.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool ClipContent
        {
            get { return (bool)GetValue(ClipContentProperty); }
            set { SetValue(ClipContentProperty, value); }
        }

        /// <summary>
        /// Gets the UI elements out of the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            border = GetTemplateChild("PART_Border") as Border;
            topLeftContentControl = GetTemplateChild("PART_TopLeftContentControl") as ContentControl;
            topRightContentControl = GetTemplateChild("PART_TopRightContentControl") as ContentControl;
            bottomRightContentControl = GetTemplateChild("PART_BottomRightContentControl") as ContentControl;
            bottomLeftContentControl = GetTemplateChild("PART_BottomLeftContentControl") as ContentControl;

            if (topLeftContentControl != null)
            {
                topLeftContentControl.SizeChanged += new SizeChangedEventHandler(ContentControl_SizeChanged);
            }

            topLeftClip = GetTemplateChild("PART_TopLeftClip") as RectangleGeometry;
            topRightClip = GetTemplateChild("PART_TopRightClip") as RectangleGeometry;
            bottomRightClip = GetTemplateChild("PART_BottomRightClip") as RectangleGeometry;
            bottomLeftClip = GetTemplateChild("PART_BottomLeftClip") as RectangleGeometry;

            UpdateClipContent(ClipContent);

            UpdateCornerRadius(CornerRadius);
        }

        /// <summary>
        /// Sets the corner radius.
        /// </summary>
        /// <param name="newCornerRadius">The new corner radius.</param>
        internal void UpdateCornerRadius(CornerRadius newCornerRadius)
        {
            if (border != null)
            {
                border.CornerRadius = newCornerRadius;
            }

            if (topLeftClip != null)
            {
                topLeftClip.RadiusX = topLeftClip.RadiusY = newCornerRadius.TopLeft - (Math.Min(BorderThickness.Left, BorderThickness.Top) / 2);
            }

            if (topRightClip != null)
            {
                topRightClip.RadiusX = topRightClip.RadiusY = newCornerRadius.TopRight - (Math.Min(BorderThickness.Top, BorderThickness.Right) / 2);
            }

            if (bottomRightClip != null)
            {
                bottomRightClip.RadiusX = bottomRightClip.RadiusY = newCornerRadius.BottomRight - (Math.Min(BorderThickness.Right, BorderThickness.Bottom) / 2);
            }

            if (bottomLeftClip != null)
            {
                bottomLeftClip.RadiusX = bottomLeftClip.RadiusY = newCornerRadius.BottomLeft - (Math.Min(BorderThickness.Bottom, BorderThickness.Left) / 2);
            }

            UpdateClipSize(new Size(ActualWidth, ActualHeight));
        }

        /// <summary>
        /// Updates whether the content is clipped.
        /// </summary>
        /// <param name="clipContent">Whether the content is clipped.</param>
        internal void UpdateClipContent(bool clipContent)
        {
            if (clipContent)
            {
                if (topLeftContentControl != null)
                {
                    topLeftContentControl.Clip = topLeftClip;
                }

                if (topRightContentControl != null)
                {
                    topRightContentControl.Clip = topRightClip;
                }

                if (bottomRightContentControl != null)
                {
                    bottomRightContentControl.Clip = bottomRightClip;
                }

                if (bottomLeftContentControl != null)
                {
                    bottomLeftContentControl.Clip = bottomLeftClip;
                }

                UpdateClipSize(new Size(ActualWidth, ActualHeight));
            }
            else
            {
                if (topLeftContentControl != null)
                {
                    topLeftContentControl.Clip = null;
                }

                if (topRightContentControl != null)
                {
                    topRightContentControl.Clip = null;
                }

                if (bottomRightContentControl != null)
                {
                    bottomRightContentControl.Clip = null;
                }

                if (bottomLeftContentControl != null)
                {
                    bottomLeftContentControl.Clip = null;
                }
            }
        }

        /// <summary>
        /// Updates the corner radius.
        /// </summary>
        /// <param name="dependencyObject">The clipping border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void CornerRadius_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ClippingBorder clippingBorder = (ClippingBorder)dependencyObject;
            clippingBorder.UpdateCornerRadius((CornerRadius)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the content clipping.
        /// </summary>
        /// <param name="dependencyObject">The clipping border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void ClipContent_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ClippingBorder clippingBorder = (ClippingBorder)dependencyObject;
            clippingBorder.UpdateClipContent((bool)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the clips.
        /// </summary>
        /// <param name="sender">The clipping border</param>
        /// <param name="e">Size Changed Event Args.</param>
        private void ClippingBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ClipContent)
            {
                UpdateClipSize(e.NewSize);
            }
        }

        /// <summary>
        /// Updates the clip size.
        /// </summary>
        /// <param name="sender">A content control.</param>
        /// <param name="e">Size Changed Event Args</param>
        private void ContentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ClipContent)
            {
                UpdateClipSize(new Size(ActualWidth, ActualHeight));
            }
        }

        /// <summary>
        /// Updates the clip size.
        /// </summary>
        /// <param name="size">The control size.</param>
        private void UpdateClipSize(Size size)
        {
            if (size.Width > 0 || size.Height > 0)
            {
                double contentWidth = Math.Max(0, size.Width - BorderThickness.Left - BorderThickness.Right);
                double contentHeight = Math.Max(0, size.Height - BorderThickness.Top - BorderThickness.Bottom);

                if (topLeftClip != null)
                {
                    topLeftClip.Rect = new Rect(0, 0, contentWidth + (CornerRadius.TopLeft * 2), contentHeight + (CornerRadius.TopLeft * 2));
                }

                if (topRightClip != null)
                {
                    topRightClip.Rect = new Rect(0 - CornerRadius.TopRight, 0, contentWidth + CornerRadius.TopRight, contentHeight + CornerRadius.TopRight);
                }

                if (bottomRightClip != null)
                {
                    bottomRightClip.Rect = new Rect(0 - CornerRadius.BottomRight, 0 - CornerRadius.BottomRight, contentWidth + CornerRadius.BottomRight, contentHeight + CornerRadius.BottomRight);
                }

                if (bottomLeftClip != null)
                {
                    bottomLeftClip.Rect = new Rect(0, 0 - CornerRadius.BottomLeft, contentWidth + CornerRadius.BottomLeft, contentHeight + CornerRadius.BottomLeft);
                }
            }
        }
    }
}
