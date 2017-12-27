using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ControlPanels
{
    /// <summary>
    /// A double headed slider for selecting a range.
    /// </summary>
    public class RangeSlider : Control
    {
        /// <summary>
        /// The minimum value dependency protperty.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(RangeSlider), new PropertyMetadata(0.0, new PropertyChangedCallback(RangeBounds_Changed)));

        /// <summary>
        /// The maximum value dependency protperty.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(RangeSlider), new PropertyMetadata(1.0, new PropertyChangedCallback(RangeBounds_Changed)));

        /// <summary>
        /// The minimum range span dependency protperty.
        /// </summary>
        public static readonly DependencyProperty MinimumRangeSpanProperty =
            DependencyProperty.Register("MinimumRangeSpan", typeof(double), typeof(RangeSlider), new PropertyMetadata(0.0));

        /// <summary>
        /// The range start dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeStartProperty =
            DependencyProperty.Register("RangeStart", typeof(double), typeof(RangeSlider), new PropertyMetadata(0.0, new PropertyChangedCallback(Range_Changed)));

        /// <summary>
        /// The range end dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeEndProperty =
            DependencyProperty.Register("RangeEnd", typeof(double), typeof(RangeSlider), new PropertyMetadata(1.0, new PropertyChangedCallback(Range_Changed)));

        /// <summary>
        /// The element name for the range start thumb.
        /// </summary>
        private const string ElementRangeStartThumb = "RangeStartThumb";

        /// <summary>
        /// The element name for the range center thumb.
        /// </summary>
        private const string ElementRangeCenterThumb = "RangeCenterThumb";

        /// <summary>
        /// The element name for the range end thumb.
        /// </summary>
        private const string ElementRangeEndThumb = "RangeEndThumb";

        /// <summary>
        /// The element name for the selected range borer.
        /// </summary>
        private const string ElementSelectedRangeBorder = "SelectedRangeBorder";

        /// <summary>
        /// The range start thumb.
        /// </summary>
        private Thumb rangeStartThumb;

        /// <summary>
        /// The range center thumb.
        /// </summary>
        private Thumb rangeCenterThumb;

        /// <summary>
        /// The range end thumb.
        /// </summary>
        private Thumb rangeEndThumb;

        /// <summary>
        /// The selected range border.
        /// </summary>
        private Border selectedRangeBorder;


        // This thumbWidth could change if you restyle the thumbs.
        private int thumbWidth = 10;

        /// <summary>
        /// RangeSlider constructor.
        /// </summary>
        public RangeSlider()
        {
            DefaultStyleKey = typeof(RangeSlider);
            SizeChanged += new SizeChangedEventHandler(RangeSlider_SizeChanged);
        }

        /// <summary>
        /// RangeChanged event.
        /// </summary>
        public event EventHandler RangeChanged;

        /// <summary>
        /// Gets or sets the slider minimum value.
        /// </summary>
        public double Minimum
        {
            get
            {
                return (double)GetValue(MinimumProperty);
            }

            set
            {
                SetValue(MinimumProperty, Math.Min(Maximum, Math.Max(0, value)));

                if (Maximum - Minimum < MinimumRangeSpan)
                {
                    MinimumRangeSpan = Maximum - Minimum;
                }
            }
        }

        /// <summary>
        /// Gets or sets the slider maximum value.
        /// </summary>
        public double Maximum
        {
            get
            {
                return (double)GetValue(MaximumProperty);
            }

            set
            {
                SetValue(MaximumProperty, Math.Max(Minimum, value));

                if (Maximum - Minimum < MinimumRangeSpan)
                {
                    MinimumRangeSpan = Maximum - Minimum;
                }
            }
        }

        /// <summary>
        /// Gets or sets the slider minimum range span.
        /// </summary>
        public double MinimumRangeSpan
        {
            get
            {
                return (double)GetValue(MinimumRangeSpanProperty);
            }

            set
            {
                SetValue(MinimumRangeSpanProperty, Math.Min(Maximum - Minimum, value));
                UpdateSelectedRangeMinimumWidth();
                UpdateRange(false);
            }
        }

        /// <summary>
        /// Gets or sets the range start.
        /// </summary>
        public double RangeStart
        {
            get
            {
                return (double)GetValue(RangeStartProperty);
            }

            set
            {
                double rangeStart = Math.Max(Minimum, value);
                SetValue(RangeStartProperty, rangeStart);
            }
        }

        /// <summary>
        /// Gets or sets the range end.
        /// </summary>
        public double RangeEnd
        {
            get
            {
                return (double)GetValue(RangeEndProperty);
            }

            set
            {
                double rangeEnd = Math.Min(Maximum, value);
                SetValue(RangeEndProperty, rangeEnd);
            }
        }

        /// <summary>
        /// Gets the template parts from the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            selectedRangeBorder = GetTemplateChild(RangeSlider.ElementSelectedRangeBorder) as Border;
            rangeStartThumb = GetTemplateChild(RangeSlider.ElementRangeStartThumb) as Thumb;
            if (rangeStartThumb != null)
            {
                rangeStartThumb.DragDelta += new DragDeltaEventHandler(RangeStartThumb_DragDelta);
                rangeStartThumb.SizeChanged += new SizeChangedEventHandler(RangeThumb_SizeChanged);
            }

            rangeCenterThumb = GetTemplateChild(RangeSlider.ElementRangeCenterThumb) as Thumb;
            if (rangeCenterThumb != null)
            {
                rangeCenterThumb.DragDelta += new DragDeltaEventHandler(RangeCenterThumb_DragDelta);
            }

            rangeEndThumb = GetTemplateChild(RangeSlider.ElementRangeEndThumb) as Thumb;
            if (rangeEndThumb != null)
            {
                rangeEndThumb.DragDelta += new DragDeltaEventHandler(RangeEndThumb_DragDelta);
                rangeEndThumb.SizeChanged += new SizeChangedEventHandler(RangeThumb_SizeChanged);
            }
        }

        #region Dependency property events.
        /// <summary>
        /// Updates the slider when the selected range changes.
        /// </summary>
        /// <param name="d">The range slider.</param>
        /// <param name="args">Dependency Property Changed Event Args.</param>
        private static void Range_Changed(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            RangeSlider rangeSlider = d as RangeSlider;
            rangeSlider.UpdateSlider();

            if (rangeSlider.RangeChanged != null)
            {
                rangeSlider.RangeChanged(rangeSlider, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Updates the range start and end values.
        /// </summary>
        /// <param name="d">The range slider.</param>
        /// <param name="args">Dependency Property Changed Event Args.</param>
        private static void RangeBounds_Changed(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as RangeSlider).UpdateRange(true);
        }
        #endregion

        #region Range Slider events
        /// <summary>
        /// Updates the slider UI.
        /// </summary>
        /// <param name="sender">The range slider.</param>
        /// <param name="e">Size Changed Event Args.</param>
        private void RangeSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSelectedRangeMinimumWidth();
            UpdateSlider();
        }
        #endregion

        #region Thumb events
        /// <summary>
        /// Updates the slider's minimum width.
        /// </summary>
        /// <param name="sender">The range thumb.</param>
        /// <param name="e">Size changed event args.</param>
        private void RangeThumb_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSelectedRangeMinimumWidth();
        }

        /// <summary>
        /// Moves the whole range slider.
        /// </summary>
        /// <param name="sender">The range cetner thumb.</param>
        /// <param name="e">Drag Delta Event Args.</param>
        private void RangeCenterThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (selectedRangeBorder != null)
            {
                double startMargin = selectedRangeBorder.Margin.Left + e.HorizontalChange;
                double endMargin = selectedRangeBorder.Margin.Right - e.HorizontalChange;

                if (startMargin + e.HorizontalChange <= 0)
                {
                    startMargin = 0;
                    endMargin = (ActualWidth - thumbWidth) - (((RangeEnd - RangeStart) / (Maximum - Minimum)) * (ActualWidth - thumbWidth));
                }
                else if (endMargin - e.HorizontalChange <= 0)
                {
                    endMargin = 0;
                    startMargin = (ActualWidth - thumbWidth) - (((RangeEnd - RangeStart) / (Maximum - Minimum)) * (ActualWidth - thumbWidth));
                }

                if (!double.IsNaN(startMargin) && !double.IsNaN(endMargin))
                {
                    selectedRangeBorder.Margin = new Thickness(
                        startMargin,
                        selectedRangeBorder.Margin.Top,
                        endMargin,
                        selectedRangeBorder.Margin.Bottom);
                }

                UpdateRange(true);
            }
        }

        /// <summary>
        /// Moves the range end thumb.
        /// </summary>
        /// <param name="sender">The range end thumb.</param>
        /// <param name="e">Drag Delta Event Args.</param>
        private void RangeEndThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (selectedRangeBorder != null)
            {
                double endMargin = Math.Min(ActualWidth - selectedRangeBorder.MinWidth, Math.Max(0, selectedRangeBorder.Margin.Right - e.HorizontalChange));
                double startMargin = selectedRangeBorder.Margin.Left;

                if (ActualWidth - startMargin - endMargin < selectedRangeBorder.MinWidth)
                {
                    startMargin = ActualWidth - endMargin - selectedRangeBorder.MinWidth;
                }

                selectedRangeBorder.Margin = new Thickness(
                    startMargin,
                    selectedRangeBorder.Margin.Top,
                    endMargin,
                    selectedRangeBorder.Margin.Bottom);

                UpdateRange(true);
            }
        }

        /// <summary>
        /// Moves the range start thumb.
        /// </summary>
        /// <param name="sender">The range start thumb.</param>
        /// <param name="e">Drag Delta Event Args.</param>
        private void RangeStartThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (selectedRangeBorder != null)
            {
                double startMargin = Math.Min(ActualWidth - selectedRangeBorder.MinWidth, Math.Max(0, selectedRangeBorder.Margin.Left + e.HorizontalChange));
                double endMargin = selectedRangeBorder.Margin.Right;

                if (ActualWidth - startMargin - endMargin < selectedRangeBorder.MinWidth)
                {
                    endMargin = ActualWidth - startMargin - selectedRangeBorder.MinWidth;
                }

                selectedRangeBorder.Margin = new Thickness(
                    startMargin,
                    selectedRangeBorder.Margin.Top,
                    endMargin,
                    selectedRangeBorder.Margin.Bottom);

                UpdateRange(true);
            }
        }

        #endregion

        /// <summary>
        /// Updates the thumb mimimum width.
        /// </summary>
        private void UpdateSelectedRangeMinimumWidth()
        {
            if (selectedRangeBorder != null && rangeStartThumb != null && rangeEndThumb != null)
            {
                selectedRangeBorder.MinWidth = Math.Max(
                    rangeStartThumb.ActualWidth + rangeEndThumb.ActualWidth,
                    (MinimumRangeSpan / ((Maximum - Minimum) == 0 ? 1 : (Maximum - Minimum))) * (ActualWidth - thumbWidth));
            }
        }

        /// <summary>
        /// Updates the slider UI.
        /// </summary>
        private void UpdateSlider()
        {
            if (selectedRangeBorder != null)
            {
                double startMargin = (RangeStart / (Maximum - Minimum)) * (ActualWidth - thumbWidth);
                double endMargin = ((Maximum - RangeEnd) / (Maximum - Minimum)) * (ActualWidth - thumbWidth);

                if (!double.IsNaN(startMargin) && !double.IsNaN(endMargin))
                {
                    selectedRangeBorder.Margin = new Thickness(
                            startMargin,
                            selectedRangeBorder.Margin.Top,
                            endMargin,
                            selectedRangeBorder.Margin.Bottom);
                }
            }
        }

        /// <summary>
        /// Updates the selected range.
        /// </summary>
        /// <param name="raiseEvent">Whether the range changed event should fire.</param>
        private void UpdateRange(bool raiseEvent)
        {
            if (selectedRangeBorder != null)
            {
                bool rangeChanged = false;
                double rangeStart = ((Maximum - Minimum) * (selectedRangeBorder.Margin.Left / (ActualWidth - thumbWidth))) + Minimum;
                double rangeEnd = Maximum - ((Maximum - Minimum) * (selectedRangeBorder.Margin.Right / (ActualWidth - thumbWidth)));

                if (rangeEnd - rangeStart < MinimumRangeSpan)
                {
                    if (rangeStart + MinimumRangeSpan > Maximum)
                    {
                        rangeStart = Maximum - MinimumRangeSpan;
                    }

                    rangeEnd = Math.Min(Maximum, rangeStart + MinimumRangeSpan);
                }

                if (rangeStart != RangeStart || rangeEnd != RangeEnd)
                {
                    rangeChanged = true;
                }

                RangeStart = rangeStart;
                RangeEnd = rangeEnd;

                if (raiseEvent && rangeChanged && RangeChanged != null)
                {
                    RangeChanged(this, EventArgs.Empty);
                }
            }
        }
    }
}
