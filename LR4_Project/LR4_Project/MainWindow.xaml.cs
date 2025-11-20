using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LR4_Project
{
    public partial class MainWindow : Window
    {
        // Для задания 4: список кнопок игры
        private List<Button> gameButtons = new List<Button>();

        public MainWindow()
        {
            InitializeComponent();

          
            gameButtons.Add(G1);
            gameButtons.Add(G2);
            gameButtons.Add(G3);
            gameButtons.Add(G4);
            gameButtons.Add(G5);

            
            CreateColorButtons();
        }

        // ---------------- Задание 1 ----------------
        private void BtnHello_Click(object sender, RoutedEventArgs e)
        {
            LblGreeting.Content = "Привіт";
        }

        private void BtnBye_Click(object sender, RoutedEventArgs e)
        {
            LblGreeting.Content = "До побачення";
        }

        // ---------------- Задание 2 ----------------
        private void BtnHideTextBlock_Click(object sender, RoutedEventArgs e)
        {
            TbExample.Visibility = Visibility.Collapsed;
        }

        private void BtnShowTextBlock_Click(object sender, RoutedEventArgs e)
        {
            TbExample.Visibility = Visibility.Visible;
        }

        // ---------------- Задание 3 ----------------
        private void BtnHideTextBox_Click(object sender, RoutedEventArgs e)
        {
            TbInput.Visibility = Visibility.Collapsed;
        }

        private void BtnShowTextBox_Click(object sender, RoutedEventArgs e)
        {
            TbInput.Visibility = Visibility.Visible;
        }

        private void BtnClearTextBox_Click(object sender, RoutedEventArgs e)
        {
            TbInput.Text = string.Empty;
        }

        // ---------------- Задание 4 (игра) ----------------
       
        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;

            int idx = gameButtons.IndexOf(btn);
            if (idx < 0) return;

            
            ToggleVisibility(gameButtons[idx]);

           
            int next = (idx + 1) % gameButtons.Count;
            ToggleVisibility(gameButtons[next]);

          
            if (AllGameButtonsHidden())
            {
                MessageBox.Show("Вітаю! Ви приховали всі кнопки.", "Перемога");
            }
        }

        private void ToggleVisibility(Button b)
        {
            b.Visibility = (b.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private bool AllGameButtonsHidden()
        {
            foreach (var b in gameButtons)
                if (b.Visibility == Visibility.Visible)
                    return false;
            return true;
        }

        private void ResetGame_Click(object sender, RoutedEventArgs e)
        {
            foreach (var b in gameButtons)
            {
                b.Visibility = Visibility.Visible;
            }
        }

        // ---------------- Задание 5 (фунты → кг) ----------------
        private void ConvertPounds_Click(object sender, RoutedEventArgs e)
        {
            string s = PoundsBox.Text.Replace(',', '.').Trim();
            if (double.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double pounds))
            {
                double kg = pounds * 0.45359237;
                KgResult.Text = $"{kg:F3} кг";
            }
            else
            {
                MessageBox.Show("Введіть коректне число (фунти).");
            }
        }

        // ---------------- Задание 6 ----------------
        private void CreateColorButtons()
        {
            
            string[] colorNames = new string[]
            {
                "Navy","Blue","Aqua","Teal","Olive","Green","Lime","Yellow",
                "Orange","Red","Maroon","Fuchsia","Purple","Black",
                "Silver","Gray","White"
            };

            foreach (var name in colorNames)
            {
                var btn = new Button
                {
                    Content = name,
                    Margin = new Thickness(2.0),        
                    Padding = new Thickness(6, 2, 6, 2),
                    MinWidth = 70
                };

                
                Brush? brush = null;
                try
                {
                    brush = (Brush)new BrushConverter().ConvertFromString(name);
                }
                catch
                {
                    brush = Brushes.LightGray;
                }

                btn.Background = brush;

                var darkNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "Black","Navy","Maroon","Purple","Teal","Olive"
                };

                btn.Foreground = darkNames.Contains(name) ? Brushes.White : Brushes.Black;

                ColorsWrapPanel.Children.Add(btn);
            }
        }
    }
}
