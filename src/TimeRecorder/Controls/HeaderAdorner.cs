using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace TimeRecorder.Controls;
internal class HeaderAdorner : Adorner
{
    private readonly ContentControl _contentControl;

    public HeaderAdorner(UIElement adornedElement) : base(adornedElement)
    {
        _contentControl = new ContentControl();

        // define a two way binding to the template
        var headerTemplateBinding = new Binding
        {
            Source = this,
            Path = new PropertyPath(HeaderTemplateProperty),
            Mode = BindingMode.TwoWay
        };
        _contentControl.SetBinding(ContentControl.ContentTemplateProperty, headerTemplateBinding);

        // content binding
        var contentBinding = new Binding
        {
            Source = this,
            Path = new PropertyPath(DataContextProperty),
            Mode = BindingMode.TwoWay
        };
        _contentControl.SetBinding(ContentControl.ContentProperty, contentBinding);

        this.AddVisualChild(_contentControl);
        this.AddLogicalChild(_contentControl);
    }

    protected override int VisualChildrenCount
    {
        get
        {
            return 1;
        }
    }

    protected override Visual GetVisualChild(int index)
    {
        if (index != 0)
        {
            throw new ArgumentOutOfRangeException("index");
        }
        return _contentControl;
    }

    protected override IEnumerator LogicalChildren
    {
        get
        {
            var list = new ArrayList { _contentControl };
            return list.GetEnumerator();
        }
    }


    #region HeaderTemplate

    /// <summary>
    /// Identifies the <see cref="HeaderTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        "HeaderTemplate",
        typeof(DataTemplate),
        typeof(HeaderAdorner),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets or sets the HeaderTemplate property. This is a dependency property.
    /// </summary>
    /// <value>
    ///
    /// </value>
    public DataTemplate HeaderTemplate
    {
        get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
        set { SetValue(HeaderTemplateProperty, value); }
    }

    #endregion

    protected override Size MeasureOverride(Size constraint)
    {
        _contentControl.Measure(constraint);

        // get the AdornedElement size
        return this.AdornedElement.RenderSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double top = this.Top;
        if ((top + _contentControl.DesiredSize.Height) > finalSize.Height)
        {
            top = finalSize.Height - _contentControl.DesiredSize.Height;
        }

        var location = new Point(0, top);
        var size = new Size(finalSize.Width, _contentControl.DesiredSize.Height);
        _contentControl.Arrange(new Rect(location, size));

        return finalSize;
    }

    public double Top { get; set; }

    public void UpdateLocation(double top)
    {
        this.Top = Math.Abs(top);
        this.InvalidateArrange();
    }
}
