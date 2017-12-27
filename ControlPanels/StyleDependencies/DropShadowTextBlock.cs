namespace ControlPanels
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// A control that displays text, with a drop shadow.
    /// </summary>
    public class DropShadowTextBlock : Control
    {
        /// <summary>
        /// The drop shadow color property.
        /// </summary>
        public static readonly DependencyProperty DropShadowColorProperty =
            DependencyProperty.Register("DropShadowColor", typeof(Color), typeof(DropShadowTextBlock), new PropertyMetadata(new PropertyChangedCallback(DropShadowColor_Changed)));

        /// <summary>
        /// The drop shadow opacity property.
        /// </summary>
        public static readonly DependencyProperty DropShadowOpacityProperty =
            DependencyProperty.Register("DropShadowOpacity", typeof(double), typeof(DropShadowTextBlock), new PropertyMetadata(new PropertyChangedCallback(DropShadowOpacity_Changed)));

        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(DropShadowTextBlock), null);

        /// <summary>
        /// The text decorations property.
        /// </summary>
        public static readonly DependencyProperty TextDecorationsProperty =
            DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(DropShadowTextBlock), null);

        /// <summary>
        /// The text wrapping property.
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(DropShadowTextBlock), null);

        /// <summary>
        /// The drop shadow distance property.
        /// </summary>
        public static readonly DependencyProperty DropShadowDistanceProperty =
            DependencyProperty.Register("DropShadowDistance", typeof(double), typeof(DropShadowTextBlock), new PropertyMetadata(new PropertyChangedCallback(DropShadowDistance_Changed)));

        /// <summary>
        /// The drop shadow angle property.
        /// </summary>
        public static readonly DependencyProperty DropShadowAngleProperty =
            DependencyProperty.Register("DropShadowAngle", typeof(double), typeof(DropShadowTextBlock), new PropertyMetadata(new PropertyChangedCallback(DropShadowAngle_Changed)));

        /// <summary>
        /// Stores the drop shadow translate transform.
        /// </summary>
        private TranslateTransform dropShadowTranslate;

        /// <summary>
        /// Stores the drop shadow brush.
        /// </summary>
        private SolidColorBrush dropShadowBrush;

        /// <summary>
        /// DropShadowTextBlock constructor.
        /// </summary>
        public DropShadowTextBlock()
        {
            DefaultStyleKey = typeof(DropShadowTextBlock);
        }

        /// <summary>
        /// Gets or sets the drop shadow color.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow color.")]
        public Color DropShadowColor
        {
            get { return (Color)GetValue(DropShadowColorProperty); }
            set { SetValue(DropShadowColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow opacity.")]
        public double DropShadowOpacity
        {
            get { return (double)GetValue(DropShadowOpacityProperty); }
            set { SetValue(DropShadowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the link text.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The text content.")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text decorations.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The text decorations.")]
        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text wrapping.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Whether the text wraps.")]
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow distance.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow distance.")]
        public double DropShadowDistance
        {
            get { return (double)GetValue(DropShadowDistanceProperty); }
            set { SetValue(DropShadowDistanceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow angle.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow angle.")]
        public double DropShadowAngle
        {
            get { return (double)GetValue(DropShadowAngleProperty); }
            set { SetValue(DropShadowAngleProperty, value); }
        }

        /// <summary>
        /// Gets the UI elements out of the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            dropShadowTranslate = GetTemplateChild("PART_DropShadowTranslate") as TranslateTransform;
            dropShadowBrush = GetTemplateChild("PART_DropShadowBrush") as SolidColorBrush;
            UpdateDropShadowPosition();
            UpdateDropShadowBrush();
        }

        /// <summary>
        /// Updates the drop shadow.
        /// </summary>
        internal void UpdateDropShadowPosition()
        {
            if (dropShadowTranslate != null)
            {
                Point offset = MathHelper.GetOffset(DropShadowAngle, DropShadowDistance);

                dropShadowTranslate.X = offset.X;
                dropShadowTranslate.Y = offset.Y;
            }
        }

        /// <summary>
        /// Updates the drop shadow brush.
        /// </summary>
        internal void UpdateDropShadowBrush()
        {
            if (dropShadowBrush != null)
            {
                dropShadowBrush.Color = DropShadowColor;
                dropShadowBrush.Opacity = DropShadowOpacity;
            }
        }

        /// <summary>
        /// Updates the drop shadow.
        /// </summary>
        /// <param name="dependencyObject">The drop shadow text block.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void DropShadowDistance_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DropShadowTextBlock dropShadowTextBlock = (DropShadowTextBlock)dependencyObject;
            dropShadowTextBlock.UpdateDropShadowPosition();
        }

        /// <summary>
        /// Updates the drop shadow.
        /// </summary>
        /// <param name="dependencyObject">The drop shadow text block.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void DropShadowAngle_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DropShadowTextBlock dropShadowTextBlock = (DropShadowTextBlock)dependencyObject;
            dropShadowTextBlock.UpdateDropShadowPosition();
        }

        /// <summary>
        /// Updates the drop shadow.
        /// </summary>
        /// <param name="dependencyObject">The drop shadow text block.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void DropShadowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DropShadowTextBlock dropShadowTextBlock = (DropShadowTextBlock)dependencyObject;
            dropShadowTextBlock.UpdateDropShadowBrush();
        }

        /// <summary>
        /// Updates the drop shadow.
        /// </summary>
        /// <param name="dependencyObject">The drop shadow text block.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void DropShadowOpacity_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DropShadowTextBlock dropShadowTextBlock = (DropShadowTextBlock)dependencyObject;
            dropShadowTextBlock.UpdateDropShadowBrush();
        }
    }
}
