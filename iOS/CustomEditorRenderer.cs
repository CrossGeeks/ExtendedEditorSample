using System.ComponentModel;
using ExtendedEditorSample.Controls;
using ExtendedEditorSample.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEditorControl), typeof(CustomEditorRenderer))]
namespace ExtendedEditorSample.iOS
{
    public class CustomEditorRenderer : EditorRenderer
    {
        UILabel _placeholderLabel;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (_placeholderLabel == null) 
                {
                    CreatePlaceholder();
                }
           
            }

            if(e.NewElement!=null)
            {
                var customControl = (ExtendedEditorControl)e.NewElement;

                if (customControl.IsExpandable)
                    Control.ScrollEnabled = false;
                else
                    Control.ScrollEnabled = true;
                
                if (customControl.HasRoundedCorner)
                    Control.Layer.CornerRadius = 5;
                else
                    Control.Layer.CornerRadius = 0;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var customControl = (ExtendedEditorControl)Element;

            if (e.PropertyName == Editor.TextProperty.PropertyName)
            {
                _placeholderLabel.Hidden = !string.IsNullOrEmpty(Control.Text);
            }else if (ExtendedEditorControl.PlaceholderProperty.PropertyName == e.PropertyName)
            {
                _placeholderLabel.Text = customControl.Placeholder;
            }
            else if (ExtendedEditorControl.PlaceholderColorProperty.PropertyName == e.PropertyName)
            {
                _placeholderLabel.TextColor = customControl.PlaceholderColor.ToUIColor();
            }
            else if (ExtendedEditorControl.HasRoundedCornerProperty.PropertyName == e.PropertyName)
            {
                if(customControl.HasRoundedCorner)
                    Control.Layer.CornerRadius = 5;
                else
                    Control.Layer.CornerRadius = 0;
            }
            else if (ExtendedEditorControl.IsExpandableProperty.PropertyName == e.PropertyName)
            {
                if (customControl.IsExpandable)
                    Control.ScrollEnabled = false;
                else
                    Control.ScrollEnabled = true;
            }
        }

        public void CreatePlaceholder()
        {
            var element = Element as ExtendedEditorControl;

            _placeholderLabel = new UILabel
            {
                Text = element?.Placeholder,
                TextColor = element.PlaceholderColor.ToUIColor(),
                BackgroundColor = UIColor.Clear
            };

            var edgeInsets = Control.TextContainerInset;
            var lineFragmentPadding = Control.TextContainer.LineFragmentPadding;

            Control.AddSubview(_placeholderLabel);

            var vConstraints = NSLayoutConstraint.FromVisualFormat(
                "V:|-" + edgeInsets.Top + "-[PlaceholderLabel]-" + edgeInsets.Bottom + "-|", 0, new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] { _placeholderLabel }, new NSObject[] { new NSString("PlaceholderLabel") })
            );

            var hConstraints = NSLayoutConstraint.FromVisualFormat(
                "H:|-" + lineFragmentPadding + "-[PlaceholderLabel]-" + lineFragmentPadding + "-|",
                0, new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] { _placeholderLabel }, new NSObject[] { new NSString("PlaceholderLabel") })
            );

            _placeholderLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            Control.AddConstraints(hConstraints);
            Control.AddConstraints(vConstraints);
        }

       
    }
}
