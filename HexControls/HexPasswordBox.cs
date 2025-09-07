using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class HexPasswordBox : Control
    {
        #region Fields
        // Campos privados que representam partes internas do controle (PasswordBox, TextBox, ToggleButton).
        // Não são expostos publicamente; servem apenas para a implementação interna.

        private PasswordBox? _passwordBox;
        private TextBox? _textBox;
        private ToggleButton? _toggleButton;
        private bool _isUpdating;
        private int _lastCaretIndex; // Guarda a posição do cursor

        #endregion

        #region Constructor
        // Construtores usados para inicializar o controle.
        // Normalmente define o estilo padrão (DefaultStyleKey).

        static HexPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HexPasswordBox),
                new FrameworkPropertyMetadata(typeof(HexPasswordBox)));
        }

        #endregion

        #region Template
        // Associa os elementos do ControlTemplate (PART_*) às variáveis privadas 
        // e conecta os eventos necessários para funcionamento do controle.

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _passwordBox = GetTemplateChild("PART_PasswordBox") as PasswordBox;
            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            _toggleButton = GetTemplateChild("PART_ToggleButton") as ToggleButton;

            if (_passwordBox != null)
                _passwordBox.PasswordChanged += PasswordTextChanged;

            if (_textBox != null)
                _textBox.TextChanged += PasswordTextChanged;

            if (!string.IsNullOrEmpty(Password))
                UpdateChildrenFromPassword();
        }

        #endregion

        #region Events
        // Manipuladores de eventos dos elementos internos (PasswordChanged, TextChanged, etc.).
        // Responsáveis por manter a sincronização entre interface visual e propriedades de dependência.

        private void PasswordTextChanged(object sender, RoutedEventArgs e)
        {
            if (_isUpdating) return;

            try
            {
                _isUpdating = true;

                if (_passwordBox.Visibility == Visibility.Visible)
                {
                    Password = _passwordBox.Password;
                }
                else if (_textBox.Visibility == Visibility.Visible)
                {
                    Password = _textBox.Text;
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }

        #endregion

        #region Methods
        // Métodos auxiliares (helpers) usados internamente no controle.
        // Ex.: posicionar o cursor, atualizar valores, etc.

        private void UpdateChildrenFromPassword()
        {
            if (_isUpdating) return;

            _isUpdating = true;

            var passwordValue = Password ?? string.Empty;

            if (_passwordBox != null && _passwordBox.Password != passwordValue)
                _passwordBox.Password = passwordValue;

            if (_textBox != null && _textBox.Text != passwordValue)
                _textBox.Text = passwordValue;

            _isUpdating = false;
        }

        private void SetPasswordBoxCaretIndex(int index)
        {
            try
            {
                var passwordBoxType = _passwordBox.GetType();
                var selectMethod = passwordBoxType.GetMethod("Select",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (selectMethod != null)
                {
                    selectMethod.Invoke(_passwordBox, new object[] {
                    _passwordBox.Password.Length, 0
                });
                }
            }
            catch
            {
                // Fallback: pelo menos o foco funciona
            }
        }

        private void SetTextBoxCaretIndex(int index)
        {
            if (_textBox != null)
            {
                _textBox.CaretIndex = index;
            }
        }

        private void ResetCaretPosition()
        {
            int length = Password.Length;
            if (IsPasswordVisible)
            {
                SetTextBoxCaretIndex(length);
            }
            else
            {
                SetPasswordBoxCaretIndex(length);
            }
        }

        private void ResetFocus()
        {
            if (IsPasswordVisible)
            {
                _textBox?.Focus();
            }
            else
            {
                _passwordBox?.Focus();
            }
        }

        #endregion

        #region DP - Declarations
        // Declaração dos DependencyProperty estáticos. 
        // Permite que propriedades do controle sejam bindáveis e customizáveis via XAML.

        public static readonly DependencyProperty FocusBorderBrushProperty =
            DependencyProperty.Register(
                nameof(FocusBorderBrush),
                typeof(Brush),
                typeof(HexPasswordBox),
                new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(HexPasswordBox),
                new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                nameof(Label),
                typeof(string),
                typeof(HexPasswordBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.Register(
                nameof(LabelFontSize),
                typeof(double),
                typeof(HexPasswordBox),
                new PropertyMetadata(12.0));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(HexPasswordBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register(
                nameof(IsEditable),
                typeof(bool),
                typeof(HexPasswordBox),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsPasswordVisibleProperty =
            DependencyProperty.Register(
                nameof(IsPasswordVisible),
                typeof(bool),
                typeof(HexPasswordBox),
                new PropertyMetadata(false, OnIsPasswordVisibleChanged));

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(HexPasswordBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordChanged));

        #endregion

        #region DP - Wrappers
        // Propriedades públicas que encapsulam os DependencyProperty.
        // São a interface utilizada por consumidores do controle (XAML ou código).

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

        public bool IsPasswordVisible
        {
            get => (bool)GetValue(IsPasswordVisibleProperty);
            set => SetValue(IsPasswordVisibleProperty, value);
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        #endregion

        #region DP - Callbacks
        // Métodos executados automaticamente quando valores de DependencyProperty mudam.
        // Usados para reagir a alterações de estado (ex.: alternar visibilidade da senha).

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HexPasswordBox)d;
            control.UpdateChildrenFromPassword();
        }

        private static void OnIsPasswordVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HexPasswordBox)d;
            control.ResetCaretPosition();
            //control.ResetFocus();
        }

        #endregion

        //#region Para Classificar

        //#region Custom Events

        //// Definir o RoutedEvent
        //public static readonly RoutedEvent PasswordVisibilityChangedEvent =
        //    EventManager.RegisterRoutedEvent(
        //        nameof(PasswordVisibilityChanged),
        //        RoutingStrategy.Bubble,
        //        typeof(RoutedEventHandler),
        //        typeof(HexPasswordBox));

        //// Evento para consumo externo
        //public event RoutedEventHandler PasswordVisibilityChanged
        //{
        //    add { AddHandler(PasswordVisibilityChangedEvent, value); }
        //    remove { RemoveHandler(PasswordVisibilityChangedEvent, value); }
        //}

        //// Método para disparar o evento
        //protected virtual void OnPasswordVisibilityChanged(bool isVisible)
        //{
        //    var args = new PasswordVisibilityChangedRoutedEventArgs(
        //        PasswordVisibilityChangedEvent,
        //        isVisible);
        //    RaiseEvent(args);
        //}

        //#endregion

        //#endregion
    }
}