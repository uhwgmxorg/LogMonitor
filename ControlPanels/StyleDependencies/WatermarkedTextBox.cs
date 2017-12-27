namespace ControlPanels
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A TextBox control that provides subtle help text when no text is currently entered.
    /// </summary>    
    [TemplatePart(Name = WatermarkedTextBox.PARTWatermark, Type = typeof(ContentControl))]
    public class WatermarkedTextBox : TextBox
    {
        #region Dependency Properties      

        /// <summary>
        /// The Watermark Property.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(object), typeof(WatermarkedTextBox), null);

        /// <summary>
        /// WatermarkTemplate Property.
        /// </summary>
        public static readonly DependencyProperty WatermarkTemplateProperty =
            DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(WatermarkedTextBox), new PropertyMetadata(null));

        #endregion

        #region Private Members

        /// <summary>
        /// Determines the name of the WatermarkedTextBox templated part.
        /// </summary>
        private const string PARTWatermark = "Watermark";
        
        /// <summary>
        /// Indicates whether the control currently has focus.
        /// </summary>
        private bool hasFocus;

        /// <summary>
        /// The templated part that shows the WatermarkContent.
        /// </summary>
        private ContentControl watermarkPresenter;        

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the WatermarkedTextBox class.
        /// </summary>
        public WatermarkedTextBox()
        {
            // Set default values
            DefaultStyleKey = typeof(WatermarkedTextBox);            
            Loaded += new RoutedEventHandler(WatermarkedTextBox_Loaded);
        }

        #endregion

        #region Public Properties         

        /// <summary>
        /// Gets or sets the WatermarkTemplate (this defines how the Watermark will appear when no text is entered and the control does not have focus).
        /// </summary>
        [System.ComponentModel.Category("Miscellaneous"), System.ComponentModel.Description("Gets or sets the WatermarkTemplate (this defines how the Watermark will appear when no text is entered and the control does not have focus).")]
        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Watermark that will appear when no text is entered and the control does not have focus.
        /// </summary>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Gets or sets the Watermark that will appear when no text is entered and the control does not have focus.")]
        public object Watermark
        {
            get { return (object)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        #endregion      

        #region Override Methods

        /// <summary>
        /// Retrieves the specific Templated components that are required once a Template has been applied to the control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            watermarkPresenter = GetTemplateChild(PARTWatermark) as ContentControl;            
            CheckText();
        }

        #endregion

        #region Private Methods  
        /// <summary>
        /// Checks to determine if the Watermark should be displayed.
        /// </summary>
        private void CheckText()
        {
            if (Text.Length > 0)
            {
                watermarkPresenter.Visibility = Visibility.Collapsed;
            }
            else if (!hasFocus)
            {
                watermarkPresenter.Visibility = Visibility.Visible;
            }
            else
            {
                watermarkPresenter.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region UI Events

        /// <summary>
        /// Initializes event handlers for the control once it has loaded.
        /// </summary>
        /// <param name="sender">The WatermarkedTextBox control.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void WatermarkedTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextChanged += new TextChangedEventHandler(WatermarkedTextBox_TextChanged);
            GotFocus += new RoutedEventHandler(WatermarkedTextBox_GotFocus);
            LostFocus += new RoutedEventHandler(WatermarkedTextBox_LostFocus);            
        }       

        /// <summary>
        /// Checks whether the WatermarkContent should appear following an update to the Text.
        /// </summary>
        /// <param name="sender">The WatermarkedTextBox control.</param>
        /// <param name="e">TextChanged Event Arguments.</param>
        private void WatermarkedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckText();
        }

        /// <summary>
        /// Checks whether the WatermarkContent should appear once the control has lost focus.
        /// </summary>
        /// <param name="sender">The WatermarkedTextBox control.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void WatermarkedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            hasFocus = false;
            CheckText();
        }

        /// <summary>
        /// Checks whether the WatermarkContent should appear once the control has received focus.
        /// </summary>
        /// <param name="sender">The WatermarkedTextBox control.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void WatermarkedTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            hasFocus = true;
            CheckText();
        }

        #endregion       
    }
}