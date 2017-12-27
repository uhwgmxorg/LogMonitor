namespace ControlPanels
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    /// <summary>
    /// A control that shows a loading animation.
    /// </summary>
    public class LoadingAnimation : ContentControl
    {
        /// <summary>
        /// Ellipse fill property.
        /// </summary>
        public static readonly DependencyProperty EllipseFillProperty =
            DependencyProperty.Register("EllipseFill", typeof(Brush), typeof(LoadingAnimation), null);

        /// <summary>
        /// Ellipse stroke property.
        /// </summary>
        public static readonly DependencyProperty EllipseStrokeProperty =
            DependencyProperty.Register("EllipseStroke", typeof(Brush), typeof(LoadingAnimation), null);

        /// <summary>
        /// Stores the loading animation storyboard.
        /// </summary>
        private Storyboard loadingAnimation;

        /// <summary>
        /// Stores whether the animation is running.
        /// </summary>
        private AnimationState animationState;

        /// <summary>
        /// Stores whether the animation should play on load.
        /// </summary>
        private bool autoPlay;

        /// <summary>
        /// LoadingAnimation constructor.
        /// </summary>
        public LoadingAnimation()
        {
            DefaultStyleKey = typeof(LoadingAnimation);
        }

        /// <summary>
        /// Gets or sets the ellipse fill.
        /// </summary>
        [System.ComponentModel.Category("Loading Animation Properties"), System.ComponentModel.Description("The fill for the little ellipses.")]
        public Brush EllipseFill
        {
            get { return (Brush)GetValue(EllipseFillProperty); }
            set { SetValue(EllipseFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ellipse stroke.
        /// </summary>
        [System.ComponentModel.Category("Loading Animation Properties"), System.ComponentModel.Description("The stroke for the little ellipses.")]
        public Brush EllipseStroke
        {
            get { return (Brush)GetValue(EllipseStrokeProperty); }
            set { SetValue(EllipseStrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the animation should play on load.
        /// </summary>
        [System.ComponentModel.Category("Loading Animation Properties"), System.ComponentModel.Description("Whether the animation auto plays.")]
        public bool AutoPlay
        {
            get 
            { 
                return autoPlay; 
            }

            set 
            { 
                autoPlay = value;

                if (loadingAnimation != null)
                {
                    loadingAnimation.Stop();
                    if (autoPlay)
                    {
                        loadingAnimation.Begin();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the animation state,
        /// </summary>
        public AnimationState AnimationState
        {
            get { return animationState; }
        }

        /// <summary>
        /// Gets the parts out of the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            loadingAnimation = (Storyboard)GetTemplateChild("PART_LoadingAnimation");

            if (loadingAnimation != null && autoPlay)
            {
                loadingAnimation.Begin();
            }
        }

        /// <summary>
        /// Begins the loading animation.
        /// </summary>
        public void Begin()
        {
            if (loadingAnimation != null)
            {
                animationState = AnimationState.Playing;
                loadingAnimation.Begin();
            }
        }

        /// <summary>
        /// Pauses the animation.
        /// </summary>
        public void Pause()
        {
            if (loadingAnimation != null)
            {
                animationState = AnimationState.Paused;
                loadingAnimation.Pause();
            }
        }

        /// <summary>
        /// Resumes the animation.
        /// </summary>
        public void Resume()
        {
            if (loadingAnimation != null)
            {
                animationState = AnimationState.Playing;
                loadingAnimation.Resume();
            }
        }

        /// <summary>
        /// Stops the animation.
        /// </summary>
        public void Stop()
        {
            if (loadingAnimation != null)
            {
                animationState = AnimationState.Stopped;
                loadingAnimation.Stop();
            }
        }
    }
}
