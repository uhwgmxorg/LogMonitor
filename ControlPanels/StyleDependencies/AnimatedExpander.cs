namespace ControlPanels
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// An animated expanding / collapsing control.
    /// </summary>
    public class AnimatedExpander : HeaderedContentControl
    {
        /// <summary>
        /// The IsExpanded Dependency Property.
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty =
           DependencyProperty.Register("IsExpanded", typeof(bool), typeof(AnimatedExpander), new PropertyMetadata(true));

        /// <summary>
        /// The LayoutRoot Template Part Name.
        /// </summary>
        private const string ElementLayoutRoot = "LayoutRoot";

        /// <summary>
        /// The ContentContentPresenter Template Part Name.
        /// </summary>
        private const string ElementContentContentPresenter = "ContentContentPresenter";

        /// <summary>
        /// The ExpandToggleButton Template Part Name.
        /// </summary>
        private const string ElementExpandToggleButton = "ExpandToggleButton";

        /// <summary>
        /// The ExpandStoryboard Template Part Name.
        /// </summary>
        private const string ElementExpandStoryboard = "ExpandStoryboard";

        /// <summary>
        /// The ExpandKeyFrame Template Part Name.
        /// </summary>
        private const string ElementExpandKeyFrame = "ExpandKeyFrame";

        /// <summary>
        /// The CollapseStoryboard Template Part Name.
        /// </summary>
        private const string ElementCollapseStoryboard = "CollapseStoryboard";

        /// <summary>
        /// Store the expand toggle button.
        /// </summary>
        private ToggleButton expandToggleButton;

        /// <summary>
        /// Stores the expand key frame.
        /// </summary>
        private SplineDoubleKeyFrame expandKeyFrame;
        
        /// <summary>
        /// Stores the expand storyboard.
        /// </summary>
        private Storyboard expandStoryboard;

        /// <summary>
        /// Stores the collapse storyboard.
        /// </summary>
        private Storyboard collapseStoryboard;

        /// <summary>
        /// AnimatedExpander constructor.
        /// </summary>
        public AnimatedExpander()
        {
            DefaultStyleKey = typeof(AnimatedExpander);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is expanded on not.
        /// </summary>
        [System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Gets or sets a value indicating whether the control is expanded on not.")]
        public bool IsExpanded
        {
            get
            {
                if (expandToggleButton != null)
                {
                    return expandToggleButton.IsChecked.Value;
                }

                return (bool)GetValue(IsExpandedProperty);
            }

            set
            {
                SetValue(IsExpandedProperty, value);
            }
        }

        /// <summary>
        /// Gets the template parts from the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            FrameworkElement layoutRoot = GetTemplateChild(AnimatedExpander.ElementLayoutRoot) as FrameworkElement;
            if (layoutRoot != null)
            {
                if (layoutRoot.Resources.Contains(AnimatedExpander.ElementExpandStoryboard))
                {
                    expandStoryboard = layoutRoot.Resources[AnimatedExpander.ElementExpandStoryboard] as Storyboard;
                }

                if (layoutRoot.Resources.Contains(AnimatedExpander.ElementCollapseStoryboard))
                {
                    collapseStoryboard = layoutRoot.Resources[AnimatedExpander.ElementCollapseStoryboard] as Storyboard;
                }

                expandKeyFrame = layoutRoot.FindName(AnimatedExpander.ElementExpandKeyFrame) as SplineDoubleKeyFrame;
            }

            expandToggleButton = GetTemplateChild(AnimatedExpander.ElementExpandToggleButton) as ToggleButton;
            if (expandToggleButton != null)
            {
                expandToggleButton.Checked += new RoutedEventHandler(ExpandToggleButton_Checked);
                expandToggleButton.Unchecked += new RoutedEventHandler(ExpandToggleButton_Unchecked);
            }

            ContentPresenter contentContentPresenter = GetTemplateChild(AnimatedExpander.ElementContentContentPresenter) as ContentPresenter;
            if (contentContentPresenter != null)
            {
                contentContentPresenter.SizeChanged += new SizeChangedEventHandler(ContentContentPresenter_SizeChanged);
            }
        }

        /// <summary>
        /// Expands the control.
        /// </summary>
        /// <param name="sender">The expand toggle button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void ExpandToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (expandStoryboard != null)
            {
                expandStoryboard.Begin();
            }
        }

        /// <summary>
        /// Collapses the control.
        /// </summary>
        /// <param name="sender">The expand toggle button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void ExpandToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (collapseStoryboard != null)
            {
                collapseStoryboard.Begin();
            }
        }

        /// <summary>
        /// Updates the key frame and height of the control.
        /// </summary>
        /// <param name="sender">The content content presenter.</param>
        /// <param name="e">Size Changed Event Args.</param>
        private void ContentContentPresenter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (expandKeyFrame != null)
            {
                expandKeyFrame.Value = e.NewSize.Height;

                if (expandToggleButton != null && expandToggleButton.IsChecked.Value && expandStoryboard != null)
                {
                    expandStoryboard.Begin();
                }
            }
        }
    }
}
