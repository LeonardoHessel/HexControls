using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HexControls
{
    public class HexTextBox : Control
    {

        #region Fields
        // Campos privados que representam partes internas do controle (PasswordBox, TextBox, ToggleButton).
        // Não são expostos publicamente; servem apenas para a implementação interna.

        #endregion

        #region Constructors
        // Construtores usados para inicializar o controle.
        // Normalmente define o estilo padrão (DefaultStyleKey).

        static HexTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HexTextBox),
                new FrameworkPropertyMetadata(typeof(HexTextBox)));
        }

        #endregion

        #region Template
        // Associa os elementos do ControlTemplate (PART_*) às variáveis privadas 
        // e conecta os eventos necessários para funcionamento do controle.

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("Part_InfoButton") is Button button)
            {
                button.ApplyTemplate();
                if (button.Template.FindName("Part_Popup", button) is Popup popup)
                {
                    popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(PlacePopup);
                } 
            }
        }

        #endregion

        #region Methods
        // Métodos auxiliares (helpers) usados internamente no controle.
        // Ex.: posicionar o cursor, atualizar valores, etc.
        private CustomPopupPlacement[] PlacePopup(Size popupSize, Size targetSize, Point offset)
        {
            var placement = new CustomPopupPlacement(
                new Point(targetSize.Width - popupSize.Width, -popupSize.Height - 2),
                PopupPrimaryAxis.Horizontal);

            return new CustomPopupPlacement[] { placement };
        }
        #endregion

        #region DP - Declarations
        // Declaração dos DependencyProperty estáticos. 
        // Permite que propriedades do controle sejam bindáveis e customizáveis via XAML.

        public static readonly DependencyProperty InfoTextProperty =
            DependencyProperty.Register(
                nameof(InfoText),
                typeof(string),
                typeof(HexTextBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty FocusBorderBrushProperty =
            DependencyProperty.Register(
                nameof(FocusBorderBrush),
                typeof(Brush),
                typeof(HexTextBox),
                new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(HexTextBox),
                new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                nameof(Label),
                typeof(string),
                typeof(HexTextBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.Register(
                nameof(LabelFontSize),
                typeof(double),
                typeof(HexTextBox),
                new PropertyMetadata(12.0));

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register(
               nameof(Text),
               typeof(string),
               typeof(HexTextBox),
               new FrameworkPropertyMetadata(
                   string.Empty,
                   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register(
                nameof(IsEditable),
                typeof(bool),
                typeof(HexTextBox),
                new PropertyMetadata(true));
        #endregion

        #region DP - Wrappers
        // Propriedades públicas que encapsulam os DependencyProperty.
        // São a interface utilizada por consumidores do controle (XAML ou código).

        public string InfoText
        {
            get => (string)GetValue(InfoTextProperty);
            set => SetValue(InfoTextProperty, value);
        }

        public Brush FocusBorderBrush
        {
            get => (Brush)GetValue(FocusBorderBrushProperty);
            set => SetValue(FocusBorderBrushProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public double LabelFontSize
        {
            get => (double)GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        #endregion
    }
}