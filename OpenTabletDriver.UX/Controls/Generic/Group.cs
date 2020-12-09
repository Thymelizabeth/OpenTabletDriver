using System;
using Eto.Drawing;
using Eto.Forms;
using OpenTabletDriver.Desktop.Interop;
using OpenTabletDriver.Plugin;

namespace OpenTabletDriver.UX.Controls.Generic
{
    public class Group : Panel
    {
        public Group()
        {
        }

        public Group(string text, Control content, Orientation orientation = DEFAULT_ORIENTATION, bool expand = true)
            : this()
        {
            this.Text = text;
            this.Content = content;
            this.Orientation = orientation;
            this.ExpandContent = expand;
        }

        private const Orientation DEFAULT_ORIENTATION = Orientation.Vertical;

        protected virtual Padding ContentPadding => SystemInterop.CurrentPlatform == PluginPlatform.Windows ? new Padding(5, 10, 5, 5) : new Padding(5);

        protected virtual Color HorizontalBackgroundColor => SystemColors.ControlBackground;
        protected virtual Color VerticalBackgroundColor => SystemColors.WindowBackground;
        
        private Label titleLabel = new Label
        {
            Font = Fonts.Sans(SystemInterop.CurrentPlatform == PluginPlatform.MacOS ? 12 : 9, FontStyle.Bold)
        };

        public string Text
        {
            set => titleLabel.Text = value;
            get => titleLabel.Text;
        }

        public new Control Content { set; get; }

        public Orientation Orientation { set; get; } = DEFAULT_ORIENTATION;
        public bool ExpandContent { set; get; } = true;
        public HorizontalAlignment TitleHorizontalAlignment { set; get; } = HorizontalAlignment.Left;        
        public VerticalAlignment TitleVerticalAlignment { set; get; } = VerticalAlignment.Center;

        protected void UpdateControlLayout()
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                {
                    var groupBox = new GroupBox
                    {
                        Content = new StackLayout
                        {
                            VerticalContentAlignment = VerticalAlignment.Stretch,
                            Orientation = Orientation.Horizontal,
                            Spacing = 5,
                            Padding = ContentPadding,
                            Items =
                            {
                                new StackLayoutItem(titleLabel, TitleVerticalAlignment),
                                new StackLayoutItem(this.Content, ExpandContent)
                            }
                        }
                    };
                    if (SystemInterop.CurrentPlatform != PluginPlatform.MacOS)
                    {
                        groupBox.BackgroundColor = HorizontalBackgroundColor;
                    }
                    if (!ExpandContent)
                    {
                        (groupBox.Content as StackLayout).Items.Insert(1, new StackLayoutItem(null, true));
                    }
                    base.Content = groupBox;
                    break;
                }
                case Orientation.Vertical:
                {
                    GroupBox groupBox;
                    base.Content = new StackLayout
                    {
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Spacing = 5,
                        Padding = ContentPadding,
                        Items =
                        {
                            new StackLayoutItem(titleLabel, TitleHorizontalAlignment),
                            new StackLayoutItem
                            {
                                Expand = true,
                                Control = groupBox = new GroupBox
                                {
                                    Padding = ContentPadding,
                                    Content = this.Content
                                }
                            }
                        }
                    };
                    if (SystemInterop.CurrentPlatform != PluginPlatform.MacOS)
                    {
                        groupBox.BackgroundColor = VerticalBackgroundColor;
                    }
                    break;
                }
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            UpdateControlLayout();
        }
    }
}