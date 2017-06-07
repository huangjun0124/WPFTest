using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UserDictionary
{
    /// <summary>
    /// Interaction logic for FontViewer.xaml
    /// </summary>
    public partial class FontViewer : Window
    {
        public FontViewer()
        {
            InitializeComponent();
            Binding binding = new Binding();
            binding.Source = lsFonts;
            lsFont.SetBinding(ListBox.ItemsSourceProperty, binding);
        }

        private List<TextBlock> lsFonts
        {
            get
            {
                List<TextBlock> result = new List<TextBlock>();
                foreach (FontFamily family in Fonts.SystemFontFamilies)
                {
                    foreach (KeyValuePair<XmlLanguage, string> pair in family.FamilyNames)
                    {
                        TextBlock t = new TextBlock();
                        t.Text = pair.Value;
                        t.FontFamily = family;
                        t.FontSize = 12;
                        result.Add(t);
                    }
                }
                return result;
            }
        }
    }
}
