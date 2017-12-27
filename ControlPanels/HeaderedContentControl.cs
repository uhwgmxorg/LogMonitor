namespace ControlPanels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.ComponentModel;

    /// <summary>
    /// The base class for all controls that contain single content and have a header.
    /// </summary>
    /// <remarks>
    /// HeaderedContentControl adds Header and HeaderTemplatefeatures to a ContentControl.
    /// HasHeader and HeaderTemplateSelector are removed for lack of support 
    /// and consistency with other Silverlight controls.
    /// </remarks>
    /// <QualityBand>Stable</QualityBand>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Headered", Justification = "Consistency with WPF")]
    public partial class HeaderedContentControl : ContentControl
    {
        #region public object Header
        /// <summary>
        /// Identifies the Header dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
                DependencyProperty.Register(
                        "Header",
                        typeof(object),
                        typeof(HeaderedContentControl),
                        new PropertyMetadata(OnHeaderPropertyChanged));
        #endregion public object Header

        #region public DataTemplate HeaderTemplate
        /// <summary>
        /// Identifies the HeaderTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
                DependencyProperty.Register(
                        "HeaderTemplate",
                        typeof(DataTemplate),
                        typeof(HeaderedContentControl),
                        new PropertyMetadata(OnHeaderTemplatePropertyChanged));
        #endregion public DataTemplate HeaderTemplate

        /// <summary>
        /// Default DependencyObject constructor.
        /// </summary>
        public HeaderedContentControl()
        {
            DefaultStyleKey = typeof(HeaderedContentControl);
        }

        /// <summary>
        /// Gets or sets an object that labels the HeaderedContentControl.  The
        /// default value is null.  The header can be a string, UIElement, or
        /// any other content.
        /// </summary>
        [Category("Common Properties"), Description("Gets or sets an object that labels the HeaderedContentControl.")]
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the template used to display a control's header.
        /// The default is null.
        /// </summary>
        [Category("Common Properties"), Description("Gets or sets the template used to display a control's header.")]
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// This method is invoked when the Header property changes.
        /// </summary>
        /// <param name="oldHeader">The old value of the Header property.</param>
        /// <param name="newHeader">The new value of the Header property.</param>
        protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
        {
        }

        /// <summary>
        /// This method is invoked when the HeaderTemplate property changes.
        /// </summary>
        /// <param name="oldHeaderTemplate">The old value of the HeaderTemplate property.</param>
        /// <param name="newHeaderTemplate">The new value of the HeaderTemplate property.</param>
        protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
        {
        }

        /// <summary>
        /// HeaderTemplateProperty property changed handler.
        /// </summary>
        /// <param name="d">HeaderedContentControl whose HeaderTemplate property is changed.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs, which contains the old and new value.</param>
        private static void OnHeaderTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeaderedContentControl ctrl = (HeaderedContentControl)d;
            ctrl.OnHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        /// <summary>
        /// HeaderProperty property changed handler.
        /// </summary>
        /// <param name="d">HeaderedContentControl whose Header property is changed.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs, which contains the old and new value.</param>
        private static void OnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeaderedContentControl ctrl = (HeaderedContentControl)d;
            ctrl.OnHeaderChanged(e.OldValue, e.NewValue);
        }
    }
}
